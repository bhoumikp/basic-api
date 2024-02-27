using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using basic_api.Interfaces;
using basic_api.Data;
using basic_api.Models;
using basic_api.Repository;

namespace basic_api.Tests
{

    public class RetryTests
    {
        private readonly Entity _entity;
        private readonly EntityRepository _entityRepo;

        public RetryTests()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = config.GetConnectionString("DefaultConnection");
            var options = new DbContextOptionsBuilder<MockDatabaseContext>()
                .UseSqlServer(connectionString)
                .Options;

            var context = new MockDatabaseContext(options);
            _entityRepo = new EntityRepository(context);

            var address = new Address{AddressLine= "81160 Margaret Streets",
                                        City= "Melissashire",
                                        Country= "British Indian Ocean Territory (Chagos Archipelago)"};
            var dates = new Date {DateType= "Birth",
                                    DateValue= DateTime.Parse("1969-10-30")};
            var names = new Name {FirstName= "Kristi",
                                    MiddleName= "Matthew",
                                    Surname= "Wilkerson"};
            _entity = new Entity{Id= "1",
                                    Deceased= true,
                                    Gender= "Male",
                                    Addresses= [address],
                                    Dates = [dates],
                                    Names = [names]};
        }

        [Fact]
        public async Task CreateEntity_OnDuplicateKeyError_ShouldAddNewId() 
        {
            // Act
            var createdEntity = await _entityRepo.CreateEntityAsync(_entity);

            // Assert
            Assert.NotNull(createdEntity);
            Assert.Equal(_entity, createdEntity);
        }

    }
}