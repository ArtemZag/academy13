using System.Data.Entity;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        protected DatabaseContext()
        {
        }

        public DatabaseContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<PhotoModel> PhotoModels { get; set; }
        public DbSet<PhotoCommentModel> PhotoComments { get; set; }
        public DbSet<GroupModel> GroupModels { get; set; }
        public DbSet<AvailableGroupModel> AvailableGroups { get; set; }
        public DbSet<AuthInfoModel> AuthInfoModels { get; set; }
        public DbSet<AlbumModel> AlbumModels { get; set; }
        public DbSet<PhotoTagModel> PhotoTagModels { get; set; }
        public DbSet<AlbumTagModel> AlbumTagModels { get; set; }
    }
}