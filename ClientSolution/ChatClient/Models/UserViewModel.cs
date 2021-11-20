using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ChatClient.Models
{
    public class UserViewModel
    {
        [Required]
        [Display(Name = "Nickname")]
        public string Nickname { get; set; }
    }
}
