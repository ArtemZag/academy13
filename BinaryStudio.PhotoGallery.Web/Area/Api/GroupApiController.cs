using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [Authorize]
    [RoutePrefix("api/group")]
    public class GroupApiController : BaseApiController
    {
        private readonly IGroupService groupService;

        public GroupApiController(IGroupService groupService)
        {
            this.groupService = groupService;
        }

        [GET("")]
        public HttpResponseMessage GetAvialableGroups(int albumId)
        {
            IEnumerable<AvailableGroupModel> result = groupService.GetAvialableGroups(User.Id, albumId).Select(model => model);

            throw new NotImplementedException();
        }
    }
}