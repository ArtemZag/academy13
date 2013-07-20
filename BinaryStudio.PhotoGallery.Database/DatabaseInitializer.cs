using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            var user = new UserModel();

            var authInfo = new AuthInfoModel();
            var album = new AlbumModel();
            var group = new GroupModel();

            user.AuthInfos = new Collection<AuthInfoModel>();
            user.Albums = new Collection<AlbumModel>();
            user.Groups = new Collection<GroupModel>();

            user.Department = "C# prommer";
            user.Email = "Maaak@gmail.com";
            user.ID = 123;
            user.IsAdmin = true;
            user.FirstName = "Alexander";
            user.LastName = "Towstonog";

            user.AuthInfos.Add(authInfo);
            user.Albums.Add(album);
            user.Groups.Add(group);

            databaseContext.Users.Add(user);



            databaseContext.SaveChanges();
            base.Seed(databaseContext); 
        }
    }
}
