using System;
using Microsoft.EntityFrameworkCore;
using Pagila.API.Models;

namespace Pagila.API.Data;

public partial class PagilaDbContext : DbContext
{
    public PagilaDbContext(DbContextOptions<PagilaDbContext> options) : base(options) {}

    public virtual DbSet<Actor> Actors { get; set; }

    public virtual DbSet<ActorInfo> ActorInfos { get; set; }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerList> CustomerLists { get; set; }

    public virtual DbSet<Film> Films { get; set; }

    public virtual DbSet<FilmActor> FilmActors { get; set; }

    public virtual DbSet<FilmCategory> FilmCategories { get; set; }

    public virtual DbSet<FilmList> FilmLists { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<NicerButSlowerFilmList> NicerButSlowerFilmLists { get; set; }

    public virtual DbSet<PaymentP202201> PaymentP202201s { get; set; }

    public virtual DbSet<PaymentP202202> PaymentP202202s { get; set; }

    public virtual DbSet<PaymentP202203> PaymentP202203s { get; set; }

    public virtual DbSet<PaymentP202204> PaymentP202204s { get; set; }

    public virtual DbSet<PaymentP202205> PaymentP202205s { get; set; }

    public virtual DbSet<PaymentP202206> PaymentP202206s { get; set; }

    public virtual DbSet<PaymentP202207> PaymentP202207s { get; set; }

    public virtual DbSet<Rental> Rentals { get; set; }

    public virtual DbSet<RentalByCategory> RentalByCategories { get; set; }

    public virtual DbSet<SalesByFilmCategory> SalesByFilmCategories { get; set; }

    public virtual DbSet<SalesByStore> SalesByStores { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<StaffList> StaffLists { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum("mpaa_rating", new[] { "G", "PG", "PG-13", "R", "NC-17" });

        modelBuilder.Entity<Actor>(entity =>
        {
            entity.HasKey(e => e.ActorId).HasName("actor_pkey");

            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<ActorInfo>(entity =>
        {
            entity.ToView("actor_info");
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("address_pkey");

            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.City).WithMany(p => p.Addresses)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("address_city_id_fkey");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("category_pkey");

            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("city_pkey");

            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Country).WithMany(p => p.Cities)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("city_country_id_fkey");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("country_pkey");

            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("customer_pkey");

            entity.Property(e => e.Activebool).HasDefaultValue(true);
            entity.Property(e => e.CreateDate).HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Address).WithMany(p => p.Customers)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("customer_address_id_fkey");

            entity.HasOne(d => d.Store).WithMany(p => p.Customers)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("customer_store_id_fkey");
        });

        modelBuilder.Entity<CustomerList>(entity =>
        {
            entity.ToView("customer_list");
        });

        modelBuilder.Entity<Film>(entity =>
        {
            entity.HasKey(e => e.FilmId).HasName("film_pkey");

            entity.HasIndex(e => e.Fulltext, "film_fulltext_idx").HasMethod("gist");

            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.RentalDuration).HasDefaultValue((short)3);
            entity.Property(e => e.RentalRate).HasDefaultValueSql("4.99");
            entity.Property(e => e.ReplacementCost).HasDefaultValueSql("19.99");

            entity.HasOne(d => d.Language).WithMany(p => p.FilmLanguages)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("film_language_id_fkey");

            entity.HasOne(d => d.OriginalLanguage).WithMany(p => p.FilmOriginalLanguages)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("film_original_language_id_fkey");
        });

        modelBuilder.Entity<FilmActor>(entity =>
        {
            entity.HasKey(e => new { e.ActorId, e.FilmId }).HasName("film_actor_pkey");

            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Actor).WithMany(p => p.FilmActors)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("film_actor_actor_id_fkey");

            entity.HasOne(d => d.Film).WithMany(p => p.FilmActors)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("film_actor_film_id_fkey");
        });

