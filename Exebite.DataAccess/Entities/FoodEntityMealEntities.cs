﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exebite.DataAccess.Entities
{
    [Table(nameof(FoodEntityMealEntities))]
    public class FoodEntityMealEntities
    {
        [Key]
        public int Id { get; set; }

        public int FoodEntityId { get; set; }

        [ForeignKey("FoodEntityId")]
        public virtual FoodEntity FoodEntity { get; set; }

        public int MealEntityId { get; set; }

        [ForeignKey("MealEntityId")]
        public virtual MealEntity MealEntity { get; set; }
    }
}