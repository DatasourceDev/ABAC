using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ABAC.Models;
using ABAC.DAL;
using ABAC.Services;
using Microsoft.EntityFrameworkCore;
using ABAC.DTO;
using ABAC.Extensions;
using ABAC.Identity;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace ABAC.Controllers
{
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private IUserProvider provider;

        public ProfileController(SpuContext context, ILogger<ProfileController> logger, ILoginServices loginServices, IUserProvider provider,  IOptions<SystemConf> conf) : base(context, logger, loginServices, conf, provider)
        {
            this.provider = provider;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context);
            if (model != null)
            {

            }
            return View(model);
        }

        public IActionResult ChangePassword()
        {
            var model = new ChangePasswordDTO();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO model)
        {
            var aduser = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context);
            if (aduser == null)
                return RedirectToAction("Logout", "Auth");

            //if (model.OldPassword == model.Password)
            //{
            //    ModelState.AddModelError("OldPassword", "รหัสผ่านใหม่ต้องไม่ตรงกับรหัสผ่านเก่า กรุณาเปลี่ยนรหัสผ่านใหม่");
            //    ModelState.AddModelError("Password", "รหัสผ่านใหม่ต้องไม่ตรงกับรหัสผ่านเก่า กรุณาเปลี่ยนรหัสผ่านใหม่");
            //}
            if (ModelState.IsValid)
            {
                var setup = _context.table_setup.FirstOrDefault();
                if (_provider.ValidateCredentials(aduser.SamAccountName, model.OldPassword, _context).result == false)
                {
                    ModelState.AddModelError("OldPassword", "รหัสผ่านเดิมไม่ถูกต้อง");
                    return View(model);
                }

                ViewBag.Message = ReturnMessage.ChangePasswordFail;
                ViewBag.ReturnCode = ReturnCode.Error;

                var result_ad = _provider.ChangePwd(aduser, model.Password, _context);
                if (result_ad.result == true)
                    writelog(LogType.log_change_password, LogStatus.successfully, IDMSource.AD, aduser.SamAccountName);
                else
                    writelog(LogType.log_change_password, LogStatus.failed, IDMSource.AD, aduser.SamAccountName, log_exception: result_ad.Message);

                ViewBag.Message = ReturnMessage.ChangePasswordSuccess;
                ViewBag.ReturnCode = ReturnCode.Success;

            }

            return View(model);
        }
    }
}
