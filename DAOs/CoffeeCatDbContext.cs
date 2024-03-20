using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models;

namespace DAOs;

public partial class CoffeeCatDbContext : DbContext
{
    public CoffeeCatDbContext()
    {
    }

    public CoffeeCatDbContext(DbContextOptions<CoffeeCatDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<AreaCat> AreaCats { get; set; }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<BillProduct> BillProducts { get; set; }

    public virtual DbSet<Cat> Cats { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<ReservationTable> ReservationTables { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Table> Tables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString());

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", true, true)
        .Build();
        var strConn = config["ConnectionStrings:CoffeeCatDB"];

        return strConn;
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.AreaId).HasName("PK__area__52936C37BB5FFC7C");

            entity.ToTable("area");

            entity.Property(e => e.AreaId).HasColumnName("areaID");
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .HasColumnName("location");
            entity.Property(e => e.ShopId).HasColumnName("shopID");

            entity.HasOne(d => d.Shop).WithMany(p => p.Areas)
                .HasForeignKey(d => d.ShopId)
                .HasConstraintName("FK__area__shopID__2A4B4B5E");
        });

        modelBuilder.Entity<AreaCat>(entity =>
        {
            entity.HasKey(e => e.AreaCatId).HasName("PK__areaCat__4FED9561CE937B6F");

            entity.ToTable("areaCat");

            entity.Property(e => e.AreaCatId).HasColumnName("areaCatID");
            entity.Property(e => e.AreaId).HasColumnName("areaID");
            entity.Property(e => e.CatId).HasColumnName("catID");
            entity.Property(e => e.Date)
                .HasColumnType("date")
                .HasColumnName("date");

            entity.HasOne(d => d.Area).WithMany(p => p.AreaCats)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("FK__areaCat__areaID__3C69FB99");

            entity.HasOne(d => d.Cat).WithMany(p => p.AreaCats)
                .HasForeignKey(d => d.CatId)
                .HasConstraintName("FK__areaCat__catID__3D5E1FD2");
        });

        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(e => e.BillId).HasName("PK__bill__6D903F2365E8C12E");

            entity.ToTable("bill");

            entity.Property(e => e.BillId).HasColumnName("billID");
            entity.Property(e => e.Note)
                .HasMaxLength(200)
                .HasColumnName("note");
            entity.Property(e => e.PaymentTime)
                .HasColumnType("datetime")
                .HasColumnName("paymentTime");
            entity.Property(e => e.PromotionId).HasColumnName("promotionID");
            entity.Property(e => e.ReservationId).HasColumnName("reservationID");
            entity.Property(e => e.StaffId).HasColumnName("staffID");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("totalPrice");

            entity.HasOne(d => d.Promotion).WithMany(p => p.Bills)
                .HasForeignKey(d => d.PromotionId)
                .HasConstraintName("FK__bill__promotionI__5165187F");

            entity.HasOne(d => d.Reservation).WithMany(p => p.Bills)
                .HasForeignKey(d => d.ReservationId)
                .HasConstraintName("FK__bill__reservatio__4F7CD00D");

            entity.HasOne(d => d.Staff).WithMany(p => p.Bills)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK__bill__staffID__5070F446");
        });

        modelBuilder.Entity<BillProduct>(entity =>
        {
            entity.HasKey(e => e.BillProductId).HasName("PK__billProd__7D873D31E92DE9C2");

            entity.ToTable("billProduct");

            entity.Property(e => e.BillProductId).HasColumnName("billProductID");
            entity.Property(e => e.BillId).HasColumnName("billID");
            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Bill).WithMany(p => p.BillProducts)
                .HasForeignKey(d => d.BillId)
                .HasConstraintName("FK__billProdu__billI__5441852A");

            entity.HasOne(d => d.Product).WithMany(p => p.BillProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__billProdu__produ__5535A963");
        });

        modelBuilder.Entity<Cat>(entity =>
        {
            entity.HasKey(e => e.CatId).HasName("PK__cat__17B6DD26D814FFB8");

            entity.ToTable("cat");

            entity.Property(e => e.CatId).HasColumnName("catID");
            entity.Property(e => e.Birthday)
                .HasColumnType("date")
                .HasColumnName("birthday");
            entity.Property(e => e.Breed)
                .HasMaxLength(50)
                .HasColumnName("breed");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.HealthStatus)
                .HasMaxLength(255)
                .HasColumnName("healthStatus");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.ShopId).HasColumnName("shopID");

            entity.HasOne(d => d.Shop).WithMany(p => p.Cats)
                .HasForeignKey(d => d.ShopId)
                .HasConstraintName("FK__cat__shopID__398D8EEE");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__customer__B611CB9DB4021FD3");

            entity.ToTable("customer");

            entity.HasIndex(e => e.Email, "UQ__customer__AB6E61648BD542B5").IsUnique();

            entity.Property(e => e.CustomerId).HasColumnName("customerID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__product__2D10D16A5DC7F3BE");

            entity.ToTable("product");

            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.ShopId).HasColumnName("shopID");

            entity.HasOne(d => d.Shop).WithMany(p => p.Products)
                .HasForeignKey(d => d.ShopId)
                .HasConstraintName("FK__product__shopID__48CFD27E");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.PromotionId).HasName("PK__promotio__99EB690ED185D7C3");

            entity.ToTable("promotion");

            entity.Property(e => e.PromotionId).HasColumnName("promotionID");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.PromotionAmount).HasColumnName("promotionAmount");
            entity.Property(e => e.PromotionType).HasColumnName("promotionType");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.ReservationId).HasName("PK__reservat__B14BF5A5CEDD8FD8");

            entity.ToTable("reservation", tb => tb.HasTrigger("CalculateTotalPrice"));

            entity.Property(e => e.ReservationId).HasColumnName("reservationID");
            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                .HasColumnName("createTime");
            entity.Property(e => e.ArrivalDate)
                .HasColumnType("date")
                .HasColumnName("arrivalDate");
            entity.Property(e => e.CustomerId).HasColumnName("customerID");
            entity.Property(e => e.EndTime).HasColumnName("endTime");
            entity.Property(e => e.StartTime).HasColumnName("startTime");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("totalPrice");

            entity.HasOne(d => d.Customer).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__reservati__custo__31EC6D26");
        });

        modelBuilder.Entity<ReservationTable>(entity =>
        {
            entity.HasKey(e => e.ReservationTableId).HasName("PK__reservat__908010E7847214AC");

            entity.ToTable("reservationTable");

            entity.Property(e => e.ReservationTableId).HasColumnName("reservationTableID");
            entity.Property(e => e.ReservationId).HasColumnName("reservationID");
            entity.Property(e => e.TableId).HasColumnName("tableID");

            entity.HasOne(d => d.Reservation).WithMany(p => p.ReservationTables)
                .HasForeignKey(d => d.ReservationId)
                .HasConstraintName("FK__reservati__reser__34C8D9D1");

            entity.HasOne(d => d.Table).WithMany(p => p.ReservationTables)
                .HasForeignKey(d => d.TableId)
                .HasConstraintName("FK__reservati__table__35BCFE0A");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__role__CD98462A53A47693");

            entity.ToTable("role");

            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Shop>(entity =>
        {
            entity.HasKey(e => e.ShopId).HasName("PK__shop__E5C424FCF56A3543");

            entity.ToTable("shop");

            entity.Property(e => e.ShopId).HasColumnName("shopID");
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("contactNumber");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .HasColumnName("location");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__staff__6465E19EF5141E3E");

            entity.ToTable("staff");

            entity.HasIndex(e => e.Email, "UQ__staff__AB6E61644AD6B501").IsUnique();

            entity.Property(e => e.StaffId).HasColumnName("staffID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.Property(e => e.ShopId).HasColumnName("shopID");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Role).WithMany(p => p.Staff)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__staff__roleId__44FF419A");

            entity.HasOne(d => d.Shop).WithMany(p => p.Staff)
                .HasForeignKey(d => d.ShopId)
                .HasConstraintName("FK__staff__shopID__45F365D3");
        });

        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasKey(e => e.TableId).HasName("PK__table__5408ADBAF550A637");

            entity.ToTable("table");

            entity.Property(e => e.TableId).HasColumnName("tableID");
            entity.Property(e => e.AreaId).HasColumnName("areaID");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Area).WithMany(p => p.Tables)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("FK__table__areaID__2E1BDC42");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
