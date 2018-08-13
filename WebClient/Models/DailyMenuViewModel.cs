using System.Collections.Generic;
using Exebite.DomainModel;
using Exebite.DtoModels;

namespace WebClient.Models
{
    public class DailyMenuViewModel
    {
        public List<FoodDto> foods;

        public FoodType FoodType { get; set; }

        public int RestaurantId { get; set; }

        public int Id { get; set; }
    }
}
