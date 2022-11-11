using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Phone_Ecommerce_Manage.Models
{
    [Table("CommentRating")]
    public partial class CommentRating
    {
        [Key]
        public int IdCommentRating { get; set; }
        [StringLength(255)]
        public string? Content { get; set; }
        public int? ParentComment { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }
        public int? Rating { get; set; }
        public int? IdManager { get; set; }
        public int? IdCustomer { get; set; }
        public int? IdProductVersion { get; set; }

        [ForeignKey("IdCustomer")]
        [InverseProperty("CommentRatings")]
        public virtual Customer? IdCustomerNavigation { get; set; }
        [ForeignKey("IdManager")]
        [InverseProperty("CommentRatings")]
        public virtual Manager? IdManagerNavigation { get; set; }
        [ForeignKey("IdProductVersion")]
        [InverseProperty("CommentRatings")]
        public virtual ProductVersion? IdProductVersionNavigation { get; set; }
    }
}
