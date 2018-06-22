﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Exebite.DataAccess.Context;
using Exebite.DataAccess.Entities;
using Exebite.DomainModel;
using Microsoft.Extensions.Logging;

namespace Exebite.DataAccess.Repositories
{
    public class CustomerRepository : DatabaseRepository<Customer, CustomerEntity, CustomerQueryModel>, ICustomerRepository
    {
        public CustomerRepository(IFoodOrderingContextFactory factory, IMapper mapper, ILogger<CustomerRepository> logger)
            : base(factory, mapper, logger)
        {
        }

        public Customer GetByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new System.ArgumentException("Name can't be empty string");
            }

            using (var context = _factory.Create())
            {
                var customerEntity = context.Customers.FirstOrDefault(ce => ce.Name == name);
                if (customerEntity != null)
                {
                    return _mapper.Map<Customer>(customerEntity);
                }
                else
                {
                    return null;
                }
            }
        }

        public override Customer Insert(Customer entity)
        {
            if (entity == null)
            {
                throw new System.ArgumentNullException(nameof(entity));
            }

            using (var context = _factory.Create())
            {
                // do not use automaper to add entites to DB
                var customerEntity = new CustomerEntity()
                {
                    AppUserId = entity.AppUserId,
                    Balance = entity.Balance,
                    LocationId = entity.LocationId,
                    Name = entity.Name
                };

                var resultEntity = context.Customers.Add(customerEntity);
                context.SaveChanges();
                return _mapper.Map<Customer>(resultEntity.Entity);
            }
        }

        public override Customer Update(Customer entity)
        {
            if (entity == null)
            {
                throw new System.ArgumentNullException(nameof(entity));
            }

            using (var context = _factory.Create())
            {
                var customerEntity = new CustomerEntity
                {
                    AppUserId = entity.AppUserId,
                    Balance = entity.Balance,
                    Id = entity.Id,
                    LocationId = entity.LocationId,
                    Name = entity.Name,
                };
                context.Update(customerEntity);
                context.SaveChanges();
                var resultEntity = context.Customers.FirstOrDefault(c => c.Id == entity.Id);
                return _mapper.Map<Customer>(resultEntity);
            }
        }

        public override IList<Customer> Query(CustomerQueryModel queryModel)
        {
            queryModel = queryModel ?? new CustomerQueryModel();

            using (var context = _factory.Create())
            {
                var query = context.Customers.AsQueryable();

                if (queryModel.Id != null)
                {
                    query = query.Where(x => x.Id == queryModel.Id.Value);
                }

                var results = query.ToList();
                return _mapper.Map<IList<Customer>>(results);
            }
        }
    }
}
