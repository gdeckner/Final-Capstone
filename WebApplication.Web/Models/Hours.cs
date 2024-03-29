﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class Hours
    {
        [Display(Name = "Hours ID")]
        public int HoursId { get; set; }

        //[Required]
        [Display(Name = "User ID")]
        public int? UserId { get; set; }


        [Required]
        [Display(Name = "Task")]
        public int? TaskId { get; set; }

        [Display(Name = "Old Task ID")]
        public int? OldTask { get; set; }

        [Required(ErrorMessage = "Time worked is required")]
        [Range (0,99,ErrorMessage = "Hours must be between 0 and 99")]
        [Display(Name = "Hours Worked")]
        public decimal? TimeInHours { get; set; }

        [Required(ErrorMessage = "Date worked is required")]
        [Display(Name = "Date Worked")]
        public DateTime DateWorked { get; set; }

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
