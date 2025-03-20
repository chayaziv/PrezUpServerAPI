using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrezUp.Core.Entity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PrezUp.Data
{
   public class DataContext:DbContext

    {
        public DbSet<Presentation> Presentations { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Tag> Tags { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            base.OnConfiguring(optionsBuilder);
            optionsBuilder.LogTo(mesege => Console.Write(mesege));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // הגדרת הקשר Many-to-Many בין Presentation ל-Tag
            modelBuilder.Entity<Presentation>()
                .HasMany(p => p.Tags)
                .WithMany(t => t.Presentations)
                .UsingEntity<Dictionary<string, object>>(
                    "PresentationTag",  // שם טבלת הקישור
                    j => j.HasOne<Tag>().WithMany().HasForeignKey("TagId"),  // Foreign Key מצד ה-Tag
                    j => j.HasOne<Presentation>().WithMany().HasForeignKey("PresentationId")  // Foreign Key מצד ה-Presentation
                );
        }
    }
}
