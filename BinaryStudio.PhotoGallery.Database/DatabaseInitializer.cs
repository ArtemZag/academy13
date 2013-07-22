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
            var userFirstNames = new[] {"Artem", "Anton", "Andrey", "Александр", "Mikhail", "Oleg", "Alexander"};
            var userLastNames = new[] { "Zagorodnuk", "Golovin", "Spivakov", "Носов", "Bratukha", "", "Towstonog" };
            var tags = new[] {"summer", "wind", "friends", "animals", "pentax", "binary", "cherdak", "work&fun"};
            var groups = new[] {"friends", "enemies", "kill", "neighbor", "boss", "partners"};


            var unitOfWorkFactory = new UnitOfWorkFactory();
            var unitOfWork = unitOfWorkFactory.GetUnitOfWork();


            // Creating accounts for team
            for (var i = 0; i < userFirstNames.Count(); i++)
            {
                var random = new Random();
                
                unitOfWork.Users.Add(new UserModel()
                {
                    FirstName = userFirstNames[i],
                    LastName = userLastNames[i],
                    Email = string.Format("{0}{1}@bingally.com",userFirstNames[i],userLastNames[i]),
                    IsAdmin = (random.Next(1, 10)%2 == 1)
                });
                //adds local auth provider with preset password
                unitOfWork.AuthInfos.Add(i + 1, userLastNames[i], "local");
            }

            // Creating a list of usefull tags
            foreach (var photoTag in tags)
            {
                unitOfWork.PhotoTags.Add(photoTag);
            }

            // Creating a list of useful tags
            foreach (var albumTag in tags)
            {
                unitOfWork.AlbumTags.Add(new AlbumTagModel(){TagName = albumTag});
            }

            // Creating a list of useful groups
            foreach (var group in groups)
            {
                unitOfWork.Groups.Add(new GroupModel() {GroupName = group});
            }

            // Creating album without any informations about it
            unitOfWork.Albums.Add(1);

            // Creating album with some informations about it
            unitOfWork.Albums.Add(new AlbumModel()
                {
                    AlbumName = "Academy",
                    Description = ".Net student group in Binary Studio Academy. Donetsk 2013.",
                    UserModelID = 5
                });


            // Adding 100 photos from different users to album with ID 2(Academy)
            for (var i = 0; i < 100; i++)
            {
                var random = new Random();
                unitOfWork.Photos.Add(2, random.Next(1,7));
            }

            base.Seed(databaseContext); 
        }
    }
}
