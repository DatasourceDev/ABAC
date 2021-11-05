//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using ABAC.Models;
//using ABAC.DAL;
//using ABAC.Services;
//using Microsoft.EntityFrameworkCore;
//using ABAC.DTO;
//using ABAC.Extensions;
//using ABAC.Identity;
//using Microsoft.Extensions.Options;
//using Microsoft.AspNetCore.Authorization;

//namespace ABAC.Controllers
//{
//    [Authorize]
//    public class SystemController : ControllerBase
//    {
//        private IUserProvider provider;

//        public SystemController(SpuContext context, ILogger<SystemController> logger, ILoginServices loginServices, IUserProvider provider, ILDAPUserProvider providerldap, IOptions<SystemConf> conf) : base(context, logger, loginServices, conf, provider, providerldap)
//        {
//            this.provider = provider;

//        }
      
       
//        public IActionResult Setup()
//        {
//            if (!checkrole(new string[] { UserRole.admin }))
//                return RedirectToAction("Logout", "Auth");

//            var model = _context.table_setup.FirstOrDefault();
//            if (model == null)
//                model = new setup();
//            return View(model);
//        }
//        [HttpPost]
//        public IActionResult Setup(setup model)
//        {
//            if (!checkrole(new string[] { UserRole.admin}))
//                return RedirectToAction("Logout", "Auth");

//            var userlogin = this._context.table_visual_fim_user.Where(w => w.basic_uid == this.HttpContext.User.Identity.Name).FirstOrDefault();
//            if (userlogin == null)
//                return RedirectToAction("Logout", "Auth");

//            if (ModelState.IsValid)
//            {
//                ViewBag.Message = ReturnMessage.Error;
//                ViewBag.ReturnCode = ReturnCode.Error;
//                if (model.ID > 0)
//                {
//                    var setup = _context.table_setup.Where(w => w.ID == model.ID).FirstOrDefault();
//                    if(setup != null)
//                    {
//                        setup.Host = model.Host;
//                        setup.Port = model.Port;
//                        setup.Base = model.Base;
//                        setup.Username = model.Username;
//                        setup.Password = model.Password;
//                        setup.SMTP_From = model.SMTP_From;
//                        setup.SMTP_Password = model.SMTP_Password;
//                        setup.SMTP_Port = model.SMTP_Port;
//                        setup.SMTP_Server = model.SMTP_Server;
//                        setup.SMTP_SSL = model.SMTP_SSL;
//                        setup.SMTP_Username = model.SMTP_Username;
//                        setup.change_password_approve_enable = model.change_password_approve_enable;
//                        setup.change_password_otp_enable = model.change_password_otp_enable;
//                        setup.first_page_description = model.first_page_description;
//                        setup.Update_On = DateUtil.Now();
//                        setup.Update_By = userlogin.basic_uid;
//                        this._context.SaveChanges();
//                    }
//                }
//                else
//                {
//                    model.Create_On = DateUtil.Now();
//                    model.Create_By = userlogin.basic_uid;
//                    model.Update_On = DateUtil.Now();
//                    model.Update_By = userlogin.basic_uid;
//                    this._context.table_setup.Add(model);
//                    this._context.SaveChanges();
//                }
//                ViewBag.Message = ReturnMessage.Success;
//                ViewBag.ReturnCode = ReturnCode.Success;
//            }
//            return View(model);
//        }

//        public IActionResult PasswordGenerator()
//        {
//            var model = new PasswordGenerateDTO();
//            model.Length = 8;
//            model.PasswordCnt = 1;
//            model.Number = true;
//            model.Lower = true;
//            model.Passwords = new List<string>();
//            return View(model);
//        }
//        [HttpPost]
//        public IActionResult PasswordGenerator(PasswordGenerateDTO model)
//        {
//            model.Passwords = new List<string>();

//            if (model.Number == false & model.Lower == false && model.Upper == false)
//                ModelState.AddModelError("Condition", "กรุณาระบุเงือนไข");

