using Microsoft.EntityFrameworkCore;
using ButceTakip.Models;

namespace ButceTakip.Data
{
    public class UygulamaDbContext : DbContext
    {
        public UygulamaDbContext(DbContextOptions<UygulamaDbContext> options) : base(options)
        {
        }

        // Tablolarımızın veritabanındaki karşılıkları
        public DbSet<Islem> Islemler { get; set; }
        public DbSet<Kategori> Kategoriler { get; set; }
    }
}