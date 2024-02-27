#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using basic_api.Data;
using basic_api.Models;
using Newtonsoft.Json;

namespace basic_api.Data
{
    public class DBInitializer
    {
        public static void Initialize(MockDatabaseContext context)
        {
            context.Database.EnsureCreated(); 

            if (context.Entities.Any())
            {
                // Database already seeded
                return;
            }

            var jsonData = File.ReadAllText("./Data/data.json");

            var entities = JsonConvert.DeserializeObject<List<Entity>>(jsonData);

            // Add entities to the context and save changes
            context.Entities.AddRange(entities);
            context.SaveChanges();
        }
    }
}