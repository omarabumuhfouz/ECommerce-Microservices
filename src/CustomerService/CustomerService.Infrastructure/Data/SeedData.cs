// using Microsoft.Extensions.DependencyInjection;

// namespace CustomerService.Infrastructure.Data;

// public static class SeedData
// {
//     public static async Task SeedAsync(IServiceProvider services)
//     {
//         using var scope = services.CreateScope();
//         var db = scope.ServiceProvider.GetRequiredService<CustomerDbContext>();

//         await db.Database.EnsureCreatedAsync();

//         if (!await db.Customers.AnyAsync())
//         {
//             Console.WriteLine("DEBUG: ABout to check if customers exist..");

//             var customers = new List<Customer>();

//             // Example real customers
//             var customerData = new[]
//             {
//             new { FirstName="Omar", LastName="Abumuhfouz", Phone="+96270000101" },
//             new { FirstName="Sara", LastName="Al-Hassan", Phone="+96270000102" },
//             new { FirstName="Ahmad", LastName="Khalil", Phone="+96270000103" },
//             new { FirstName="Lina", LastName="Mahmoud", Phone="+96270000104" },
//             new { FirstName="Khaled", LastName="Al-Tamimi", Phone="+96270000105" },
//             new { FirstName="Mona", LastName="Jaber", Phone="+96270000106" },
//             new { FirstName="Yousef", LastName="Saleh", Phone="+96270000107" },
//             new { FirstName="Rana", LastName="Hussein", Phone="+96270000108" },
//             new { FirstName="Fadi", LastName="Nasser", Phone="+96270000109" },
//             new { FirstName="Aya", LastName="Samir", Phone="+96270000110" },
//             new { FirstName="Tariq", LastName="Zaid", Phone="+96270000111" },
//             new { FirstName="Reem", LastName="Naji", Phone="+96270000112" },
//             new { FirstName="Omar", LastName="Hamed", Phone="+96270000113" },
//             new { FirstName="Salma", LastName="Fahad", Phone="+96270000114" },
//             new { FirstName="Mohammed", LastName="Sami", Phone="+96270000115" },
//         };

//             foreach (var c in customerData)
//             {
//                 var customer = Customer.Create(Guid.NewGuid(), Guid.NewGuid(), c.FirstName, c.LastName, c.Phone).Value;

//                 // Example addresses for some customers
//                 var addresses = new List<Address>();

//                 if (c.FirstName == "Omar")
//                 {
//                     addresses.Add(Address.Create(Guid.NewGuid(), customer.Id, "123 King Abdullah Street", "Apt 4B", "Amman", "Amman", "11118", "Jordan", true).Value);
//                     addresses.Add(Address.Create(Guid.NewGuid(), customer.Id, "456 Queen Rania Street", "Floor 2", "Amman", "Amman", "11119", "Jordan").Value);
//                 }
//                 else if (c.FirstName == "Sara")
//                 {
//                     addresses.Add(Address.Create(Guid.NewGuid(), customer.Id, "7 Al-Madina Street", "Building 3", "Zarqa", "Zarqa", "21321", "Jordan", true).Value);
//                 }
//                 else if (c.FirstName == "Ahmad")
//                 {
//                     addresses.Add(Address.Create(Guid.NewGuid(), customer.Id, "21 Petra Road", "Tests", "Amman", "Amman", "11122", "Jordan", true).Value);
//                     addresses.Add(Address.Create(Guid.NewGuid(), customer.Id, "45 King Hussein Street", "Tests", "Amman", "Amman", "11123", "Jordan").Value);
//                     addresses.Add(Address.Create(Guid.NewGuid(), customer.Id, "78 Abdali Street", "Tests", "Amman", "Amman", "11124", "Jordan").Value);
//                 }

//                 foreach (var addr in addresses)
//                 {
//                     customer.AddAddress(addr);
//                 }

//                 customers.Add(customer);
//             }

//             await db.Customers.AddRangeAsync(customers);
//             await db.SaveChangesAsync();
//         }
//         else
//         {
//             Console.WriteLine("DEBUG: Customer already exist . skipping see.");
//         }
//     }
// }