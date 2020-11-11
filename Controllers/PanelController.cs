using Blog.Data.FileManeger;
using Blog.Data.Repository;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PanelController : Controller
    {
        private IRepository repo;
        private IFileManeger fileManeger;

        public PanelController(IRepository repo, IFileManeger fileManeger)
        {
            this.repo = repo;
            this.fileManeger = fileManeger;
        }

        public IActionResult Index()
        {
            var posts = repo.GetAllPosts();
            return View(posts);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View(new PostViewModel());
            }
            else
            {
                var post = repo.GetPost((int)id);
                return View(new PostViewModel 
                {
                    Id = post.Id,
                    Title = post.Title,
                    Body = post.Body,
                    CurrentImage = post.Image,
                    Description = post.Description,
                    Tags = post.Tags,
                    Category = post.Category
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostViewModel vm)
        {
            var post = new Post
            {
                Id = vm.Id,
                Title = vm.Title,
                Body = vm.Body,
                Description = vm.Description,
                Tags = vm.Tags,
                Category = vm.Category
            };

            if (vm.Image == null)
            {
                post.Image = vm.CurrentImage;
            }
            else
            {
                post.Image = await fileManeger.SaveImage(vm.Image);
            }

            if (post.Id > 0)
            {
                repo.UpdatePost(post);
            }
            else
            {
                repo.AddPost(post);
            }

            if (await repo.SaveChangesAsync())
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(post);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            repo.RemovePost(id);
            await repo.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
