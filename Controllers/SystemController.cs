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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ABAC.Controllers
{
    [Authorize]
    public class SystemController : ControllerBase
    {
        private IUserProvider provider;
        public string _rootPath;
        public SystemController(SpuContext context, ILogger<SystemController> logger, ILoginServices loginServices, IUserProvider provider, IOptions<SystemConf> conf, IHostingEnvironment env) : base(context, logger, loginServices, conf, provider)
        {
            this.provider = provider;
            this._rootPath = env.WebRootPath;
        }

        public IActionResult LandingPage()
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.WebMaster }))
                return RedirectToAction("Logout", "Auth");

            var model = new SearchDTO();
            model.lists = _context.table_landing_page;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LandingPage(IFormFile file)
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.WebMaster }))
                return RedirectToAction("Logout", "Auth");

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context, _conf.Env);
            if (userlogin == null)
                return RedirectToAction("Logout", "Auth");

            if (ModelState.IsValid)
            {
                ViewBag.Message = ReturnMessage.Error;
                ViewBag.ReturnCode = ReturnCode.Error;
                if (file != null)
                {
                    var ex = Path.GetExtension(file.FileName);
                    var filename = Guid.NewGuid().ToString() + ex;
                    var filePath = Path.Combine(_rootPath + "\\files\\landingpage", filename);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);

                    }
                    filePath = filePath.Replace(_rootPath, "");
                    filePath = filePath.Replace("\\", "/");
                    var model = new landing_page();
                    model.Url = filePath;
                    model.File_Name = filename;
                    model.Create_On = DateUtil.Now();
                    model.Create_By = userlogin.SamAccountName;
                    model.Update_On = DateUtil.Now();
                    model.Update_By = userlogin.SamAccountName;
                    this._context.table_landing_page.Add(model);
                    this._context.SaveChanges();


                }
                ViewBag.Message = ReturnMessage.Success;
                ViewBag.ReturnCode = ReturnCode.Success;
                return RedirectToAction("LandingPage");
            }
            return RedirectToAction("LandingPage");
        }

        public IActionResult LandingPageDelete(int? id)
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.WebMaster }))
                return RedirectToAction("Logout", "Auth");

            ViewBag.Message = ReturnMessage.Error;
            ViewBag.ReturnCode = ReturnCode.Error;
            if (id.HasValue)
            {
                var model = this._context.table_landing_page.Where(a => a.ID == id).FirstOrDefault();
                if (model == null)
                    return RedirectToAction("LandingPage");
                else
                {
                    this._context.table_landing_page.Remove(model);
                    this._context.SaveChanges();

                    var filePath = Path.Combine(_rootPath + "\\files\\landingpage", model.File_Name);
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                    this._context.SaveChanges();
                    ViewBag.Message = ReturnMessage.Success;
                    ViewBag.ReturnCode = ReturnCode.Success;
                }
            }
            return RedirectToAction("LandingPage");
        }

        public IActionResult CMSStaff()
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.WebMaster }))
                return RedirectToAction("Logout", "Auth");

            var model = _context.table_cms.FirstOrDefault();
            if (model == null)
                model = new cms();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CMSStaff(cms model)
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.WebMaster }))
                return RedirectToAction("Logout", "Auth");

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context, _conf.Env);
            if (userlogin == null)
                return RedirectToAction("Logout", "Auth");

            if (ModelState.IsValid)
            {
                ViewBag.Message = ReturnMessage.Error;
                ViewBag.ReturnCode = ReturnCode.Error;
                if (model.ID > 0)
                {
                    var setup = _context.table_cms.Where(w => w.ID == model.ID).FirstOrDefault();
                    if (setup != null)
                    {
                        setup.HOME_Staff = model.HOME_Staff;
                        setup.Update_On = DateUtil.Now();
                        setup.Update_By = userlogin.SamAccountName;
                        this._context.SaveChanges();
                    }
                }
                else
                {
                    model.HOME_Staff = model.HOME_Staff;
                    model.Create_On = DateUtil.Now();
                    model.Create_By = userlogin.SamAccountName;
                    model.Update_On = DateUtil.Now();
                    model.Update_By = userlogin.SamAccountName;
                    this._context.table_cms.Add(model);
                    this._context.SaveChanges();
                }
                ViewBag.Message = ReturnMessage.Success;
                ViewBag.ReturnCode = ReturnCode.Success;
            }
            return View(model);
        }

        public IActionResult CMSVIP()
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.WebMaster }))
                return RedirectToAction("Logout", "Auth");

            var model = _context.table_cms.FirstOrDefault();
            if (model == null)
                model = new cms();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CMSVIP(cms model)
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.WebMaster }))
                return RedirectToAction("Logout", "Auth");

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context, _conf.Env);
            if (userlogin == null)
                return RedirectToAction("Logout", "Auth");

            if (ModelState.IsValid)
            {
                ViewBag.Message = ReturnMessage.Error;
                ViewBag.ReturnCode = ReturnCode.Error;
                if (model.ID > 0)
                {
                    var setup = _context.table_cms.Where(w => w.ID == model.ID).FirstOrDefault();
                    if (setup != null)
                    {
                        setup.HOME_VIP = model.HOME_VIP;
                        setup.Update_On = DateUtil.Now();
                        setup.Update_By = userlogin.SamAccountName;
                        this._context.SaveChanges();
                    }
                }
                else
                {
                    model.HOME_VIP = model.HOME_VIP;
                    model.Create_On = DateUtil.Now();
                    model.Create_By = userlogin.SamAccountName;
                    model.Update_On = DateUtil.Now();
                    model.Update_By = userlogin.SamAccountName;
                    this._context.table_cms.Add(model);
                    this._context.SaveChanges();
                }
                ViewBag.Message = ReturnMessage.Success;
                ViewBag.ReturnCode = ReturnCode.Success;
            }
            return View(model);
        }

        public IActionResult CMSStudent()
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.WebMaster }))
                return RedirectToAction("Logout", "Auth");

            var model = _context.table_cms.FirstOrDefault();
            if (model == null)
                model = new cms();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CMSStudent(cms model)
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.WebMaster }))
                return RedirectToAction("Logout", "Auth");

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context, _conf.Env);
            if (userlogin == null)
                return RedirectToAction("Logout", "Auth");

            if (ModelState.IsValid)
            {
                ViewBag.Message = ReturnMessage.Error;
                ViewBag.ReturnCode = ReturnCode.Error;
                if (model.ID > 0)
                {
                    var setup = _context.table_cms.Where(w => w.ID == model.ID).FirstOrDefault();
                    if (setup != null)
                    {
                        setup.HOME_Student = model.HOME_Student;
                        setup.Update_On = DateUtil.Now();
                        setup.Update_By = userlogin.SamAccountName;
                        this._context.SaveChanges();
                    }
                }
                else
                {
                    model.HOME_Student = model.HOME_Student;
                    model.Create_On = DateUtil.Now();
                    model.Create_By = userlogin.SamAccountName;
                    model.Update_On = DateUtil.Now();
                    model.Update_By = userlogin.SamAccountName;
                    this._context.table_cms.Add(model);
                    this._context.SaveChanges();
                }
                ViewBag.Message = ReturnMessage.Success;
                ViewBag.ReturnCode = ReturnCode.Success;
            }
            return View(model);
        }

        public IActionResult CMSGuest()
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.WebMaster }))
                return RedirectToAction("Logout", "Auth");

            var model = _context.table_cms.FirstOrDefault();
            if (model == null)
                model = new cms();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CMSGuest(cms model)
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.WebMaster }))
                return RedirectToAction("Logout", "Auth");

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context, _conf.Env);
            if (userlogin == null)
                return RedirectToAction("Logout", "Auth");

            if (ModelState.IsValid)
            {
                ViewBag.Message = ReturnMessage.Error;
                ViewBag.ReturnCode = ReturnCode.Error;
                if (model.ID > 0)
                {
                    var setup = _context.table_cms.Where(w => w.ID == model.ID).FirstOrDefault();
                    if (setup != null)
                    {
                        setup.HOME_Guest = model.HOME_Guest;
                        setup.Update_On = DateUtil.Now();
                        setup.Update_By = userlogin.SamAccountName;
                        this._context.SaveChanges();
                    }
                }
                else
                {
                    model.HOME_Guest = model.HOME_Guest;
                    model.Create_On = DateUtil.Now();
                    model.Create_By = userlogin.SamAccountName;
                    model.Update_On = DateUtil.Now();
                    model.Update_By = userlogin.SamAccountName;
                    this._context.table_cms.Add(model);
                    this._context.SaveChanges();
                }
                ViewBag.Message = ReturnMessage.Success;
                ViewBag.ReturnCode = ReturnCode.Success;
            }
            return View(model);
        }

        public IActionResult CMSOffice()
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.WebMaster }))
                return RedirectToAction("Logout", "Auth");

            var model = _context.table_cms.FirstOrDefault();
            if (model == null)
                model = new cms();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CMSOffice(cms model)
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.WebMaster }))
                return RedirectToAction("Logout", "Auth");

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context, _conf.Env);
            if (userlogin == null)
                return RedirectToAction("Logout", "Auth");

            if (ModelState.IsValid)
            {
                ViewBag.Message = ReturnMessage.Error;
                ViewBag.ReturnCode = ReturnCode.Error;
                if (model.ID > 0)
                {
                    var setup = _context.table_cms.Where(w => w.ID == model.ID).FirstOrDefault();
                    if (setup != null)
                    {
                        setup.HOME_Office = model.HOME_Office;
                        setup.Update_On = DateUtil.Now();
                        setup.Update_By = userlogin.SamAccountName;
                        this._context.SaveChanges();
                    }
                }
                else
                {
                    model.HOME_Office = model.HOME_Office;
                    model.Create_On = DateUtil.Now();
                    model.Create_By = userlogin.SamAccountName;
                    model.Update_On = DateUtil.Now();
                    model.Update_By = userlogin.SamAccountName;
                    this._context.table_cms.Add(model);
                    this._context.SaveChanges();
                }
                ViewBag.Message = ReturnMessage.Success;
                ViewBag.ReturnCode = ReturnCode.Success;
            }
            return View(model);
        }

        //public IActionResult Setup()
        //{
        //    if (!checkrole(new string[] { UserRole.admin }))
        //        return RedirectToAction("Logout", "Auth");

        //    var model = _context.table_setup.FirstOrDefault();
        //    if (model == null)
        //        model = new setup();
        //    return View(model);
        //}
        //[HttpPost]
        //public IActionResult Setup(setup model)
        //{
        //    if (!checkrole(new string[] { UserRole.admin }))
        //        return RedirectToAction("Logout", "Auth");

        //    var userlogin = this._context.table_visual_fim_user.Where(w => w.basic_uid == this.HttpContext.User.Identity.Name).FirstOrDefault();
        //    if (userlogin == null)
        //        return RedirectToAction("Logout", "Auth");

        //    if (ModelState.IsValid)
        //    {
        //        ViewBag.Message = ReturnMessage.Error;
        //        ViewBag.ReturnCode = ReturnCode.Error;
        //        if (model.ID > 0)
        //        {
        //            var setup = _context.table_setup.Where(w => w.ID == model.ID).FirstOrDefault();
        //            if (setup != null)
        //            {
        //                setup.Host = model.Host;
        //                setup.Port = model.Port;
        //                setup.Base = model.Base;
        //                setup.Username = model.Username;
        //                setup.Password = model.Password;
        //                setup.SMTP_From = model.SMTP_From;
        //                setup.SMTP_Password = model.SMTP_Password;
        //                setup.SMTP_Port = model.SMTP_Port;
        //                setup.SMTP_Server = model.SMTP_Server;
        //                setup.SMTP_SSL = model.SMTP_SSL;
        //                setup.SMTP_Username = model.SMTP_Username;
        //                setup.change_password_approve_enable = model.change_password_approve_enable;
        //                setup.change_password_otp_enable = model.change_password_otp_enable;
        //                setup.first_page_description = model.first_page_description;
        //                setup.Update_On = DateUtil.Now();
        //                setup.Update_By = userlogin.basic_uid;
        //                this._context.SaveChanges();
        //            }
        //        }
        //        else
        //        {
        //            model.Create_On = DateUtil.Now();
        //            model.Create_By = userlogin.basic_uid;
        //            model.Update_On = DateUtil.Now();
        //            model.Update_By = userlogin.basic_uid;
        //            this._context.table_setup.Add(model);
        //            this._context.SaveChanges();
        //        }
        //        ViewBag.Message = ReturnMessage.Success;
        //        ViewBag.ReturnCode = ReturnCode.Success;
        //    }
        //    return View(model);
        //}

        public IActionResult PasswordGenerator()
        {
            var model = new PasswordGenerateDTO();
            model.Length = 8;
            model.PasswordCnt = 1;
            model.Number = true;
            model.Lower = true;
            model.Passwords = new List<string>();
            return View(model);
        }
        [HttpPost]
        public IActionResult PasswordGenerator(PasswordGenerateDTO model)
        {
            model.Passwords = new List<string>();

            if (model.Number == false & model.Lower == false && model.Upper == false)
                ModelState.AddModelError("Condition", "กรุณาระบุเงือนไข");

            if (ModelState.IsValid)
            {
                for (var i = 0; i < model.PasswordCnt; i++)
                {
                    var password = RandomPassword(model.Length, model.Number, model.Lower, model.Upper);
                    model.Passwords.Add(password);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UserRole(UserRoleDTO model)
        {
            if (!checkrole(new string[] { roleType.Admin}))
                return RedirectToAction("Logout", "Auth");

            if (string.IsNullOrEmpty(model.userrole_search))
                model.userrole_search = roleType.Admin;
            model.lists = _context.table_user_role.Where(w => w.roleType == model.userrole_search);

            if (!string.IsNullOrEmpty(model.text_search))
            {
                var smodel = new SearchDTO();
                smodel.text_search = model.text_search;
                smodel.usertype_search = aUUserType.admin;
                string[] roles = { aUUserType.admin, aUUserType.staff};
                var adusers = await _provider.FindUser(smodel, roles, _context, _conf.Env);
                model.lists2 = adusers.AsQueryable();
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult UserRole(UserRoleDTO model, string userrole_search)
        {
            if (!checkrole(new string[] { roleType.Admin }))
                return RedirectToAction("Logout", "Auth");

            model.lists = _context.table_landing_page;
            return View(model);
        }
        public async Task<JsonResult> UserRoleDelete(string choose)
        {
            if (!checkrole(new string[] { roleType.Admin }))
                return Json(new { error = ReturnMessage.Error, result = ReturnCode.Error });

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context, _conf.Env);
            if (userlogin == null)
                return Json(new { error = ReturnMessage.Error, result = ReturnCode.Error });

            if (!string.IsNullOrEmpty(choose))
            {
                var lists = choose.Split(";", StringSplitOptions.RemoveEmptyEntries).ToList();
                if (lists.Count() > 0)
                {
                    foreach (var id in lists)
                    {
                        try
                        {
                            var userrole = _context.table_user_role.Where(w => w.username == id).FirstOrDefault();
                            if (userrole != null)
                            {
                                _context.table_user_role.Remove(userrole);
                            }
                        }
                        catch (Exception ex)
                        {
                            writelog(LogType.log_delete_user_role, LogStatus.failed, IDMSource.Database, id, log_exception: ex.Message);
                        }
                    }
                    _context.SaveChanges();
                    writelog(LogType.log_delete_user_role, LogStatus.successfully, IDMSource.Database, userlogin.SamAccountName);
                    return Json(new { error = ReturnMessage.Success, result = ReturnCode.Success });
                }
            }
            return Json(new { error = ReturnMessage.Error, result = ReturnCode.Error });
        }

        public async Task<JsonResult> UserRoleAdd(string choose, string usertype_search)
        {
            if (!checkrole(new string[] { roleType.Admin }))
                return Json(new { error = ReturnMessage.Error, result = ReturnCode.Error });

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context, _conf.Env);
            if (userlogin == null)
                return Json(new { error = ReturnMessage.Error, result = ReturnCode.Error });

            if (!string.IsNullOrEmpty(choose))
            {
                var lists = choose.Split(";", StringSplitOptions.RemoveEmptyEntries).ToList();
                if (lists.Count() > 0)
                {
                    foreach (var id in lists)
                    {
                        try
                        {
                            var userrole = _context.table_user_role.Where(w => w.username == id & w.roleType == usertype_search).FirstOrDefault();
                            if (userrole == null)
                            {
                                var model = new user_role();
                                model.username = id;
                                model.roleType = usertype_search;
                                model.Create_By = userlogin.SamAccountName;
                                model.Create_On = DateUtil.Now();
                                _context.table_user_role.Add(model);
                            }
                        }
                        catch (Exception ex)
                        {
                            writelog(LogType.log_add_user_role, LogStatus.failed, IDMSource.Database, id, log_exception: ex.Message);
                        }
                    }
                    _context.SaveChanges();
                    writelog(LogType.log_add_user_role, LogStatus.successfully, IDMSource.Database, userlogin.SamAccountName);
                    return Json(new { error = ReturnMessage.Success, result = ReturnCode.Success });
                }
            }
            return Json(new { error = ReturnMessage.Error, result = ReturnCode.Error });
        }
    }
}
