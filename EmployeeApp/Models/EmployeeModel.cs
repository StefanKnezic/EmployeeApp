using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeApp.Models
{
    public class EmployeeModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Please enter Full name.")]
        [DisplayName("Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please enter the phone number")]
        [DisplayName("Phone number")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "please enter numbers only.")]

        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter the date of birth.")]
        [DisplayName("date of Birth")]

        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please enter the montly salary.")]
        [DisplayName("Montly Salary ($)")]
        public int MonthlySalary { get; set; }


        [Required(ErrorMessage = "Please enter the Location.")]
        [DisplayName("Location")]
        public LocationModel Location { get; set; }
    }
}
