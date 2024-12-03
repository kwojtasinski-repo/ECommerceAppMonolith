using ECommerce.Modules.PurchaseProfiler.Api.Entities;

namespace ECommerce.Modules.PurchaseProfiler.Api.Data
{
    public static class StaticData
    {
        public static List<Item> Items = new List<Item>
        {
            new Item { Id = Guid.NewGuid(), ItemName = "Laptop A", Description = "High-performance laptop", Brand = "BrandX", Type = "Electronics", Price = 1200, IsActive = true },
            new Item { Id = Guid.NewGuid(), ItemName = "Smartphone B", Description = "Latest smartphone", Brand = "BrandY", Type = "Electronics", Price = 800, IsActive = true },
            new Item { Id = Guid.NewGuid(), ItemName = "Headphones C", Description = "Noise-cancelling headphones", Brand = "BrandZ", Type = "Accessories", Price = 200, IsActive = true },
            new Item { Id = Guid.NewGuid(), ItemName = "Backpack D", Description = "Durable travel backpack", Brand = "BrandA", Type = "Accessories", Price = 100, IsActive = true }
        };

        public static List<Customer> Customers = new List<Customer>
        {
            new Customer { Id = Guid.Parse("3b0bc258-6778-4c1c-8f3a-00b0178cd597"), FirstName = "Alice", LastName = "Smith", PurchasedItemIds = new List<Guid> { Items[0].Id } },
            new Customer { Id = Guid.Parse("54a8ce80-670f-4c87-be06-65cbc38729ee"), FirstName = "Bob", LastName = "Brown", PurchasedItemIds = new List<Guid> { Items[1].Id, Items[2].Id } }
        };

        public static List<(Guid CustomerId, Guid ItemId, int PurchaseCount, DateTime LastPurchaseDate)> Sales = new List<(Guid, Guid, int, DateTime)>
        {
            // Customer 0 (Alice) purchases
            (Customers[0].Id, Items[0].Id, 2, new DateTime(2024, 07, 12)),
            (Customers[0].Id, Items[1].Id, 1, new DateTime(2024, 08, 15)),
            (Customers[0].Id, Items[1].Id, 1, new DateTime(2024, 09, 15)),
            (Customers[0].Id, Items[1].Id, 1, new DateTime(2024, 10, 14)),
            (Customers[0].Id, Items[3].Id, 1, new DateTime(2024, 10, 20)), // Additional purchase
            (Customers[0].Id, Items[2].Id, 1, new DateTime(2024, 11, 02)), // Additional purchase

            // Customer 1 (Bob) purchases
            (Customers[1].Id, Items[2].Id, 3, new DateTime(2024, 10, 22)),
            (Customers[1].Id, Items[3].Id, 1, new DateTime(2024, 11, 10)),
            (Customers[1].Id, Items[3].Id, 1, new DateTime(2024, 11, 12)),
            (Customers[1].Id, Items[0].Id, 2, new DateTime(2024, 11, 18)), // Additional purchase
            (Customers[1].Id, Items[1].Id, 1, new DateTime(2024, 11, 25)) // Additional purchase
        };

        public static List<WeeklyCustomerData> CustomerWeeklyData = new List<WeeklyCustomerData>
        {
            // Customer 0 (Alice) weekly data
            new WeeklyCustomerData { CustomerId = Customers[0].Id, WeekNumber = 28, PurchaseFrequency = 1, TotalSpent = 2400, DaysSinceLastPurchase = 7 },
            new WeeklyCustomerData { CustomerId = Customers[0].Id, WeekNumber = 33, PurchaseFrequency = 1, TotalSpent = 800, DaysSinceLastPurchase = 7 },
            new WeeklyCustomerData { CustomerId = Customers[0].Id, WeekNumber = 38, PurchaseFrequency = 1, TotalSpent = 800, DaysSinceLastPurchase = 7 },
            new WeeklyCustomerData { CustomerId = Customers[0].Id, WeekNumber = 42, PurchaseFrequency = 2, TotalSpent = 1200, DaysSinceLastPurchase = 3 }, // Two purchases in one week
            new WeeklyCustomerData { CustomerId = Customers[0].Id, WeekNumber = 44, PurchaseFrequency = 1, TotalSpent = 200, DaysSinceLastPurchase = 7 }, // Headphones purchase

            // Customer 1 (Bob) weekly data
            new WeeklyCustomerData { CustomerId = Customers[1].Id, WeekNumber = 43, PurchaseFrequency = 3, TotalSpent = 600, DaysSinceLastPurchase = 2 },
            new WeeklyCustomerData { CustomerId = Customers[1].Id, WeekNumber = 46, PurchaseFrequency = 2, TotalSpent = 1600, DaysSinceLastPurchase = 3 },
            new WeeklyCustomerData { CustomerId = Customers[1].Id, WeekNumber = 47, PurchaseFrequency = 1, TotalSpent = 800, DaysSinceLastPurchase = 7 } // Smartphone purchase
        };

