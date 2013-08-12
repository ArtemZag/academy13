using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain
{
    internal interface IGlobalEventsAggregator
    {
        void PushCommentAdded(PhotoCommentModel phCommentModel);
        void PushPhotoAdded(PhotoModel phModel);
    }

    internal class GlobalEventsAggregator : IGlobalEventsAggregator
    {
        private static GlobalEventsAggregator _instance;

        private GlobalEventsAggregator() { }

        public static GlobalEventsAggregator Instance
        {
            get { return _instance ?? (_instance = new GlobalEventsAggregator()); }
        }

        public void PushCommentAdded(PhotoCommentModel phCommentModel)
        {
            throw new NotImplementedException();
        }

        public void PushPhotoAdded(PhotoModel phModel)
        {
            throw new NotImplementedException();
        }
    }

}
