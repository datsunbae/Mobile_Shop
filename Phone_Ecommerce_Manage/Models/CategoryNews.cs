using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class CategoryNews
    {
        public CategoryNews()
        {
            News = new HashSet<News>();
        }

        [Key]
        public int IdCategoryNews { get; set; }
        [StringLength(255)]
        public string NameCategory { get; set; } = null!;

        [InverseProperty("IdCategoryNewsNavigation")]
        public virtual ICollection<News> News { get; set; }
    }
}