        public static List<WeeklyCustomerDataInput> WeeklyCustomerDataList = new List<WeeklyCustomerDataInput>
        {
             // Dane dla Alice
            new WeeklyCustomerDataInput
            {
                CustomerId = "3b0bc258-6778-4c1c-8f3a-00b0178cd597",  // Alice
                WeekNumber = 48, // aktualny tydzień
                PurchaseFrequency = 2,
                TotalSpent = 1800,
                DaysSinceLastPurchase = 5,
                PurchasedItemIds = new List<int> { 0, 1 }
            },
            new WeeklyCustomerDataInput
            {
                CustomerId = "3b0bc258-6778-4c1c-8f3a-00b0178cd597",  // Alice
                WeekNumber = 47,
                PurchaseFrequency = 1,
                TotalSpent = 800,
                DaysSinceLastPurchase = 7,
                PurchasedItemIds = new List<int> { 2 } // IDs of purchased items
            },

            // Dane dla Boba
            new WeeklyCustomerDataInput
            {
                CustomerId = "54a8ce80-670f-4c87-be06-65cbc38729ee",  // Bob
                WeekNumber = 48,
                PurchaseFrequency = 3,
                TotalSpent = 1400,
                DaysSinceLastPurchase = 2,
                PurchasedItemIds = new List<int> { 3, 2, 1 } // IDs of purchased items
            },
            new WeeklyCustomerDataInput
            {
                CustomerId = "54a8ce80-670f-4c87-be06-65cbc38729ee",  // Bob
                WeekNumber = 47,
                PurchaseFrequency = 2,
                TotalSpent = 1200,
                DaysSinceLastPurchase = 3,
                PurchasedItemIds = new List<int> { 0, 2 } // IDs of purchased items
            },

            // Dane dla nowego klienta - Charlie
            new WeeklyCustomerDataInput
            {
                CustomerId = "66a8ce80-550f-4c87-be06-65cbc38729ef",  // Charlie
                WeekNumber = 48,
                PurchaseFrequency = 1,
                TotalSpent = 200,
                DaysSinceLastPurchase = 14,
                PurchasedItemIds = new List<int> { 3 } // IDs of purchased items
            },
            new WeeklyCustomerDataInput
            {
                CustomerId = "66a8ce80-550f-4c87-be06-65cbc38729ef",  // Charlie
                WeekNumber = 47,
                PurchaseFrequency = 0,
                TotalSpent = 0,
                DaysSinceLastPurchase = 21,
                PurchasedItemIds = new List<int>() // No purchases in this week
            },

            // Dane dla Diany
            new WeeklyCustomerDataInput
            {
                CustomerId = "89a8cf50-650f-4d87-be06-65cbe38729ee",  // Diana
                WeekNumber = 48,
                PurchaseFrequency = 4,
                TotalSpent = 2500,
                DaysSinceLastPurchase = 1,
                PurchasedItemIds = new List<int> { 3, 0, 2, 1 } // IDs of purchased items
            },
            new WeeklyCustomerDataInput
            {
                CustomerId = "89a8cf50-650f-4d87-be06-65cbe38729ee",  // Diana
                WeekNumber = 47,
                PurchaseFrequency = 2,
                TotalSpent = 1400,
                DaysSinceLastPurchase = 3,
                PurchasedItemIds = new List<int> { 2, 1 } // IDs of purchased items
            }
        };

        public class WeeklyCustomerDataInput
        {
            public string CustomerId { get; set; }
            public int WeekNumber { get; set; }
            public int PurchaseFrequency { get; set; }
            public float TotalSpent { get; set; }
            public int DaysSinceLastPurchase { get; set; }
            public List<int> PurchasedItemIds { get; set; }
        }

        public class WeeklyCustomerDataInput2
        {
            public long CustomerId { get; set; }
            public int WeekNumber { get; set; }
            public int PurchaseFrequency { get; set; }
            public float TotalSpent { get; set; }
            public int DaysSinceLastPurchase { get; set; }
            public long ItemId { get; set; }
            public float Label { get; set; }  // Może to być np. PurchaseFrequency
        }

        public class ProductRecommendationPrediction
        {
            public Guid CustomerId { get; set; }
            public Guid ItemId { get; set; }
            public float Score { get; set; }  // Predicted probability of purchase
        }
    }
}
