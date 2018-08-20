using System.Linq;
using System.Threading.Tasks;
using Exebite.Common;
using Exebite.DtoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebClient.Services;

namespace WebClient.Controllers
{
    [Authorize]
    public class CustomerAliasController : Controller
    {
        private readonly ICustomerAliasService _service;

        public CustomerAliasController(ICustomerAliasService service)
        {
            _service = service;
        }

        // GET: CustomerAlias
        public async Task<IActionResult> Index()
        {
            var queryDto = new CustomerAliasQueryDto()
            {
                Page = 1,
                Size = QueryConstants.MaxElements - 1
            };
            var authToken = User.Claims.FirstOrDefault(claim => claim.Type == "id_token").Value;
            var res = await _service.QueryAsync(queryDto, authToken).ConfigureAwait(false);
            return View(res.Items);
        }

        // GET: CustomerAlias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authToken = User.Claims.FirstOrDefault(claim => claim.Type == "id_token").Value;
            var customerAliasDto = await _service.QueryAsync(new CustomerAliasQueryDto { Id = id, Page = 1, Size = 1 }, authToken).ConfigureAwait(false);
            if (customerAliasDto.Total == 0)
            {
                return NotFound();
            }

            return View(customerAliasDto);
        }

        // GET: CustomerAlias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CustomerAlias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Alias,CustomerId,RestaurantId")] CustomerAliasDto model)
        {
            if (ModelState.IsValid)
            {
                var authToken = User.Claims.FirstOrDefault(claim => claim.Type == "id_token").Value;
                await _service.CreateAsync(new CreateCustomerAliasDto { Alias = model.Alias, CustomerId = model.CustomerId, RestaurantId = model.RestaurantId }, authToken).ConfigureAwait(false);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: CustomerAlias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authToken = User.Claims.FirstOrDefault(claim => claim.Type == "id_token").Value;
            var customerAliasDto = await _service.QueryAsync(new CustomerAliasQueryDto { Id = id, Page = 1, Size = 1 }, authToken).ConfigureAwait(false);
            if (customerAliasDto == null)
            {
                return NotFound();
            }
            return View(customerAliasDto);
        }

        // POST: CustomerAlias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Alias,CustomerId,RestaurantId")] CustomerAliasDto model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var authToken = User.Claims.FirstOrDefault(claim => claim.Type == "id_token").Value;
                    await _service.UpdateAsync(id, new UpdateCustomerAliasDto { Alias = model.Alias, CustomerId = model.CustomerId, RestaurantId = model.RestaurantId }, authToken).ConfigureAwait(false);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerAliasExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: CustomerAlias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authToken = User.Claims.FirstOrDefault(claim => claim.Type == "id_token").Value;
            var customerAliasDto = await _service.QueryAsync(new CustomerAliasQueryDto { Id = id, Page = 1, Size = 1 }, authToken).ConfigureAwait(false);
            if (customerAliasDto.Total == 0)
            {
                return NotFound();
            }

            return View(customerAliasDto);
        }

        // POST: CustomerAlias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var authToken = User.Claims.FirstOrDefault(claim => claim.Type == "id_token").Value;
            await _service.DeleteByIdAsync(id, authToken).ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerAliasExists(int id)
        {
            var authToken = User.Claims.FirstOrDefault(claim => claim.Type == "id_token").Value;
            return _service.QueryAsync(new CustomerAliasQueryDto { Id = id, Page = 1, Size = 1 }, authToken).Result.Total != 0;
        }
    }
}