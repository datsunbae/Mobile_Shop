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
        public virtual DbSet<CommentNews> CommentNews { get; set; } = null!;
        public virtual DbSet<CommentProduct> CommentProducts { get; set; } = null!;
        public virtual DbSet<CommentRating> CommentRatings { get; set; } = null!;
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
                    .HasName("PK__AccountU__76959AFB8CBAEEC7");

                entity.ToTable("AccountUser");

                entity.Property(e => e.AddressUser).HasMaxLength(255);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FullName).HasMaxLength(255);

                entity.Property(e => e.Images)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastLogin).HasColumnType("datetime");

                entity.Property(e => e.PasswordAccount)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.AccountUsers)
                    .HasForeignKey(d => d.IdRole)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_IdRole_AccountUser");
            });

            modelBuilder.Entity<BrandMobile>(entity =>
            {
                entity.HasKey(e => e.IdBrandMobile)
                    .HasName("PK__BrandMob__3D412B03701C6A0A");

                entity.ToTable("BrandMobile");

                entity.Property(e => e.ImgBrand)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.IsPublished).HasDefaultValueSql("((0))");

                entity.Property(e => e.NameBrand).HasMaxLength(100);
            });

            modelBuilder.Entity<CategoryNews>(entity =>
            {
                entity.HasKey(e => e.IdCategoryNews)
                    .HasName("PK__Category__002B75A5787E1923");

                entity.Property(e => e.NameCategory).HasMaxLength(255);
            });

            modelBuilder.Entity<CommentNews>(entity =>
            {
                entity.HasKey(e => e.IdCommentNew)
                    .HasName("PK__CommentN__708A0203BB49DA88");

                entity.Property(e => e.Content).HasMaxLength(255);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.OptionEmail)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.OptionFullName).HasMaxLength(255);

                entity.Property(e => e.OptionPhone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

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
                    .HasName("PK__CommentP__542057BCB75C105E");

                entity.ToTable("CommentProduct");

                entity.Property(e => e.Content).HasMaxLength(255);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.OptionEmail)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.OptionFullName).HasMaxLength(255);

                entity.Property(e => e.OptionPhone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

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
                    .HasName("PK__CommentR__F4D21E306CB44D83");

                entity.ToTable("CommentRating");

                entity.Property(e => e.Content).HasMaxLength(255);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.OptionEmail)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.OptionFullName).HasMaxLength(255);

                entity.Property(e => e.OptionPhone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ParentComment).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.IdProductVersionNavigation)
                    .WithMany(p => p.CommentRatings)
                    .HasForeignKey(d => d.IdProductVersion)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_IdProductVersion_CommentRating");
            });

            modelBuilder.Entity<EventSale>(entity =>
            {
                entity.HasKey(e => e.IdEventSale)
                    .HasName("PK__EventSal__520D18949083A37F");

                entity.ToTable("EventSale");

                entity.Property(e => e.EndDateTime).HasColumnType("datetime");

                entity.Property(e => e.IsPublished).HasDefaultValueSql("((0))");

                entity.Property(e => e.NameEventSale).HasMaxLength(255);

                entity.Property(e => e.StartDateTime).HasColumnType("datetime");
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
                    .HasName("PK__Location__FB5FABA9F1ECCAC3");

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.District).HasMaxLength(100);
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.HasKey(e => e.IdNews)
                    .HasName("PK__News__4559C72D929F6A36");

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DescriptionNew).HasMaxLength(255);

                entity.Property(e => e.Thumb)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Title).HasMaxLength(255);

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
                    .HasName("PK__OrderBil__251DF393902953FC");

                entity.ToTable("OrderBill");

                entity.Property(e => e.Note).HasMaxLength(255);

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.ShipDate).HasColumnType("datetime");

                entity.HasOne(d => d.IdAccountUserNavigation)
                    .WithMany(p => p.OrderBills)
                    .HasForeignKey(d => d.IdAccountUser)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_IdAccountUser_OrderBill");

                entity.HasOne(d => d.IdStatusOrderNavigation)
                    .WithMany(p => p.OrderBills)
                    .HasForeignKey(d => d.IdStatusOrder)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_IdStatusOrder_OrderBill");
            });

            modelBuilder.Entity<OrderBillDetail>(entity =>
            {
                entity.HasKey(e => e.IdOrderBillDetails)
                    .HasName("PK__OrderBil__3748B242CBDC1C6F");

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
                    .HasName("PK__Product__2E8946D484312E25");

                entity.ToTable("Product");

                entity.Property(e => e.IsHot).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsPublished).HasDefaultValueSql("((0))");

                entity.Property(e => e.NameProduct).HasMaxLength(255);

                entity.HasOne(d => d.IdBrandMobileNavigation)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.IdBrandMobile)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_IdBrandMobile_Product");
            });

            modelBuilder.Entity<ProductColor>(entity =>
            {
                entity.HasKey(e => e.IdProductColor)
                    .HasName("PK__ProductC__44B498FAEABD7A98");

                entity.ToTable("ProductColor");

                entity.Property(e => e.AvailableAtShop)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Color).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ImgProductColor)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.IsPublished).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.IdStatusProductNavigation)
                    .WithMany(p => p.ProductColors)
                    .HasForeignKey(d => d.IdStatusProduct)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_IdProductVersion_ProductColor");

                entity.HasOne(d => d.IdStatusProduct1)
                    .WithMany(p => p.ProductColors)
                    .HasForeignKey(d => d.IdStatusProduct)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_IdStatusProduct_ProductColor");
            });

            modelBuilder.Entity<ProductVersion>(entity =>
            {
                entity.HasKey(e => e.IdProductVersion)
                    .HasName("PK__ProductV__AD31AF69251D3048");

                entity.ToTable("ProductVersion");

                entity.Property(e => e.Desciprtion).HasColumnType("ntext");

                entity.Property(e => e.IsPublished).HasDefaultValueSql("((0))");

                entity.Property(e => e.NameProductVersion).HasMaxLength(255);

                entity.Property(e => e.TechnicalParameters)
                    .HasColumnType("ntext")
                    .HasColumnName("Technical_Parameters");

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.ProductVersions)
                    .HasForeignKey(d => d.IdProduct)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_IdProduct_ProductVersion");
            });

            modelBuilder.Entity<PromotionProduct>(entity =>
            {
                entity.HasKey(e => e.IdPromotionProduct)
                    .HasName("PK__Promotio__2D9BA1685434C900");

                entity.ToTable("PromotionProduct");

                entity.Property(e => e.EndDateTime).HasColumnType("datetime");

                entity.Property(e => e.IsPublished).HasDefaultValueSql("((0))");

                entity.Property(e => e.StartDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.IdProductVersionNavigation)
                    .WithMany(p => p.PromotionProducts)
                    .HasForeignKey(d => d.IdProductVersion)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_IdProductVersion_PromotionProduct");
            });

            modelBuilder.Entity<PromotionProductDetail>(entity =>
            {
                entity.Property(e => e.NamePromotionProduct).HasMaxLength(255);

                entity.Property(e => e.Urlpromotion)
                    .HasMaxLength(255)
                    .HasColumnName("URLPromotion");

                entity.HasOne(d => d.IdPromotionProductNavigation)
                    .WithMany(p => p.PromotionProductDetails)
                    .HasForeignKey(d => d.IdPromotionProduct)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_IdPromotionProduct_PromotionProductDetails");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.ToTable("Rating");

                entity.HasOne(d => d.IdProductVersionNavigation)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.IdProductVersion)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_IdProductVersion_Rating");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.IdRole)
                    .HasName("PK__Role__B43690546E07219B");

                entity.ToTable("Role");

                entity.Property(e => e.RoleName).HasMaxLength(100);
            });

            modelBuilder.Entity<StatusOrder>(entity =>
            {
                entity.HasKey(e => e.IdStatusOrder)
                    .HasName("PK__StatusOr__361588A84EA6227E");

                entity.ToTable("StatusOrder");

                entity.Property(e => e.NameStatus).HasMaxLength(100);
            });

            modelBuilder.Entity<StatusProduct>(entity =>
            {
                entity.HasKey(e => e.IdStatusProduct)
                    .HasName("PK__StatusPr__D980E094B1182F45");

                entity.ToTable("StatusProduct");

                entity.Property(e => e.NameStatus).HasMaxLength(100);
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.HasKey(e => e.IdStore)
                    .HasName("PK__Stores__2A8EB27859356710");

                entity.Property(e => e.AddressStore).HasMaxLength(255);

                entity.Property(e => e.ImgStore)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NameStore).HasMaxLength(255);

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

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
