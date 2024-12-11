using ECommerce.Modules.PurchaseProfiler.Core.Repositories;
using Microsoft.ML;
using System.IO.Compression;

namespace ECommerce.Modules.PurchaseProfiler.Core.Services
{
    internal sealed class FastTreePurchaseProfilerModel
        (
            IUserModelRepository userModelRepository
        )
        : IFastTreePurchaseProfilerModel
    {
        private readonly MLContext _mlContext = new ();

        public async Task<ITransformer?> GetModel(Guid userId)
        {
            var model = await userModelRepository.GetByUserIdAsync(userId);
            if (model is null)
            {
                return null;
            }

            var compressedData = Convert.FromBase64String(model.ModelData);
            var modelBytes = Decompress(compressedData);
            using var memoryStream = new MemoryStream(modelBytes);
            return _mlContext.Model.Load(memoryStream, out _);
        }

        public async Task<ITransformer> GenerateModel(IEnumerable<CustomerData> trainData, Guid userId)
        {
            var trainingData = _mlContext.Data.LoadFromEnumerable(trainData);
            var pipeline = _mlContext.Transforms.Concatenate("Features", "CustomerId", "ProductId", "Price", "PurchaseFrequency")
                    .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
                    .Append(_mlContext.BinaryClassification.Trainers.FastTree(labelColumnName: "PurchasedProduct", featureColumnName: "Features",
                                                                                    numberOfLeaves: 25,
                                                                                    numberOfTrees: 150,
                                                                                    minimumExampleCountPerLeaf: 10,
                                                                                    learningRate: 0.05));
            var dataSplit = _mlContext.Data.TrainTestSplit(trainingData, testFraction: 0.2);
            var trainDataSplit = dataSplit.TrainSet;
            var fastTreeModel = pipeline.Fit(trainDataSplit);
            await SaveModelToDatabase(fastTreeModel, trainingData.Schema, userId, trainData);
            return fastTreeModel;
        }

        private async Task SaveModelToDatabase(ITransformer model, DataViewSchema schema, Guid userId, IEnumerable<CustomerData> trainingData)
        {
            using var memoryStream = new MemoryStream();
            _mlContext.Model.Save(model, schema, memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            byte[] modelBytes = memoryStream.ToArray();

            byte[] compressedModel = Compress(modelBytes);
            string base64Model = Convert.ToBase64String(compressedModel);

            await userModelRepository.AddAsync(new Entities.UserModel
            {
                CreateDate = DateTime.UtcNow,
                UserId = userId,
                Version = 1,
                TrainingDataSet = trainingData,
            });
        }

        private byte[] Compress(byte[] data)
        {
            using var output = new MemoryStream();
            using (var gzip = new GZipStream(output, CompressionMode.Compress))
            {
                gzip.Write(data, 0, data.Length);
            }
            return output.ToArray();
        }

        private byte[] Decompress(byte[] data)
        {
            using var input = new MemoryStream(data);
            using var output = new MemoryStream();
            using (var gzip = new GZipStream(input, CompressionMode.Decompress))
            {
                gzip.CopyTo(output);
            }
            return output.ToArray();
        }
    }
}
