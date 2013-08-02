﻿using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using BinaryStudio.PhotoGallery.Core.Helpers;
using BinaryStudio.PhotoGallery.Core.UserUtils;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<DatabaseContext>
    {
        protected override void Seed(DatabaseContext databaseContext)
        {
            var userFirstNames = new[] {"Artem", "Anton", "Andrey", "Александр", "Mikhail", "Oleg", "Alexander"};
            var userLastNames = new[] {"Zagorodnuk", "Golovin", "Spivakov", "Носов", "Bratukha", "", "Towstonog"};
            var tags = new[] {"summer", "wind", "friends", "animals", "pentax", "binary", "cherdak", "work&fun"};
            var groups = new[] {"friends", "enemies", "kill", "neighbor", "boss", "partners"};


            var unitOfWorkFactory = new UnitOfWorkFactory();
            using (IUnitOfWork unitOfWork = unitOfWorkFactory.GetUnitOfWork())
            {

                var random = new Random();
                var crypto = new CryptoProvider();

                // Creating accounts for team
                for (int i = 0; i < userFirstNames.Count(); i++)
                {
                    string salt = crypto.GetNewSalt();

                    unitOfWork.Users.Add(
                        new UserModel
                            {
                                FirstName = userFirstNames[i],
                                LastName = userLastNames[i],
                                Email = string.Format("{0}{1}@bingally.com", userFirstNames[i], userLastNames[i]),
                                IsAdmin = (random.Next(1, 10)%2 == 1),
                                UserPassword = crypto.CreateHashForPassword(userLastNames[i], salt),
                                Salt = salt
                            });
                }
                unitOfWork.SaveChanges();

                // Creating a list of usefull tags
                foreach (string photoTag in tags)
                {
                    unitOfWork.PhotoTags.Add(photoTag);
                }

                // Creating a list of useful tags
                foreach (string albumTag in tags)
                {
                    unitOfWork.AlbumTags.Add(new AlbumTagModel {TagName = albumTag});
                }

                // Creating a list of useful groups
                foreach (string group in groups)
                {
                    unitOfWork.Groups.Add(new GroupModel {GroupName = group});
                }

                // Creating album without any informations about it
                unitOfWork.Albums.Add("First album", 1);

                // Creating album with some informations about it
                unitOfWork.Albums.Add(new AlbumModel("Academy", 5)
                    {
                        Description = ".Net student group in Binary Studio Academy. Donetsk 2013."
                    });

                // Adding 100 photos from different users to album with ID 2(Academy)
                /*for (int i = 0; i < 100; i++)
                {
                    unitOfWork.Photos.Add(2, random.Next(1, 7));
                }*/
                unitOfWork.SaveChanges();

                ///////////////////////////////////////////////////////
                for (int i = 0; i < 29; i++)
                {
                    var comm = new Collection<PhotoCommentModel>();
                    for (int j = 0; j < Randomizer.GetNumber(10); j++)
                    {
                        comm.Add(new PhotoCommentModel(7,Randomizer.GetNumber(i),Randomizer.GetString(20),null));
                    }
                    unitOfWork.Photos.Add(new PhotoModel(3, 7){PhotoName = i + ".jpg",PhotoComments = comm});
                }
                
                unitOfWork.Albums.Add(new AlbumModel("Test", 7));




                unitOfWork.SaveChanges();

            }

            base.Seed(databaseContext);
        }
    }
}