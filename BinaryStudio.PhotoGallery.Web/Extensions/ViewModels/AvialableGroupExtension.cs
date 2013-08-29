using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;
using Microsoft.Ajax.Utilities;

namespace BinaryStudio.PhotoGallery.Web.Extensions.ViewModels
{
    public static class AvialableGroupExtension
    {
        public static AvialableGroupViewModel ToAvialableGroupViewModel(this AvailableGroupModel groupModel)
        {
            return new AvialableGroupViewModel
            {
                GroupId = groupModel.GroupId
            }
        }
    }
}