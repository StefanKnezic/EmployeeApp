using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeApp.ViewModels
{
    public class EmployeeListViewModel
    {
        public Guid Id { get; set; }

       
        [DisplayName("Full name")]
        public string FullName { get; set; }
        
        [DisplayName("Phone number")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "please enter numbers only.")]
        public string PhoneNumber { get; set; }

        
        [DisplayName("Date of Birth")]
        public string DateOfBirth { get; set; }

        // public DateTime DateOfBirth { get; set; }
        
        [DisplayName("Monthly salary ($)")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "please enter numbers only.")]
        public string MonthlySalary { get; set; }

     
        [DisplayName("City")]
        public string City { get; set; }

        
        [DisplayName("Address")]
        public string Address { get; set; }

        [DisplayName("Workplace name")]
        public string NameOfWorkplace { get; set; }

    }
}
