using System.Data.Entity;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database
{
    internal interface IDatabaseContext
    {
        DbSet<UserModel> Users { get; set; }
        DbSet<PhotoModel> PhotoModels { get; set; }
        DbSet<PhotoCommentModel> PhotoComments { get; set; }
        DbSet<GroupModel> GroupModels { get; set; }
        DbSet<AvailableGroupModel> AvailableGroups { get; set; }
        DbSet<AuthInfoModel> AuthInfoModels { get; set; }
        DbSet<AlbumModel> AlbumModels { get; set; }
        DbSet<PhotoTagModel> PhotoTagModels { get; set; }
        DbSet<AlbumTagModel> AlbumTagModels { get; set; } 
    }
}
