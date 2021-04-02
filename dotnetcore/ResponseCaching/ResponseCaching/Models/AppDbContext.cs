using Microsoft.EntityFrameworkCore;
using NCacheResponseCaching.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResponseCaching.Models
{
    public class AppDbContext: DbContext
    {
        /// <summary>
        ///     Using EFCore to interact with database.
        /// </summary>
        /// <param name="options"></param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        
        }
        public DbSet<Product> Products { get; set; }
    }
}
