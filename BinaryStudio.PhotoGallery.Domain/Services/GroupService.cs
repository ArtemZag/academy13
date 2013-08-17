using System;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class GroupService : DbService, IGroupService
    {
        public GroupService(IUnitOfWorkFactory workFactory)
            : base(workFactory)
        {
        }

        //todo: add check permission for creating group event 
        public void Create(int userID, GroupModel groupModel)
        {
            using (var unitOfWork = WorkFactory.GetUnitOfWork())
            {
                if (unitOfWork.Groups.Find(group => group.GroupName == groupModel.GroupName) != null)
                {
                    throw new GroupAlreadyExistException(groupModel.GroupName);
                }

                unitOfWork.Groups.Add(groupModel);
                unitOfWork.SaveChanges();
            }
        }

        //todo: add check permission for creating group event 
        public void Create(int userID, string groupName)
        {
            var groupModel = new GroupModel
                {
                    GroupName = groupName,
                    OwnerID = userID
                };

            try
            {
                this.Create(userID, groupModel);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception);
            }
        }

        public GroupModel GetGroup(int groupID)
        {
            using (var unitOfWork = WorkFactory.GetUnitOfWork())
            {
                try
                {
                    return GetGroup(groupID, unitOfWork);
                }
                catch (Exception exception)
                {
                    throw new Exception(exception.Message, exception);
                }
            }
        }

        //todo: add check permission for adding group event 
        public void AddUser(int ownerID, int userID, int groupID)
        {
            using (var unitOfWork = WorkFactory.GetUnitOfWork())
            {
                try
                {
                    var group = GetGroup(groupID);
                    var owner = GetUser(ownerID, unitOfWork);
                    var user = GetUser(userID, unitOfWork);

                    if (group.OwnerID == ownerID || owner.IsAdmin)
                    {
                        unitOfWork.Groups.Find(groupID).Users.Add(user);
                        unitOfWork.SaveChanges();
                    }
                }
                catch (Exception exception)
                {
                    throw new Exception(exception.Message, exception);
                }
            }
        }

        //todo: add check permission for adding group event 
        public void AddUser(int ownerID, int userID, string groupName)
        {
            using (var unitOfWork = WorkFactory.GetUnitOfWork())
            {
                try
                {
                    var group = GetGroup(groupName, unitOfWork);

                    this.AddUser(ownerID, userID, group.Id);
                }
                catch (Exception exception)
                {
                    throw new Exception(exception.Message, exception);
                }
            }
        }

        //todo: add check permission for adding group event
        public void RemoveUser(int ownerID, int userID, int groupID)
        {
            using (var unitOfWork = WorkFactory.GetUnitOfWork())
            {
                try
                {
                    var group = GetGroup(groupID);
                    var owner = GetUser(ownerID, unitOfWork);
                    var user = GetUser(userID, unitOfWork);

                    if (group.OwnerID == ownerID || owner.IsAdmin)
                    {
                        unitOfWork.Groups.Find(groupID).Users.Remove(user);
                        unitOfWork.SaveChanges();
                    }
                }
                catch (Exception exception)
                {
                    throw new Exception(exception.Message, exception);
                }
            }
        }

        //todo: add check permission for adding group event
        public void RemoveUser(int ownerID, int userID, string groupName)
        {
            using (var unitOfWork = WorkFactory.GetUnitOfWork())
            {
                try
                {
                    var group = GetGroup(groupName, unitOfWork);

                    this.RemoveUser(ownerID, userID, group.Id);
                }
                catch (Exception exception)
                {
                    throw new Exception(exception.Message, exception);
                }
            }
        }

        //todo: add check permission for deleting group event
        public void Delete(int ownerID, int groupID)
        {
            using (var unitOfWork = WorkFactory.GetUnitOfWork())
            {
                try
                {
                    var group = GetGroup(groupID);
                    var owner = GetUser(ownerID, unitOfWork);

                    if (group.OwnerID == ownerID || owner.IsAdmin)
                    {
                        unitOfWork.Groups.Delete(group);
                        unitOfWork.SaveChanges();
                    }
                }
                catch (Exception exception)
                {
                    throw new Exception(exception.Message, exception);
                }
            }
        }
    }
}
