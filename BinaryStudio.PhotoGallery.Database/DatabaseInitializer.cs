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
//    public class DatabaseInitializer : DropCreateDatabaseAlways<DatabaseContext>
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<DatabaseContext>
    {
        protected override void Seed(DatabaseContext databaseContext)
        {
            //var random = new Random();
            var cryptoProvider = new CryptoProvider();

            #region adminModel creation

            string adminSalt = cryptoProvider.GetNewSalt();

            var adminModel = new UserModel
            {
                Email = "Admin@bingally.com",
                FirstName = ";)",
                LastName = ";)",
                NickName = ";)",
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
            UserModel user;

            #region Artem Zagorodnuk

            string userSalt = cryptoProvider.GetNewSalt();
            
            user = new UserModel
                {
                    Email = "ArtemZagorodnuk@bingally.com",
                    FirstName = "Artem",
                    LastName = "Zagorodnuk",
                    NickName = ";)",
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
                Email = "AntonGolovin@bingally.com",
                FirstName = "Anton",
                LastName = "Golovin",
                NickName = ";)",
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
                NickName = ";)",
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
                Email = "АлександрНосов@bingally.com",
                FirstName = "Александр",
                LastName = "Носов",
                NickName = ";)",
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
                NickName = ";)",
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
                NickName = ";)",
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
                NickName = ";)",
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
                        AlbumName = "Temporary",
                        Description = "System album not for use",
                        IsDeleted = false,
                        Permissions = 11111,
                        OwnerId = userModel.Id,
                        Photos = new Collection<PhotoModel>(),
                        AlbumTags = new Collection<AlbumTagModel>(),
                        AvailableGroups = new Collection<AvailableGroupModel>()
                    });
                    unitOfWork.Users.Update(userModel);
                }
                unitOfWork.SaveChanges();

                #region adding album to user with lastname Towstonog

                UserModel currentUser = unitOfWork.Users.Find(x => x.LastName == "Towstonog");
                currentUser.Albums.Add(new AlbumModel
                {
                    AlbumName = "First album",
                    Description = "Default album by DBinit",
                    IsDeleted = false,
                    Permissions = 11111,
                    OwnerId = currentUser.Id,
                    AlbumTags = new Collection<AlbumTagModel>(),
                    AvailableGroups = new Collection<AvailableGroupModel>(),
                    Photos = new Collection<PhotoModel>()
                });
                unitOfWork.Users.Update(currentUser);
                unitOfWork.SaveChanges();

                #endregion

                #region adding photos to album

                var photosForAlbum = new Collection<PhotoModel>();
                AlbumModel albumModel = unitOfWork.Albums.Find(album => album.AlbumName == "First album");


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
                            -1) {Rating = Randomizer.GetNumber(64)});
                    }

                    var photoModel = new PhotoModel
                    {
                        Format = "jpg",
                        Description = "test photo",
                        OwnerId = albumModel.OwnerId,
                        AlbumId = albumModel.Id,
                        Likes = new Collection<UserModel>(),
                        Rating = 0,
                        PhotoTags = new Collection<PhotoTagModel>(),
                        PhotoComments = comm,
                        IsDeleted = false
                    };
                    photosForAlbum.Add(photoModel);
                }


                albumModel.Photos = photosForAlbum;

                unitOfWork.Albums.Update(albumModel);
                unitOfWork.SaveChanges();

                #endregion
            }

            base.Seed(databaseContext);
        }
    }
}