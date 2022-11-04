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

        public virtual DbSet<AccountUser> AccountUsers { get; set; } = null!;
        public virtual DbSet<BrandMobile> BrandMobiles { get; set; } = null!;
        public virtual DbSet<CategoryNews> CategoryNews { get; set; } = null!;
        public virtual DbSet<ColorProduct> ColorProducts { get; set; } = null!;
        public virtual DbSet<CommentNews> CommentNews { get; set; } = null!;
        public virtual DbSet<CommentProduct> CommentProducts { get; set; } = null!;
        public virtual DbSet<CommentRating> CommentRatings { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<EventSale> EventSales { get; set; } = null!;
        public virtual DbSet<EventSaleDetail> EventSaleDetails { get; set; } = null!;
        public virtual DbSet<Location> Locations { get; set; } = null!;
        public virtual DbSet<News> News { get; set; } = null!;
        public virtual DbSet<OrderBill> OrderBills { get; set; } = null!;
        public virtual DbSet<OrderBillDetail> OrderBillDetails { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductColor> ProductColors { get; set; } = null!;
        public virtual DbSet<ProductVersion> ProductVersions { get; set; } = null!;
        public virtual DbSet<PromotionProduct> PromotionProducts { get; set; } = null!;
        public virtual DbSet<PromotionProductDetail> PromotionProductDetails { get; set; } = null!;
        public virtual DbSet<Rating> Ratings { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<StatusOrder> StatusOrders { get; set; } = null!;
        public virtual DbSet<StatusProduct> StatusProducts { get; set; } = null!;
        public virtual DbSet<Store> Stores { get; set; } = null!;

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
            modelBuilder.Entity<AccountUser>(entity =>
            {
                entity.HasKey(e => e.IdAccountUser)
                    .HasName("PK__AccountU__76959AFB65179C7C");

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.AccountUsers)
                    .HasForeignKey(d => d.IdRole)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_IdRole_AccountUser");
            });

            modelBuilder.Entity<BrandMobile>(entity =>
            {
                entity.HasKey(e => e.IdBrandMobile)
                    .HasName("PK__BrandMob__3D412B03B40612D5");

                entity.Property(e => e.IsPublished).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<CategoryNews>(entity =>
            {
                entity.HasKey(e => e.IdCategoryNews)
                    .HasName("PK__Category__002B75A5BFE87192");
            });

            modelBuilder.Entity<ColorProduct>(entity =>
            {
                entity.HasKey(e => e.IdColor)
                    .HasName("PK__ColorPro__E83D55CB54AA9E4E");
            });

            modelBuilder.Entity<CommentNews>(entity =>
            {
                entity.HasKey(e => e.IdCommentNew)
                    .HasName("PK__CommentN__708A02031C25C396");

                entity.Property(e => e.ParentComment).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.IdNewsNavigation)
                    .WithMany(p => p.CommentNews)
                    .HasForeignKey(d => d.IdNews)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_IdNews_CommentNews");
            });

            modelBuilder.Entity<CommentProduct>(entity =>
            {
                entity.HasKey(e => e.IdCommentProduct)
                    .HasName("PK__CommentP__542057BC53352CAA");

                entity.Property(e => e.ParentComment).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.IdProductVersionNavigation)
                    .WithMany(p => p.CommentProducts)
                    .HasForeignKey(d => d.IdProductVersion)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_IdProductVersion_CommentProduct");
            });

            modelBuilder.Entity<CommentRating>(entity =>
            {
                entity.HasKey(e => e.IdCommentRating)
                    .HasName("PK__CommentR__F4D21E301D24A5E4");

                entity.Property(e => e.ParentComment).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.IdProductVersionNavigation)
                    .WithMany(p => p.CommentRatings)
                    .HasForeignKey(d => d.IdProductVersion)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_IdProductVersion_CommentRating");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.IdCustomer)
                    .HasName("PK__Customer__DB43864A2C8BDF64");

                entity.HasOne(d => d.IdAccountUserNavigation)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.IdAccountUser)
                    .HasConstraintName("FK_IdAccountUser_Customer");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.IdEmployee)
                    .HasName("PK__Employee__51C8DD7AE168188B");

                entity.HasOne(d => d.IdAccountUserNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.IdAccountUser)
                    .HasConstraintName("FK_IdAccountUser_Employee");
            });

            modelBuilder.Entity<EventSale>(entity =>
            {
                entity.HasKey(e => e.IdEventSale)
                    .HasName("PK__EventSal__520D1894F0A17DE4");

                entity.Property(e => e.IsPublished).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<EventSaleDetail>(entity =>
            {
                entity.HasOne(d => d.IdEventSaleNavigation)
                    .WithMany(p => p.EventSaleDetails)
                    .HasForeignKey(d => d.IdEventSale)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_IdEventSale_EventSaleDetails");

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.EventSaleDetails)
                    .HasForeignKey(d => d.IdProduct)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_IdProduct_EventSaleDetails");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.IdLocation)
                    .HasName("PK__Location__FB5FABA9DAB96A2E");
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.HasKey(e => e.IdNews)
                    .HasName("PK__News__4559C72D8D02F258");

                entity.HasOne(d => d.IdAccountUserNavigation)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.IdAccountUser)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_IdAccountUser_News");

                entity.HasOne(d => d.IdCategoryNewsNavigation)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.IdCategoryNews)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_IdCategoryNews_News");
            });

            modelBuilder.Entity<OrderBill>(entity =>
            {
                entity.HasKey(e => e.IdOrderBill)
                    .HasName("PK__OrderBil__251DF39340F1C368");

                entity.HasOne(d => d.IdAccountUserCustomerNavigation)
                    .WithMany(p => p.OrderBills)
                    .HasForeignKey(d => d.IdAccountUserCustomer)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_IdAccountUserCustomer_OrderBill");

                entity.HasOne(d => d.IdEmployeeNavigation)
                    .WithMany(p => p.OrderBills)
                    .HasForeignKey(d => d.IdEmployee)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_IdEmployee_OrderBill");

                entity.HasOne(d => d.IdStatusOrderNavigation)
                    .WithMany(p => p.OrderBills)
                    .HasForeignKey(d => d.IdStatusOrder)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_IdStatusOrder_OrderBill");
            });

            modelBuilder.Entity<OrderBillDetail>(entity =>
            {
                entity.HasKey(e => e.IdOrderBillDetails)
                    .HasName("PK__OrderBil__3748B2422E04C136");

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

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.IdProduct)
                    .HasName("PK__Product__2E8946D4C12014CE");

                entity.Property(e => e.IsHot).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsPublished).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.IdBrandMobileNavigation)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.IdBrandMobile)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_IdBrandMobile_Product");
            });

            modelBuilder.Entity<ProductColor>(entity =>
            {
                entity.HasKey(e => e.IdProductColor)
                    .HasName("PK__ProductC__44B498FA0111A0D0");

                entity.Property(e => e.IsPublished).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.IdColorNavigation)
                    .WithMany(p => p.ProductColors)
                    .HasForeignKey(d => d.IdColor)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_IdColor_ProductColor");

                entity.HasOne(d => d.IdProductVersionNavigation)
                    .WithMany(p => p.ProductColors)
                    .HasForeignKey(d => d.IdProductVersion)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_IdProductVersion_ProductColor");

                entity.HasOne(d => d.IdStatusProductNavigation)
                    .WithMany(p => p.ProductColors)
                    .HasForeignKey(d => d.IdStatusProduct)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_IdStatusProduct_ProductColor");
            });

            modelBuilder.Entity<ProductVersion>(entity =>
            {
                entity.HasKey(e => e.IdProductVersion)
                    .HasName("PK__ProductV__AD31AF69DE39033A");

                entity.Property(e => e.IsPublished).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.ProductVersions)
                    .HasForeignKey(d => d.IdProduct)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_IdProduct_ProductVersion");
            });

            modelBuilder.Entity<PromotionProduct>(entity =>
            {
                entity.HasKey(e => e.IdPromotionProduct)
                    .HasName("PK__Promotio__2D9BA16801FC3F2C");

                entity.Property(e => e.IsPublished).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.IdProductVersionNavigation)
                    .WithMany(p => p.PromotionProducts)
                    .HasForeignKey(d => d.IdProductVersion)
                    .OnDelete(DeleteBehavior.Cascade)
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
                    .HasName("PK__Role__B43690548D6E1A09");
            });

            modelBuilder.Entity<StatusOrder>(entity =>
            {
                entity.HasKey(e => e.IdStatusOrder)
                    .HasName("PK__StatusOr__361588A83D4EBE97");
            });

            modelBuilder.Entity<StatusProduct>(entity =>
            {
                entity.HasKey(e => e.IdStatusProduct)
                    .HasName("PK__StatusPr__D980E0941136BAE4");
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.HasKey(e => e.IdStore)
                    .HasName("PK__Stores__2A8EB2781A8A8CEB");

                entity.HasOne(d => d.IdLocationNavigation)
                    .WithMany(p => p.Stores)
                    .HasForeignKey(d => d.IdLocation)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_IdLocation_Stores");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
