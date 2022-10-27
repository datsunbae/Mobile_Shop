using System;
using System.Collections.Generic;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class CategoryNews
    {
        public CategoryNews()
        {
            News = new HashSet<News>();
        }

        public int IdCategoryNews { get; set; }
        public string? NameCategory { get; set; }

        public virtual ICollection<News> News { get; set; }
    }
}
