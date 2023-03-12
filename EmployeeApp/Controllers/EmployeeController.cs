using EmployeeApp.DataAccess;
using EmployeeApp.Models;
using EmployeeApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;

namespace EmployeeApp.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly DatabaseContext _dbContext;

        public EmployeeController(ILogger<EmployeeController> logger,DatabaseContext databaseContext)
        {
            _logger = logger;
            _dbContext = databaseContext;
        }


        [HttpGet]
        public async Task<IActionResult> CreateEmployee()
        {

            var result = await _dbContext.Locations
            .Select(p => new
            {
                Id = p.Id,
                Name = $"{p.City} - {p.Address}"
            })
           .ToListAsync();

            ViewBag.Location = new SelectList(result, "Id", "Name");



            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(EmployeeViewModel model)
        {
            var result = await _dbContext.Locations
            .Select(p => new
            {
                Id = p.Id,
                Name = $"{p.Address} - {p.City}"
            })
           .ToListAsync();

            ViewBag.Location = new SelectList(result, "Id", "Name");

            try
            {

                EmployeeModel modelToDb = new EmployeeModel
                {
                    DateOfBirth = model.DateOfBirth,
                    FullName = model.FullName,
                    MonthlySalary = Convert.ToInt32(model.MonthlySalary),
                    Location = _dbContext.Locations.Where(x => x.Id == model.Location).FirstOrDefault(),
                    PhoneNumber = model.PhoneNumber,

                };

                _dbContext.Employees.Add(modelToDb);
                var result1 = _dbContext.SaveChanges();

                if (result1 > 0)
                {
                    ModelState.Clear();
                    ViewBag.Message = "The Employee is Succesfully added!";
                }
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                
            }


            return View();
        }


        [HttpGet]
        public IActionResult ListEmployee()
        {
            List<EmployeeListViewModel> list = new List<EmployeeListViewModel>();

            var employyes = from employee in _dbContext.Employees
                        join location in _dbContext.Locations on employee.Location.Id equals location.Id
                        select  new
                        {
                            employee.Id,
                            employee.FullName,
                            employee.PhoneNumber,
                            employee.DateOfBirth,
                            employee.MonthlySalary,
                            location.City,
                            location.Address,
                            location.NameOfWorkplace
                        };
           
            foreach (var employee in employyes) 
            {
                list.Add(new EmployeeListViewModel 
                {
                Id = employee.Id,
                FullName = employee.FullName,
                MonthlySalary = employee.MonthlySalary.ToString(),
                City = employee.City,
                Address = employee.Address,
                DateOfBirth = employee.DateOfBirth.ToShortDateString(),
                NameOfWorkplace = employee.NameOfWorkplace,
                PhoneNumber = employee.PhoneNumber,
                });
            }

            return View(list);
        }


        public IActionResult DeleteEmployee(Guid id)
        {


            var result = _dbContext.Employees
            .Join(_dbContext.Locations,
                a => a.Location.Id,
                b => b.Id,
                (a, b) => new { Employees = a, Location = b })
            .Where(x => x.Employees.Id == id)
            .Select(value => new {
                Id = value.Employees.Id,
                FullName = value.Employees.FullName,
                PhoneNumber = value.Employees.PhoneNumber,
                DateOfBirth = value.Employees.DateOfBirth,
                MonthlySalary = value.Employees.MonthlySalary,
                City = value.Location.City,
                Address = value.Location.Address,
                NameOfWorkPlace = value.Location.NameOfWorkplace
            })
            .FirstOrDefault();

            if (result == null)
            {
                return RedirectToAction("Index", "Home");
            }

            EmployeeListViewModel model = new EmployeeListViewModel

            {
                Id = result.Id,
                FullName = result.FullName,
                PhoneNumber = result.PhoneNumber,
                DateOfBirth = result.DateOfBirth.ToShortDateString(),
                MonthlySalary = result.MonthlySalary.ToString(),
                City = result.City,
                NameOfWorkplace = result.NameOfWorkPlace,
                Address = result.Address
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult DeleteEmployee(EmployeeListViewModel model)
        {

            var employee = _dbContext.Employees.FirstOrDefault(e => e.Id == model.Id);

            if (employee == null)
            {
                return View("ListEmployee");
            }

            try
            {

                _dbContext.Employees.Remove(employee);
                _dbContext.SaveChanges();

                ViewBag.Message = "Removed succesfully!";

            }
            catch (Exception e)
            {

                ViewBag.Message = e.Message;
            }



            return View();
        }

       

        public IActionResult DetailsEmployee(Guid? id)
        {

        var result = _dbContext.Employees
        .Join(_dbContext.Locations,
            a => a.Location.Id,
            b => b.Id,
            (a, b) => new { Employees = a, Location = b })
        .Where(x => x.Employees.Id == id)
        .Select(value => new {
            Id = value.Employees.Id, 
            FullName = value.Employees.FullName,
            PhoneNumber = value.Employees.PhoneNumber,
            DateOfBirth = value.Employees.DateOfBirth,
            MonthlySalary = value.Employees.MonthlySalary,
            City = value.Location.City,
            Address = value.Location.Address,
            NameOfWorkPlace = value.Location.NameOfWorkplace   
        })
        .FirstOrDefault();

            if (result == null)
            {
                return RedirectToAction("Index", "Home");
            }

            EmployeeListViewModel model = new EmployeeListViewModel 
            
            { Id = result.Id, FullName = result.FullName,
                PhoneNumber = result.PhoneNumber,
                DateOfBirth = result.DateOfBirth.ToShortDateString(),
                MonthlySalary = result.MonthlySalary.ToString(),
                City = result.City,
                NameOfWorkplace = result.NameOfWorkPlace,
              Address = result.Address
            };

            return View(model);
        }



        public IActionResult EditEmployee(Guid? id)
        {

            var result =  _dbContext.Locations
            .Select(p => new
            {
                Id = p.Id,
                Name = $"{p.City} - {p.Address}"
            })
           .ToList();


            var employee = _dbContext.Employees
    .Include(e => e.Location) 
    .FirstOrDefault(e => e.Id == id);

            if (result == null || employee == null) 
            {
                return View("ListEmployee");
            }


            var SelectedValue = result.Where(x => x.Id == employee.Location.Id).FirstOrDefault();

            ViewBag.Show = new SelectList(result, "Id", "Name",SelectedValue);

            return View(employee);

        }


        [HttpPost]
        public IActionResult EditEmployee(EmployeeModel model)
        {


            var result1 = _dbContext.Locations
            .Select(p => new
            {
                Id = p.Id,
                Name = $"{p.City} - {p.Address}"
            })
           .ToList();


            var employee1 = _dbContext.Employees
    .Include(e => e.Location)
    .FirstOrDefault(e => e.Id == model.Id);

            if (result1 == null || employee1 == null)
            {
                return View("ListEmployee");
            }


            var SelectedValue = result1.Where(x => x.Id == employee1.Location.Id).FirstOrDefault();

            ViewBag.Show = new SelectList(result1, "Id", "Name", SelectedValue);



            /////////////////////////////////////////////////////////////////////////////////

            var employee = _dbContext.Employees.FirstOrDefault(e => e.Id == model.Id);

            var location = _dbContext.Locations.Where(x => x.Id == model.Location.Id).FirstOrDefault();

            if (employee == null || location == null)
            {
                return View("ListEmployee");
            }

            try
            {
                employee.Location = location;
                employee.PhoneNumber = model.PhoneNumber;
                employee.DateOfBirth = model.DateOfBirth;
                employee.FullName = model.FullName;
                employee.MonthlySalary = model.MonthlySalary;

                var result =  _dbContext.Employees.Update(employee);
                _dbContext.SaveChanges();

                ViewBag.Message = "Update succesfully!";

            }
            catch (Exception e)
            {

                ViewBag.Message = e.Message;
            }


             
            return View(model);
        }
    }
}
