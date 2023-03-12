using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeApp.ViewModels
{
    public class TaskListViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Please enter full name.")]
        [DisplayName("Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please enter phone Number.")]
        [DisplayName("Phone Number")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "please enter numbers only.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter title.")]
        [DisplayName("Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter description.")]
        [DisplayName("Description")]
        public string Description { get; set; }


        [Required(ErrorMessage = "Please enter due date.")]
        [DisplayName("Due date")]
        public string DueDate { get; set; }

        
        [DisplayName("Task Completed")]
        public bool TaskCompleted { get; set; }
    }
}
