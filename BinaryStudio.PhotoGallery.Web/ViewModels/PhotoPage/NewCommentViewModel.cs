﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BinaryStudio.PhotoGallery.Web.ViewModels.PhotoPage
{
    public class NewCommentViewModel
    {
        public string NewComment { get; set; }
        public int PhotoID { get; set; }
        public int Reply { get; set; }
    }
}