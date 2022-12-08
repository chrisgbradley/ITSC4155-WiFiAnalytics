using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NinerFiVisualize.API.Data.Models;

namespace NinerFiVisualize.API.Data
{
    public partial class NINERFIContext : DbContext
    {
        public NINERFIContext()
        {
        }

        public NINERFIContext(DbContextOptions<NINERFIContext> options)
            : base(options)
        {
        }

        public virtual DbSet<VwErrorTracking> VwErrorTracking { get; set; } = null!;
        public virtual DbSet<VwLogCount> VwLogCount { get; set; } = null!;
        public virtual DbSet<VwTrafficStats> VwTrafficStats { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:NINERFI");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VwErrorTracking>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwErrorTracking");

                entity.Property(e => e.Hostname)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.LogEntries).HasColumnName("log_entries");

                entity.Property(e => e.TypeName)
                    .HasMaxLength(5)
                    .IsFixedLength();
            });

            modelBuilder.Entity<VwLogCount>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwLogCount");

                entity.Property(e => e.NumberOfLogs).HasColumnName("number_of_logs");
            });

            modelBuilder.Entity<VwTrafficStats>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwTrafficStats");

                entity.Property(e => e.Hostname)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.LogEntries).HasColumnName("log_entries");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
