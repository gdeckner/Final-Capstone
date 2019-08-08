using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class Hours
    {
        /// <summary>
        /// The user's id.
        /// </summary>
        [Required]
        public int? UserId { get; set; }

        /// <summary>
        /// The user's assigned task id.
        /// </summary>
        [Required]
        public int? TaskId { get; set; }

        /// <summary>
        /// The user's time worked in hours.
        /// </summary>
        [Required]
        public decimal? TimeInHours { get; set; }

        /// <summary>
        /// The user's date of work.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }
    }
}
