using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Micro_Social_Platform.Models
{
    public class Profile
    {
        [Key]
        public int ProfileId { get; set; }
        [Required(ErrorMessage = "The name is required")]
        [MaxLength(50, ErrorMessage = "the name must not exceed 50 characters")]
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public bool IsPublic { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}