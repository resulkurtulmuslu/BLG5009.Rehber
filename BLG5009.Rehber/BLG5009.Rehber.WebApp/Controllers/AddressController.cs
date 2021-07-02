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
    public class AddressController : Controller
    {
        private readonly DatabaseContext _dbContext;

        public AddressController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Route("kullanici-adresleri/{userId}")]
        public IActionResult Index(int userId)
        {
            ViewBag.User = _dbContext.Users.SingleOrDefault(u => u.Id == userId);

            return View();
        }

        public async Task<IActionResult> GetAddressByUserId(int userId)
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"]
                    .FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                var telephoneData = _dbContext.Addresses.Where(t => t.UserId == userId).AsQueryable();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                    telephoneData = telephoneData.OrderBy(sortColumn + " " + sortColumnDirection);

                if (!string.IsNullOrEmpty(searchValue))
                    telephoneData = telephoneData.Where(m => m.AddressText.Contains(searchValue));

                recordsTotal = await telephoneData.Where(m => m.AddressText.Contains(searchValue)).CountAsync();

                var models = await telephoneData.Skip(skip).Take(pageSize).ToListAsync();

                List<AddressViewModel> data = new List<AddressViewModel>();

                models.ForEach(x =>
                {
                    data.Add(new AddressViewModel
                    {
                        Id = x.Id,
                        AddressText = x.AddressText,
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
            Address address = new Address();
            address.UserId = userId;

            if (id != null)
                address = await _dbContext.Addresses.SingleOrDefaultAsync(u => u.Id == id.Value);

            ViewBag.Types = EnumExtensionMethods.GetDescriptionList<AddressType>();

            return PartialView(address);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<int> Form(Address address)
        {
            if (address.Id == 0)//yeni kayıt
            {
                await _dbContext.Addresses.AddAsync(address);
            }
            else//update
            {
                var edit = _dbContext.Addresses.SingleOrDefault(u => u.Id == address.Id);

                edit.AddressText = address.AddressText;
                edit.Type = address.Type;
            }

            return await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> Delete(int id)
        {
            var select = await _dbContext.Addresses.SingleOrDefaultAsync(u => u.Id == id);

            if (select == null)
                return false;

            _dbContext.Addresses.Remove(select);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
