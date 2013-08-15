using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BinaryStudio.PhotoGallery.Database;

namespace BinaryStudio.PhotoGallery.Web.Events
{
    //public delegate void PhotoAddedHandler();
    public delegate void UserHaveConnectedHandler();

    public static class GlobalEventManager
    {
        //public static event PhotoAddedHandler PhotoHasBeenAdded;

        //private static void OnPhotoHasBeenAdded()
        //{
        //    PhotoAddedHandler handler = PhotoHasBeenAdded;
        //    if (handler != null) handler();
        //}


        public static event UserHaveConnectedHandler UserHaveConnected;

        public static void OnUserHaveConnected()
        {
            UserHaveConnectedHandler handler = UserHaveConnected;
            if (handler != null) handler();

            //using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            //{
            //    unitOfWork.PhotoComments.Add(newPhotoCommentModel);
            //    unitOfWork.SaveChanges();
            //}
        }
    }
}