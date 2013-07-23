using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain
{
    internal class TestServiceClass
    {
        // interface instance of UnitOfWorkFactory(singltone)
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        // unitOfWork provided by factory
        private IUnitOfWork _unitOfWork;


        public TestServiceClass(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }


        private void SomeMethod()
        {
            /*using (_unitOfWork = _unitOfWorkFactory.GetUnitOfWork())
            {
                _unitOfWork.Users.Add(new UserModel());

                _unitOfWork.PhotoTags.Add(new PhotoTagModel());

                _unitOfWork.Photos.Add(new PhotoModel());

                _unitOfWork.Albums.Add(new AlbumModel());
            }*/
        }
    }
}