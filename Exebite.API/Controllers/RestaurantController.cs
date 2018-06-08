﻿using System.Collections.Generic;
using Exebite.API.Models;
using Exebite.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exebite.API.Controllers
{
    [Produces("application/json")]
    [Route("api/restaurant")]
    [Authorize]
    public class RestaurantController : Controller
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var restaurants = _restaurantService.GetAllRestaurants();
            List<RestaurantViewModel> model = new List<RestaurantViewModel>();
            foreach (var restaurant in restaurants)
            {
                model.Add(new RestaurantViewModel { Id = restaurant.Id, Name = restaurant.Name });
            }

            return Ok(model);
        }

        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var restaurantFromDb = _restaurantService.GetRestaurantById(id);
            if (restaurantFromDb == null)
            {
                return BadRequest();
            }

            var restaurant = new RestaurantViewModel { Id = restaurantFromDb.Id, Name = restaurantFromDb.Name };
            return Ok(restaurant);
        }

        [HttpPost]
        public IActionResult Post([FromBody]string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return BadRequest();
            }

            var createdRestaurant = _restaurantService.CreateNewRestaurant(
                    new Model.Restaurant
                    {
                        Name = value,
                        DailyMenu = new List<Model.Food>(),
                        Foods = new List<Model.Food>()
                    });

            return Ok(createdRestaurant.Id);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return BadRequest();
            }

            var currentRestaurant = _restaurantService.GetRestaurantById(id);
            if (currentRestaurant == null)
            {
                return BadRequest();
            }

            currentRestaurant.Name = value;
            var updatedRestaurant = _restaurantService.UpdateRestaurant(currentRestaurant);

            return Ok(updatedRestaurant.Id);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _restaurantService.DeleteRestaurant(id);
            return NoContent();
        }
    }
}
