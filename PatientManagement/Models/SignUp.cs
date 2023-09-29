﻿using System.ComponentModel.DataAnnotations;

namespace PatientManagement.Models
{
    public class SignUp
    {
        [Required, StringLength(10)]
        public string FirstName { get; set; }

        [Required, StringLength(10)]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        [Compare("ConfirmPassword")]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
    }
}
