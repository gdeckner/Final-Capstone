using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class PayrollTable
    {
        /// <summary>
        /// The user's id.
        /// </summary>
        [Required]
        public int? UserId { get; set; }

        /// <summary>
        /// The user's start paydate.
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The user end paydate.
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// The user's pay approval status.
        /// </summary>
        [Required]
        public bool Approved { get; set; }

        /// <summary>
        /// The user's pay submitted status.
        /// </summary>
        [Required]
        public bool Submitted { get; set; }
    }
}
