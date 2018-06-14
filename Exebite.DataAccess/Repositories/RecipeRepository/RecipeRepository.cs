﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Exebite.DataAccess.Context;
using Exebite.DataAccess.Entities;
using Exebite.Model;

namespace Exebite.DataAccess.Repositories
{
    public class RecipeRepository : DatabaseRepository<Recipe, RecipeEntity, RecipeQueryModel>, IRecipeRepository
    {
        public RecipeRepository(IFoodOrderingContextFactory factory, IMapper mapper)
            : base(factory, mapper)
        {
        }

        public List<Recipe> GetRecipesForFood(Food food)
        {
            if (food == null)
            {
                throw new System.ArgumentNullException(nameof(food));
            }

            using (var context = _factory.Create())
            {
                var entities = context.FoodEntityRecipeEntity.Where(fe => fe.FoodEntityId == food.Id).Select(r => r.RecipeEntity).ToList();
                return entities.Select(r => _mapper.Map<Recipe>(r)).ToList();
            }
        }

        public List<Recipe> GetRecipesForMainCourse(Food mainCourse)
        {
            if (mainCourse == null)
            {
                throw new System.ArgumentNullException(nameof(mainCourse));
            }

            using (var context = _factory.Create())
            {
                var entities = context.Recipes.Where(r => r.MainCourseId == mainCourse.Id).ToList();
                return entities.Select(r => _mapper.Map<Recipe>(r)).ToList();
            }
        }

        public override Recipe Insert(Recipe entity)
        {
            if (entity == null)
            {
                throw new System.ArgumentNullException(nameof(entity));
            }

            using (var context = _factory.Create())
            {
                var recipeEntity = _mapper.Map<RecipeEntity>(entity);
                var resultEntity = context.Attach(recipeEntity).Entity;
                context.SaveChanges();
                return _mapper.Map<Recipe>(resultEntity);
            }
        }

        public override IList<Recipe> Query(RecipeQueryModel queryModel)
        {
            if (queryModel == null)
            {
                throw new System.ArgumentException("queryModel can't be null");
            }

            using (var context = _factory.Create())
            {
                var query = context.Recipes.AsQueryable();

                if (queryModel.Id != null)
                {
                    query = query.Where(x => x.Id == queryModel.Id.Value);
                }

                var results = query.ToList();
                return _mapper.Map<IList<Recipe>>(results);
            }
        }

        public override Recipe Update(Recipe entity)
        {
            if (entity == null)
            {
                throw new System.ArgumentNullException(nameof(entity));
            }

            using (var context = _factory.Create())
            {
                var recipeEntity = _mapper.Map<RecipeEntity>(entity);
                foreach (var fre in recipeEntity.FoodEntityRecipeEntities)
                {
                    context.Attach(fre);
                }

                var old = context.Recipes.Find(entity.Id);
                context.Entry(old).CurrentValues.SetValues(recipeEntity);
                context.SaveChanges();

                var resultEntity = context.Recipes.FirstOrDefault(r => r.Id == entity.Id);
                return _mapper.Map<Recipe>(resultEntity);
            }
        }
    }
}