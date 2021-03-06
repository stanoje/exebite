﻿using System;

namespace Exebite.DomainModel
{
    public class Order
    {
        public int Id { get; set; }

        public decimal Price { get; set; }

        public DateTime Date { get; set; }

        public int MealId { get; set; }

        public Meal Meal { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        public string Note { get; set; }
    }
}
