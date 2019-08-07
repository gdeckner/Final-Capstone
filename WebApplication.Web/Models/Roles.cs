using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class Roles
    {
        /// <summary>
        /// The role id.
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// 
        /// The role description.
        /// </summary>
        [Required]
        public string Description { get; set; }
    }
}
