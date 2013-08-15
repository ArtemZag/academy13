using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using BinaryStudio.PhotoGallery.Core;
using BinaryStudio.PhotoGallery.Core.UserUtils;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database
{
    public class DatabaseInitializer : DropCreateDatabaseAlways<DatabaseContext>
    {
        protected override void Seed(DatabaseContext databaseContext)
        {
            var userFirstNames = new[] {"Artem", "Anton", "Andrey", "Александр", "Mikhail", "Oleg", "Alexander","Tester"};
            var userLastNames = new[] {"Zagorodnuk", "Golovin", "Spivakov", "Носов", "Bratukha", "Beloy", "Towstonog",""};
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
                                IsAdmin = userFirstNames[i] != "Tester",
                                UserPassword = crypto.CreateHashForPassword(userLastNames[i].Length > 5 ? userLastNames[i] : "123456", salt),
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

                

                unitOfWork.SaveChanges();

                ///////////////////////////////////////////////////////

                var photosForAlbum = new Collection<PhotoModel>();

                var generatedRandomComment = new StringBuilder();

                for (int i = 0; i < 29; i++)
                {
                    var comm = new Collection<PhotoCommentModel>();

                    var upper = i == 0 ? 100 : Randomizer.GetNumber(10);


                    for (var j = 0; j < upper; j++)
                    {
                        generatedRandomComment.Clear();
                        for (var k = 0; k < Randomizer.GetNumber(32); k++)
                        {
                            generatedRandomComment.Append(Randomizer.GetString(Randomizer.GetNumber(64)));
                            generatedRandomComment.Append(" ");
                        }
                        comm.Add(new PhotoCommentModel(7, Randomizer.GetNumber(i), generatedRandomComment.ToString(),
                                                       -1) {Rating = Randomizer.GetNumber(64)});
                    }
                    photosForAlbum.Add(new PhotoModel(3, 7) { PhotoFileName = "Photo name", Format = ".jpg", PhotoComments = comm, Description = string.Empty });
                    unitOfWork.Photos.Add(new PhotoModel(4, 6) { PhotoFileName = "Photo name", Format = ".jpg" });
                }


                /////////////////////////////////////////////////////////////////////////////////

                /*unitOfWork.Albums.Add(new AlbumModel("Test", 7));*/
                var availableGroupModel = new AvailableGroupModel {AlbumId = 3, GroupId = 1, CanSeeComments = true, CanSeePhotos = true};
                var availableGroupModel1 = new AvailableGroupModel {AlbumId = 3, GroupId = 2, CanSeeComments = true, CanSeePhotos = true};
                var availableGroupModel2 = new AvailableGroupModel {AlbumId = 3, GroupId = 3, CanSeeComments = true, CanSeePhotos = true};
                var availableGroupModel3 = new AvailableGroupModel {AlbumId = 3, GroupId = 4, };
                var availableGroupModel4 = new AvailableGroupModel {AlbumId = 3, GroupId = 5, };

                var AGList = new List<AvailableGroupModel>
                {
                    availableGroupModel,
                    availableGroupModel1,
                    availableGroupModel2,
                    availableGroupModel3,
                    availableGroupModel4
                };

                var album = new AlbumModel("Test", 7) {AvailableGroups = AGList, Photos = photosForAlbum};

                var groupCollection = new Collection<GroupModel>
                    {
                        unitOfWork.Groups.Find(1),
                        unitOfWork.Groups.Find(3),
                        unitOfWork.Groups.Find(6)
                    };


                unitOfWork.Users.Find(7).Groups = groupCollection;

                unitOfWork.Users.Find(7).Albums = new Collection<AlbumModel> {album};
                
                //////////////////////////////////////////////////////////
                unitOfWork.Albums.Add(new AlbumModel("TestAvi", 6));
                

                unitOfWork.SaveChanges();

            }

            base.Seed(databaseContext);
        }
    }
}