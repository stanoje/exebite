﻿using System.Collections.Generic;
using AutoMapper;
using Exebite.API.Models;
using Exebite.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Exebite.API.Controllers
{
    [Produces("application/json")]
    [Route("api/customeraliases")]
    public class CustomerAliasesController : Controller
    {
        private readonly ICustomerAliasRepository _customerAliasRepository;
        private readonly IMapper _mapper;

        public CustomerAliasesController(ICustomerAliasRepository customerAliasesRepository, IMapper mapper)
        {
            _mapper = mapper;
            _customerAliasRepository = customerAliasesRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var customerAliases = _mapper.Map<IEnumerable<MealModel>>(_customerAliasRepository.Get(0, int.MaxValue));
            return Ok(customerAliases);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var customerAlias = _customerAliasRepository.GetByID(id);
            if (customerAlias == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CustomerAliasModel>(customerAlias));
        }

        [HttpPost]
        public IActionResult Post([FromBody]CreateCustomerAliasModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var createdCustomerAlias = _customerAliasRepository.Insert(_mapper.Map<Model.CustomerAliases>(model));
            return Ok(new { createdCustomerAlias.Id });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UpdateCustomerAliasModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var currentCustomerAlias = _customerAliasRepository.GetByID(id);
            if (currentCustomerAlias == null)
            {
                return NotFound();
            }

            _mapper.Map(model, currentCustomerAlias);

            var updatedMeal = _customerAliasRepository.Update(currentCustomerAlias);
            return Ok(new { updatedMeal.Id });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _customerAliasRepository.Delete(id);
            return NoContent();
        }

        [HttpGet("Query")]
        public IActionResult Query(CustomerAliasQueryModel query)
        {
            var locations = _customerAliasRepository.Query(query);
            return Ok(_mapper.Map<IEnumerable<CustomerAliasModel>>(locations));
        }
    }
}