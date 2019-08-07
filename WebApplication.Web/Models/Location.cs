using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class Location
    {
        /// <summary>
        /// The Location's id.
        /// </summary>
        [Required]
        public int? LocationId { get; set; }

        /// <summary>
        /// The job's title.
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// The job's title.
        /// </summary>
        [Required]
        public string Description { get; set; }
    }
}
