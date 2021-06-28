using BLG5009.Rehber.WebApp.Models;
using BLG5009.Rehber.WebApp.Models.Context;
using BLG5009.Rehber.WebApp.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace BLG5009.Rehber.WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly DatabaseContext _dbContext;

        public UserController(DatabaseContext dbContext, IWebHostEnvironment hostEnvironment)
        {
            _dbContext = dbContext;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Form(int? id)
        {
            UserViewModel user = new UserViewModel();

            if (id != null)
            {
                var select = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == id.Value);

                user.Id = select.Id;
                user.FirstName = select.FirstName;
                user.LastName = select.LastName;
                user.NickName = select.NickName;
                user.Star = select.Star;
                user.Image = select.Image;
            }

            return PartialView(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<int> Form(UserViewModel user)
        {
            if (user.File != null)
                user.Image = UploadedFile(user.File);

            if (user.Id == 0)//yeni kayıt
            {
                await _dbContext.Users.AddAsync(new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    NickName = user.NickName,
                    Star = user.Star,
                    Image = user.Image
                });
            }
            else//update
            {
                var editUser = _dbContext.Users.SingleOrDefault(u => u.Id == user.Id);

                editUser.FirstName = user.FirstName;
                editUser.LastName = user.LastName;
                editUser.NickName = user.NickName;
                editUser.Star = user.Star;
                editUser.Image = user.Image;
            }

            return await _dbContext.SaveChangesAsync();
        }

        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                var userData = _dbContext.Users.AsQueryable();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                    userData = userData.OrderBy(sortColumn + " " + sortColumnDirection);

                if (!string.IsNullOrEmpty(searchValue))
                    userData = userData.Where(m =>
                                           m.FirstName.Contains(searchValue)
                                        || m.LastName.Contains(searchValue)
                                        || m.NickName.Contains(searchValue));

                recordsTotal = await userData.CountAsync();
                var data = await userData.Skip(skip).Take(pageSize).ToListAsync();

                var jsonData = new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = data
                };

                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ICollection<User>> GetStarRandomUsers()
        {
            return await _dbContext.Users.Where(x => x.Star).OrderBy(r => Guid.NewGuid()).Take(6).ToListAsync();
        }

        public async Task<bool> SetStar(int id, bool status)
        {
            var selectUser = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == id);

            if (selectUser == null)
                return false;

            selectUser.Star = !status;

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var selectUser = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == id);

            if (selectUser == null)
                return false;

            _dbContext.Users.Remove(selectUser);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        private string UploadedFile(IFormFile file)
        {
            string uniqueFileName = null;

            if (file != null)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "image/users");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
