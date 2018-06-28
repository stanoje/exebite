﻿using System;
using System.Collections.Generic;
using System.Linq;
using Exebite.DataAccess.Entities;
using Exebite.DataAccess.Repositories;
using Exebite.DataAccess.Test.Mocks;
using Exebite.DomainModel;
using Microsoft.Data.Sqlite;
using static Exebite.DataAccess.Test.RepositoryTestHelpers;

namespace Exebite.DataAccess.Test
{
    public sealed class RestaurantQueryRepositoryTest : QueryRepositoryTests<RestaurantQueryRepositoryTest.Data, Restaurant, RestaurantQueryModel>
    {
        protected override IEnumerable<Data> SampleData =>
            Enumerable.Range(1, int.MaxValue).Select(content => new Data
            {
                Id = content,
                Name = $"Name {content}"
            });

        protected override RestaurantQueryModel ConvertEmptyToQuery()
        {
            return new RestaurantQueryModel();
        }

        protected override RestaurantQueryModel ConvertNullToQuery()
        {
#pragma warning disable RETURN0001 // Do not return null
            return null;
#pragma warning restore RETURN0001 // Do not return null
        }

        protected override RestaurantQueryModel ConvertToQuery(Data data)
        {
            return new RestaurantQueryModel
            {
                Id = data.Id,
                Name = data.Name
            };
        }

        protected override RestaurantQueryModel ConvertToQuery(int id)
        {
            return new RestaurantQueryModel { Id = id };
        }

        protected override RestaurantQueryModel ConvertWithPageAndSize(int page, int size)
        {
            return new RestaurantQueryModel(page, size);
        }

        protected override IDatabaseQueryRepository<Restaurant, RestaurantQueryModel> CreateSut(SqliteConnection connection)
        {
            return CreateOnlyRestaurantQueryRepositoryInstanceNoData(connection);
        }

        protected override int GetId(Restaurant result)
        {
            return result.Id;
        }

        protected override void InitializeStorage(SqliteConnection connection, int count)
        {
            using (var context = new InMemoryDBFactory(connection).Create())
            {
                var locations = Enumerable.Range(1, count).Select(x => new RestaurantEntity()
                {
                    Id = x,
                    Name = $"Name {x}",
                    DailyMenu = new DailyMenuEntity()
                    {
                        Id = x
                    }
                });

                context.Restaurants.AddRange(locations);
                context.SaveChanges();
            }
        }

        public sealed class Data
        {
            public int? Id { get; set; }

            public string Name { get; set; }
        }
    }
}
