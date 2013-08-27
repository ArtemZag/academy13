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
//        public class DatabaseInitializer : DropCreateDatabaseAlways<DatabaseContext>
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<DatabaseContext>
    {
        protected override void Seed(DatabaseContext databaseContext)
        {
            //var random = new Random();
            var cryptoProvider = new CryptoProvider();
            var systemGroupList = new List<GroupModel>();

            #region adminModel creation

            string adminSalt = cryptoProvider.GetNewSalt();

            var adminModel = new UserModel
            {
                Email = "Admin@bingally.com",
                FirstName = ";)",
                LastName = ";)",
                Department = ";)",
                Albums = new Collection<AlbumModel>(),
                AuthInfos = new Collection<AuthInfoModel>(),
                Groups = new Collection<GroupModel>(),
                IsActivated = true,
                IsAdmin = true,
                Salt = adminSalt,
                UserPassword = cryptoProvider.CreateHashForPassword("qwerty", adminSalt)
            };

            #endregion

            #region userModels creating

            var userModelsList = new List<UserModel>();

            #region Artem Zagorodnuk

            string userSalt = cryptoProvider.GetNewSalt();
            var user = new UserModel
            {
                Email = "ArtemZagorodnuk@bingally.com",
                FirstName = "Artem",
                LastName = "Zagorodnuk",
                Department = ".Net",
                Albums = new Collection<AlbumModel>(),
                AuthInfos = new Collection<AuthInfoModel>(),
                Groups = new Collection<GroupModel>(),
                IsActivated = true,
                IsAdmin = false,
                Salt = userSalt,
                UserPassword = cryptoProvider.CreateHashForPassword("qwerty", userSalt)
            };
            userModelsList.Add(user);

            #endregion

            #region Anton Golovin

            userSalt = cryptoProvider.GetNewSalt();
            user = new UserModel
            {
                Email = "anton@bingally.com",
                FirstName = "Anton",
                LastName = "Golovin",
                Department = "Academy",
                Albums = new Collection<AlbumModel>(),
                AuthInfos = new Collection<AuthInfoModel>(),
                Groups = new Collection<GroupModel>(),
                IsActivated = true,
                IsAdmin = false,
                Salt = userSalt,
                UserPassword = cryptoProvider.CreateHashForPassword("qwerty", userSalt)
            };
            userModelsList.Add(user);

            #endregion

            #region Andrey Spivakov

            userSalt = cryptoProvider.GetNewSalt();
            user = new UserModel
            {
                Email = "AndreySpivakov@bingally.com",
                FirstName = "Andrey",
                LastName = "Spivakov",
                Department = "Academy",
                Albums = new Collection<AlbumModel>(),
                AuthInfos = new Collection<AuthInfoModel>(),
                Groups = new Collection<GroupModel>(),
                IsActivated = true,
                IsAdmin = false,
                Salt = userSalt,
                UserPassword = cryptoProvider.CreateHashForPassword("qwerty", userSalt)
            };
            userModelsList.Add(user);

            #endregion

            #region Александр Носов

            userSalt = cryptoProvider.GetNewSalt();
            user = new UserModel
            {
                Email = "San4ez@bingally.com",
                FirstName = "Александр",
                LastName = "Носов",
                Department = "Academy",
                Albums = new Collection<AlbumModel>(),
                AuthInfos = new Collection<AuthInfoModel>(),
                Groups = new Collection<GroupModel>(),
                IsActivated = true,
                IsAdmin = false,
                Salt = userSalt,
                UserPassword = cryptoProvider.CreateHashForPassword("qwerty", userSalt)
            };
            userModelsList.Add(user);

            #endregion

            #region Mikhail Bratukha

            userSalt = cryptoProvider.GetNewSalt();
            user = new UserModel
            {
                Email = "MikhailBratukha@bingally.com",
                FirstName = "Mikhail",
                LastName = "Bratukha",
                Department = "Academy",
                Albums = new Collection<AlbumModel>(),
                AuthInfos = new Collection<AuthInfoModel>(),
                Groups = new Collection<GroupModel>(),
                IsActivated = true,
                IsAdmin = false,
                Salt = userSalt,
                UserPassword = cryptoProvider.CreateHashForPassword("qwerty", userSalt)
            };
            userModelsList.Add(user);

            #endregion

            #region Oleg Beloy

            userSalt = cryptoProvider.GetNewSalt();
            user = new UserModel
            {
                Email = "OlegBeloy@bingally.com",
                FirstName = "Oleg",
                LastName = "Beloy",
                Department = "Academy",
                Albums = new Collection<AlbumModel>(),
                AuthInfos = new Collection<AuthInfoModel>(),
                Groups = new Collection<GroupModel>(),
                IsActivated = true,
                IsAdmin = false,
                Salt = userSalt,
                UserPassword = cryptoProvider.CreateHashForPassword("qwerty", userSalt)
            };
            userModelsList.Add(user);

            #endregion

            #region Alexander Towstonog

            userSalt = cryptoProvider.GetNewSalt();
            user = new UserModel
            {
                Email = "AlexanderTowstonog@bingally.com",
                FirstName = "Alexander",
                LastName = "Towstonog",
                Department = "Academy",
                Albums = new Collection<AlbumModel>(),
                AuthInfos = new Collection<AuthInfoModel>(),
                Groups = new Collection<GroupModel>(),
                IsActivated = true,
                IsAdmin = false,
                Salt = userSalt,
                UserPassword = cryptoProvider.CreateHashForPassword("qwerty", userSalt)
            };
            userModelsList.Add(user);

            #endregion

            #endregion

            #region BlockedUsers
            var groupModel = new GroupModel()
                {
                    GroupName = "BlockedUsers",
                    Description = "System group. Not for use",
                    OwnerId = -1,
                    Users = new Collection<UserModel>()
                };
            systemGroupList.Add(groupModel);
            #endregion


            var unitOfWorkFactory = new UnitOfWorkFactory();
            using (IUnitOfWork unitOfWork = unitOfWorkFactory.GetUnitOfWork())
            {
                // Admin account creating
                unitOfWork.Users.Add(adminModel);
                unitOfWork.SaveChanges();


                // Users' acoount creating
                foreach (UserModel userModel in userModelsList)
                {
                    unitOfWork.Users.Add(userModel);
                }
                unitOfWork.SaveChanges();


                // Temporary album adding to each user (ever admin)
                List<UserModel> allUsersList = unitOfWork.Users.All().ToList();
                foreach (UserModel userModel in allUsersList)
                {
                    userModel.Albums.Add(new AlbumModel
                    {
                        Name = "Temporary",
                        Description = "System album not for use",
                        IsDeleted = false,
                        Permissions = (int)AlbumModel.PermissionsMask.PublicAlbum,
                        OwnerId = userModel.Id,
                        Photos = new Collection<PhotoModel>(),
                        Tags = new Collection<AlbumTagModel>(),
                        AvailableGroups = new Collection<AvailableGroupModel>()
                    });
                    unitOfWork.Users.Update(userModel);
                }
                unitOfWork.SaveChanges();

                // Adding all system groups (Owner for all groups is Admin)
                var adminID = unitOfWork.Users.Find(a => a.Email == "Admin@bingally.com").Id;
                foreach (var systemGroup in systemGroupList)
                {
                    systemGroup.OwnerId = adminID;
                    unitOfWork.Groups.Add(systemGroup);
                }
                unitOfWork.SaveChanges();


                #region adding test groups

                var group = new GroupModel
                {
                    GroupName = "Test group",
                    OwnerId = unitOfWork.Users.Find(a => a.Email == "Admin@bingally.com").Id,
                    Description = "Test group"
                };

                unitOfWork.Groups.Add(group);

                unitOfWork.SaveChanges();

                #endregion

                #region adding album to user with lastname Towstonog

                UserModel maaak = unitOfWork.Users.Find(x => x.LastName == "Towstonog");
                maaak.Albums.Add(new AlbumModel
                {
                    Name = "First album",
                    Description = "Default album by DBinit",
                    IsDeleted = false,
                    Permissions = (int)AlbumModel.PermissionsMask.PublicAlbum,
                    OwnerId = maaak.Id,
                    Tags = new Collection<AlbumTagModel>(),
                    AvailableGroups = new Collection<AvailableGroupModel>(),
                    Photos = new Collection<PhotoModel>()
                });
                unitOfWork.Users.Update(maaak);
                unitOfWork.SaveChanges();

                #endregion

                #region adding photos and group to album with name "First album"

                AlbumModel albumModel = unitOfWork.Albums.Find(album => album.Name == "First album");

                var avialableGroup = new AvailableGroupModel
                {
                    AlbumId = albumModel.Id,
                    GroupId = 1,
                    CanAddComments = true,
                    CanAddPhotos = true,
                    CanSeeComments = true,
                    CanSeeLikes = true,
                    CanSeePhotos = true
                };

                albumModel.AvailableGroups.Add(avialableGroup);

                GeneratePhotos(albumModel, unitOfWork);

                #endregion

                #region adding album to user with lastname Golovin

                var golovinUser = unitOfWork.Users.Find(x => x.LastName == "Golovin");

                var tags = new Collection<AlbumTagModel>
                {
                    new AlbumTagModel
                    {
                        TagName = "tag"
                    },
                    new AlbumTagModel
                    {
                        TagName = "tag1"
                    },
                    new AlbumTagModel
                    {
                        TagName = "looooooooooooooooooong tag"
                    }
                };

                var albumForGolovin = new AlbumModel
                {
                    Name = "Anton album",
                    Description = "Default album by DBinit",
                    IsDeleted = false,
                    Permissions = (int)AlbumModel.PermissionsMask.PublicAlbum,
                    OwnerId = golovinUser.Id,
                    Tags = tags,
                    AvailableGroups = new Collection<AvailableGroupModel>(),
                    Photos = new Collection<PhotoModel>()
                };

                var currentGroup = unitOfWork.Groups.Find(x => x.OwnerId == 1);

                golovinUser.Groups.Add(currentGroup);
                golovinUser.Albums.Add(albumForGolovin);

                unitOfWork.SaveChanges();

                #endregion

                #region adding photos and group to album with name "Anton album"

                albumModel = unitOfWork.Albums.Find(album => album.Name == "Anton album");

                avialableGroup = new AvailableGroupModel
                {
                    AlbumId = albumModel.Id,
                    GroupId = 1,
                    CanAddComments = true,
                    CanAddPhotos = true,
                    CanSeeComments = true,
                    CanSeeLikes = true,
                    CanSeePhotos = true
                };

                albumForGolovin.AvailableGroups.Add(avialableGroup);


                GeneratePhotos(albumModel, unitOfWork);

                #endregion
            }

            base.Seed(databaseContext);
        }

        private void GeneratePhotos(AlbumModel albumModel, IUnitOfWork unitOfWork)
        {
            var photosForAlbum = new Collection<PhotoModel>();

            var generatedRandomComment = new StringBuilder();

            for (int i = 0; i < 29; i++)
            {
                var comm = new Collection<PhotoCommentModel>();

                int upper = i == 0 ? 100 : Randomizer.GetNumber(10);


                for (int j = 0; j < upper; j++)
                {
                    generatedRandomComment.Clear();
                    for (int k = 0; k < Randomizer.GetNumber(32); k++)
                    {
                        generatedRandomComment.Append(Randomizer.GetString(Randomizer.GetNumber(64)));
                        generatedRandomComment.Append(" ");
                    }
                    comm.Add(new PhotoCommentModel(7, Randomizer.GetNumber(i), generatedRandomComment.ToString(),
                        -1) { Rating = Randomizer.GetNumber(64) });
                }

                var tags = new List<PhotoTagModel>
                    {
                        new PhotoTagModel
                        {
                            TagName = "tag"
                        },
                        new PhotoTagModel
                        {
                            TagName = "check"
                        }
                    };

                var photoModel = new PhotoModel
                {
                    Format = "jpg",
                    Description = "test photo",
                    OwnerId = albumModel.OwnerId,
                    AlbumId = albumModel.Id,
                    Likes = new Collection<UserModel>(),
                    Rating = 0,
                    Tags = tags,
                    PhotoComments = comm,
                    IsDeleted = false
                };
                photosForAlbum.Add(photoModel);
            }

            albumModel.Photos = photosForAlbum;

            unitOfWork.Albums.Update(albumModel);
            unitOfWork.SaveChanges();
        }
    }
}