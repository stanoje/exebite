﻿using System.Linq;
using Exebite.DataAccess.Context;
using Exebite.DataAccess.Entities;
using Exebite.Model;

namespace Exebite.DataAccess.Repositories
{
    public class LocationRepository : DatabaseRepository<Location,LocationEntity>, ILocationRepository
    {
        IFoodOrderingContextFactory _factory;
        public LocationRepository(IFoodOrderingContextFactory factory)
            :base(factory)
        {
            this._factory = factory;
        }
        

        public override Location Insert(Location entity)
        {
            using (var context = _factory.Create())
            {
                var locEntity = AutoMapperHelper.Instance.GetMappedValue<LocationEntity>(entity);
                var resultEntity = context.Locations.Add(locEntity);
                var result = AutoMapperHelper.Instance.GetMappedValue<Location>(resultEntity);
                return result;
            }
        }
        
        public override Location Update(Location entity)
        {
            using (var context = _factory.Create())
            {
                var locationEntity = AutoMapperHelper.Instance.GetMappedValue<LocationEntity>(entity);
                var oldLocationEntry = context.Locations.FirstOrDefault(l => l.Id == entity.Id);
                context.Entry(oldLocationEntry).CurrentValues.SetValues(locationEntity);
                context.SaveChanges();
                var resultEntry = context.Locations.FirstOrDefault(l => l.Id == entity.Id);
                var result = AutoMapperHelper.Instance.GetMappedValue<Location>(resultEntry);
                return result;
            }
        }
    }
}
