using ECommerce.Modules.Items.Infrastructure.EF.DAL;
using ECommerce.Shared.Tests;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Tests.Integration.Common
{
    public class TestItemsDbContext : IDisposable
    {
        internal ItemsDbContext DbContext { get; }

        public TestItemsDbContext()
        {
            DbContext = new ItemsDbContext(DbHelper.GetOptions<ItemsDbContext>());
        }

        public void Dispose()
        {
            EnsureFilesDeteled();
            DbContext?.Database.EnsureDeleted();
            DbContext?.Dispose();
        }

        private void EnsureFilesDeteled()
        {
            var images = DbContext.Images.ToList();

            if (images.Any())
            {
                foreach (var image in images)
                {
                    if (File.Exists(image.SourcePath))
                    {
                        File.Delete(image.SourcePath);
                    }

                    DbContext.Images.Remove(image);
                }
            }
        }
    }
}