        modelBuilder.Entity<FilmCategory>(entity =>
        {
            entity.HasKey(e => new { e.FilmId, e.CategoryId }).HasName("film_category_pkey");

            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Category).WithMany(p => p.FilmCategories)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("film_category_category_id_fkey");

            entity.HasOne(d => d.Film).WithMany(p => p.FilmCategories)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("film_category_film_id_fkey");
        });

        modelBuilder.Entity<FilmList>(entity =>
        {
            entity.ToView("film_list");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.InventoryId).HasName("inventory_pkey");

            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Film).WithMany(p => p.Inventories)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("inventory_film_id_fkey");

            entity.HasOne(d => d.Store).WithMany(p => p.Inventories)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("inventory_store_id_fkey");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.LanguageId).HasName("language_pkey");

            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.Name).IsFixedLength();
        });

        modelBuilder.Entity<NicerButSlowerFilmList>(entity =>
        {
            entity.ToView("nicer_but_slower_film_list");
        });

        modelBuilder.Entity<PaymentP202201>(entity =>
        {
            entity.HasKey(e => new { e.PaymentDate, e.PaymentId }).HasName("payment_p2022_01_pkey");

            entity.Property(e => e.PaymentId).HasDefaultValueSql("nextval('payment_payment_id_seq'::regclass)");

            entity.HasOne(d => d.Customer).WithMany(p => p.PaymentP202201s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payment_p2022_01_customer_id_fkey");

            entity.HasOne(d => d.Rental).WithMany(p => p.PaymentP202201s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payment_p2022_01_rental_id_fkey");

            entity.HasOne(d => d.Staff).WithMany(p => p.PaymentP202201s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payment_p2022_01_staff_id_fkey");
        });

        modelBuilder.Entity<PaymentP202202>(entity =>
        {
            entity.HasKey(e => new { e.PaymentDate, e.PaymentId }).HasName("payment_p2022_02_pkey");

            entity.Property(e => e.PaymentId).HasDefaultValueSql("nextval('payment_payment_id_seq'::regclass)");

            entity.HasOne(d => d.Customer).WithMany(p => p.PaymentP202202s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payment_p2022_02_customer_id_fkey");

            entity.HasOne(d => d.Rental).WithMany(p => p.PaymentP202202s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payment_p2022_02_rental_id_fkey");

            entity.HasOne(d => d.Staff).WithMany(p => p.PaymentP202202s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payment_p2022_02_staff_id_fkey");
        });

        modelBuilder.Entity<PaymentP202203>(entity =>
        {
            entity.HasKey(e => new { e.PaymentDate, e.PaymentId }).HasName("payment_p2022_03_pkey");

            entity.Property(e => e.PaymentId).HasDefaultValueSql("nextval('payment_payment_id_seq'::regclass)");

            entity.HasOne(d => d.Customer).WithMany(p => p.PaymentP202203s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payment_p2022_03_customer_id_fkey");

            entity.HasOne(d => d.Rental).WithMany(p => p.PaymentP202203s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payment_p2022_03_rental_id_fkey");

            entity.HasOne(d => d.Staff).WithMany(p => p.PaymentP202203s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payment_p2022_03_staff_id_fkey");
        });

        modelBuilder.Entity<PaymentP202204>(entity =>
        {
            entity.HasKey(e => new { e.PaymentDate, e.PaymentId }).HasName("payment_p2022_04_pkey");

            entity.Property(e => e.PaymentId).HasDefaultValueSql("nextval('payment_payment_id_seq'::regclass)");

            entity.HasOne(d => d.Customer).WithMany(p => p.PaymentP202204s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payment_p2022_04_customer_id_fkey");

            entity.HasOne(d => d.Rental).WithMany(p => p.PaymentP202204s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payment_p2022_04_rental_id_fkey");

            entity.HasOne(d => d.Staff).WithMany(p => p.PaymentP202204s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payment_p2022_04_staff_id_fkey");
        });

        modelBuilder.Entity<PaymentP202205>(entity =>
        {
            entity.HasKey(e => new { e.PaymentDate, e.PaymentId }).HasName("payment_p2022_05_pkey");

            entity.Property(e => e.PaymentId).HasDefaultValueSql("nextval('payment_payment_id_seq'::regclass)");

            entity.HasOne(d => d.Customer).WithMany(p => p.PaymentP202205s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payment_p2022_05_customer_id_fkey");

            entity.HasOne(d => d.Rental).WithMany(p => p.PaymentP202205s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payment_p2022_05_rental_id_fkey");

            entity.HasOne(d => d.Staff).WithMany(p => p.PaymentP202205s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payment_p2022_05_staff_id_fkey");
        });

        modelBuilder.Entity<PaymentP202206>(entity =>
        {
            entity.HasKey(e => new { e.PaymentDate, e.PaymentId }).HasName("payment_p2022_06_pkey");

            entity.Property(e => e.PaymentId).HasDefaultValueSql("nextval('payment_payment_id_seq'::regclass)");

            entity.HasOne(d => d.Customer).WithMany(p => p.PaymentP202206s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payment_p2022_06_customer_id_fkey");

            entity.HasOne(d => d.Rental).WithMany(p => p.PaymentP202206s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payment_p2022_06_rental_id_fkey");

            entity.HasOne(d => d.Staff).WithMany(p => p.PaymentP202206s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payment_p2022_06_staff_id_fkey");
        });

        modelBuilder.Entity<PaymentP202207>(entity =>
        {
            entity.HasKey(e => new { e.PaymentDate, e.PaymentId }).HasName("payment_p2022_07_pkey");

            entity.Property(e => e.PaymentId).HasDefaultValueSql("nextval('payment_payment_id_seq'::regclass)");
        });

        modelBuilder.Entity<Rental>(entity =>
        {
            entity.HasKey(e => e.RentalId).HasName("rental_pkey");

            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Customer).WithMany(p => p.Rentals)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("rental_customer_id_fkey");

            entity.HasOne(d => d.Inventory).WithMany(p => p.Rentals)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("rental_inventory_id_fkey");

            entity.HasOne(d => d.Staff).WithMany(p => p.Rentals)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("rental_staff_id_fkey");
        });

        modelBuilder.Entity<RentalByCategory>(entity =>
        {
            entity.ToView("rental_by_category");
        });

        modelBuilder.Entity<SalesByFilmCategory>(entity =>
        {
            entity.ToView("sales_by_film_category");
        });

        modelBuilder.Entity<SalesByStore>(entity =>
        {
            entity.ToView("sales_by_store");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("staff_pkey");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Address).WithMany(p => p.Staff)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("staff_address_id_fkey");

            entity.HasOne(d => d.Store).WithMany(p => p.Staff)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("staff_store_id_fkey");
        });

        modelBuilder.Entity<StaffList>(entity =>
        {
            entity.ToView("staff_list");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.StoreId).HasName("store_pkey");

            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Address).WithMany(p => p.Stores)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("store_address_id_fkey");
        });
        modelBuilder.HasSequence("actor_actor_id_seq");
        modelBuilder.HasSequence("address_address_id_seq");
        modelBuilder.HasSequence("category_category_id_seq");
        modelBuilder.HasSequence("city_city_id_seq");
        modelBuilder.HasSequence("country_country_id_seq");
        modelBuilder.HasSequence("customer_customer_id_seq");
        modelBuilder.HasSequence("film_film_id_seq");
        modelBuilder.HasSequence("inventory_inventory_id_seq");
        modelBuilder.HasSequence("language_language_id_seq");
        modelBuilder.HasSequence("payment_payment_id_seq");
        modelBuilder.HasSequence("rental_rental_id_seq");
        modelBuilder.HasSequence("staff_staff_id_seq");
        modelBuilder.HasSequence("store_store_id_seq");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
