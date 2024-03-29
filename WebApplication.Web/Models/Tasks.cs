﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class Tasks
    {
        /// <summary>
        /// The task's id.
        /// </summary>
        [Required]
        public int? TaskId { get; set; }

        /// <summary>
        /// The task's job id.
        /// </summary>
        [Required]
        public int? JobId { get; set; }

        /// <summary>
        /// The task's title.
        /// </summary>
        [Required(ErrorMessage = "Task title is required")]
        public string Title { get; set; }

        /// <summary>
        /// The task's location.
        /// </summary>
        [Required]
        public int? LocationId { get; set; }

        /// <summary>
        /// 
        /// The task's description.
        /// </summary>
        [Required]
        public string Description { get; set; }
    }
}
