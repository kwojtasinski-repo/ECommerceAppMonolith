using ECommerce.Modules.Items.Tests.Integration.Common;
using ECommerce.Shared.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Modules.Items.Tests.Integration.Controllers
{
    [Collection("integrationItemSales")]
    public class ItemSalesControllerTests : IClassFixture<TestApplicationFactory<Program>>,
        IClassFixture<TestItemsDbContext>
    {
    }
}
