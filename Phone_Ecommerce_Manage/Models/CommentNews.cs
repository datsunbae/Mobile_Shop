using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class CommentNews
    {
        [Key]
        public int IdCommentNew { get; set; }
        [StringLength(255)]
        public string? Content { get; set; }
        public int? ParentComment { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }
        public int? IdUser { get; set; }
        [StringLength(255)]
        public string? OptionFullName { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? OptionEmail { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string? OptionPhone { get; set; }
        public int? IdNews { get; set; }

        [ForeignKey("IdNews")]
        [InverseProperty("CommentNews")]
        public virtual News? IdNewsNavigation { get; set; }
    }
}
