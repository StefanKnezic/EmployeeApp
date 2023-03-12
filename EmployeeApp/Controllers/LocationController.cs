using EmployeeApp.DataAccess;
using EmployeeApp.Models;
using EmployeeApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Controllers
{
    public class LocationController : Controller
    {
        private readonly ILogger<LocationController> _logger;
        private readonly DatabaseContext _dbContext;

        public LocationController(ILogger<LocationController> logger,DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
            _logger = logger;

        }


        public IActionResult CreateLocation()
        {


            return View();
        }

        [HttpPost]
        public IActionResult CreateLocation(LocationViewModel model)
        {
            try
            {
                 _dbContext.Locations.Add(model);
                var result = _dbContext.SaveChanges();

                if (result > 0)
                {
                    ModelState.Clear();
                    ViewBag.Message = "The Location is Succesfully added!";
                }
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                _logger.LogError(e.Message.ToString());
            }
            

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListLocation()
        {
            List<LocationViewModel> list = new List<LocationViewModel>();

            var locations = await _dbContext.Locations.ToListAsync();

            foreach (var location in locations)
            {
                list.Add(new LocationViewModel
                {
                    Id = location.Id,
                    Address = location.Address,
                    City = location.City,
                    NameOfWorkplace = location.NameOfWorkplace,
                });
            }

            return View(list);
        }

        public async Task<IActionResult> DeleteLocation(Guid id)
        {


            var location = await _dbContext.Locations.FirstOrDefaultAsync(e => e.Id == id);

            if (location == null)
            {
                return RedirectToAction("ListLocation");
            }

            LocationViewModel model = new LocationViewModel

            {
                Id = location.Id,
                City = location.City,
                NameOfWorkplace = location.NameOfWorkplace,
                Address = location.Address
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLocation(LocationViewModel model)
        {

            var location =  await _dbContext.Locations.FirstOrDefaultAsync(e => e.Id == model.Id);

            if (location == null)
            {
                return View("ListLocation");
            }

            try
            {

                _dbContext.Locations.Remove(location);
                _dbContext.SaveChanges();

                ViewBag.Message = "Removed succesfully!";

            }
            catch (Exception e)
            {

                ViewBag.Message = e.Message;
            }



            return View();
        }

        public async Task<IActionResult> DetailsLocation(Guid? id)
        {

            var location = await _dbContext.Locations.FirstOrDefaultAsync(e => e.Id == id);

            if (location == null)
            {
                return View("ListLocation");
            }


            LocationViewModel model = new LocationViewModel

            {
                Id = location.Id,
                City = location.City,
                NameOfWorkplace = location.NameOfWorkplace,
                Address = location.Address
            };

            return View(model);
        }

        public async Task<IActionResult> EditLocation(Guid? id)
        {

            var location = await _dbContext.Locations.FirstOrDefaultAsync(e => e.Id == id);

            if (location == null)
            {
                return View("ListLocation");
            }
            LocationViewModel model = new LocationViewModel

            {
                Id = location.Id,
                City = location.City,
                NameOfWorkplace = location.NameOfWorkplace,
                Address = location.Address
            };


            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> EditLocation(LocationViewModel model)
        {

            var location = await _dbContext.Locations.FirstOrDefaultAsync(e => e.Id == model.Id);

            if (location == null)
            {
                return View("ListLocation");
            }

            try
            {
                
                location.City = model.City;
                location.NameOfWorkplace = model.NameOfWorkplace;
                location.Address = model.Address;

                var result = _dbContext.Locations.Update(location);
                _dbContext.SaveChanges();

                ViewBag.Message = "Update succesfully!";

            }
            catch (Exception e)
            {

                ViewBag.Message = e.Message;
            }

            return View(model);
        }

        public async Task<IActionResult> EmployeeLocation(Guid? id)
        {

            var location = await _dbContext.Locations.FirstOrDefaultAsync(e => e.Id == id);

            if (location == null)
            {
                return View("ListLocation");
            }

            var employees = await _dbContext.Employees.Where(e => e.Location.Id == id).ToListAsync();

          
            return View(employees);
        }
    }
}
