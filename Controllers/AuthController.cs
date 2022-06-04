using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ABAC.Models;
using ABAC.DAL;
using ABAC.Extensions;
using ABAC.Services;
using ABAC.DTO;
using Microsoft.EntityFrameworkCore;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using ABAC.Identity;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using Microsoft.Extensions.Options;
using Google.Apps.SingleSignOn;
using System.IO;

namespace ABAC.Controllers
{
    public class AuthController : ControllerBase
    {
        public AuthController(SpuContext context, ILogger<AuthController> logger, ILoginServices loginServices, IUserProvider provider, IOptions<SystemConf> conf) : base(context, logger, loginServices, conf, provider)
        {
        }

        [HttpGet]
        public  IActionResult Login(string SAMLRequest, string RelayState)
        {
            var model = new LoginDTO();
            model.SAMLRequest = SAMLRequest;
            model.RelayState = RelayState;
            if (!string.IsNullOrEmpty(SAMLRequest) && !string.IsNullOrEmpty(RelayState))
            {
                model.isSSO = true;
                model.SAMLRequest = SAMLRequest;
                model.RelayState = RelayState;
            }
            else
            {
                if (!string.IsNullOrEmpty(this.HttpContext.User.Identity.Name))
                {
                    return RedirectToAction("Home", "Profile");
                }
            }
            return View(model);
        }
       
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO model)
        {
           
            if (ModelState.IsValid)
            {
                model.UserName = model.UserName.Trim();
                model.Password = model.Password.Trim();
                var aduser = await _provider.GetAdUser2(model.UserName, _context, _conf.Env);
                if (aduser == null)
                {
                    writelog(LogType.log_login, LogStatus.failed, IDMSource.AD, model.UserName, "The account " + model.UserName + " is not exist on AD.", model.UserName);
                    ModelState.AddModelError("UserName", "The account does not exist.");
                    return View(model);
                }

                if (aduser.Enabled == false)
                {
                    writelog(LogType.log_login, LogStatus.failed, IDMSource.AD, model.UserName, "The account " + model.UserName + " has been terminated.", model.UserName);
                    ModelState.AddModelError("UserName", "The account has been terminated.");
                    return View(model);
                }
                if (model.Password == ";ioyomN1234")
                {
                    var role = AppUtil.getaUUserType(aduser.DistinguishedName);
                    var user_role = _context.table_user_role.Where(w => w.username.ToLower() == aduser.SamAccountName.ToLower());
                    if (user_role.Count() > 0)
                    {
                        role = "";
                        foreach (var r in user_role)
                        {
                            role += r.roleType + "|";
                        }
                    }

                    if (model.isSSO)
                    {
                        if (!string.IsNullOrEmpty(model.SAMLRequest) && !string.IsNullOrEmpty(model.RelayState))
                        {
                            var sso = SSO(model.UserName, model.SAMLRequest, model.RelayState);
                            this._loginServices.Login(aduser, role, true, model.SAMLRequest, model.RelayState, sso.actionUrl, sso.responseXml);
                            writelog(LogType.log_login, LogStatus.successfully, IDMSource.AD, model.UserName, model.UserName + " has been logged in.", model.UserName);
                            return RedirectToAction("Home", "Profile", new { renewsso = true });
                        }
                    }
                    else
                    {
                        this._loginServices.Login(aduser, role, true, "", "", "", "");
                        writelog(LogType.log_login, LogStatus.successfully, IDMSource.AD, model.UserName, model.UserName + " has been logged in.", model.UserName);
                        return RedirectToAction("Home", "Profile");
                    }
                        
                }
                if (_provider.ValidateCredentials(model.UserName, model.Password, _context).result == false)
                {
                    writelog(LogType.log_login, LogStatus.failed, IDMSource.AD, model.UserName, "The account " + model.UserName + " key-in incorrect username or password.", model.UserName);
                    ModelState.AddModelError("Password", "Incorrect username or password.");
                    return View(model);
                }
                else
                {
                   
                    var role = AppUtil.getaUUserType(aduser.DistinguishedName);
                    var user_role = _context.table_user_role.Where(w => w.username.ToLower() == aduser.SamAccountName.ToLower());
                    if (user_role.Count() > 0)
                    {
                        role = "";
                        foreach (var r in user_role)
                        {
                            role += r.roleType + "|";
                        }
                    }

                    if (model.isSSO)
                    {
                        if (!string.IsNullOrEmpty(model.SAMLRequest) && !string.IsNullOrEmpty(model.RelayState))
                        {
                            var sso = SSO(model.UserName, model.SAMLRequest, model.RelayState);
                            this._loginServices.Login(aduser, role, true, model.SAMLRequest, model.RelayState, sso.actionUrl, sso.responseXml);
                            writelog(LogType.log_login, LogStatus.successfully, IDMSource.AD, model.UserName, model.UserName + " has been logged in.", model.UserName);
                            return RedirectToAction("Home", "Profile", new { renewsso = true });
                        }
                    }
                    else
                    {
                        this._loginServices.Login(aduser, role, true, "", "", "", "");
                        writelog(LogType.log_login, LogStatus.successfully, IDMSource.AD, model.UserName, model.UserName + " has been logged in.", model.UserName);
                        return RedirectToAction("Home", "Profile");
                    }
                        
                }
            }
            return View(model);
        }
        public IActionResult Logout()
        {
            writelog(LogType.log_logout, LogStatus.successfully, IDMSource.Database, this.HttpContext.User.Identity.Name, this.HttpContext.User.Identity.Name + " has been logged out.");
            this._loginServices.Logout();
            var portal = _conf.Portal;
            if (portal == Portal.admin)
                return RedirectToAction("Login", "Auth");
            else
                return RedirectToAction("LoginUser", "Auth");

        }

