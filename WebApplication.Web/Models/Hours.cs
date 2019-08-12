using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class Hours
    {
        [Required]
        [Display(Name = "User ID")]
        public int? UserId { get; set; }


        [Required]
        [Display(Name = "Task ID")]
        public int? TaskId { get; set; }

        [Required]
        [Display(Name = "Old Task ID")]
        public int? OldTask { get; set; }

        [Required]
        [Display(Name = "Time")]
        public decimal? TimeInHours { get; set; }

        [Required]
        [Display(Name = "Date Worked")]
        public decimal? DateWorked { get; set; }

        [Required]
        [Display(Name = "Date Logged")]
        public DateTime? Date { get; set; }

        /// <summary>
        /// The user's description of work.
        /// </summary>
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// The user's description of work.
        /// </summary>
        [Required]
        [Display(Name = "Task Title")]
        public string TaskTitle { get; set; }

        /// <summary>
        /// The user's location of work.
        /// </summary>
        [Required]
        [Display(Name = "Location")]
        public string Location { get; set; }
    }
}
