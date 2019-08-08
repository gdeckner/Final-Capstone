using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class UserJob
    {
        /// <summary>
        /// The user's id.
        /// </summary>
        [Required]
        public int? UserId { get; set; }

        /// <summary>
        /// The user's job id.
        /// </summary>
        [Required]
        public int? JobId { get; set; }
    }
}