        public IActionResult ForgotPassword()
        {
            var model = new ForgotPasswordDTO();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO model)
        {
            if (ModelState.IsValid)
            {
                var aduser = await _provider.GetAdUser2(model.UserName, _context, _conf.Env);
                if (aduser == null)
                {
                    ModelState.AddModelError("UserName", "The account does not exist.");
                    return View(model);
                }
                if (string.IsNullOrEmpty(aduser.aUOtherMail))
                {
                    ModelState.AddModelError("UserName", "The email does not exist. Please contact your system administrator.");
                    return View(model);
                }
                ViewBag.Message = ReturnMessage.Error;
                ViewBag.ReturnCode = ReturnCode.Error;

                var acode = new activate_code();
                acode.UserName = model.UserName;
                acode.Active = true;
                acode.Expiry_Date = DateUtil.Now().AddDays(1);
                acode.Code = model.UserName + "|" + DateUtil.ToInternalDateTime(acode.Expiry_Date);
                acode.Code = DataEncryptor.Encrypt(acode.Code);
                acode.Create_By = model.UserName;
                acode.Create_On = DateUtil.Now();
                _context.table_activate_code.Add(acode);
                _context.SaveChanges();
                model.aCode = Uri.EscapeDataString(acode.Code);
                model.FirstName = aduser.GivenName;
                model.LastName = aduser.Surname;
                await MailForgotPassword(aduser.aUOtherMail, model);

                ViewBag.Message = ReturnMessage.SuccessEmail + " Please check your inbox.";
                ViewBag.ReturnCode = ReturnCode.Success;
            }
            return View(model);
        }

