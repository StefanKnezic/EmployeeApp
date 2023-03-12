using EmployeeApp.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeApp.ViewModels
{
    public class TaskViewModel
    {

        [Required(ErrorMessage = "Please enter title.")]
        [DisplayName("Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter description.")]
        [DisplayName("description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter due date.")]
        [DisplayName("Due date")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Please enter employee.")]
        [DisplayName("Employee")]
        public Guid Employee { get; set; }
    }
}
