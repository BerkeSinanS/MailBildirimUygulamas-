using System;
using System.Collections.Generic;
using MailBildirimUygulaması.Models;
using Microsoft.EntityFrameworkCore;

namespace MailBildirimUygulaması.Data;

public partial class DegisiklikFormuContext : DbContext
{
    public DegisiklikFormuContext()
    {
    }

    public DegisiklikFormuContext(DbContextOptions<DegisiklikFormuContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EskiYeniKodlar> EskiYeniKodlars { get; set; }

    public virtual DbSet<Formlar> Formlars { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432; User Id=postgres;Database=Degisiklik_Formu;Username=Berke;Password=BS1603");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EskiYeniKodlar>(entity =>
        {
            entity.HasKey(e => e.KodId);

            entity.ToTable("EskiYeniKodlar");

            entity.HasIndex(e => e.FormId, "IX_EskiYeniKodlar_FormID");

            entity.Property(e => e.KodId).HasColumnName("KodID");
            entity.Property(e => e.Adet).HasDefaultValue(0);
            entity.Property(e => e.FormId).HasColumnName("FormID");

            entity.HasOne(d => d.Form).WithMany(p => p.EskiYeniKodlars).HasForeignKey(d => d.FormId);
        });

        modelBuilder.Entity<Formlar>(entity =>
        {
            entity.HasKey(e => e.FormID);

            entity.ToTable("Formlar");

            entity.Property(e => e.FormID).HasColumnName("FormID");
            entity.Property(e => e.Yayınlayan).HasDefaultValueSql("''::text");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
