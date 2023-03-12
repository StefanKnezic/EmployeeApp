using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeApp.Models
{
    public class LocationModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();


        [Required(ErrorMessage = "Please enter address.")]
        [DisplayName("Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter City.")]
        [DisplayName("City")]
        public string City { get; set; }


        [Required(ErrorMessage = "Please enter workplace name.")]
        [DisplayName("name of Workplace")]
        public string NameOfWorkplace { get; set; }
    }
}
