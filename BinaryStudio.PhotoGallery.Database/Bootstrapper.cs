using Microsoft.Practices.Unity;

namespace BinaryStudio.PhotoGallery.Database
{
    public static class Bootstrapper
    {

        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IUnitOfWorkFactory, UnitOfWorkFactory>(new ContainerControlledLifetimeManager());
        }

        // todo: delete
        public static void Test()
        {
            //var unitOfWorkFactory = new UnitOfWorkFactory();
            //var unitOfWork = unitOfWorkFactory.GetUnitOfWork();

           /* var user = new UserModel();

            var authInfo = new AuthInfoModel();
            var album = new AlbumModel();
            var group = new GroupModel();
            var photo = new PhotoModel();
            var tag = new PhotoTagModel();

            user.AuthInfos = new Collection<AuthInfoModel>();
            user.Albums = new Collection<AlbumModel>();
            user.Groups = new Collection<GroupModel>();

            tag.TagName = "winter";
            

            user.Department = "C# prommer";
            user.Email = "Maaak@gmail.com";
            user.ID = 123;
            user.IsAdmin = true;
            user.FirstName = "Alexander";
            user.LastName = "Towstonog";

            user.AuthInfos.Add(authInfo);
            user.Albums.Add(album);
            user.Groups.Add(group);

            unitOfWork.Users.Create(user);*/

            //unitOfWork.PhotoTags.Create(new PhotoTagModel());

            //unitOfWork.Albums.Add(1);

            //unitOfWork.Photos.Add(4, 2);

            //unitOfWork.PhotoComments.Create(new PhotoCommentModel());

            //unitOfWork.PhotoComments.Add(2,5,"Hi, maaak!", null);

            //unitOfWork.AvailableGroups.Add(2, 4);

            //unitOfWork.AuthInfos.Add(2,"qwerty","local");




            /*if (unitOfWork.Users.Contains(x => string.Equals(x.FirstName, "Alexangfhder")))
            {
                
            }*/

            


        }
    }
}
