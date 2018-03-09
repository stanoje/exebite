﻿using System.Linq;
using Exebite.DataAccess.Entities;
using Exebite.DataAccess.Migrations;
using Exebite.Model;

namespace Exebite.DataAccess.Repositories
{
    public class LocationRepository : DatabaseRepository<Location, LocationEntity>, ILocationRepository
    {
        private IFoodOrderingContextFactory _factory;

        public LocationRepository(IFoodOrderingContextFactory factory)
            : base(factory)
        {
            this._factory = factory;
        }

        public override Location Insert(Location entity)
        {
            using (var context = _factory.Create())
            {
                var locEntity = AutoMapperHelper.Instance.GetMappedValue<LocationEntity>(entity);
                var resultEntity = context.Locations.Update(locEntity).Entity;
                context.SaveChanges();
                var result = AutoMapperHelper.Instance.GetMappedValue<Location>(resultEntity);
                return result;
            }
        }

        public override Location Update(Location entity)
        {
            using (var context = _factory.Create())
            {
                var locationEntity = AutoMapperHelper.Instance.GetMappedValue<LocationEntity>(entity);
                context.Attach(locationEntity);
                context.SaveChanges();
                var resultEntry = context.Locations.FirstOrDefault(l => l.Id == entity.Id);
                var result = AutoMapperHelper.Instance.GetMappedValue<Location>(resultEntry);
                return result;
            }
        }
    }
}
