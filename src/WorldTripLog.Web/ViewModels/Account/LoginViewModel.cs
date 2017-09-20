using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorldTripLog.Web.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(20, ErrorMessage = "The username must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
