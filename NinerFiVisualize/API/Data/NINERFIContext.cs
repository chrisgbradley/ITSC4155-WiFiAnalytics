using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NinerFiVisualize.Data.Models;

namespace NinerFiVisualize.Data
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

        public virtual DbSet<VwErrorTracking> VwErrorTrackings { get; set; } = null!;
        public virtual DbSet<VwLogCount> VwLogCounts { get; set; } = null!;

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

                entity.ToView("vw_error_tracking");

                entity.Property(e => e.Hostname).HasMaxLength(4);

                entity.Property(e => e.LogEntries).HasColumnName("log_entries");

                entity.Property(e => e.TypeName)
                    .HasMaxLength(5)
                    .IsFixedLength();
            });

            modelBuilder.Entity<VwLogCount>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_LogCount");

                entity.Property(e => e.NumberOfLogs).HasColumnName("number_of_logs");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
