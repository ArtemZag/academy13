using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database
{
    public class DatabaseInitializer: DropCreateDatabaseIfModelChanges<DatabaseContext>
    {
        protected override void Seed(DatabaseContext databaseContext)
        {
            databaseContext.Users.Add(new UserModel());



            databaseContext.SaveChanges();
            base.Seed(databaseContext); 
        }
    }
}
