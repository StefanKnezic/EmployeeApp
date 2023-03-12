using EmployeeApp.DataAccess;
using EmployeeApp.Models;
using EmployeeApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Controllers
{
    public class TaskController : Controller
    {
        private readonly ILogger<TaskController> _logger;
        private readonly DatabaseContext _dbContext;
        public TaskController(ILogger<TaskController> logger,DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
            _logger = logger;
        }


        public async Task<IActionResult> CreateTask()
        {
            var result = await _dbContext.Employees
            .Select(p => new
            {
                Id = p.Id,
                Name = $"{p.FullName}"
            })
           .ToListAsync();

            ViewBag.Employee = new SelectList(result, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(TaskViewModel model)
        {
            var result = await _dbContext.Employees
            .Select(p => new
            {
                Id = p.Id,
                Name = $"{p.FullName}"
            })
           .ToListAsync();

            ViewBag.Employee = new SelectList(result, "Id", "Name");

            try
            {

                 TaskModel modelToDb = new TaskModel
                {
                     Title = model.Title,
                    Description = model.Description,
                    DueDate = model.DueDate,
                    Employee = _dbContext.Employees.Where(x => x.Id == model.Employee).FirstOrDefault(),
                };

                _dbContext.Tasks.Add(modelToDb);
                var result1 = _dbContext.SaveChanges();

                if (result1 > 0)
                {
                    ModelState.Clear();
                    ViewBag.Message = "The task is Succesfully added!";
                }
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                
            }


            return View();
        }

        [HttpGet]
        public IActionResult ListTask()
        {
            List<TaskListViewModel> list = new List<TaskListViewModel>();

            var tasks = from task in _dbContext.Tasks
                            join employee in _dbContext.Employees on task.Employee.Id equals employee.Id
                            select new
                            {
                                task.Id,
                                employee.FullName,
                                employee.PhoneNumber,
                                task.DueDate,
                                task.Description,
                                task.Title,
                                task.TaskCompleted,
                            };

            foreach (var task in tasks)
            {
                list.Add(new TaskListViewModel
                {
                    Id = task.Id,
                    FullName = task.FullName,
                    PhoneNumber = task.PhoneNumber,
                    DueDate = task.DueDate.ToShortDateString(),
                    Description = task.Description,
                    Title = task.Title,
                    TaskCompleted = task.TaskCompleted,
                    
                });
            }

            return View(list);
        }

        public IActionResult DeleteTask(Guid id)
        {

            var result = _dbContext.Tasks
            .Join(_dbContext.Employees,
                a => a.Employee.Id,
                b => b.Id,
                (a, b) => new { Tasks = a, Employees = b })
            .Where(x => x.Tasks.Id == id)
            .Select(value => new {
                Id = value.Tasks.Id,
                Tittle = value.Tasks.Title,
                Description = value.Tasks.Description,
                TaskCompleted = value.Tasks.TaskCompleted,
                FullName = value.Employees.FullName,
                PhoneNumber = value.Employees.PhoneNumber,
                DueDate = value.Tasks.DueDate,
            })
            .FirstOrDefault();

            if (result == null)
            {
                return RedirectToAction("Index", "Home");
            }

            TaskListViewModel model = new TaskListViewModel

            {
                Id = result.Id,
                FullName = result.FullName,
                PhoneNumber = result.PhoneNumber,
                DueDate = result.DueDate.ToShortDateString(),
                TaskCompleted = result.TaskCompleted,
                Title = result.Tittle,
                Description = result.Description
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult DeleteTask(TaskListViewModel model)
        {

            var task = _dbContext.Tasks.FirstOrDefault(e => e.Id == model.Id);

            if (task == null)
            {
                return View("ListTask");
            }

            try
            {
                _dbContext.Tasks.Remove(task);
                _dbContext.SaveChanges();

                ViewBag.Message = " Task Removed succesfully!";

            }
            catch (Exception e)
            {

                ViewBag.Message = e.Message;
            }

            return View();
        }

        public IActionResult DetailsTask(Guid? id)
        {

            var result = _dbContext.Tasks
            .Join(_dbContext.Employees,
                a => a.Employee.Id,
                b => b.Id,
                (a, b) => new { Tasks = a, Employees = b })
            .Where(x => x.Tasks.Id == id)
            .Select(value => new {
                Id = value.Tasks.Id,
                Tittle = value.Tasks.Title,
                Description = value.Tasks.Description,
                TaskCompleted = value.Tasks.TaskCompleted,
                FullName = value.Employees.FullName,
                PhoneNumber = value.Employees.PhoneNumber,
                DueDate = value.Tasks.DueDate,
            })
            .FirstOrDefault();

            if (result == null)
            {
                return RedirectToAction("Index", "Home");
            }

            TaskListViewModel model = new TaskListViewModel

            {
                Id = result.Id,
                FullName = result.FullName,
                PhoneNumber = result.PhoneNumber,
                DueDate = result.DueDate.ToShortDateString(),
                TaskCompleted = result.TaskCompleted,
                Title = result.Tittle,
                Description = result.Description
            };

            return View(model);
        }

        public async Task<IActionResult> EditTask(Guid? id)
        {

            var result = await _dbContext.Employees
            .Select(p => new
            {
                Id = p.Id,
                Name = $"{p.FullName}"
            })
           .ToListAsync();

            var task = _dbContext.Tasks.Include(e => e.Employee).FirstOrDefault(e => e.Id == id);


            if (result == null || task == null)
            {
                return View("ListTask");
            }

            var SelectedValue = result.Where(x => x.Id == task.Employee.Id).FirstOrDefault();

            ViewBag.Show = new SelectList(result, "Id", "Name", SelectedValue);

            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> EditTask(TaskModel model)
        {

            var result = await _dbContext.Employees
            .Select(p => new
            {
                Id = p.Id,
                Name = $"{p.FullName}"
            })
           .ToListAsync();

            var task = _dbContext.Tasks.Include(e => e.Employee).FirstOrDefault(e => e.Id == model.Id);


            if (result == null || task == null)
            {
                return View("ListTask");
            }

            var SelectedValue = result.Where(x => x.Id == task.Employee.Id).FirstOrDefault();

            ViewBag.Show = new SelectList(result, "Id", "Name", SelectedValue);

            /////////////////////////////////////////////////////////////////////////////////
            ///

            var task1 = _dbContext.Tasks.FirstOrDefault(e => e.Id == model.Id);

           var employees = _dbContext.Employees.Where(x => x.Id == model.Employee.Id).FirstOrDefault();

            if (task1 == null || employees == null)
            {
                return View("ListTask");
            }

            try
            {
                task1.Employee = employees;
                task1.DueDate = model.DueDate;
                task1.TaskCompleted = model.TaskCompleted;
                task1.Title = model.Title;
                task1.Description = model.Description;

                _dbContext.Tasks.Update(task1);
                 _dbContext.SaveChanges();

                ViewBag.Message = "Update succesfully!";

            }
            catch (Exception e)
            {

             ViewBag.Message = e.Message;

            }



            return View(model);
        }

        public IActionResult TopTask()
        {
            List<EmployeeModel> list = new List<EmployeeModel>();


            var result = _dbContext.Tasks
           .Join(_dbContext.Employees,
               a => a.Employee.Id,
               b => b.Id,
               (a, b) => new { Tasks = a, Employees = b })
           .Where(x => x.Tasks.DueDate.Month == DateTime.Today.Month-1 && x.Tasks.TaskCompleted == true) 
           .Select(value => new
           {
               employeeId = value.Employees.Id,

           }).GroupBy(x => x.employeeId)
            .Select(g => new
            {
                Id = g.Key,
                TaskCount = g.Count()
            })
         .OrderByDescending(x => x.TaskCount)
         .Take(5)
          .ToList();


            foreach (var task in result)
            {
                var employee = _dbContext.Employees.Where(x => x.Id == task.Id).FirstOrDefault();

                list.Add(employee);
            }

            return View(list);



        }




    }
}
