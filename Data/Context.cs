using Module6Tp1Dojo_BO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Module6Tp1Dojo.Data
{
    public class Context : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public Context() : base("name=Context")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Samourai>()
                .HasMany<ArtMartial>(s => s.ArtMartiaux)
                .WithMany(am => am.samourais);
        }

        public System.Data.Entity.DbSet<Samourai> Samourais { get; set; }

        public System.Data.Entity.DbSet<Arme> Armes { get; set; }

        public System.Data.Entity.DbSet<Module6Tp1Dojo_BO.ArtMartial> ArtMartials { get; set; }
    }
}
