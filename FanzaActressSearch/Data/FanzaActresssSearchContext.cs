using Microsoft.EntityFrameworkCore;
using FanzaActressSearch.Models;

namespace FanzaActressSearch.Data
{
    public class FanzaActressSearchContext : DbContext
    {
        public FanzaActressSearchContext (DbContextOptions<FanzaActressSearchContext> options)
            : base(options)
        {}

        public DbSet<Actress> Actress { get; set; }
        public DbSet<CupBust> CupBust { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ActressProduct> ActressProduct { get; set; }
        public DbSet<ActressScrapingResult> ActressScrapingResult { get; set; }
        public DbSet<Search> Search { get; set; }
    }
}
