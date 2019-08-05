using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class Job
    {

        /// <summary>
        /// The job's id.
        /// </summary>
        [Required]
        public int JobId { get; set; }

        /// <summary>
        /// The job's title.
        /// </summary>
        [Required]
        public string Title { get; set; }

    }
}
