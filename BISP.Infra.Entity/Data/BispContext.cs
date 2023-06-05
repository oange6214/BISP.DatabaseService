using System;
using System.Collections.Generic;
using BISP.Infra.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace BISP.Infra.Entity.Data;

public partial class BispContext : DbContext
{

    public BispContext()
    {
    }

    public BispContext(DbContextOptions<BispContext> options)
        : base(options)
    {
    }

    public virtual DbSet<OfileInfo> OfileInfos { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        //optionsBuilder.UseNpgsql("Server=localhost;Database=BISP;User Id=postgres;Password=;");
        //optionsBuilder.UseMySql(connectionString: "Server=localhost;port=3306;Database=BISP;User=root;Password=;",
        //    new MySqlServerVersion(new Version(10, 4, 17)));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.Property(e => e.Guid).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
