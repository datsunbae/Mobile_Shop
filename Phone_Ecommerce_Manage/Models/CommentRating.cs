using System;
using System.Collections.Generic;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class CommentRating
    {
        public int IdCommentRating { get; set; }
        public string? Content { get; set; }
        public int? ParentComment { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? Rating { get; set; }
        public int? IdUser { get; set; }
        public string? OptionFullName { get; set; }
        public string? OptionEmail { get; set; }
        public string? OptionPhone { get; set; }
        public int? IdProductVersion { get; set; }

        public virtual ProductVersion? IdProductVersionNavigation { get; set; }
    }
}
