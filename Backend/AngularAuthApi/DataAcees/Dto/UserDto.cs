using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAceess.Dto
{
    public class UserDto
    {
        [Required]
        [RegularExpression(@"^(?=.*[a-zA-Z]).*$", ErrorMessage = "Please enter atleast one character")]
        public string FirstName { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-zA-Z]).*$", ErrorMessage = "Please enter atleast one character")]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$", ErrorMessage = "Password must be 8 character long and required atleast one number,lowecase letter, uppercase letter and special character")]
        public string Password { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string DateOfBirth { get; set; }

    }
}
