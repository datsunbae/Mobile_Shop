using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Phone_Ecommerce_Manage.Models
{
    public partial class MobileShop_DBContext : DbContext
    {
        public MobileShop_DBContext()
        {
        }

        public MobileShop_DBContext(DbContextOptions<MobileShop_DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BrandMobile> BrandMobiles { get; set; } = null!;
        public virtual DbSet<CategoryNews> CategoryNews { get; set; } = null!;
        public virtual DbSet<ColorProduct> ColorProducts { get; set; } = null!;
        public virtual DbSet<CommentProduct> CommentProducts { get; set; } = null!;
        public virtual DbSet<CommentRating> CommentRatings { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Manager> Managers { get; set; } = null!;
        public virtual DbSet<News> News { get; set; } = null!;
        public virtual DbSet<O> Os { get; set; } = null!;
        public virtual DbSet<OrderBill> OrderBills { get; set; } = null!;
        public virtual DbSet<OrderBillDetail> OrderBillDetails { get; set; } = null!;
        public virtual DbSet<PaymentsType> PaymentsTypes { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductColor> ProductColors { get; set; } = null!;
        public virtual DbSet<ProductVersion> ProductVersions { get; set; } = null!;
        public virtual DbSet<PromotionProduct> PromotionProducts { get; set; } = null!;
        public virtual DbSet<PromotionProductDetail> PromotionProductDetails { get; set; } = null!;
        public virtual DbSet<Rating> Ratings { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<StatusOrder> StatusOrders { get; set; } = null!;
        public virtual DbSet<StatusProduct> StatusProducts { get; set; } = null!;
        public virtual DbSet<Voucher> Vouchers { get; set; } = null!;
        public virtual DbSet<VoucherDetail> VoucherDetails { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-60C14JF;Initial Catalog=MobileShop_DB;User ID=sa;Password=123456");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BrandMobile>(entity =>
            {
                entity.HasKey(e => e.IdBrandMobile)
                    .HasName("PK__BrandMob__3D412B0320BC2BBB");

                entity.Property(e => e.IsPublished).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<CategoryNews>(entity =>
            {
                entity.HasKey(e => e.IdCategoryNews)
                    .HasName("PK__Category__002B75A5B2867A7C");
            });

            modelBuilder.Entity<ColorProduct>(entity =>
            {
                entity.HasKey(e => e.IdColor)
                    .HasName("PK__ColorPro__E83D55CB00AAFDF1");
            });

            modelBuilder.Entity<CommentProduct>(entity =>
            {
                entity.HasKey(e => e.IdCommentProduct)
                    .HasName("PK__CommentP__542057BCC73B29E7");

                entity.Property(e => e.ParentComment).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.IdCustomerNavigation)
                    .WithMany(p => p.CommentProducts)
                    .HasForeignKey(d => d.IdCustomer)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_IdCustomer_CommentProduct");

                entity.HasOne(d => d.IdManagerNavigation)
                    .WithMany(p => p.CommentProducts)
                    .HasForeignKey(d => d.IdManager)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_IdManager_CommentProduct");

                entity.HasOne(d => d.IdProductVersionNavigation)
                    .WithMany(p => p.CommentProducts)
                    .HasForeignKey(d => d.IdProductVersion)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_IdProductVersion_CommentProduct");
            });

            modelBuilder.Entity<CommentRating>(entity =>
            {
                entity.HasKey(e => e.IdCommentRating)
                    .HasName("PK__CommentR__F4D21E3030D12D3D");

                entity.Property(e => e.ParentComment).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.IdCustomerNavigation)
                    .WithMany(p => p.CommentRatings)
                    .HasForeignKey(d => d.IdCustomer)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_IdCustomer_CommentRating");

                entity.HasOne(d => d.IdManagerNavigation)
                    .WithMany(p => p.CommentRatings)
                    .HasForeignKey(d => d.IdManager)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_IdManager_CommentRating");

                entity.HasOne(d => d.IdProductVersionNavigation)
                    .WithMany(p => p.CommentRatings)
                    .HasForeignKey(d => d.IdProductVersion)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_IdProductVersion_CommentRating");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.IdCustomer)
                    .HasName("PK__Customer__DB43864AD60F6DFD");
            });

            modelBuilder.Entity<Manager>(entity =>
            {
                entity.HasKey(e => e.IdManager)
                    .HasName("PK__Manager__ABC3516E84FC4F61");

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.Managers)
                    .HasForeignKey(d => d.IdRole)
                    .HasConstraintName("FK_IdRole_AccountUser");
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.HasKey(e => e.IdNews)
                    .HasName("PK__News__4559C72DC8B41A95");

                entity.HasOne(d => d.IdCategoryNewsNavigation)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.IdCategoryNews)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_IdCategoryNews_News");

                entity.HasOne(d => d.IdManagerNavigation)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.IdManager)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_IdManager_News");
            });

            modelBuilder.Entity<O>(entity =>
            {
                entity.HasKey(e => e.IdOs)
                    .HasName("PK__OS__B770330F1B99CDA7");
            });

            modelBuilder.Entity<OrderBill>(entity =>
            {
                entity.HasKey(e => e.IdOrderBill)
                    .HasName("PK__OrderBil__251DF3936539AE2E");

                entity.HasOne(d => d.IdCustomerNavigation)
                    .WithMany(p => p.OrderBills)
                    .HasForeignKey(d => d.IdCustomer)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_IdCustomer_OrderBill");

                entity.HasOne(d => d.IdManagerNavigation)
                    .WithMany(p => p.OrderBills)
                    .HasForeignKey(d => d.IdManager)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_IdManager_OrderBill");

                entity.HasOne(d => d.IdPaymentTypeNavigation)
                    .WithMany(p => p.OrderBills)
                    .HasForeignKey(d => d.IdPaymentType)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_IdPaymentType_OrderBill");

                entity.HasOne(d => d.IdStatusOrderNavigation)
                    .WithMany(p => p.OrderBills)
                    .HasForeignKey(d => d.IdStatusOrder)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_IdStatusOrder_OrderBill");
            });

            modelBuilder.Entity<OrderBillDetail>(entity =>
            {
                entity.HasKey(e => e.IdOrderBillDetails)
                    .HasName("PK__OrderBil__3748B242359FFB2C");

                entity.HasOne(d => d.IdOrderBillNavigation)
                    .WithMany(p => p.OrderBillDetails)
                    .HasForeignKey(d => d.IdOrderBill)
                    .HasConstraintName("FK_IdOrderBill_OrderBillDetails");

                entity.HasOne(d => d.IdProductColorNavigation)
                    .WithMany(p => p.OrderBillDetails)
                    .HasForeignKey(d => d.IdProductColor)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_IdProductColor_OrderBillDetails");
            });

            modelBuilder.Entity<PaymentsType>(entity =>
            {
                entity.HasKey(e => e.IdPaymentType)
                    .HasName("PK__Payments__3F36DF711CA81D6D");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.IdProduct)
                    .HasName("PK__Product__2E8946D4A1CBB64E");

                entity.Property(e => e.IsPublished).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.IdBrandMobileNavigation)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.IdBrandMobile)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_IdBrandMobile_Product");

                entity.HasOne(d => d.IdOsNavigation)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.IdOs)
                    .HasConstraintName("FK_IdOS_Product");
            });

            modelBuilder.Entity<ProductColor>(entity =>
            {
                entity.HasKey(e => e.IdProductColor)
                    .HasName("PK__ProductC__44B498FA59655D82");

                entity.Property(e => e.IsPublished).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.IdColorNavigation)
                    .WithMany(p => p.ProductColors)
                    .HasForeignKey(d => d.IdColor)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_IdColor_ProductColor");

                entity.HasOne(d => d.IdProductVersionNavigation)
                    .WithMany(p => p.ProductColors)
                    .HasForeignKey(d => d.IdProductVersion)
                    .HasConstraintName("FK_IdProductVersion_ProductColor");

                entity.HasOne(d => d.IdStatusProductNavigation)
                    .WithMany(p => p.ProductColors)
                    .HasForeignKey(d => d.IdStatusProduct)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IdStatusProduct_ProductColor");
            });

            modelBuilder.Entity<ProductVersion>(entity =>
            {
                entity.HasKey(e => e.IdProductVersion)
                    .HasName("PK__ProductV__AD31AF6974D6C5DA");

                entity.Property(e => e.IsBestseller).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsPublished).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.ProductVersions)
                    .HasForeignKey(d => d.IdProduct)
                    .HasConstraintName("FK_IdProduct_ProductVersion");
            });

            modelBuilder.Entity<PromotionProduct>(entity =>
            {
                entity.HasKey(e => e.IdPromotionProduct)
                    .HasName("PK__Promotio__2D9BA1681B8F30D9");

                entity.Property(e => e.IsPublished).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.IdProductVersionNavigation)
                    .WithMany(p => p.PromotionProducts)
                    .HasForeignKey(d => d.IdProductVersion)
                    .HasConstraintName("FK_IdProductVersion_PromotionProduct");
            });

            modelBuilder.Entity<PromotionProductDetail>(entity =>
            {
                entity.HasOne(d => d.IdPromotionProductNavigation)
                    .WithMany(p => p.PromotionProductDetails)
                    .HasForeignKey(d => d.IdPromotionProduct)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_IdPromotionProduct_PromotionProductDetails");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.HasOne(d => d.IdProductVersionNavigation)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.IdProductVersion)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_IdProductVersion_Rating");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.IdRole)
                    .HasName("PK__Role__B43690549F5CB7D6");
            });

            modelBuilder.Entity<StatusOrder>(entity =>
            {
                entity.HasKey(e => e.IdStatusOrder)
                    .HasName("PK__StatusOr__361588A8D493A63E");
            });

            modelBuilder.Entity<StatusProduct>(entity =>
            {
                entity.HasKey(e => e.IdStatusProduct)
                    .HasName("PK__StatusPr__D980E0949BBBEF90");
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.HasKey(e => e.Idvoucher)
                    .HasName("PK__Vouchers__50249A273ED0231E");
            });

            modelBuilder.Entity<VoucherDetail>(entity =>
            {
                entity.HasKey(e => e.IdvoucherDetails)
                    .HasName("PK__VoucherD__116412D5CC132D1C");

                entity.HasOne(d => d.IdOrderBillNavigation)
                    .WithMany(p => p.VoucherDetails)
                    .HasForeignKey(d => d.IdOrderBill)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_IdOrderBill_VoucherDetails");

                entity.HasOne(d => d.IdvoucherNavigation)
                    .WithMany(p => p.VoucherDetails)
                    .HasForeignKey(d => d.Idvoucher)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_IDVoucher_VoucherDetails");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