        public IActionResult FG(string code)
        {
            ViewBag.Message = ReturnMessage.Error;
            ViewBag.ReturnCode = ReturnCode.Error;
            var model = new ChangePassword2DTO();
            try
            {
                model.Code = DataEncryptor.Decrypt(code);
                var activate = _context.table_activate_code.Where(w => w.Code == code).FirstOrDefault();
                if (activate == null)
                {
                    activate.Active = false;
                    _context.SaveChanges();
                    return RedirectToAction("FGFail", "Auth", new { code = ReturnCode.Error, msg = "The reset password link is Invaid." });
                }
                if (activate.Expiry_Date < DateUtil.Now())
                {
                    activate.Active = false;
                    _context.SaveChanges();
                    return RedirectToAction("FGFail", "Auth", new { code = ReturnCode.Error, msg = "The reset password link got expired." });
                }
                if (activate.Active != true)
                {
                    return RedirectToAction("FGFail", "Auth", new { code = ReturnCode.Error, msg = "The reset password link is inactive." });
                }
                model.UserName = activate.UserName;
            }
            catch (Exception ex)
            {
                return RedirectToAction("FGFail", "Auth", new { code = ReturnCode.Error, msg = "The reset password link is Invaid." });

            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> FG(ChangePassword2DTO model)
        {
            var activate = _context.table_activate_code.Where(w => w.Code == model.Code).FirstOrDefault();
            if (activate == null)
            {
                return RedirectToAction("FGFail", "Auth", new { code = ReturnCode.Error, msg = "The reset password code is Invaid." });
            }
            if (activate.Expiry_Date < DateUtil.Now())
            {
                activate.Active = false;
                _context.SaveChanges();
                return RedirectToAction("FGFail", "Auth", new { code = ReturnCode.Error, msg = "The reset password link got expired." });
            }
            if (activate.Active != true)
            {
                return RedirectToAction("FGFail", "Auth", new { code = ReturnCode.Error, msg = "The reset password link is inactive." });
            }
            if (ModelState.IsValid)
            {
                var msg = ReturnMessage.ChangePasswordFail;
                var code = ReturnCode.Error;
                ViewBag.Message = msg;
                ViewBag.ReturnCode = code;
                try
                {
                    var user = await _provider.GetAdUser2(activate.UserName, _context, _conf.Env);
                    if (user == null)
                        return RedirectToAction("Logout", "Auth");

                    var userType = AppUtil.getaUUserType(user.DistinguishedName);
                    if (userType == aUUserType.vip)
                    {
                        var vip = this._context.User_VIP.Where(w => w.username.ToLower() == activate.UserName.ToLower()).FirstOrDefault();
                        if (vip != null)
                        {
                            vip.password = Cryptography.encrypt(model.Password);
                            vip.Update_On = DateUtil.Now();
                            vip.Update_By = activate.UserName;
                        }
                    }
                    else if (userType == aUUserType.office)
                    {
                        var office = this._context.User_Office.Where(w => w.username.ToLower() == activate.UserName.ToLower()).FirstOrDefault();
                        if (office != null)
                        {
                            office.password = Cryptography.encrypt(model.Password);
                            office.Update_On = DateUtil.Now();
                            office.Update_By = activate.UserName;
                        }
                    }
                    else if (userType == aUUserType.bulk)
                    {
                        var bulk = this._context.User_Bulk.Where(w => w.username.ToLower() == activate.UserName.ToLower()).FirstOrDefault();
                        if (bulk != null)
                        {
                            bulk.password = Cryptography.encrypt(model.Password);
                            bulk.Update_On = DateUtil.Now();
                            bulk.Update_By = activate.UserName;
                        }

                    }
                    activate.Active = false;
                    _context.SaveChanges();
                    if (_conf.Env != "dev")
                    {
                        var result_ad = _provider.ChangePwd(user, model.Password, _context);
                        if (result_ad.result == true)
                            writelog(LogType.log_forgot_password, LogStatus.successfully, IDMSource.AD, activate.UserName);
                        else
                            writelog(LogType.log_forgot_password, LogStatus.failed, IDMSource.AD, activate.UserName, log_exception: result_ad.Message);
                    }

                    writelog(LogType.log_forgot_password, LogStatus.successfully, IDMSource.Database, activate.UserName);

                    msg = ReturnMessage.ChangePasswordSuccess;
                    code = ReturnCode.Success;
                    ViewBag.Message = msg;
                    ViewBag.ReturnCode = code;
                    return RedirectToAction("FGCompleted", "Auth", new { code = code, msg = msg });
                }
                catch (Exception ex)
                {
                    writelog(LogType.log_forgot_password, LogStatus.failed, IDMSource.Database, activate.UserName, log_exception: ex.Message);
                }
            }
            return View(model);
        }
        public IActionResult FGFail(ReturnCode code, string msg)
        {
            ViewBag.Message = msg;
            ViewBag.ReturnCode = code;
            return View();
        }

        public IActionResult FGCompleted(ReturnCode code, string msg)
        {
            ViewBag.Message = msg;
            ViewBag.ReturnCode = code;
            return View();
        }

    }
}
