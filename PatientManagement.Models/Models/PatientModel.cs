using System.ComponentModel.DataAnnotations;

namespace PatientManagement.Models.Models
{
    public enum Gender
    {
        Male,
        Female,
        Unknown
    }
    public class PatientModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "FirstName is required"), StringLength(20)]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "LastName is required"), StringLength(20)]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "DateOfBirth is required")]
        public DateTime DateOfBirth { get; set; }

        [EnumDataType(typeof(Gender), ErrorMessage = "Select either Male, Female or Unknown")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Contact number is required."), StringLength(10)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter a valid 10 digit phone number.")]
        public string ContactNumber { get; set; } = null!;

        [Required(ErrorMessage = "Weight is required")]
        public double Weight { get; set; }

        [Required(ErrorMessage = "Height is required")]
        public double Height { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Address is required"), StringLength(50)]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "MedicalComments from Doctor is mandatory"), StringLength(100)]
        public string MedicalComments { get; set; } = null!;

        [Required(ErrorMessage = "Provide Either true or false")]
        public bool AnyMedicationsTaking { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime UpdatedDate { get; set; }
    }
}