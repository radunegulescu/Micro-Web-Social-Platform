using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Micro_Social_Platform.Models
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }
        [Required(ErrorMessage = "The group name is required.")]
        public string Name { get; set; }
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "The group description is required.")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}