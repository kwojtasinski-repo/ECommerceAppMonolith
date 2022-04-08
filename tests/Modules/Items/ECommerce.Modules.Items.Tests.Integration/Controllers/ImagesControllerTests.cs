using ECommerce.Modules.Items.Infrastructure.EF.DAL;
using ECommerce.Modules.Items.Tests.Integration.Common;
using ECommerce.Shared.Tests;
using Flurl.Http;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System.Net.Http;
using Shouldly;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Modules.Items.Tests.Integration.Controllers
{
    [Collection("integrationImages")]
    public class ImagesControllerTests : IClassFixture<TestApplicationFactory<Program>>,
        IClassFixture<TestItemsDbContext>
    {
        [Fact]
        public async Task given_valid_images_should_add()
        {
            var images = _imagesDir.GetFiles("*.jpg");
            var multiPart = await CreateMultipartFormDataContent(images);

            var response = (await _client.Request(Path).PostAsync(multiPart, completionOption: HttpCompletionOption.ResponseHeadersRead));
            var content = await response.GetJsonAsync<List<string>>();
            
            var ids = content.Select(c => Guid.Parse(c.Split(Path + '/')[1]));
            var imgsDb = _dbContext.Images.AsQueryable();
            var imagesFromDb = (from id in ids
                          join image in imgsDb
                            on id equals image.Id.Value
                          select image).AsQueryable().AsNoTracking().ToList();
            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            imagesFromDb.Count.ShouldBe(images.Length);
        }

        [Fact]
        public async Task given_valid_image_should_add()
        {
            var images = _imagesDir.GetFiles("Samsung.jpg").First();
            var multiPart = await CreateMultipartFormDataContent(images);

            var response = (await _client.Request(Path).PostAsync(multiPart, completionOption: HttpCompletionOption.ResponseHeadersRead));
            var content = await response.GetJsonAsync<List<string>>();
            Guid.TryParse(content[0].Split(Path + '/')[1], out var id);

            var imageFromDb = await _dbContext.Images.Where(i => i.Id == id).SingleOrDefaultAsync();
            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            imageFromDb.ImageName.ShouldContain("Samsung");
        }

        [Fact]
        public async Task given_valid_id_should_return_image()
        {
            var images = _imagesDir.GetFiles("*.jpg");
            var multiPart = await CreateMultipartFormDataContent(images);
            var responseImagesAdded = (await _client.Request(Path).PostAsync(multiPart, completionOption: HttpCompletionOption.ResponseHeadersRead));
            var contentImagesAdded = await responseImagesAdded.GetJsonAsync<List<string>>();
            var ids = contentImagesAdded.Select(c => Guid.Parse(c.Split(Path + '/')[1]));
            var id = ids.First();

            var response = await _client.Request($"{Path}/{id}").GetAsync();
            var messageResponse = await response.GetJsonAsync<string>();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            messageResponse.ShouldContain("base64");
        }

        private async Task<MultipartFormDataContent> CreateMultipartFormDataContent(FileInfo[] fileInfos)
        {
            var multiContent = new MultipartFormDataContent();

            foreach (var fileInfo in fileInfos)
            {
                ByteArrayContent bytes = new ByteArrayContent(await GetBytes(fileInfo));
                multiContent.Add(bytes, "files", fileInfo.Name);
            }

            return multiContent;
        }

        private async Task<MultipartFormDataContent> CreateMultipartFormDataContent(FileInfo fileInfo)
        {
            var multiContent = new MultipartFormDataContent();
            ByteArrayContent bytes = new ByteArrayContent(await GetBytes(fileInfo));
            multiContent.Add(bytes, "files", fileInfo.Name);
            return multiContent;
        }

        private async Task<byte[]> GetBytes(FileInfo fileInfo)
        {
            var bytes = await File.ReadAllBytesAsync(fileInfo.FullName);
            return bytes;
        }

        private const string Path = "items-module/images";
        private readonly IFlurlClient _client;
        private readonly ItemsDbContext _dbContext;
        private readonly DirectoryInfo _imagesDir;

        public ImagesControllerTests(TestApplicationFactory<Program> factory, TestItemsDbContext dbContext)
        {
            _client = new FlurlClient(factory.CreateClient());
            _dbContext = dbContext.DbContext;
            _imagesDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.GetDirectories().Where(d => d.Name == "TestData").FirstOrDefault();
        }
    }
}