//            if (ModelState.IsValid)
//            {
//                for(var i=0;i< model.PasswordCnt; i++)
//                {
//                    var password = RandomPassword(model.Length, model.Number, model.Lower, model.Upper);
//                    model.Passwords.Add(password);
//                }
//            }
//            return View(model);
//        }
//        //public IActionResult Role(SearchDTO model)
//        //{
//        //    model.lists = this._context.Roles.Include(i => i.OU).OrderBy(o => o.Index);

//        //    ViewBag.Message = model.msg;
//        //    ViewBag.ReturnCode = model.code;
//        //    return View(model);
//        //}
//        //public IActionResult RoleInfo(int? id)
//        //{
//        //    Role model = new Role();
//        //    if (id.HasValue)
//        //    {
//        //        model = _context.Roles.Include(i => i.OU).Where(w => w.ID == id).FirstOrDefault();
//        //        if (model != null)
//        //        {
//        //            model.SelectedAdmins = _context.AdminRoles.Where(w => w.RoleID == model.ID).Select(s => s.Admin);
//        //            model.SelectedID = model.SelectedAdmins.Select(s => s.ID).ToArray();
//        //        }
//        //    }

//        //    var UnSelectedAdmins = _context.table_visual_fim_user.Where(w => 1 == 1);
//        //    if (model.SelectedID != null)
//        //    {
//        //        model.UnSelecteAdmins = UnSelectedAdmins.Where(w => !model.SelectedID.Contains(w.id));
//        //        model.UnSelectedID = model.UnSelecteAdmins.Select(s => s.ID).ToArray();
//        //    }
//        //    return View(model);
//        //}


//        //[HttpPost]
//        //public IActionResult RoleInfo(Role model)
//        //{
//        //    var user = this._context.Users.Where(w => w.UserName == this.HttpContext.User.Identity.Name).FirstOrDefault();
//        //    if (user == null)
//        //        return RedirectToAction("Logout", "Auth");

//        //    if (ModelState.IsValid)
//        //    {
//        //        ViewBag.Message = ReturnMessage.Error;
//        //        ViewBag.ReturnCode = ReturnCode.Error;

//        //        if (model.ID > 0)
//        //        {
//        //            model.Update_On = DateUtil.Now();
//        //            model.Update_By = user.UserName;
//        //            model.OU = null;
//        //            this._context.Update(model);

//        //            var adminroles = _context.AdminRoles.Where(w => w.RoleID == model.ID);
//        //            if (adminroles.Count() > 0)
//        //                _context.AdminRoles.RemoveRange(adminroles);

//        //            if (model.SelectedID != null)
//        //            {
//        //                foreach (var id in model.SelectedID)
//        //                {
//        //                    var adminrole = new AdminRole();
//        //                    adminrole.AdminID = id;
//        //                    adminrole.RoleID = model.ID;
//        //                    _context.AdminRoles.Add(adminrole);
//        //                }
//        //            }
//        //            this._context.SaveChanges();
//        //            return RedirectToAction("Role", "System", new { code = ReturnCode.Success, msg = ReturnMessage.Success });
//        //        }
//        //        else
//        //        {
//        //            model.Create_On = DateUtil.Now();
//        //            model.Create_By = user.UserName;
//        //            model.Update_On = DateUtil.Now();
//        //            model.Update_By = user.UserName;
//        //            model.OU = null;
//        //            if (model.SelectedID != null)
//        //            {
//        //                foreach (var id in model.SelectedID)
//        //                {
//        //                    var adminrole = new AdminRole();
//        //                    adminrole.AdminID = id;
//        //                    adminrole.Role = model;
//        //                    _context.AdminRoles.Add(adminrole);
//        //                }
//        //            }
//        //            this._context.Roles.Add(model);
//        //            this._context.SaveChanges();
//        //            return RedirectToAction("Role", "System", new { code = ReturnCode.Success, msg = ReturnMessage.Success });
//        //        }
//        //    }
//        //    return View(model);
//        //}


//    }
//}
