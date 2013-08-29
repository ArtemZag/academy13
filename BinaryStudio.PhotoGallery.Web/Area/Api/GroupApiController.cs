using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.Extensions.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels;

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

        [GET("{albumId: int}")]
        public HttpResponseMessage GetGroups(int albumId)
        {
            try
            {
                IEnumerable<GroupModel> userGroups = groupService.GetUserGroups(User.Id);

                IEnumerable<AvailableGroupModel> albumGroups = groupService.GetAlbumGroups(albumId);

                // so cool..
                List<AvailableGroupViewModel> result = (from groupModel in userGroups
                    let viewModel =
                        albumGroups.SingleOrDefault(availableGroupModel => availableGroupModel.GroupId == groupModel.Id) ??
                        new AvailableGroupModel
                        {
                            AlbumId = albumId,
                            GroupId = groupModel.Id,
                        }
                    select viewModel.ToAvialableGroupViewModel(groupModel.GroupName)).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);                
            }
        }

        [POST("")]
        public HttpResponseMessage PostRights(IEnumerable<AvailableGroupViewModel> viewModels)
        {
            throw new NotImplementedException();
        }
    }
}