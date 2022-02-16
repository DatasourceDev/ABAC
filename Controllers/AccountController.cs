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
using ABAC.Identity;
using ABAC.DTO;
using Microsoft.EntityFrameworkCore;
using ABAC.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using System.IO;
using OfficeOpenXml;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;

namespace ABAC.Controllers
{
    [Authorize]
    public class AccountController : ControllerBase
    {
        public readonly IWebHostEnvironment _env;
        public AccountController(SpuContext context, ILogger<AccountController> logger, ILoginServices loginServices, IUserProvider provider, IOptions<SystemConf> conf, IWebHostEnvironment env) : base(context, logger, loginServices, conf, provider)
        {

            this._env = env;

        }


        #region CreateAccount
        public async Task<IActionResult> CreateAccount(ReturnCode code, string msg)
        {

            if (!checkrole(new string[] { roleType.Admin, roleType.Helpdesk }))
                return RedirectToAction("Logout", "Auth");

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context, _conf.Env);
            if (userlogin == null)
                return RedirectToAction("Logout", "Auth");

            var model = new AdUser2();
            model.aUUserType = aUUserType.vip;
            //model.system_faculty_id = 0;
            //model.cu_CUexpire_day = DateUtil.Now().Day;
            //model.cu_CUexpire_month = DateUtil.Now().Month;
            //model.cu_CUexpire_year = DateUtil.Now().Year + 1;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(AdUser2 model)
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.Helpdesk }))
                return RedirectToAction("Logout", "Auth");

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context, _conf.Env);
            if (userlogin == null)
                return RedirectToAction("Logout", "Auth");

            if (_conf.Env != "dev")
            {
                var dup = await _provider.GetAdUser2(model.SamAccountName, _context, _conf.Env);
                if (dup != null)
                {
                    ModelState.AddModelError("SamAccountName", "username already exists.");
                }
            }
            if (model.aUUserType == aUUserType.bulk) { 
                if (!string.IsNullOrEmpty(model.ValidDate) && !string.IsNullOrEmpty(model.ExpireDate))
                {
                    var vd = DateUtil.ToDate(model.ValidDate);
                    var exd = DateUtil.ToDate(model.ExpireDate);
                    if (vd.HasValue & exd.HasValue)
                    {
                        if (exd.Value.Date < vd.Value.Date)
                        {
                            ModelState.AddModelError("ValidDate", "The Expire Date should be more than Valid Date");
                            ModelState.AddModelError("ExpireDate", "The Expire Date should be more than Valid Date");
                        }
                    }
                }
            }
            if (ModelState.IsValid)
            {
                model.DisplayName = model.GivenName + " " + model.Surname;
                ViewBag.Message = ReturnMessage.Error;
                ViewBag.ReturnCode = ReturnCode.Error;
                try
                {
                    if (model.aUUserType == aUUserType.office)
                    {
                        model.DistinguishedName = _conf.OU_OFFICE;
                        var user = new User_Office();
                        user.username = model.SamAccountName;
                        user.password = model.Password;
                        user.firstname = model.GivenName;
                        user.lastname = model.Surname;
                        user.CitizenID = model.aUIDCard;
                        user.PassportID = model.PassportID;
                        user.Reference = model.Reference;
                        user.adminname = userlogin.SamAccountName;
                        user.Create_By = userlogin.SamAccountName;
                        user.Create_On = DateUtil.Now();
                        user.Update_By = userlogin.SamAccountName;
                        user.Update_On = DateUtil.Now();
                        _context.User_Office.Add(user);
                        _context.SaveChanges();
                        if (_conf.Env != "dev")
                        {
                            var result_ad = _provider.CreateUser(model, _context);
                            if (result_ad.result == true)
                                writelog(LogType.log_create_account, LogStatus.successfully, IDMSource.AD, model.SamAccountName);
                            else
                                writelog(LogType.log_create_account, LogStatus.failed, IDMSource.AD, model.SamAccountName, log_exception: result_ad.Message);

                            user.ad_created = result_ad.result;
                            _context.SaveChanges();
                        }

                        writelog(LogType.log_create_account, LogStatus.successfully, IDMSource.Database, model.SamAccountName);
                    }
                    else if (model.aUUserType == aUUserType.vip)
                    {
                        model.DistinguishedName = _conf.OU_VIP;
                        var user = new User_VIP();
                        user.username = model.SamAccountName;
                        user.password = model.Password;
                        user.firstname = model.GivenName;
                        user.lastname = model.Surname;
                        user.CitizenID = model.aUIDCard;
                        user.PassportID = model.PassportID;
                        user.Reference = model.Reference;
                        user.adminname = userlogin.SamAccountName;
                        user.Create_By = userlogin.SamAccountName;
                        user.Create_On = DateUtil.Now();
                        user.Update_By = userlogin.SamAccountName;
                        user.Update_On = DateUtil.Now();
                        _context.User_VIP.Add(user);
                        _context.SaveChanges();
                        if (_conf.Env != "dev")
                        {
                            var result_ad = _provider.CreateUser(model, _context);
                            if (result_ad.result == true)
                                writelog(LogType.log_create_account, LogStatus.successfully, IDMSource.AD, model.SamAccountName);
                            else
                                writelog(LogType.log_create_account, LogStatus.failed, IDMSource.AD, model.SamAccountName, log_exception: result_ad.Message);
                            user.ad_created = result_ad.result;
                            _context.SaveChanges();
                        }


                        writelog(LogType.log_create_account, LogStatus.successfully, IDMSource.Database, model.SamAccountName);

                    }
                    else if (model.aUUserType == aUUserType.bulk)
                    {
                        model.DistinguishedName = _conf.OU_TEMP;
                        var user = new User_Bulk();
                        user.username = model.SamAccountName;
                        user.password = model.Password;
                        user.firstname = model.GivenName;
                        user.lastname = model.Surname;
                        user.valid_date = DateUtil.ToDate(model.ValidDate);
                        user.expire_date = DateUtil.ToDate(model.ExpireDate);
                        user.today = DateUtil.Now();
                        user.adminname = userlogin.SamAccountName;
                        user.Create_By = userlogin.SamAccountName;
                        user.Create_On = DateUtil.Now();
                        user.Update_By = userlogin.SamAccountName;
                        user.Update_On = DateUtil.Now();
                        _context.User_Bulk.Add(user);
                        _context.SaveChanges();
                        if (_conf.Env != "dev")
                        {
                            var result_ad = _provider.CreateUser(model, _context);
                            if (result_ad.result == true)
                                writelog(LogType.log_create_account, LogStatus.successfully, IDMSource.AD, model.SamAccountName);
                            else
                                writelog(LogType.log_create_account, LogStatus.failed, IDMSource.AD, model.SamAccountName, log_exception: result_ad.Message);

                            user.ad_created = result_ad.result;
                            _context.SaveChanges();
                        }
                        writelog(LogType.log_create_account, LogStatus.successfully, IDMSource.Database, model.SamAccountName);
                    }
                    else
                    {
                        return View(model);
                    }
                    ViewBag.Message = ReturnMessage.Success;
                    ViewBag.ReturnCode = ReturnCode.Success;
                    return RedirectToAction("CreateAccountCompleted", new { code = ReturnCode.Success, msg = ReturnMessage.Success, id = model.SamAccountName });
                }
                catch (Exception ex)
                {
                    writelog(LogType.log_create_account, LogStatus.failed, IDMSource.Database, model.SamAccountName, log_exception: ex.Message);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> CreateAccountCompleted(ReturnCode code, string msg, string id)
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.Helpdesk }))
                return RedirectToAction("Logout", "Auth");

            var model = new AdUser2();
            if (!string.IsNullOrEmpty(id))
            {
                model = await _provider.GetAdUser2(id, _context, _conf.Env);
                if (model == null)
                    return RedirectToAction("Logout", "Auth");
            }

            if (!string.IsNullOrEmpty(msg))
            {
                ViewBag.Message = msg;
                ViewBag.ReturnCode = code;
            }
            return View(model);
        }
        #endregion

        #region CreateAccountFromFile
        public IActionResult CreateAccountFromFile(SearchDTO model)
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.Helpdesk }))
                return RedirectToAction("Logout", "Auth");

            model.lists = (new List<temp_import>()).AsQueryable();
            if (model.code == ReturnCode.Success)
                model.lists = (_context.table_temp_import).AsQueryable();
            ViewBag.Message = model.msg;
            ViewBag.ReturnCode = model.code;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccountFromFile(IFormFile file)
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.Helpdesk }))
                return RedirectToAction("Logout", "Auth");

            var model = new SearchDTO();
            var lists = new List<temp_import>();
            if (file != null)
            {
                _context.table_temp_import.RemoveRange(_context.table_temp_import);
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream).ConfigureAwait(false);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(memoryStream))
                    {
                        var workbook = package.Workbook;
                        var worksheet = workbook.Worksheets.First();

                        var rowCount = worksheet.Dimension?.Rows;
                        var colCount = worksheet.Dimension?.Columns;
                        if (rowCount.HasValue && colCount.HasValue)
                        {
                            for (int row = 1; row <= rowCount.Value; row++)
                            {
                                var remark = new StringBuilder();
                                var guest = new temp_import();
                                guest.ImportVerify = true;
                                guest.ImportRow = row;
                                try
                                {
                                    guest.firstname = worksheet.Cells["A" + row].Value != null ? worksheet.Cells["A" + row].Value.ToString() : "";
                                    guest.lastname = worksheet.Cells["B" + row].Value != null ? worksheet.Cells["B" + row].Value.ToString() : "";
                                    guest.CitizenID = worksheet.Cells["C" + row].Value != null ? worksheet.Cells["C" + row].Value.ToString() : "";
                                    guest.PassportID = worksheet.Cells["D" + row].Value != null ? worksheet.Cells["D" + row].Value.ToString() : "";
                                    guest.Reference = worksheet.Cells["E" + row].Value != null ? worksheet.Cells["E" + row].Value.ToString() : "";
                                    if (string.IsNullOrEmpty(guest.firstname))
                                    {
                                        guest.ImportVerify = false;
                                        remark.AppendLine("firstname ไม่สามารถเป็นค่าว่าง");
                                    }
                                    if (string.IsNullOrEmpty(guest.lastname))
                                    {
                                        guest.ImportVerify = false;
                                        remark.AppendLine("lastname ไม่สามารถเป็นค่าว่าง");
                                    }
                                    guest.ImportRemark = remark.ToString();
                                }
                                catch (Exception ex)
                                {
                                    remark.AppendLine(ex.Message);
                                    guest.ImportVerify = false;
                                }
                                lists.Add(guest);
                                if (guest.ImportVerify == true)
                                    _context.table_temp_import.Add(guest);
                            }
                            _context.SaveChanges();
                        }
                    }
                }
            }
            model.lists = lists.AsQueryable();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccountFromFile2(SearchDTO model2)
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.Helpdesk }))
                return RedirectToAction("Logout", "Auth");

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context, _conf.Env);
            if (userlogin == null)
                return RedirectToAction("Logout", "Auth");

            var msg = ReturnMessage.ImportFail;
            var code = ReturnCode.Error;

            var setup = _context.table_setup.FirstOrDefault();
            if (setup == null)
                return RedirectToAction("Logout", "Auth");

            if (!string.IsNullOrEmpty(model2.dfrom) && !string.IsNullOrEmpty(model2.dto))
            {
                var vd = DateUtil.ToDate(model2.dfrom);
                var exd = DateUtil.ToDate(model2.dto);
                if (vd.HasValue & exd.HasValue)
                {
                    if (exd.Value.Date < vd.Value.Date)
                    {
                        return RedirectToAction("CreateAccountFromFile", new { code = code, msg = "The Expire Date should be more than Valid Date" });
                    }
                }
            }

            int runNumber = setup.GuestRowNumber + 1;
            var imports = _context.table_temp_import.OrderBy(o => o.ImportRow);
            foreach (var imp in imports.ToList())
            {
                var username = "guest" + runNumber.ToString("00000");
                var dup = true;
                while (dup == true)
                {
                    if (_conf.Env == "dev")
                    {
                        dup = false;
                        break;
                    }
                    var account = await _provider.GetAdUser2(username, _context, _conf.Env);
                    if (account == null)
                    {
                        dup = false;
                        break;
                    }
                    runNumber++;
                    username = "guest" + runNumber.ToString("00000");
                }

                var user = new User_Bulk_Import();
                user.firstname = imp.firstname;
                user.lastname = imp.lastname;
                user.CitizenID = imp.CitizenID;
                user.PassportID = imp.PassportID;
                user.Reference = imp.Reference;
                user.username = username;
                user.password = RandomPassword(8);
                user.adminname = userlogin.SamAccountName;
                user.Create_By = userlogin.SamAccountName;
                user.Create_On = DateUtil.Now();
                user.Update_By = userlogin.SamAccountName;
                user.Update_On = DateUtil.Now();
                user.valid_date = DateUtil.ToDate(model2.dfrom);
                user.expire_date = DateUtil.ToDate(model2.dto);
                user.today = DateUtil.Now();
                _context.User_Bulk_Import.Add(user);

                var aduser = new AdUser2();
                aduser.DistinguishedName = _conf.OU_TEMP;
                aduser.SamAccountName = user.username;
                aduser.GivenName = user.firstname;
                aduser.Surname = user.lastname;
                aduser.DisplayName = aduser.GivenName + " " + aduser.Surname;
                aduser.aUIDCard = user.CitizenID;
                aduser.Reference = user.Reference;
                aduser.PassportID = user.PassportID;
                aduser.Password = user.password;
                aduser.ValidDate = DateUtil.ToDisplayDate(user.valid_date);
                aduser.ExpireDate = DateUtil.ToDisplayDate(user.expire_date);
                aduser.aUUserType = aUUserType.bulk;
                setup.GuestRowNumber = runNumber;
                _context.SaveChanges();
                runNumber++;
                if (_conf.Env != "dev")
                {
                    var result_ad = _provider.CreateUser(aduser, _context);
                    if (result_ad.result == true)
                        writelog(LogType.log_create_account_with_file, LogStatus.successfully, IDMSource.AD, aduser.SamAccountName);
                    else
                        writelog(LogType.log_create_account_with_file, LogStatus.failed, IDMSource.AD, aduser.SamAccountName, log_exception: result_ad.Message);

                    user.ad_created = result_ad.result;
                }
                imp.username = user.username;
                imp.password = user.password;
                imp.ImportRemark = "Completed";
                _context.SaveChanges();
                writelog(LogType.log_create_account_with_file, LogStatus.successfully, IDMSource.Database, aduser.SamAccountName);
            }
            //_context.table_temp_import.RemoveRange(_context.table_temp_import);
            _context.SaveChanges();
            msg = ReturnMessage.ImportSuccess;
            code = ReturnCode.Success;
            return RedirectToAction("CreateAccountFromFileCompleted", new { code = code, msg = msg });
        }

        public IActionResult CreateAccountFromFileCompleted(SearchDTO model)
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.Helpdesk }))
                return RedirectToAction("Logout", "Auth");

            model.lists = (new List<temp_import>()).AsQueryable();
            if (model.code == ReturnCode.Success)
                model.lists = (_context.table_temp_import).AsQueryable();
            ViewBag.Message = model.msg;
            ViewBag.ReturnCode = model.code;
            return View(model);
        }
        #endregion

        #region CreateAccount
        public async Task<IActionResult> CreateAccountBulk(ReturnCode code, string msg)
        {

            if (!checkrole(new string[] { roleType.Admin, roleType.Helpdesk }))
                return RedirectToAction("Logout", "Auth");

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context, _conf.Env);
            if (userlogin == null)
                return RedirectToAction("Logout", "Auth");

            var model = new Bulk();
            if (!string.IsNullOrEmpty(msg))
            {
                ViewBag.Message = msg;
                ViewBag.ReturnCode = code;
            }
            model.NumberOfPeople = 1;
            model.ValidDate = DateUtil.ToDisplayDate(DateUtil.Now());
            //model.system_faculty_id = 0;
            //model.cu_CUexpire_day = DateUtil.Now().Day;
            //model.cu_CUexpire_month = DateUtil.Now().Month;
            //model.cu_CUexpire_year = DateUtil.Now().Year + 1;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccountBulk(Bulk model)
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.Helpdesk }))
                return RedirectToAction("Logout", "Auth");

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context, _conf.Env);
            if (userlogin == null)
                return RedirectToAction("Logout", "Auth");

            var msg = ReturnMessage.Error;
            var code = ReturnCode.Error;

            if (!string.IsNullOrEmpty(model.ValidDate) && !string.IsNullOrEmpty(model.ExpireDate))
            {
                var vd = DateUtil.ToDate(model.ValidDate);
                var exd = DateUtil.ToDate(model.ExpireDate);
                if (vd.HasValue & exd.HasValue)
                {
                    if (exd.Value.Date < vd.Value.Date)
                    {
                        return RedirectToAction("CreateAccountBulk", new { code = code, msg = "The Expire Date should be more than Valid Date" });
                    }
                }
            }

            var setup = _context.table_setup.FirstOrDefault();
            if (setup == null)
                return RedirectToAction("Logout", "Auth");

            if (model.NumberOfPeople <= 0)
                model.NumberOfPeople = 1;

            var temp = "";
            int runNumber = setup.GuestRowNumber + 1;
            for (var i = 0; i < model.NumberOfPeople; i++)
            {
                var username = "guest" + runNumber.ToString("00000");
                var dup = true;
                while (dup == true)
                {
                    if (_conf.Env == "dev")
                    {
                        dup = false;
                        break;
                    }
                    var account = await _provider.GetAdUser2(username, _context, _conf.Env);
                    if (account == null)
                    {
                        dup = false;
                        break;
                    }
                    runNumber++;
                    username = "guest" + runNumber.ToString("00000");
                }

                var user = new User_Bulk();
                user.username = username;
                user.password = RandomPassword(8);
                user.adminname = userlogin.SamAccountName;
                user.Create_By = userlogin.SamAccountName;
                user.Create_On = DateUtil.Now();
                user.Update_By = userlogin.SamAccountName;
                user.Update_On = DateUtil.Now();
                user.valid_date = DateUtil.ToDate(model.ValidDate);
                user.expire_date = DateUtil.ToDate(model.ExpireDate);
                user.today = DateUtil.Now();
                _context.User_Bulk.Add(user);

                var aduser = new AdUser2();
                aduser.DistinguishedName = _conf.OU_TEMP;
                aduser.SamAccountName = user.username;
                aduser.GivenName = user.firstname;
                aduser.Surname = user.lastname;
                aduser.DisplayName = aduser.GivenName + " " + aduser.Surname;
                aduser.Password = user.password;
                aduser.ValidDate = DateUtil.ToDisplayDate(user.valid_date);
                aduser.ExpireDate = DateUtil.ToDisplayDate(user.expire_date);
                aduser.aUUserType = aUUserType.bulk;
                setup.GuestRowNumber = runNumber;
                _context.SaveChanges();
                runNumber++;

                if (_conf.Env != "dev")
                {
                    var result_ad = _provider.CreateUser(aduser, _context);
                    if (result_ad.result == true)
                        writelog(LogType.log_create_account_bulk, LogStatus.successfully, IDMSource.AD, aduser.SamAccountName);
                    else
                        writelog(LogType.log_create_account_bulk, LogStatus.failed, IDMSource.AD, aduser.SamAccountName, log_exception: result_ad.Message);

                    user.ad_created = result_ad.result;
                }

                _context.SaveChanges();
                temp += user.username + "|";
                writelog(LogType.log_create_account_bulk, LogStatus.successfully, IDMSource.Database, aduser.SamAccountName);

            }
            msg = ReturnMessage.Success;
            code = ReturnCode.Success;
            return RedirectToAction("CreateAccountBulkCompleted", new { code = code, msg = msg, temp = temp });
        }

        public IActionResult CreateAccountBulkCompleted(SearchDTO model)
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.Helpdesk }))
                return RedirectToAction("Logout", "Auth");

            if (!string.IsNullOrEmpty(model.temp))
            {
                var users = model.temp.Split("|", StringSplitOptions.RemoveEmptyEntries);
                model.lists = users.AsQueryable();
            }

            ViewBag.Message = model.msg;
            ViewBag.ReturnCode = model.code;
            return View(model);
        }
        #endregion

        #region ManageAccount     
        public async Task<IActionResult> AccountInfo(string id)
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.Helpdesk }))
                return RedirectToAction("Logout", "Auth");

            var aduser = await _provider.GetAdUser2(id, _context, _conf.Env);
            if (aduser != null)
            {
                if (aduser.aUUserType == aUUserType.bulk)
                {
                    var bulk = _context.User_Bulk.Where(w => w.username == aduser.SamAccountName).FirstOrDefault();
                    if (bulk != null)
                        aduser.ValidDate = DateUtil.ToDisplayDate(bulk.valid_date);
                    else
                    {
                        var bulkimp = _context.User_Bulk_Import.Where(w => w.username == aduser.SamAccountName).FirstOrDefault();
                        if (bulkimp != null)
                            aduser.ValidDate = DateUtil.ToDisplayDate(bulkimp.valid_date);
                    }

                    aduser.ExpireDate = DateUtil.ToDisplayDate(aduser.accountExpires);
                }
            }

            return View(aduser);
        }

        [HttpPost]
        public async Task<IActionResult> AccountInfo(AdUser2 model)
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.Helpdesk }))
                return RedirectToAction("Logout", "Auth");

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context, _conf.Env);
            if (userlogin == null)
                return RedirectToAction("Logout", "Auth");

            if (!string.IsNullOrEmpty(model.ValidDate) && !string.IsNullOrEmpty(model.ExpireDate))
            {
                var vd = DateUtil.ToDate(model.ValidDate);
                var exd = DateUtil.ToDate(model.ExpireDate);
                if(vd.HasValue & exd.HasValue)
                {
                    if (exd.Value.Date < vd.Value.Date)
                    {
                        ModelState.AddModelError("ValidDate", "The Expire Date should be more than Valid Date");
                        ModelState.AddModelError("ExpireDate", "The Expire Date should be more than Valid Date");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                ViewBag.Message = ReturnMessage.Error;
                ViewBag.ReturnCode = ReturnCode.Error;

                if (model.isnew == false)
                {
                    var aduser = await _provider.GetAdUser2(model.SamAccountName, _context, _conf.Env);
                    if (aduser != null)
                    {
                        aduser.GivenName = model.GivenName;
                        aduser.Surname = model.Surname;
                        aduser.DisplayName = model.GivenName + " " + model.Surname;
                        aduser.EmailAddress = model.EmailAddress;
                        aduser.aUIDCard = model.aUIDCard;
                        aduser.PassportID = model.PassportID;
                        aduser.Reference = model.Reference;
                        aduser.accountExpires = DateUtil.ToDate(model.ExpireDate);
                        aduser.aUOtherMail = model.aUOtherMail;

                        if (model.aUUserType == aUUserType.office)
                        {
                            var user = _context.User_Office.Where(w => w.username == aduser.SamAccountName).FirstOrDefault();
                            if (user != null)
                            {
                                user.firstname = model.GivenName;
                                user.lastname = model.Surname;
                                user.CitizenID = model.aUIDCard;
                                user.PassportID = model.PassportID;
                                user.Reference = model.Reference;
                                user.adminname = userlogin.SamAccountName;
                                user.Update_By = userlogin.SamAccountName;
                                user.Update_On = DateUtil.Now();
                                _context.SaveChanges();
                                writelog(LogType.log_edit_account, LogStatus.successfully, IDMSource.Database, model.SamAccountName);
                            }
                        }
                        else if (model.aUUserType == aUUserType.vip)
                        {
                            var user = _context.User_VIP.Where(w => w.username == aduser.SamAccountName).FirstOrDefault();
                            if (user != null)
                            {
                                user.firstname = model.GivenName;
                                user.lastname = model.Surname;
                                user.CitizenID = model.aUIDCard;
                                user.PassportID = model.PassportID;
                                user.Reference = model.Reference;
                                user.adminname = userlogin.SamAccountName;
                                user.Update_By = userlogin.SamAccountName;
                                user.Update_On = DateUtil.Now();
                                _context.SaveChanges();
                                writelog(LogType.log_edit_account, LogStatus.successfully, IDMSource.Database, model.SamAccountName);
                            }

                        }
                        else if (model.aUUserType == aUUserType.bulk)
                        {
                            var user = _context.User_Bulk.Where(w => w.username == aduser.SamAccountName).FirstOrDefault();
                            if (user != null)
                            {
                                user.firstname = model.GivenName;
                                user.lastname = model.Surname;
                                user.valid_date = DateUtil.ToDate(model.ValidDate);
                                user.expire_date = DateUtil.ToDate(model.ExpireDate);
                                user.today = DateUtil.Now();
                                user.adminname = userlogin.SamAccountName;
                                user.Update_By = userlogin.SamAccountName;
                                user.Update_On = DateUtil.Now();
                                _context.SaveChanges();
                                writelog(LogType.log_edit_account, LogStatus.successfully, IDMSource.Database, model.SamAccountName);
                            }
                            else
                            {
                                var bulkimp = _context.User_Bulk_Import.Where(w => w.username == aduser.SamAccountName).FirstOrDefault();
                                if (bulkimp != null)
                                {
                                    bulkimp.firstname = model.GivenName;
                                    bulkimp.lastname = model.Surname;
                                    bulkimp.valid_date = DateUtil.ToDate(model.ValidDate);
                                    bulkimp.expire_date = DateUtil.ToDate(model.ExpireDate);
                                    bulkimp.today = DateUtil.Now();
                                    bulkimp.adminname = userlogin.SamAccountName;
                                    bulkimp.Update_By = userlogin.SamAccountName;
                                    bulkimp.Update_On = DateUtil.Now();
                                    _context.SaveChanges();
                                    writelog(LogType.log_edit_account, LogStatus.successfully, IDMSource.Database, model.SamAccountName);
                                }
                            }
                        }
                    }
                    var result_ad = _provider.UpdateUser(aduser, _context);
                    if (result_ad.result == true)
                        writelog(LogType.log_edit_account, LogStatus.successfully, IDMSource.AD, model.SamAccountName);
                    else
                        writelog(LogType.log_edit_account, LogStatus.failed, IDMSource.AD, model.SamAccountName, log_exception: result_ad.Message);


                    return RedirectToAction("CheckAccount", "Account", new { code = ReturnCode.Success, msg = ReturnMessage.Success });
                }
            }
            return View(model);
        }
        #endregion
        #region Rename Account     
        public async Task<IActionResult> Rename(SearchDTO model)
        {
            ViewBag.Message = model.msg;
            ViewBag.ReturnCode = model.code;

            if (!checkrole(new string[] { roleType.Admin, roleType.Helpdesk }))
                return RedirectToAction("Logout", "Auth");

            if (string.IsNullOrEmpty(model.text_search))
                return View(model);

            model.text_search = model.text_search.Trim();
            if (model.text_search.Length <= 3)
            {
                ModelState.AddModelError("text_search", "Search Text must have at least 3 characters.");
                return View(model);
            }
            //string[] roles = { model.usertype_search.toUserTypeName() };
            var adusers = await _provider.FindUser(model, null, _context);


            int skipRows = (model.pageno - 1) * _pagelen;
            var itemcnt = adusers.Count();
            var pagelen = itemcnt / _pagelen;
            if (itemcnt % _pagelen > 0)
                pagelen += 1;

            model.itemcnt = itemcnt;
            model.pagelen = pagelen;
            //model.lists = lists.Skip(skipRows).Take(_pagelen).AsQueryable();
            model.lists = adusers.AsQueryable();
            return View(model);
        }
        public async Task<IActionResult> RenameInfo(string id)
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.Helpdesk }))
                return RedirectToAction("Logout", "Auth");

            var aduser = await _provider.GetAdUser2(id, _context, _conf.Env);
            if (aduser == null)
            {
                return RedirectToAction("Rename");
            }
            var model = new RenameDTO();
            model.SamAccountName = aduser.SamAccountName;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RenameInfo(RenameDTO model)
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.Helpdesk }))
                return RedirectToAction("Logout", "Auth");

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context, _conf.Env);
            if (userlogin == null)
                return RedirectToAction("Logout", "Auth");

            if (ModelState.IsValid)
            {
                ViewBag.Message = ReturnMessage.Error;
                ViewBag.ReturnCode = ReturnCode.Error;

                var aduser = await _provider.GetAdUser2(model.SamAccountName, _context, _conf.Env);
                if (aduser != null)
                {
                    if (aduser.aUUserType == aUUserType.office)
                    {
                        var dup = _context.User_Office.Where(w => w.username == model.newSamAccountName).FirstOrDefault();
                        if (dup != null)
                        {
                            ModelState.AddModelError("newSamAccountName", "Account name is already existed");
                            return View(model);
                        }

                        var user = _context.User_Office.Where(w => w.username == aduser.SamAccountName).FirstOrDefault();
                        if (user != null)
                        {
                            user.username = model.newSamAccountName;
                            user.adminname = userlogin.SamAccountName;
                            user.Update_By = userlogin.SamAccountName;
                            user.Update_On = DateUtil.Now();
                            _context.SaveChanges();
                            writelog(LogType.log_rename, LogStatus.successfully, IDMSource.Database, model.SamAccountName);
                        }
                    }
                    else if (aduser.aUUserType == aUUserType.vip)
                    {
                        var dup = _context.User_VIP.Where(w => w.username == model.newSamAccountName).FirstOrDefault();
                        if (dup != null)
                        {
                            ModelState.AddModelError("newSamAccountName", "Account name is already existed");
                            return View(model);
                        }
                        var user = _context.User_VIP.Where(w => w.username == aduser.SamAccountName).FirstOrDefault();
                        if (user != null)
                        {
                            user.username = model.newSamAccountName;
                            user.adminname = userlogin.SamAccountName;
                            user.Update_By = userlogin.SamAccountName;
                            user.Update_On = DateUtil.Now();
                            _context.SaveChanges();
                            writelog(LogType.log_rename, LogStatus.successfully, IDMSource.Database, model.SamAccountName);
                        }

                    }
                    else if (aduser.aUUserType == aUUserType.bulk)
                    {
                        var dup = _context.User_Bulk.Where(w => w.username == model.newSamAccountName).FirstOrDefault();
                        if (dup != null)
                        {
                            ModelState.AddModelError("newSamAccountName", "Account name is already existed");
                            return View(model);
                        }
                        var user = _context.User_Bulk.Where(w => w.username == aduser.SamAccountName).FirstOrDefault();
                        if (user != null)
                        {
                            user.username = model.newSamAccountName;
                            user.today = DateUtil.Now();
                            user.adminname = userlogin.SamAccountName;
                            user.Update_By = userlogin.SamAccountName;
                            user.Update_On = DateUtil.Now();
                            _context.SaveChanges();
                            writelog(LogType.log_rename, LogStatus.successfully, IDMSource.Database, model.SamAccountName);
                        }
                        else
                        {
                            var bulkimp = _context.User_Bulk_Import.Where(w => w.username == aduser.SamAccountName).FirstOrDefault();
                            if (bulkimp != null)
                            {
                                user.username = model.newSamAccountName;
                                bulkimp.today = DateUtil.Now();
                                bulkimp.adminname = userlogin.SamAccountName;
                                bulkimp.Update_By = userlogin.SamAccountName;
                                bulkimp.Update_On = DateUtil.Now();
                                _context.SaveChanges();
                                writelog(LogType.log_rename, LogStatus.successfully, IDMSource.Database, model.SamAccountName);
                            }
                        }
                    }
                }
                if (_conf.Env != "dev")
                {
                    var result_ad = _provider.RenameUser(aduser, model.newSamAccountName, _context);
                    if (result_ad.result == true)
                        writelog(LogType.log_rename, LogStatus.successfully, IDMSource.AD, model.SamAccountName);
                    else
                        writelog(LogType.log_rename, LogStatus.failed, IDMSource.AD, model.SamAccountName, log_exception: result_ad.Message);

                }

                return RedirectToAction("Rename", "Account", new { code = ReturnCode.Success, msg = ReturnMessage.Success });

            }
            return View(model);
        }
        #endregion
        #region DeleteAccount
        public async Task<IActionResult> DeleteAccount(SearchDTO model)
        {
            ViewBag.Message = model.msg;
            ViewBag.ReturnCode = model.code;

            if (!checkrole(new string[] { roleType.Admin }))
                return RedirectToAction("Logout", "Auth");

            if (string.IsNullOrEmpty(model.text_search))
                return View(model);

            model.text_search = model.text_search.Trim();
            if (model.text_search.Length <= 3)
            {
                ModelState.AddModelError("text_search", "Search Text must have at least 3 characters.");
                return View(model);
            }
            //string[] roles = new { aUUserType.student.toUserTypeName(), aUUserType.staff.toUserTypeName() };
            var adusers = await _provider.FindUser(model, null, _context);


            int skipRows = (model.pageno - 1) * _pagelen;
            var itemcnt = adusers.Count();
            var pagelen = itemcnt / _pagelen;
            if (itemcnt % _pagelen > 0)
                pagelen += 1;

            model.itemcnt = itemcnt;
            model.pagelen = pagelen;
            //model.lists = lists.Skip(skipRows).Take(_pagelen).AsQueryable();
            model.lists = adusers.AsQueryable();
            return View(model);
        }

        public async Task<JsonResult> Delete(string choose)
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.Helpdesk }))
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
                            var model = await _provider.GetAdUser2(id, _context, _conf.Env);
                            if (model != null)
                            {
                                var userType = AppUtil.getaUUserType(model.DistinguishedName);
                                if (userType == aUUserType.vip)
                                {
                                    var vip = _context.User_VIP.Where(w => w.username == id).FirstOrDefault();
                                    if (vip != null)
                                    {
                                        _context.Remove(vip);
                                        _context.SaveChanges();
                                    }
                                }
                                else if (userType == aUUserType.office)
                                {
                                    var office = _context.User_Office.Where(w => w.username.ToLower() == id.ToLower()).FirstOrDefault();
                                    if (office != null)
                                    {
                                        _context.Remove(office);
                                        _context.SaveChanges();
                                    }
                                }
                                else if (userType == aUUserType.bulk)
                                {
                                    var bulk = _context.User_Bulk.Where(w => w.username.ToLower() == id.ToLower()).FirstOrDefault();
                                    if (bulk != null)
                                    {
                                        _context.Remove(bulk);
                                        _context.SaveChanges();
                                    }
                                    var bulkimp = _context.User_Bulk_Import.Where(w => w.username.ToLower() == id.ToLower()).FirstOrDefault();
                                    if (bulkimp != null)
                                    {
                                        _context.Remove(bulkimp);
                                        _context.SaveChanges();
                                    }
                                }
                                var result_ad = _provider.DeleteUser(model, _context);
                                if (result_ad.result == true)
                                    writelog(LogType.log_delete_account, LogStatus.successfully, IDMSource.AD, model.SamAccountName);
                                else
                                    writelog(LogType.log_delete_account, LogStatus.failed, IDMSource.AD, model.SamAccountName, log_exception: result_ad.Message);


                                writelog(LogType.log_delete_account, LogStatus.successfully, IDMSource.Database, model.SamAccountName);
                            }

                        }
                        catch (Exception ex)
                        {
                            writelog(LogType.log_delete_account, LogStatus.failed, IDMSource.Database, id, log_exception: ex.Message);
                        }
                    }
                    return Json(new { error = ReturnMessage.Success, result = ReturnCode.Success });
                }
            }
            return Json(new { error = ReturnMessage.Error, result = ReturnCode.Error });
        }
        #endregion


        #region ResetPassword
        public async Task<IActionResult> ResetPassword(SearchDTO model)
        {
            ViewBag.Message = model.msg;
            ViewBag.ReturnCode = model.code;

            if (!checkrole(new string[] { roleType.Admin, roleType.Helpdesk, roleType.PasswordOperator }))
                return RedirectToAction("Logout", "Auth");

            if (string.IsNullOrEmpty(model.text_search))
                return View(model);

            model.text_search = model.text_search.Trim();
            if (model.text_search.Length <= 3)
            {
                ModelState.AddModelError("text_search", "Search Text must have at least 3 characters.");
                return View(model);
            }
            //string[] roles = new { aUUserType.student.toUserTypeName(), aUUserType.staff.toUserTypeName() };
            var adusers = await _provider.FindUser(model, null, _context);


            int skipRows = (model.pageno - 1) * _pagelen;
            var itemcnt = adusers.Count();
            var pagelen = itemcnt / _pagelen;
            if (itemcnt % _pagelen > 0)
                pagelen += 1;

            model.itemcnt = itemcnt;
            model.pagelen = pagelen;
            //model.lists = lists.Skip(skipRows).Take(_pagelen).AsQueryable();
            model.lists = adusers.AsQueryable();
            return View(model);
        }

        public IActionResult ChangePassword(string id)
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.Helpdesk, roleType.PasswordOperator }))
                return RedirectToAction("Logout", "Auth");

            var model = new ChangePassword3DTO();
            model.id = id;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassword3DTO model)
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.Helpdesk, roleType.PasswordOperator }))
                return RedirectToAction("Logout", "Auth");

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context, _conf.Env);
            if (userlogin == null)
                return RedirectToAction("Logout", "Auth");

            if (ModelState.IsValid)
            {
                var msg = ReturnMessage.ChangePasswordFail;
                var code = ReturnCode.Error;
                ViewBag.Message = msg;
                ViewBag.ReturnCode = code;
                try
                {
                    var user = await _provider.GetAdUser2(model.id, _context, _conf.Env);
                    if (user == null)
                        return RedirectToAction("Logout", "Auth");

                    var userType = AppUtil.getaUUserType(user.DistinguishedName);
                    if (userType == aUUserType.vip)
                    {
                        var vip = this._context.User_VIP.Where(w => w.username.ToLower() == model.id.ToLower()).FirstOrDefault();
                        if (vip != null)
                        {
                            vip.password = Cryptography.encrypt(model.Password);
                            vip.Update_On = DateUtil.Now();
                            vip.Update_By = userlogin.SamAccountName;
                        }
                    }
                    else if (userType == aUUserType.office)
                    {
                        var office = this._context.User_Office.Where(w => w.username.ToLower() == model.id.ToLower()).FirstOrDefault();
                        if (office != null)
                        {
                            office.password = Cryptography.encrypt(model.Password);
                            office.Update_On = DateUtil.Now();
                            office.Update_By = userlogin.SamAccountName;
                        }
                    }
                    else if (userType == aUUserType.bulk)
                    {
                        var bulk = this._context.User_Bulk.Where(w => w.username.ToLower() == model.id.ToLower()).FirstOrDefault();
                        if (bulk != null)
                        {
                            bulk.password = Cryptography.encrypt(model.Password);
                            bulk.Update_On = DateUtil.Now();
                            bulk.Update_By = userlogin.SamAccountName;
                        }
                    }
                    _context.SaveChanges();

                    var result_ad = _provider.ChangePwd(user, model.Password, _context);
                    if (result_ad.result == true)
                        writelog(LogType.log_reset_password, LogStatus.successfully, IDMSource.AD, model.id);
                    else
                        writelog(LogType.log_reset_password, LogStatus.failed, IDMSource.AD, model.id, log_exception: result_ad.Message);

                    writelog(LogType.log_reset_password, LogStatus.successfully, IDMSource.Database, model.id);

                    msg = ReturnMessage.ChangePasswordSuccess;
                    code = ReturnCode.Success;
                    ViewBag.Message = msg;
                    ViewBag.ReturnCode = code;
                    return RedirectToAction("ResetPassword", "Account", new { code = code, msg = msg });
                }
                catch (Exception ex)
                {
                    writelog(LogType.log_reset_password, LogStatus.failed, IDMSource.Database, model.id, log_exception: ex.Message);
                }
            }
            return View(model);
        }
        #endregion

        #region EnableAccount
        public async Task<IActionResult> EnableAccount(SearchDTO model)
        {
            ViewBag.Message = model.msg;
            ViewBag.ReturnCode = model.code;

            if (!checkrole(new string[] { roleType.Admin, roleType.Helpdesk }))
                return RedirectToAction("Logout", "Auth");

            if (string.IsNullOrEmpty(model.text_search))
                return View(model);

            model.text_search = model.text_search.Trim();
            if (model.text_search.Length <= 3)
            {
                ModelState.AddModelError("text_search", "Search Text must have at least 3 characters.");
                return View(model);
            }
            //string[] roles = new { aUUserType.student.toUserTypeName(), aUUserType.staff.toUserTypeName() };
            var adusers = await _provider.FindUser(model, null, _context);


            int skipRows = (model.pageno - 1) * _pagelen;
            var itemcnt = adusers.Count();
            var pagelen = itemcnt / _pagelen;
            if (itemcnt % _pagelen > 0)
                pagelen += 1;

            model.itemcnt = itemcnt;
            model.pagelen = pagelen;
            //model.lists = lists.Skip(skipRows).Take(_pagelen).AsQueryable();
            model.lists = adusers.AsQueryable();
            return View(model);
        }
        public async Task<JsonResult> ChangeStatus(string id, string remark)
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.Helpdesk }))
                return Json(new { error = ReturnMessage.Error, result = ReturnCode.Error });

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context, _conf.Env);
            if (userlogin == null)
                return Json(new { error = ReturnMessage.Error, result = ReturnCode.Error });

            if (!string.IsNullOrEmpty(id))
            {
                var model = await _provider.GetAdUser2(id, _context, _conf.Env);
                if (model != null)
                {
                    var userType = AppUtil.getaUUserType(model.DistinguishedName);
                    if (userType == aUUserType.vip)
                    {
                        var vip = this._context.User_VIP.Where(w => w.username.ToLower() == id.ToLower()).FirstOrDefault();
                        if (vip != null)
                        {
                            vip.Update_On = DateUtil.Now();
                            vip.Update_By = userlogin.SamAccountName;
                        }
                    }
                    else if (userType == aUUserType.office)
                    {
                        var office = this._context.User_Office.Where(w => w.username.ToLower() == id.ToLower()).FirstOrDefault();
                        if (office != null)
                        {
                            office.Update_On = DateUtil.Now();
                            office.Update_By = userlogin.SamAccountName;
                        }
                    }
                    else if (userType == aUUserType.bulk)
                    {
                        var bulk = this._context.User_Bulk.Where(w => w.username.ToLower() == id.ToLower()).FirstOrDefault();
                        if (bulk != null)
                        {
                            bulk.Update_On = DateUtil.Now();
                            bulk.Update_By = userlogin.SamAccountName;
                        }
                    }
                    _context.SaveChanges();

                    model.comment = remark;
                    if (NumUtil.ParseInteger(model.userAccountControl) == (int)userAccountControl.Disable || NumUtil.ParseInteger(model.userAccountControl) == (int)userAccountControl.DisablePasswordNotRequired)
                    {
                        try
                        {

                            var result_ad = _provider.EnableUser(model, _context);
                            if (result_ad.result == true)
                                writelog(LogType.log_unlock_account, LogStatus.successfully, IDMSource.AD, model.SamAccountName);
                            else
                                writelog(LogType.log_unlock_account, LogStatus.failed, IDMSource.AD, model.SamAccountName, log_exception: result_ad.Message);

                            writelog(LogType.log_unlock_account, LogStatus.successfully, IDMSource.Database, model.SamAccountName);
                        }
                        catch (Exception ex)
                        {
                            writelog(LogType.log_unlock_account, LogStatus.failed, IDMSource.Database, model.SamAccountName, log_exception: ex.Message);
                        }
                        return Json(new { error = ReturnMessage.Success, result = ReturnCode.Success, status = userAccountControl.EnablePasswordNotRequired });
                    }
                    else
                    {
                        try
                        {

                            var result_ad = _provider.DisableUser(model, _context);
                            if (result_ad.result == true)
                                writelog(LogType.log_lock_account, LogStatus.successfully, IDMSource.AD, model.SamAccountName);
                            else
                                writelog(LogType.log_lock_account, LogStatus.failed, IDMSource.AD, model.SamAccountName, log_exception: result_ad.Message);

                            writelog(LogType.log_lock_account, LogStatus.successfully, IDMSource.Database, model.SamAccountName);
                        }
                        catch (Exception ex)
                        {
                            writelog(LogType.log_lock_account, LogStatus.failed, IDMSource.Database, model.SamAccountName, log_exception: ex.Message);
                        }
                        return Json(new { error = ReturnMessage.Success, result = ReturnCode.Success, status = userAccountControl.DisablePasswordNotRequired });
                    }
                }
            }
            return Json(new { error = ReturnMessage.Error, result = ReturnCode.Error });
        }
        #endregion        

        #region CheckAccount
        public async Task<IActionResult> CheckAccount(SearchDTO model)
        {
            ViewBag.Message = model.msg;
            ViewBag.ReturnCode = model.code;

            if (!checkrole(new string[] { roleType.Admin, roleType.Helpdesk }))
                return RedirectToAction("Logout", "Auth");

            if (string.IsNullOrEmpty(model.text_search))
                return View(model);

            model.text_search = model.text_search.Trim();
            if (model.text_search.Length <= 3)
            {
                ModelState.AddModelError("text_search", "Search Text must have at least 3 characters.");
                return View(model);
            }
            //string[] roles = { model.usertype_search.toUserTypeName() };
            var adusers = await _provider.FindUser(model, null, _context);


            int skipRows = (model.pageno - 1) * _pagelen;
            var itemcnt = adusers.Count();
            var pagelen = itemcnt / _pagelen;
            if (itemcnt % _pagelen > 0)
                pagelen += 1;

            model.itemcnt = itemcnt;
            model.pagelen = pagelen;
            //model.lists = lists.Skip(skipRows).Take(_pagelen).AsQueryable();
            model.lists = adusers.AsQueryable();
            return View(model);
        }
        #endregion


    }
}
