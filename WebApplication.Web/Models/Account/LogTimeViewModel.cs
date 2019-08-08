using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models.Account
{
    public class LogTimeViewModel
    {
        [Required]
        [Display(Name = "User ID")]
        public int UserId { get; set; }

        
        [Required]
        [Display(Name = "Task ID")]
        public int TaskId { get; set; }

   
        [Required]
        [Display(Name = "Time")]
        public decimal TimeInHours { get; set; }


        [Required]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
    }
}
