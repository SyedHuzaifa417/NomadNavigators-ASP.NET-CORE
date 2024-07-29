using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NomadNavigator_BE_.Models;

public partial class NNContext : DbContext
{
    public NNContext()
    {
    }

    public NNContext(DbContextOptions<NNContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Flight> Flights { get; set; }

    public virtual DbSet<Partner> Partners { get; set; }

    public virtual DbSet<Place> Places { get; set; }

    public virtual DbSet<PlaceGreatFor> PlaceGreatFors { get; set; }

    public virtual DbSet<Restaurant> Restaurants { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=syedhuzaifa\\sqlexpress;Initial Catalog=N_N;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Bookings__3213E83FE2D8229C");

            entity.HasOne(d => d.Flight).WithMany(p => p.Bookings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bookings__flight__19DFD96B");

            entity.HasOne(d => d.Place).WithMany(p => p.Bookings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bookings__placeI__17F790F9");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.Bookings).HasConstraintName("FK__Bookings__restau__18EBB532");
        });

        modelBuilder.Entity<Flight>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Flights__3213E83FF7041841");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Partner>(entity =>
        {
            entity.HasKey(e => e.PartnerId).HasName("PK__Partners__6E8AD1743E5FF7AF");
        });

        modelBuilder.Entity<Place>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Places__3213E83FD2F989F8");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<PlaceGreatFor>(entity =>
        {
            entity.HasKey(e => new { e.PlaceId, e.GreatFor }).HasName("PK__PlaceGre__AB17685E5E6055A9");

            entity.HasOne(d => d.Place).WithMany(p => p.PlaceGreatFors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PlaceGrea__place__71D1E811");
        });

        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Restaura__3213E83FF45A4C6B");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Place).WithMany(p => p.Restaurants).HasConstraintName("FK__Restauran__place__05D8E0BE");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Review__3213E83F6EC0895D");

            entity.Property(e => e.Date).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C657CE613");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
