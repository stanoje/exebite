﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Either;
using Exebite.Business;
using Exebite.Common;
using Exebite.DataAccess.Context;
using Exebite.DataAccess.Entities;
using Exebite.DataAccess.Repositories;
using Exebite.DomainModel;

namespace FeatureTestingConsole
{
    public sealed class App : IApp
    {
        private readonly IOrderCommandRepository _orderCommandRepo;
        private readonly IRestaurantQueryRepository _restaurantQueryRepository;
        private readonly IRestaurantCommandRepository _restaurantCommandRepository;
        private readonly IFoodCommandRepository _foodCommandRepository;
        private readonly IRoleCommandRepository _roleCommandRepo;
        private readonly IFoodQueryRepository _foodQueryRepository;
        private readonly ICustomerCommandRepository _customerCommandRepo;
        private readonly ILocationCommandRepository _locationCommandRepo;
        private readonly IDailyMenuCommandRepository _dailyMenuCommandRepo;
        private readonly IMealCommandRepository _mealCommandRepo;
        private readonly IFoodOrderingContextFactory _factory;
        private readonly IMapper _mapper;

        public App(
            IOrderCommandRepository orderCommandRepo,
            IRestaurantQueryRepository restaurantQueryRepo,
            IRestaurantCommandRepository restaurantCommandRepo,
            ICustomerCommandRepository customerCommandRepo,
            ILocationCommandRepository locationCommandRepo,
            IMealCommandRepository mealCommandRepo,
            IFoodOrderingContextFactory factory,
            IDailyMenuCommandRepository dailyMenuCommand,
            IMapper mapper,
            IFoodQueryRepository foodQueryRepository,
            IFoodCommandRepository foodCommandRepository,
            IRoleCommandRepository roleCommandRepo)
        {
            _orderCommandRepo = orderCommandRepo;
            _restaurantQueryRepository = restaurantQueryRepo;
            _restaurantCommandRepository = restaurantCommandRepo;
            _customerCommandRepo = customerCommandRepo;
            _locationCommandRepo = locationCommandRepo;
            _mealCommandRepo = mealCommandRepo;
            _factory = factory;
            _dailyMenuCommandRepo = dailyMenuCommand;
            _mapper = mapper;
            _foodQueryRepository = foodQueryRepository;
            _foodCommandRepository = foodCommandRepository;
            _roleCommandRepo = roleCommandRepo;
        }

        public void Run(string[] args)
        {
            ResetDatabase();

            // liefs
            SeedRestaurant();
            SeedLocation();
            SeedRole();

            SeedCustomer(); // restaurant related
            SeedPayment();
            SeedFoods();
            SeedMeal();

            SeedDailyMenu();

            var restaurant = _restaurantQueryRepository.Query(new RestaurantQueryModel { Id = 1 })
                                                       .Map(x => x.Items.First())
                                                       .Reduce(_ => throw new Exception());

            _restaurantCommandRepository.Update(restaurant.Id, _mapper.Map<RestaurantUpdateModel>(restaurant));

            var order = new OrderInsertModel()
            {
                CustomerId = 1,
                MealId = 1,
                Note = "Test insert"
            };

            this._orderCommandRepo.Insert(order);
        }

        private void SeedRole()
        {
            _roleCommandRepo.Insert(new RoleInsertModel { Name = "Admin" });
            _roleCommandRepo.Insert(new RoleInsertModel { Name = "User" });
        }

        private void SeedPayment()
        {
            using (var dc = _factory.Create())
            {
                dc.Payment.Add(new PaymentEntity()
                {
                    Amount = 2000,
                    CustomerId = 1,
                });
                dc.SaveChanges();
            }
        }

        private void ResetDatabase()
        {
            using (var dc = _factory.Create())
            {
                dc.Database.EnsureDeleted();
                dc.Database.EnsureCreated();
            }
        }

        private void SeedCustomer()
        {
            _customerCommandRepo.Insert(new CustomerInsertModel
            {
                Name = "Admin customer",
                GoogleUserId = "AdminGoogleId",
                Balance = 2000m,
                LocationId = 1,
                RoleId = 1
            });

            _customerCommandRepo.Insert(new CustomerInsertModel
            {
                Name = "User customer",
                GoogleUserId = "UserGoogleId",
                Balance = -400m,
                LocationId = 2,
                RoleId = 2
            });
        }

        private void SeedLocation()
        {
            _locationCommandRepo.Insert(new LocationInsertModel()
            {
                Name = "Execom VS",
                Address = "Vojvode stepe 50"
            });

            _locationCommandRepo.Insert(new LocationInsertModel()
            {
                Name = "Execom JD",
                Address = "Jovana ducica 50"
            });
        }

        private void SeedRestaurant()
        {
            _restaurantCommandRepository.Insert(new RestaurantInsertModel()
            {
                Name = "Lipa restaurant"
            });

            _restaurantCommandRepository.Insert(new RestaurantInsertModel()
            {
                Name = "Hedone restaurant"
            });
        }

        private void SeedFoods()
        {
            // Lipa
            _foodCommandRepository.Insert(new FoodInsertModel()
            {
                Name = "Supa",
                Price = 100,
                RestaurantId = 1,
                Type = FoodType.SOUP
            });

            // lipa
            _foodCommandRepository.Insert(new FoodInsertModel()
            {
                Name = "Kaubojska pasta",
                Price = 100,
                RestaurantId = 1,
                Type = FoodType.MAIN_COURSE
            });

            // Hedone
            _foodCommandRepository.Insert(new FoodInsertModel()
            {
                Name = "Ramstek",
                Price = 400,
                RestaurantId = 2,
                Type = FoodType.MAIN_COURSE
            });

            // Hedone
            _foodCommandRepository.Insert(new FoodInsertModel()
            {
                Name = "Princes krofna",
                Price = 100,
                RestaurantId = 2,
                Type = FoodType.DESERT
            });
        }

        private void SeedMeal()
        {
            _mealCommandRepo.Insert(new MealInsertModel()
            {
                Foods = new List<int>() { 1, 2 },
                Price = 400m
            });
        }

        private void SeedDailyMenu()
        {
            _dailyMenuCommandRepo.Insert(new DailyMenuInsertModel()
            {
                Foods = new List<Food>()
                {
                    new Food()
                    {
                        Id = 1
                    },
                    new Food()
                    {
                        Id = 2
                    }
                },
                RestaurantId = 1
            });

            _dailyMenuCommandRepo.Insert(new DailyMenuInsertModel()
            {
                Foods = new List<Food>()
                {
                    new Food()
                    {
                        Id = 1
                    }
                },
                RestaurantId = 2
            });
        }
    }
}
