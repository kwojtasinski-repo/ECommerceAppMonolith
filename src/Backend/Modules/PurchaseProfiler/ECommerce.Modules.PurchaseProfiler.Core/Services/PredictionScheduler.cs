using ECommerce.Modules.PurchaseProfiler.Core.Entities;
using ECommerce.Modules.PurchaseProfiler.Core.Repositories;
using ECommerce.Shared.Abstractions.SchedulerJobs;
using System.Globalization;

namespace ECommerce.Modules.PurchaseProfiler.Core.Services
{
    internal class PredictionScheduler
        (
            IUserRepository userRepository,
            IWeekPredictionRepository weekPredictionRepository,
            IComputePredictionRepository computePredictionRepository
        )
        : ISchedulerTask<PredictionScheduler>
    {
        public async Task DoWork(CancellationToken cancellationToken)
        {
            var paginatedUsers = userRepository.GetPaginatedUsers(50);
            List<User> users;
            var currentDate = DateTime.UtcNow;
            var generateNewPredictions = new List<ComputePrediction>();

            while ((users = await paginatedUsers.GetNextAsync()).Count != 0)
            {
                foreach (var user in users)
                {
                    var weekNumber = ISOWeek.GetWeekOfYear(currentDate);
                    if (await weekPredictionRepository.GetByYearWeekNumberAndUserIdAsync(currentDate.Year, weekNumber, user.UserId)
                        is not null)
                    {
                        continue;
                    }

                    generateNewPredictions.Add(new ComputePrediction
                    {
                        UserId = user.UserId,
                        Year = currentDate.Year,
                        WeekNumber = weekNumber
                    });
                }

                await computePredictionRepository.AddRangeAsync(generateNewPredictions);
            }
        }
    }
}
