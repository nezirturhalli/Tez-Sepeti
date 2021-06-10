using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TezSepeti.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }
        public DbSet<UserModel> TezUsers { get; set; }
        public DbSet<ProjectModel> TezProjects { get; set; }
        public DbSet<FavModel> TezFav { get; set; }
        public DbSet<LikeModel> TezLike { get; set; }
        


    }
}
