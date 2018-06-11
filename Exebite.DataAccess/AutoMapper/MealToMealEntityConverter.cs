﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Exebite.DataAccess.Entities;
using Exebite.DataAccess.Migrations;
using Exebite.Model;

namespace Exebite.DataAccess.AutoMapper
{
    public class MealToMealEntityConverter : IMealToMealEntityConverter
    {
        private readonly IFoodOrderingContextFactory _factory;
        private readonly IMapper _mapper;

        public MealToMealEntityConverter(IFoodOrderingContextFactory factory, IMapper mapper)
        {
            _factory = factory;
            _mapper = mapper;
        }

        public MealEntity Convert(Meal source, MealEntity destination, ResolutionContext context)
        {
            using (var dbContext = _factory.Create())
            {
                destination = new MealEntity();
                destination.Id = source.Id;
                destination.Price = source.Price;
                destination.Id = source.Id;
                destination.FoodEntityMealEntities = new List<FoodEntityMealEntities>();
                foreach (var food in source.Foods)
                {
                    var dbFoodMealEntity = dbContext.FoodEntityMealEntities.SingleOrDefault(fm => fm.FoodEntityId == food.Id && fm.MealEntityId == source.Id);
                    if (dbFoodMealEntity == null)
                    {
                        destination.FoodEntityMealEntities.Add(new FoodEntityMealEntities
                        {
                            FoodEntity = _mapper.Map<FoodEntity>(food),
                            FoodEntityId = food.Id,
                            MealEntityId = source.Id,
                            MealEntity = destination
                        });
                    }
                    else
                    {
                        destination.FoodEntityMealEntities.Add(dbFoodMealEntity);
                    }
                }

                return destination;
            }
        }
    }
}
