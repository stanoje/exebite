using System.ComponentModel.DataAnnotations;

namespace Exebite.DomainModel
{
    public enum FoodType
    {
        [Display(Name = "Main course")]
        MAIN_COURSE,

        DESERT,
        SALAD,

        [Display(Name = "Side dish")]
        SIDE_DISH,

        SOUP,
        CONDIMENTS
    }
}
