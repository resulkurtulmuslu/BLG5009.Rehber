using BLG5009.Rehber.WebApp.Methods;
using BLG5009.Rehber.WebApp.Models;
using BLG5009.Rehber.WebApp.Models.Context;
using BLG5009.Rehber.WebApp.Models.Enums;
using BLG5009.Rehber.WebApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace BLG5009.Rehber.WebApp.Controllers
{
    public class TelephoneController : Controller
    {
        private readonly DatabaseContext _dbContext;

        public TelephoneController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Route("telefon-numaralari/{userId}")]
        public IActionResult Index(int userId)
        {
            ViewBag.User = _dbContext.Users.SingleOrDefault(u => u.Id == userId);

            return View();
        }

        public async Task<IActionResult> GetTelephoneNumbersByUserId(int userId)
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

                var telephoneData = _dbContext.Telephones.Where(t => t.UserId == userId).AsQueryable();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                    telephoneData = telephoneData.OrderBy(sortColumn + " " + sortColumnDirection);

                if (!string.IsNullOrEmpty(searchValue))
                    telephoneData = telephoneData.Where(m => m.Number.Contains(searchValue));

                recordsTotal = await telephoneData.Where(m => m.Number.Contains(searchValue)).CountAsync();

                var models = await telephoneData.Skip(skip).Take(pageSize).ToListAsync();

                List<TelephoneViewModel> data = new List<TelephoneViewModel>();

                models.ForEach(x =>
                {
                    data.Add(new TelephoneViewModel
                    {
                        Id = x.Id,
                        Number = x.Number,
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
            Telephone telephone = new Telephone();
            telephone.UserId = userId;

            if (id != null)
                telephone = await _dbContext.Telephones.SingleOrDefaultAsync(u => u.Id == id.Value);

            ViewBag.Types = EnumExtensionMethods.GetDescriptionList<TelephoneType>();

            return PartialView(telephone);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<int> Form(Telephone telephone)
        {
            if (telephone.Id == 0)//yeni kayıt
            {
                await _dbContext.Telephones.AddAsync(telephone);
            }
            else//update
            {
                var editUser = _dbContext.Telephones.SingleOrDefault(u => u.Id == telephone.Id);

                editUser.Number = telephone.Number;
                editUser.Type = telephone.Type;
            }

            return await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> Delete(int id)
        {
            var select = await _dbContext.Telephones.SingleOrDefaultAsync(u => u.Id == id);

            if (select == null)
                return false;

            _dbContext.Telephones.Remove(select);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
