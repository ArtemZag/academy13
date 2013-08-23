using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using BinaryStudio.PhotoGallery.Domain.Services;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
	[RoutePrefix("Api/PublicPhoto")]
    public class PublicPhotoController : BaseApiController
    {
        private readonly IPhotoService _photoService;

        public PublicPhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }


    }
}
