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

            if (!checkrole())
                return RedirectToAction("Logout", "Auth");

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context);
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
            if (!checkrole())
                return RedirectToAction("Logout", "Auth");

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context);
            if (userlogin == null)
                return RedirectToAction("Logout", "Auth");

            var dup = await _provider.GetAdUser2(model.SamAccountName, _context);
            if (dup != null)
            {
                ModelState.AddModelError("SamAccountName", "username ซ้ำในระบบ");
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
                        user.password = Cryptography.encrypt(model.Password);
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

                        var result_ad = _provider.CreateUser(model, _context);
                        if (result_ad.result == true)
                            writelog(LogType.log_create_account, LogStatus.successfully, IDMSource.AD, model.SamAccountName);
                        else
                            writelog(LogType.log_create_account, LogStatus.failed, IDMSource.AD, model.SamAccountName, log_exception: result_ad.Message);

                        user.ad_created = result_ad.result;
                        _context.SaveChanges();
                        writelog(LogType.log_create_account, LogStatus.successfully, IDMSource.VisualFim, model.SamAccountName);

                    }
                    else if (model.aUUserType == aUUserType.vip)
                    {
                        model.DistinguishedName = _conf.OU_VIP;
                        var user = new User_VIP();
                        user.username = model.SamAccountName;
                        user.password = Cryptography.encrypt(model.Password);
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

                        var result_ad = _provider.CreateUser(model, _context);
                        if (result_ad.result == true)
                            writelog(LogType.log_create_account, LogStatus.successfully, IDMSource.AD, model.SamAccountName);
                        else
                            writelog(LogType.log_create_account, LogStatus.failed, IDMSource.AD, model.SamAccountName, log_exception: result_ad.Message);

                        user.ad_created = result_ad.result;
                        _context.SaveChanges();
                        writelog(LogType.log_create_account, LogStatus.successfully, IDMSource.VisualFim, model.SamAccountName);

                    }
                    else if (model.aUUserType == aUUserType.bulk)
                    {
                        model.DistinguishedName = _conf.OU_TEMP;
                        var user = new User_Bulk();
                        user.username = model.SamAccountName;
                        user.password = Cryptography.encrypt(model.Password);
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

                        var result_ad = _provider.CreateUser(model, _context);
                        if (result_ad.result == true)
                            writelog(LogType.log_create_account, LogStatus.successfully, IDMSource.AD, model.SamAccountName);
                        else
                            writelog(LogType.log_create_account, LogStatus.failed, IDMSource.AD, model.SamAccountName, log_exception: result_ad.Message);

                        user.ad_created = result_ad.result;
                        _context.SaveChanges();
                        writelog(LogType.log_create_account, LogStatus.successfully, IDMSource.VisualFim, model.SamAccountName);
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
                    writelog(LogType.log_create_account, LogStatus.failed, IDMSource.VisualFim, model.SamAccountName, log_exception: ex.Message);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> CreateAccountCompleted(ReturnCode code, string msg, string id)
        {
            if (!checkrole())
                return RedirectToAction("Logout", "Auth");

            var model = new AdUser2();
            if (!string.IsNullOrEmpty(id))
            {
                model = await _provider.GetAdUser2(id, _context);
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
            if (!checkrole())
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
            if (!checkrole())
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
            if (!checkrole())
                return RedirectToAction("Logout", "Auth");

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context);
            if (userlogin == null)
                return RedirectToAction("Logout", "Auth");

            var msg = ReturnMessage.ImportFail;
            var code = ReturnCode.Error;

            var setup = _context.table_setup.FirstOrDefault();
            if (setup == null)
                return RedirectToAction("Logout", "Auth");

            int runNumber = setup.GuestRowNumber+1;
            var imports = _context.table_temp_import.OrderBy(o => o.ImportRow);
            foreach (var imp in imports.ToList())
            {
                var username = "guest" + runNumber.ToString("00000");
                var dup = true;
                while(dup == true)
                {
                    var account = await _provider.GetAdUser2(username, _context);
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
                var result_ad = _provider.CreateUser(aduser, _context);
                if (result_ad.result == true)
                    writelog(LogType.log_create_account_with_file, LogStatus.successfully, IDMSource.AD, aduser.SamAccountName);
                else
                    writelog(LogType.log_create_account_with_file, LogStatus.failed, IDMSource.AD, aduser.SamAccountName, log_exception: result_ad.Message);

                user.ad_created = result_ad.result;

                _context.SaveChanges();
                writelog(LogType.log_create_account_with_file, LogStatus.successfully, IDMSource.VisualFim, aduser.SamAccountName);
            }
            _context.table_temp_import.RemoveRange(_context.table_temp_import);
            _context.SaveChanges();
            msg = ReturnMessage.ImportSuccess;
            code = ReturnCode.Success;
            return RedirectToAction("CreateAccountFromFile", new { code = code, msg = msg });
        }


        #endregion

        #region CreateAccount
        public async Task<IActionResult> CreateAccountBulk(ReturnCode code, string msg)
        {

            if (!checkrole())
                return RedirectToAction("Logout", "Auth");

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context);
            if (userlogin == null)
                return RedirectToAction("Logout", "Auth");

            var model = new Bulk();
            ViewBag.Message = msg;
            ViewBag.ReturnCode = code;
            model.NumberOfPeople = 1;
            //model.system_faculty_id = 0;
            //model.cu_CUexpire_day = DateUtil.Now().Day;
            //model.cu_CUexpire_month = DateUtil.Now().Month;
            //model.cu_CUexpire_year = DateUtil.Now().Year + 1;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccountBulk(Bulk model)
        {
            if (!checkrole())
                return RedirectToAction("Logout", "Auth");

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context);
            if (userlogin == null)
                return RedirectToAction("Logout", "Auth");

            var msg = ReturnMessage.Error;
            var code = ReturnCode.Error;

            var setup = _context.table_setup.FirstOrDefault();
            if (setup == null)
                return RedirectToAction("Logout", "Auth");

            if (model.NumberOfPeople <= 0)
                model.NumberOfPeople = 1;

            int runNumber = setup.GuestRowNumber + 1;
            for (var i=0; i < model.NumberOfPeople; i++)
            {
                var username = "guest" + runNumber.ToString("00000");
                var dup = true;
                while (dup == true)
                {
                    var account = await _provider.GetAdUser2(username, _context);
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
                var result_ad = _provider.CreateUser(aduser, _context);
                if (result_ad.result == true)
                    writelog(LogType.log_create_account_bulk, LogStatus.successfully, IDMSource.AD, aduser.SamAccountName);
                else
                    writelog(LogType.log_create_account_bulk, LogStatus.failed, IDMSource.AD, aduser.SamAccountName, log_exception: result_ad.Message);

                user.ad_created = result_ad.result;

                _context.SaveChanges();
                writelog(LogType.log_create_account_bulk, LogStatus.successfully, IDMSource.VisualFim, aduser.SamAccountName);
               
            }
            msg = ReturnMessage.Success;
            code = ReturnCode.Success;
            return RedirectToAction("CreateAccountBulk", new { code = code, msg = msg });
        }
        #endregion

        #region ManageAccount
        //public IActionResult ManageAccount(SearchDTO model)
        //{
        //    ViewBag.Message = model.msg;
        //    ViewBag.ReturnCode = model.code;

        //     if (!checkrole())
        //        return RedirectToAction("Logout", "Auth");

        //    if (string.IsNullOrEmpty(model.text_search))
        //        return View(model);

        //    model.text_search = model.text_search.Trim();
        //    if (model.text_search.Length <= 3)
        //    {
        //        ModelState.AddModelError("text_search", "คำค้นจะต้องมากกว่า 3 ตัวอักษร");
        //        return View(model);
        //    }

        //    var lists = this._context.table_visual_fim_user.Where(w => 1 == 1);

        //    if (!string.IsNullOrEmpty(model.text_search))
        //    {
        //        lists = lists.Where(w => (!string.IsNullOrEmpty(w.basic_uid) && w.basic_uid.ToLower().Contains(model.text_search.ToLower()))
        //      | (!string.IsNullOrEmpty(w.basic_givenname) && w.basic_givenname.ToLower().Contains(model.text_search.ToLower()))
        //      | (!string.IsNullOrEmpty(w.basic_sn) && w.basic_sn.ToLower().Contains(model.text_search.ToLower()))
        //      | (!string.IsNullOrEmpty(w.cu_thcn) && w.cu_thcn.ToLower().Contains(model.text_search.ToLower()))
        //      | (!string.IsNullOrEmpty(w.cu_thsn) && w.cu_thsn.ToLower().Contains(model.text_search.ToLower()))
        //      | (!string.IsNullOrEmpty(w.basic_cn) && w.basic_cn.ToLower().Contains(model.text_search.ToLower()))
        //      | (!string.IsNullOrEmpty(w.cu_pplid) && w.cu_pplid.ToLower().Contains(model.text_search.ToLower()))
        //      | (!string.IsNullOrEmpty(w.cu_jobcode) && w.cu_jobcode.ToLower().Contains(model.text_search.ToLower()))
        //      | (!string.IsNullOrEmpty(w.basic_mobile) && w.basic_mobile.ToLower().Contains(model.text_search.ToLower()))
        //      | (!string.IsNullOrEmpty(w.basic_mail) && w.basic_mail.ToLower().Contains(model.text_search.ToLower())));
        //    }

        //    if (model.usertype_search.HasValue)
        //        lists = lists.Where(w => w.system_idm_user_type == model.usertype_search);

        //    lists = lists.OrderByDescending(o => o.system_create_date);

        //    int skipRows = (model.pageno - 1) * _pagelen;
        //    var itemcnt = lists.Count();
        //    var pagelen = itemcnt / _pagelen;
        //    if (itemcnt % _pagelen > 0)
        //        pagelen += 1;

        //    model.itemcnt = itemcnt;
        //    model.pagelen = pagelen;
        //    //model.lists = lists.Skip(skipRows).Take(_pagelen).AsQueryable();
        //    model.lists = lists.AsQueryable();
        //    return View(model);
        //}
        public async Task<IActionResult> AccountInfo(string id)
        {
            if (!checkrole())
                return RedirectToAction("Logout", "Auth");

            var aduser = await _provider.GetAdUser2(id, _context);
            aduser.ExpireDate = DateUtil.ToDisplayDate( aduser.accountExpires);
            //model.cu_CUexpire_select = true;
            //model.cu_CUexpire_day = DateUtil.Now().Day;
            //model.cu_CUexpire_month = DateUtil.Now().Month;
            //model.cu_CUexpire_year = DateUtil.Now().Year + 1;

            return View(aduser);
        }

        [HttpPost]
        public async Task<IActionResult> AccountInfo(AdUser2 model)
        {
            if (!checkrole())
                return RedirectToAction("Logout", "Auth");

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context);
            if (userlogin == null)
                return RedirectToAction("Logout", "Auth");

            if (ModelState.IsValid)
            {
                ViewBag.Message = ReturnMessage.Error;
                ViewBag.ReturnCode = ReturnCode.Error;

                if (model.isnew == false)
                {
                    var aduser = await _provider.GetAdUser2(model.SamAccountName, _context);
                    if (aduser != null)
                    {
                        aduser.GivenName = model.GivenName;
                        aduser.Surname = model.Surname;
                        aduser.DisplayName = model.GivenName + " " + model.Surname;
                        aduser.EmailAddress = model.EmailAddress;
                        aduser.aUIDCard = model.aUIDCard;
                        aduser.PassportID = model.PassportID;
                        aduser.Reference = model.Reference;
                        aduser.accountExpires = DateUtil.ToDate( model.ExpireDate);
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

        #region DeleteAccount
        public async Task<IActionResult> DeleteAccount(SearchDTO model)
        {
            ViewBag.Message = model.msg;
            ViewBag.ReturnCode = model.code;

            if (!checkrole())
                return RedirectToAction("Logout", "Auth");

            if (string.IsNullOrEmpty(model.text_search))
                return View(model);

            model.text_search = model.text_search.Trim();
            if (model.text_search.Length <= 3)
            {
                ModelState.AddModelError("text_search", "คำค้นจะต้องมากกว่า 3 ตัวอักษร");
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
            if (!checkrole())
                return Json(new { error = ReturnMessage.Error, result = ReturnCode.Error });

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context);
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
                            var model = await _provider.GetAdUser2(id, _context);
                            if(model != null)
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

                                }
                                var result_ad = _provider.DeleteUser(model, _context);
                                if (result_ad.result == true)
                                    writelog(LogType.log_delete_account, LogStatus.successfully, IDMSource.AD, model.SamAccountName);
                                else
                                    writelog(LogType.log_delete_account, LogStatus.failed, IDMSource.AD, model.SamAccountName, log_exception: result_ad.Message);

                            
                                writelog(LogType.log_delete_account, LogStatus.successfully, IDMSource.VisualFim, model.SamAccountName);
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            writelog(LogType.log_delete_account, LogStatus.failed, IDMSource.VisualFim, id, log_exception: ex.Message);
                        }
                    }
                    return Json(new { error = ReturnMessage.Success, result = ReturnCode.Success });
                }
            }
            return Json(new { error = ReturnMessage.Error, result = ReturnCode.Error });
        }
        #endregion

        //#region DeleteAccountFromFile
        //public IActionResult DeleteAccountFromFile(SearchDTO model)
        //{
        //    if (!checkrole())
        //        return RedirectToAction("Logout", "Auth");

        //    model.lists = (new List<import>()).AsQueryable();
        //    ViewBag.Message = model.msg;
        //    ViewBag.ReturnCode = model.code;
        //    return View(model);
        //}

        //[HttpPost]
        //public IActionResult DeleteAccountFromFile(IFormFile file, ImportDeleteOption import_option)
        //{
        //    if (!checkrole())
        //        return RedirectToAction("Logout", "Auth");

        //    var model = new SearchDTO();
        //    var lists = new List<import>();
        //    if (file != null)
        //    {
        //        _context.table_import.RemoveRange(_context.table_import.Where(w => w.import_Type == ImportType.delete));
        //        using (var reader = new StreamReader(file.OpenReadStream()))
        //        {
        //            string input;
        //            var row = 1;
        //            while ((input = reader.ReadLine()) != null)
        //            {
        //                if (string.IsNullOrEmpty(input))
        //                    continue;
        //                var columnNameList = input.Split(":");
        //                var remark = new StringBuilder();
        //                var imp = new import();
        //                imp.ImportVerify = true;
        //                imp.ImportRow = row;
        //                imp.basic_uid = "";
        //                imp.basic_givenname = "";
        //                imp.basic_sn = "";
        //                imp.cu_pplid = "";
        //                imp.LockStaus = "";
        //                imp.import_Type = ImportType.delete;
        //                try
        //                {
        //                    var j = 0;
        //                    if (import_option == ImportDeleteOption.pplid)
        //                    {
        //                        imp.cu_pplid = columnNameList[j]; j++;
        //                        if (string.IsNullOrEmpty(imp.cu_pplid))
        //                            continue;

        //                        var fim_users = _context.table_visual_fim_user.Where(w => w.cu_pplid.ToLower() == imp.cu_pplid.ToLower());
        //                        if (fim_users.Count() == 0)
        //                        {
        //                            imp.ImportVerify = false;
        //                            remark.AppendLine("ไม่พบข้อมูลผู้ใช้ที่มี Citizen ID นี้");
        //                        }
        //                        foreach (var fim_user in fim_users)
        //                        {
        //                            imp.basic_uid += fim_user.basic_uid + "|";
        //                            imp.basic_givenname += fim_user.basic_givenname + "|";
        //                            imp.basic_sn += fim_user.basic_sn + "|";
        //                            imp.LockStaus += fim_user.cu_nsaccountlock + "|";
        //                        }
        //                    }
        //                    else if (import_option == ImportDeleteOption.loginname)
        //                    {
        //                        imp.basic_uid = columnNameList[j]; j++;
        //                        if (string.IsNullOrEmpty(imp.basic_uid))
        //                            continue;

        //                        var fim_users = _context.table_visual_fim_user.Where(w => w.basic_uid.ToLower() == imp.basic_uid.ToLower());
        //                        if (fim_users.Count() == 0)
        //                        {
        //                            imp.ImportVerify = false;
        //                            remark.AppendLine("ไม่พบข้อมูลผู้ใช้ที่มี Login Name นี้");
        //                        }
        //                        foreach (var fim_user in fim_users)
        //                        {
        //                            imp.cu_pplid = fim_user.cu_pplid;
        //                            imp.basic_givenname = fim_user.basic_givenname;
        //                            imp.basic_sn = fim_user.basic_sn;
        //                            imp.LockStaus = fim_user.cu_nsaccountlock;

        //                        }
        //                    }
        //                    imp.ImportRemark = remark.ToString();

        //                }
        //                catch (Exception ex)
        //                {
        //                    remark.AppendLine(ex.Message);
        //                    imp.ImportVerify = false;
        //                }
        //                lists.Add(imp);
        //                if (imp.ImportVerify == true)
        //                    _context.table_import.Add(imp);
        //                row++;
        //            }
        //        }
        //        _context.SaveChanges();


        //    }
        //    model.lists = lists.AsQueryable();
        //    return View(model);
        //}

        //[HttpPost]
        //public IActionResult DeleteAccountFromFile2()
        //{
        //    if (!checkrole())
        //        return RedirectToAction("Logout", "Auth");

        //    var userlogin = this._context.table_visual_fim_user.Where(w => w.basic_uid == this.HttpContext.User.Identity.Name).FirstOrDefault();
        //    if (userlogin == null)
        //        return RedirectToAction("Logout", "Auth");

        //    var msg = ReturnMessage.ImportFail;
        //    var code = ReturnCode.Error;

        //    var imports = _context.table_import.Where(w => w.import_Type == ImportType.delete).OrderBy(o => o.ImportRow);
        //    foreach (var import in imports.ToList())
        //    {
        //        var basic_uids = import.basic_uid.Split("|", StringSplitOptions.RemoveEmptyEntries);
        //        foreach (var basic_uid in basic_uids)
        //        {
        //            var model = _context.table_visual_fim_user.Where(w => w.basic_uid.ToLower() == basic_uid.ToLower()).FirstOrDefault();
        //            if (model != null)
        //            {
        //                try
        //                {
        //                    var result_ldap = _providerldap.DeleteUser(model, _context);
        //                    if (result_ldap.result == true)
        //                        writelog(LogType.log_delete_account_with_file, LogStatus.successfully, IDMSource.LDAP, model.basic_uid);
        //                    else
        //                        writelog(LogType.log_delete_account_with_file, LogStatus.failed, IDMSource.LDAP, model.basic_uid, log_exception: result_ldap.Message);

        //                    var result_ad = _provider.DeleteUser(model, _context);
        //                    if (result_ad.result == true)
        //                        writelog(LogType.log_delete_account_with_file, LogStatus.successfully, IDMSource.AD, model.basic_uid);
        //                    else
        //                        writelog(LogType.log_delete_account_with_file, LogStatus.failed, IDMSource.AD, model.basic_uid, log_exception: result_ad.Message);

        //                    _context.Remove(model);
        //                    _context.SaveChanges();
        //                    writelog(LogType.log_delete_account_with_file, LogStatus.successfully, IDMSource.VisualFim, model.basic_uid);
        //                }
        //                catch (Exception ex)
        //                {
        //                    writelog(LogType.log_delete_account_with_file, LogStatus.failed, IDMSource.VisualFim, model.basic_uid, log_exception: ex.Message);
        //                }
        //            }
        //        }
        //    }
        //    _context.table_import.RemoveRange(_context.table_import.Where(w => w.import_Type == ImportType.delete));
        //    _context.SaveChanges();
        //    msg = ReturnMessage.Success;
        //    code = ReturnCode.Success;
        //    return RedirectToAction("DeleteAccountFromFile", new { code = code, msg = msg });
        //}
        //#endregion

        #region ResetPassword
        public async Task<IActionResult> ResetPassword(SearchDTO model)
        {
            ViewBag.Message = model.msg;
            ViewBag.ReturnCode = model.code;

            if (!checkrole())
                return RedirectToAction("Logout", "Auth");

            if (string.IsNullOrEmpty(model.text_search))
                return View(model);

            model.text_search = model.text_search.Trim();
            if (model.text_search.Length <= 3)
            {
                ModelState.AddModelError("text_search", "คำค้นจะต้องมากกว่า 3 ตัวอักษร");
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
            if (!checkrole())
                return RedirectToAction("Logout", "Auth");

            var model = new ChangePassword3DTO();
            model.id = id;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassword3DTO model)
        {
            if (!checkrole())
                return RedirectToAction("Logout", "Auth");

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context);
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
                    var user = await _provider.GetAdUser2(model.id, _context);
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

                    }
                    _context.SaveChanges();

                    var result_ad = _provider.ChangePwd(user, model.Password, _context);
                    if (result_ad.result == true)
                        writelog(LogType.log_reset_password, LogStatus.successfully, IDMSource.AD, model.id);
                    else
                        writelog(LogType.log_reset_password, LogStatus.failed, IDMSource.AD, model.id, log_exception: result_ad.Message);

                    writelog(LogType.log_reset_password, LogStatus.successfully, IDMSource.VisualFim, model.id);

                    msg = ReturnMessage.ChangePasswordSuccess;
                    code = ReturnCode.Success;
                    ViewBag.Message = msg;
                    ViewBag.ReturnCode = code;
                    return RedirectToAction("ResetPassword", "Account", new { code = code, msg = msg });
                }
                catch (Exception ex)
                {
                    writelog(LogType.log_reset_password, LogStatus.failed, IDMSource.VisualFim, model.id, log_exception: ex.Message);
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

            if (!checkrole())
                return RedirectToAction("Logout", "Auth");

            if (string.IsNullOrEmpty(model.text_search))
                return View(model);

            model.text_search = model.text_search.Trim();
            if (model.text_search.Length <= 3)
            {
                ModelState.AddModelError("text_search", "คำค้นจะต้องมากกว่า 3 ตัวอักษร");
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
            if (!checkrole())
                return Json(new { error = ReturnMessage.Error, result = ReturnCode.Error });

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context);
            if (userlogin == null)
                return Json(new { error = ReturnMessage.Error, result = ReturnCode.Error });

            if (!string.IsNullOrEmpty(id))
            {
                var model = await _provider.GetAdUser2(id, _context);
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

                    }
                    _context.SaveChanges();


                    if (NumUtil.ParseInteger(model.userAccountControl) == (int)userAccountControl.Disable || NumUtil.ParseInteger(model.userAccountControl) == (int)userAccountControl.DisablePasswordNotRequired)
                    {
                        try
                        {
                            var result_ad = _provider.EnableUser(model, _context);
                            if (result_ad.result == true)
                                writelog(LogType.log_unlock_account, LogStatus.successfully, IDMSource.AD, model.SamAccountName);
                            else
                                writelog(LogType.log_unlock_account, LogStatus.failed, IDMSource.AD, model.SamAccountName, log_exception: result_ad.Message);

                            writelog(LogType.log_unlock_account, LogStatus.successfully, IDMSource.VisualFim, model.SamAccountName);
                        }
                        catch (Exception ex)
                        {
                            writelog(LogType.log_unlock_account, LogStatus.failed, IDMSource.VisualFim, model.SamAccountName, log_exception: ex.Message);
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

                            writelog(LogType.log_lock_account, LogStatus.successfully, IDMSource.VisualFim, model.SamAccountName);
                        }
                        catch (Exception ex)
                        {
                            writelog(LogType.log_lock_account, LogStatus.failed, IDMSource.VisualFim, model.SamAccountName, log_exception: ex.Message);
                        }
                        return Json(new { error = ReturnMessage.Success, result = ReturnCode.Success, status = userAccountControl.DisablePasswordNotRequired });
                    }
                }
            }
            return Json(new { error = ReturnMessage.Error, result = ReturnCode.Error });
        }
        #endregion

        //#region EnableAccountFromFile
        //public IActionResult EnableAccountFromFile(SearchDTO model)
        //{
        //    if (!checkrole())
        //        return RedirectToAction("Logout", "Auth");

        //    model.lists = (new List<import>()).AsQueryable();
        //    ViewBag.Message = model.msg;
        //    ViewBag.ReturnCode = model.code;
        //    return View(model);
        //}

        //[HttpPost]
        //public IActionResult EnableAccountFromFile(IFormFile file, ImportLockOption import_option)
        //{
        //    if (!checkrole())
        //        return RedirectToAction("Logout", "Auth");

        //    var model = new SearchDTO();
        //    var lists = new List<import>();
        //    if (file != null)
        //    {
        //        _context.table_import.RemoveRange(_context.table_import.Where(w => w.import_Type == ImportType.lockunlock));
        //        using (var reader = new StreamReader(file.OpenReadStream()))
        //        {
        //            string input;
        //            var row = 1;
        //            while ((input = reader.ReadLine()) != null)
        //            {
        //                if (string.IsNullOrEmpty(input))
        //                    continue;
        //                var columnNameList = input.Split(":");
        //                var remark = new StringBuilder();
        //                var imp = new import();
        //                imp.ImportVerify = true;
        //                imp.ImportRow = row;
        //                imp.basic_uid = "";
        //                imp.basic_givenname = "";
        //                imp.basic_sn = "";
        //                imp.cu_pplid = "";
        //                imp.LockStaus = "";
        //                imp.import_Type = ImportType.lockunlock;
        //                try
        //                {
        //                    var j = 0;
        //                    if (import_option == ImportLockOption.pplid)
        //                    {
        //                        imp.cu_pplid = columnNameList[j]; j++;
        //                        if (string.IsNullOrEmpty(imp.cu_pplid))
        //                            continue;

        //                        var fim_users = _context.table_visual_fim_user.Where(w => w.cu_pplid.ToLower() == imp.cu_pplid.ToLower());
        //                        if (fim_users.Count() == 0)
        //                        {
        //                            imp.ImportVerify = false;
        //                            remark.AppendLine("ไม่พบข้อมูลผู้ใช้ที่มี Citizen ID นี้");
        //                        }
        //                        foreach (var fim_user in fim_users)
        //                        {
        //                            imp.basic_uid += fim_user.basic_uid + "|";
        //                            imp.basic_givenname += fim_user.basic_givenname + "|";
        //                            imp.basic_sn += fim_user.basic_sn + "|";
        //                            imp.LockStaus += fim_user.cu_nsaccountlock + "|";
        //                        }
        //                    }
        //                    else if (import_option == ImportLockOption.loginname)
        //                    {
        //                        imp.basic_uid = columnNameList[j]; j++;
        //                        if (string.IsNullOrEmpty(imp.basic_uid))
        //                            continue;

        //                        var fim_users = _context.table_visual_fim_user.Where(w => w.basic_uid.ToLower() == imp.basic_uid.ToLower());
        //                        if (fim_users.Count() == 0)
        //                        {
        //                            imp.ImportVerify = false;
        //                            remark.AppendLine("ไม่พบข้อมูลผู้ใช้ที่มี Login Name นี้");
        //                        }
        //                        foreach (var fim_user in fim_users)
        //                        {
        //                            imp.cu_pplid = fim_user.cu_pplid;
        //                            imp.basic_givenname = fim_user.basic_givenname;
        //                            imp.basic_sn = fim_user.basic_sn;
        //                            imp.LockStaus = fim_user.cu_nsaccountlock;

        //                        }
        //                    }
        //                    imp.ImportRemark = remark.ToString();

        //                }
        //                catch (Exception ex)
        //                {
        //                    remark.AppendLine(ex.Message);
        //                    imp.ImportVerify = false;
        //                }
        //                lists.Add(imp);
        //                if (imp.ImportVerify == true)
        //                    _context.table_import.Add(imp);
        //                row++;
        //            }
        //        }
        //        _context.SaveChanges();
        //    }
        //    model.lists = lists.AsQueryable();
        //    return View(model);
        //}

        //[HttpPost]
        //public IActionResult EnableAccountFromFile2(string lockstatus)
        //{
        //    if (!checkrole())
        //        return RedirectToAction("Logout", "Auth");

        //    var userlogin = this._context.table_visual_fim_user.Where(w => w.basic_uid == this.HttpContext.User.Identity.Name).FirstOrDefault();
        //    if (userlogin == null)
        //        return RedirectToAction("Logout", "Auth");

        //    var msg = ReturnMessage.ImportFail;
        //    var code = ReturnCode.Error;

        //    var imports = _context.table_import.Where(w => w.import_Type == ImportType.lockunlock).OrderBy(o => o.ImportRow);
        //    foreach (var import in imports.ToList())
        //    {
        //        var basic_uids = import.basic_uid.Split("|", StringSplitOptions.RemoveEmptyEntries);
        //        foreach (var basic_uid in basic_uids)
        //        {
        //            var model = _context.table_visual_fim_user.Where(w => w.basic_uid.ToLower() == basic_uid.ToLower()).FirstOrDefault();
        //            if (model != null)
        //            {
        //                try
        //                {
        //                    model.cu_nsaccountlock = lockstatus;
        //                    model.system_modify_by_uid = userlogin.basic_uid;
        //                    model.system_modify_date = DateUtil.Now();
        //                    _context.SaveChanges();

        //                    if (lockstatus == LockStaus.Lock)
        //                    {
        //                        var result_ldap = _providerldap.NsLockUser(model, _context);
        //                        if (result_ldap.result == true)
        //                            writelog(LogType.log_lock_account_with_file, LogStatus.successfully, IDMSource.LDAP, model.basic_uid);
        //                        else
        //                            writelog(LogType.log_lock_account_with_file, LogStatus.failed, IDMSource.LDAP, model.basic_uid, log_exception: result_ldap.Message);

        //                        var result_ad = _provider.DisableUser(model, _context);
        //                        if (result_ad.result == true)
        //                            writelog(LogType.log_lock_account_with_file, LogStatus.successfully, IDMSource.LDAP, model.basic_uid);
        //                        else
        //                            writelog(LogType.log_lock_account_with_file, LogStatus.failed, IDMSource.AD, model.basic_uid, log_exception: result_ad.Message);

        //                        writelog(LogType.log_lock_account_with_file, LogStatus.successfully, IDMSource.AD, model.basic_uid);
        //                    }
        //                    else
        //                    {
        //                        var result_ldap = _providerldap.NsLockUser(model, _context);
        //                        if (result_ldap.result == true)
        //                            writelog(LogType.log_unlock_account_with_file, LogStatus.successfully, IDMSource.LDAP, model.basic_uid);
        //                        else
        //                            writelog(LogType.log_unlock_account_with_file, LogStatus.failed, IDMSource.LDAP, model.basic_uid, log_exception: result_ldap.Message);

        //                        var result_ad = _provider.EnableUser(model, _context);
        //                        if (result_ad.result == true)
        //                            writelog(LogType.log_unlock_account_with_file, LogStatus.successfully, IDMSource.AD, model.basic_uid);
        //                        else
        //                            writelog(LogType.log_unlock_account_with_file, LogStatus.failed, IDMSource.AD, model.basic_uid, log_exception: result_ad.Message);

        //                        writelog(LogType.log_unlock_account_with_file, LogStatus.successfully, IDMSource.VisualFim, model.basic_uid);
        //                    }

        //                }
        //                catch (Exception ex)
        //                {
        //                    if (lockstatus == LockStaus.Lock)
        //                        writelog(LogType.log_lock_account_with_file, LogStatus.failed, IDMSource.VisualFim, model.basic_uid, log_exception: ex.Message);
        //                    else
        //                        writelog(LogType.log_unlock_account_with_file, LogStatus.failed, IDMSource.VisualFim, model.basic_uid, log_exception: ex.Message);
        //                }
        //            }
        //        }

        //    }
        //    _context.table_import.RemoveRange(_context.table_import.Where(w => w.import_Type == ImportType.delete));
        //    _context.SaveChanges();
        //    msg = ReturnMessage.Success;
        //    code = ReturnCode.Success;
        //    return RedirectToAction("EnableAccountFromFile", new { code = code, msg = msg });
        //}
        //#endregion       

        #region CheckAccount
        public async Task<IActionResult> CheckAccount(SearchDTO model)
        {
            ViewBag.Message = model.msg;
            ViewBag.ReturnCode = model.code;

            if (!checkrole())
                return RedirectToAction("Logout", "Auth");

            if (string.IsNullOrEmpty(model.text_search))
                return View(model);

            model.text_search = model.text_search.Trim();
            if (model.text_search.Length <= 3)
            {
                ModelState.AddModelError("text_search", "คำค้นจะต้องมากกว่า 3 ตัวอักษร");
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
