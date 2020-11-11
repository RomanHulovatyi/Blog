using Blog.Data;
using Blog.Data.FileManeger;
using Blog.Data.Repository;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private IRepository repo;
        private IFileManeger fileManeger;

        public HomeController(IRepository repo, IFileManeger fileManeger)
        {
            this.repo = repo;
            this.fileManeger = fileManeger;
        }


        public IActionResult Index(string category)
        {
            var posts = string.IsNullOrEmpty(category) ? repo.GetAllPosts() : repo.GetAllPosts(category);
            return View(posts);
        }

        public IActionResult Post(int id)
        {
            var post = repo.GetPost(id);

            return View(post);
        }

        [HttpGet("/Image/{image}")]
        public IActionResult Image(string image)
        {
            var mime = image.Substring(image.LastIndexOf('.') + 1);
            return new FileStreamResult(fileManeger.ImageStream(image), $"image/{mime}");
        }
    }
}
