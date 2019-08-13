using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class Log
    {
        //[Required]
        [Display(Name = "Log ID")]
        public int? LogId { get; set; }

        [Required]
        [Display(Name = "Target User")]
        public int? TargetUser { get; set; }

        [Display(Name = "Date Worked")]
        public DateTime? DateWorked { get; set; }

        [Required]
        [Display(Name = "Date Logged")]
        public DateTime? DateLogged { get; set; }

        [Required]
        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "Hours Before")]
        public decimal? HoursBefore { get; set; }

        [Display(Name = "Hours ID")]
        public int? HoursId { get; set; }

        /// <summary>
        /// The user's description of work.
        /// </summary>
        [Required]
        [Display(Name = "Hours After")]
        public decimal? HoursAfter { get; set; }
    }
}
