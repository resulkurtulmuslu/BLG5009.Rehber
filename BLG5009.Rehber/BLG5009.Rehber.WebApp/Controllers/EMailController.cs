using BLG5009.Rehber.WebApp.Models.Context;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using BLG5009.Rehber.WebApp.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using BLG5009.Rehber.WebApp.Methods;
using BLG5009.Rehber.WebApp.Models;
using BLG5009.Rehber.WebApp.Models.Enums;

namespace BLG5009.Rehber.WebApp.Controllers
{
    public class EMailController : Controller
    {
        private readonly DatabaseContext _dbContext;

        public EMailController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Route("eposta-adresleri/{userId}")]
        public IActionResult Index(int userId)
        {
            ViewBag.User = _dbContext.Users.SingleOrDefault(u => u.Id == userId);

            return View();
        }

        public async Task<IActionResult> GetEMailByUserId(int userId)
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

                var telephoneData = _dbContext.Emails.Where(t => t.UserId == userId).AsQueryable();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                    telephoneData = telephoneData.OrderBy(sortColumn + " " + sortColumnDirection);

                if (!string.IsNullOrEmpty(searchValue))
                    telephoneData = telephoneData.Where(m => m.EmailAddress.Contains(searchValue));

                recordsTotal = await telephoneData.Where(m => m.EmailAddress.Contains(searchValue)).CountAsync();

                var models = await telephoneData.Skip(skip).Take(pageSize).ToListAsync();

                List<EmailViewModel> data = new List<EmailViewModel>();

                models.ForEach(x =>
                {
                    data.Add(new EmailViewModel
                    {
                        Id = x.Id,
                        EmailAddress = x.EmailAddress,
                        Type = x.Type,
                        TypeText = EnumExtensionMethods.GetDescription(x.Type),
                        UserId = x.UserId
                    });
                });

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

        public async Task<IActionResult> Form(int userId, int? id)
        {
            Email email = new Email();
            email.UserId = userId;

            if (id != null)
                email = await _dbContext.Emails.SingleOrDefaultAsync(u => u.Id == id.Value);

            ViewBag.Types = EnumExtensionMethods.GetDescriptionList<EmailType>();

            return PartialView(email);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<int> Form(Email email)
        {
            if (email.Id == 0)//yeni kayıt
            {
                await _dbContext.Emails.AddAsync(email);
            }
            else//update
            {
                var edit = _dbContext.Emails.SingleOrDefault(u => u.Id == email.Id);

                edit.EmailAddress = email.EmailAddress;
                edit.Type = email.Type;
            }

            return await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> Delete(int id)
        {
            var select = await _dbContext.Emails.SingleOrDefaultAsync(u => u.Id == id);

            if (select == null)
                return false;

            _dbContext.Emails.Remove(select);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
