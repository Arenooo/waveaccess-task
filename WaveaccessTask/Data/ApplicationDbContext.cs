using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WaveaccessTask.Models;

namespace WaveaccessTask.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<WaveaccessTask.Models.Movie> Movie { get; set; }
        public DbSet<WaveaccessTask.Models.Actor> Actor { get; set; }
        public DbSet<WaveaccessTask.Entities.ActorStarredInMovie> ActorStarredInMovie { get; set; }
    }
}