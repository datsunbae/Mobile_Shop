using System;
using System.Collections.Generic;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class News
    {
        public News()
        {
            CommentNews = new HashSet<CommentNews>();
        }

        public int IdNews { get; set; }
        public string? Title { get; set; }
        public string? DescriptionNew { get; set; }
        public string? Content { get; set; }
        public string? Thumb { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? IsHot { get; set; }
        public int? IdCategoryNews { get; set; }
        public int? IdAccountUser { get; set; }

        public virtual AccountUser? IdAccountUserNavigation { get; set; }
        public virtual CategoryNews? IdCategoryNewsNavigation { get; set; }
        public virtual ICollection<CommentNews> CommentNews { get; set; }
    }
}
