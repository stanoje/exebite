using System.Linq;
using System.Threading.Tasks;
using Exebite.DtoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebClient.Models;
using WebClient.Services;

namespace WebClient.Controllers
{
    [Authorize]
    public class LocationController : Controller
    {
        private readonly ILocationService _service;

        public LocationController(ILocationService service)
        {
            _service = service;
        }

        // GET: Location
        public async Task<IActionResult> Index(int page = 1)
        {
            var pageSize = 3;
            var queryDto = new LocationQueryDto()
            {
                Page = page,
                Size = pageSize
            };
            var authToken = User.Claims.FirstOrDefault(claim => claim.Type == "id_token").Value;
            var res = await _service.QueryAsync(queryDto, authToken).ConfigureAwait(false);

            var pagination = new PaginatedList<LocationDto>(res.Items.ToList(), res.Total, page, pageSize);

            return View(pagination);
        }

        // GET: Location/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var authToken = User.Claims.FirstOrDefault(claim => claim.Type == "id_token").Value;
            var locationDto = await _service.QueryAsync(new LocationQueryDto { Id = id, Page = 1, Size = 1 }, authToken).ConfigureAwait(false);
            if (locationDto.Total == 0)
            {
                return NotFound();
            }

            return View(locationDto.Items.First());
        }

        // GET: Location/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Location/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Address")] LocationDto model)
        {
            if (ModelState.IsValid)
            {
                var authToken = User.Claims.FirstOrDefault(claim => claim.Type == "id_token").Value;
                await _service.CreateAsync(new CreateLocationDto { Name = model.Name, Address = model.Address }, authToken).ConfigureAwait(false);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Location/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authToken = User.Claims.FirstOrDefault(claim => claim.Type == "id_token").Value;
            var locationDto = await _service.QueryAsync(new LocationQueryDto { Id = id, Page = 1, Size = 1 }, authToken).ConfigureAwait(false);
            if (locationDto.Total == 0)
            {
                return NotFound();
            }
            return View(locationDto.Items.First());
        }

        // POST: Location/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address")] LocationDto model)
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
                    await _service.UpdateAsync(id, new UpdateLocationDto { Name = model.Name, Address = model.Address }, authToken).ConfigureAwait(false);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(model.Id))
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

        // GET: Location/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var authToken = User.Claims.FirstOrDefault(claim => claim.Type == "id_token").Value;
            var locationDto = await _service.QueryAsync(new LocationQueryDto { Id = id, Page = 1, Size = 1 }, authToken).ConfigureAwait(false);
            if (locationDto.Total == 0)
            {
                return NotFound();
            }

            return View(locationDto.Items.First());
        }

        // POST: Location/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var authToken = User.Claims.FirstOrDefault(claim => claim.Type == "id_token").Value;
            await _service.DeleteByIdAsync(id, authToken).ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }

        private bool LocationExists(int id)
        {
            var authToken = User.Claims.FirstOrDefault(claim => claim.Type == "id_token").Value;
            return _service.QueryAsync(new LocationQueryDto { Id = id, Page = 1, Size = 1 }, authToken).Result.Total != 0;
        }
    }
}