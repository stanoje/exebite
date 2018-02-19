﻿using Exebite.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exebite.Business
{
    public interface IMenuService
    {
        List<Restaurant> GetRestorantsWithMenus();
        int CheckPrice(Meal meal);
        List<Food> CheckAvailableSideDishes(int foodId);
    }
}
