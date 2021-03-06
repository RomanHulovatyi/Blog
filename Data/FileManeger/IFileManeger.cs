﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.FileManeger
{
    public interface IFileManeger
    {
        Task<string> SaveImage(IFormFile image);
        FileStream ImageStream(string image);
    }
}
