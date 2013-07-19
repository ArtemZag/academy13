﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryStudio.PhotoGallery.Database.Model_Interfaces;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database.Model_Repositories
{
    class AuthInfoRepository : BaseRepository<AuthInfoModel>, IAuthInfoRepository
    {
        public AuthInfoRepository(DatabaseContext dataBaseContext) : base(dataBaseContext)
        {
        }
    }
}