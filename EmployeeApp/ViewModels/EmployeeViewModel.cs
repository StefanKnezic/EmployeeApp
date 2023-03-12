
using EmployeeApp.Validatiors;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeApp.ViewModels
{

    public class EmployeeViewModel
    {

        [Required(ErrorMessage = "Please enter the full name.")]
        [DisplayName("Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please enter the phone number.")]
        [DisplayName("Phone number")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage ="please enter numbers only.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter the date of birth.")]
        [DisplayName("Date of Birth")]
        
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please enter the montly salary.")]
        [DisplayName("Montly Salary ($)")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "please enter numbers only.")]
        public string MonthlySalary { get; set; }

        [Required(ErrorMessage = "Please enter the location.")]
        [DisplayName("Location")]
        public Guid Location { get; set; }
    }
}
