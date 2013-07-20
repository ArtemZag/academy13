using BinaryStudio.PhotoGallery.Models;
using Microsoft.Practices.Unity;

namespace BinaryStudio.PhotoGallery.Database
{
    public static class Bootstrapper
    {

        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType(typeof (IUnitOfWorkFactory), typeof (UnitOfWorkFactory), new ContainerControlledLifetimeManager());
        }

        public static void Test()
        {
            var unitOfWorkFactory = new UnitOfWorkFactory();
            var unitOfWork = unitOfWorkFactory.GetUnitOfWork();

            unitOfWork.Users.Create(new UserModel());
        }
    }
}
