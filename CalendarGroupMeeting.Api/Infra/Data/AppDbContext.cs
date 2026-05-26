using Microsoft.EntityFrameworkCore;
using CalendarGroupMeeting.Api.Domain.Entities;

namespace CalendarGroupMeeting.Api.Infra.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Event> Events { get; set; }
    public DbSet<EventOption> EventOptions { get; set; }
    public DbSet<Response> Responses { get; set; }
    public DbSet<ResponseOption> ResponseOptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Token).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.Token).IsUnique();
            entity.Property(e => e.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<EventOption>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StartDateTime).IsRequired();
            entity.Property(e => e.EndDateTime).IsRequired();

            entity.HasOne(e => e.Event)
                .WithMany(e => e.Options)
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Response>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Token).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Token).IsUnique();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();

            entity.HasOne(e => e.Event)
                .WithMany(e => e.Responses)
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.EventId, e.Name }).IsUnique();
        });

        modelBuilder.Entity<ResponseOption>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Response)
                .WithMany(r => r.ResponseOptions)
                .HasForeignKey(e => e.ResponseId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.EventOption)
                .WithMany(o => o.ResponseOptions)
                .HasForeignKey(e => e.EventOptionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.ResponseId, e.EventOptionId }).IsUnique();
        });
    }
}
