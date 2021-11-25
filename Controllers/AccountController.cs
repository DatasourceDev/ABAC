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
                }
                catch (Exception ex)
                {
                    writelog(LogType.log_create_account, LogStatus.failed, IDMSource.VisualFim, model.SamAccountName, log_exception: ex.Message);
                }

                ViewBag.Message = ReturnMessage.Success;
                ViewBag.ReturnCode = ReturnCode.Success;
                return RedirectToAction("CreateAccountCompleted", new { code = ReturnCode.Success, msg = ReturnMessage.Success, id = model.SamAccountName });

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

        //#region CreateAccountFromFile
        //public IActionResult CreateAccountFromFile(SearchDTO model)
        //{
        //     if (!checkrole())
        //        return RedirectToAction("Logout", "Auth");
        //    model.lists = (new List<import>()).AsQueryable();
        //    if (model.code == ReturnCode.Success)
        //        model.lists = (_context.table_import.Where(w => w.import_Type == ImportType.create)).AsQueryable();
        //    ViewBag.Message = model.msg;
        //    ViewBag.ReturnCode = model.code;
        //    return View(model);
        //}

        //[HttpPost]
        //public IActionResult CreateAccountFromFile(IFormFile file, ImportCreateOption import_option)
        //{
        //     if (!checkrole())
        //        return RedirectToAction("Logout", "Auth");

        //    var model = new SearchDTO();
        //    var lists = new List<import>();
        //    if (file != null)
        //    {
        //        _context.table_import.RemoveRange(_context.table_import.Where(w => w.import_Type == ImportType.create));
        //        using (var reader = new StreamReader(file.OpenReadStream()))
        //        {
        //            string input;
        //            var row = 1;
        //            while ((input = reader.ReadLine()) != null)
        //            {
        //                if (string.IsNullOrEmpty(input))
        //                    continue;
        //                var columnNameList = input.Split(":");
        //                if (import_option == ImportCreateOption.staff_hr | import_option == ImportCreateOption.staff_other)
        //                {
        //                    columnNameList = input.Split("\t");
        //                }
        //                var remark = new StringBuilder();
        //                var imp = new import();
        //                imp.ImportVerify = true;
        //                imp.ImportRow = row;
        //                imp.basic_uid = "";
        //                imp.basic_givenname = "";
        //                imp.basic_sn = "";
        //                imp.cu_pplid = "";
        //                imp.LockStaus = "";
        //                imp.import_Type = ImportType.create;
        //                imp.import_create_option = import_option;
        //                try
        //                {
        //                    var j = 0;
        //                    if (import_option == ImportCreateOption.student)
        //                    {
        //                        if (columnNameList.Length != 11)
        //                        {
        //                            ModelState.AddModelError("format_error", "รูปแบบไฟล์ไม่ถูกต้อง");
        //                            return View(model);
        //                        }
        //                        imp.system_idm_user_types = IDMUserType.student;
        //                        imp.cu_jobcode = columnNameList[j]; j++;
        //                        /*imp.other_prename = columnNameList[j];*/
        //                        j++;
        //                        /*imp.prename = columnNameList[j];*/
        //                        j++;
        //                        imp.basic_givenname = columnNameList[j]; j++;
        //                        imp.basic_sn = columnNameList[j]; j++;
        //                        /*imp.other_prenameth =columnNameList[j];*/
        //                        j++;
        //                        /*imp.prenameth = columnNameList[j];*/
        //                        j++;
        //                        imp.cu_thcn = columnNameList[j]; j++;
        //                        imp.cu_thsn = columnNameList[j]; j++;
        //                        /*imp.barcode = columnNameList[j];*/
        //                        j++;
        //                        imp.cu_pplid = columnNameList[j]; j++;

        //                        if (string.IsNullOrEmpty(imp.cu_jobcode))
        //                            continue;

        //                        imp.basic_uid = imp.cu_jobcode;
        //                        var fim_user = _context.table_visual_fim_user.Where(w => w.basic_uid.ToLower() == imp.basic_uid.ToLower()).FirstOrDefault();
        //                        if (fim_user != null)
        //                        {
        //                            imp.ImportVerify = false;
        //                            remark.AppendLine("ผู้ใช้ซ้ำในระบบ");
        //                        }
        //                    }
        //                    else if (import_option == ImportCreateOption.student_sasin)
        //                    {
        //                        if (columnNameList.Length != 7)
        //                        {
        //                            ModelState.AddModelError("format_error", "รูปแบบไฟล์ไม่ถูกต้อง");
        //                            return View(model);
        //                        }
        //                        imp.system_idm_user_types = IDMUserType.student;
        //                        imp.faculty_shot_name = columnNameList[j]; j++;
        //                        imp.cu_jobcode = columnNameList[j]; j++;
        //                        imp.basic_givenname = columnNameList[j]; j++;
        //                        imp.basic_sn = columnNameList[j]; j++;
        //                        imp.cu_thcn = columnNameList[j]; j++;
        //                        imp.cu_thsn = columnNameList[j]; j++;
        //                        imp.cu_pplid = columnNameList[j]; j++;

        //                        if (string.IsNullOrEmpty(imp.cu_jobcode))
        //                            continue;

        //                        imp.basic_uid = imp.cu_jobcode;
        //                        if (imp.cu_jobcode.Length > 10)
        //                            imp.basic_uid = imp.cu_jobcode.Substring(0, 10);

        //                        var fim_user = _context.table_visual_fim_user.Where(w => w.basic_uid.ToLower() == imp.basic_uid.ToLower()).FirstOrDefault();
        //                        if (fim_user != null)
        //                        {
        //                            imp.ImportVerify = false;
        //                            remark.AppendLine("ผู้ใช้ซ้ำในระบบ");
        //                        }
        //                    }
        //                    else if (import_option == ImportCreateOption.student_ppc)
        //                    {
        //                        if (columnNameList.Length != 7)
        //                        {
        //                            ModelState.AddModelError("format_error", "รูปแบบไฟล์ไม่ถูกต้อง");
        //                            return View(model);
        //                        }
        //                        imp.system_idm_user_types = IDMUserType.student;
        //                        imp.faculty_shot_name = columnNameList[j]; j++;
        //                        imp.cu_jobcode = columnNameList[j]; j++;
        //                        imp.basic_givenname = columnNameList[j]; j++;
        //                        imp.basic_sn = columnNameList[j]; j++;
        //                        imp.cu_thcn = columnNameList[j]; j++;
        //                        imp.cu_thsn = columnNameList[j]; j++;
        //                        imp.cu_pplid = columnNameList[j]; j++;

        //                        if (string.IsNullOrEmpty(imp.cu_jobcode))
        //                            continue;

        //                        imp.basic_uid = imp.cu_jobcode;
        //                        if (imp.cu_jobcode.Length > 10)
        //                            imp.basic_uid = imp.cu_jobcode.Trim().Substring(0, 9) + "p";

        //                        var fim_user = _context.table_visual_fim_user.Where(w => w.basic_uid.ToLower() == imp.basic_uid.ToLower()).FirstOrDefault();
        //                        if (fim_user != null)
        //                        {
        //                            imp.ImportVerify = false;
        //                            remark.AppendLine("ผู้ใช้ซ้ำในระบบ");
        //                        }
        //                    }
        //                    else if (import_option == ImportCreateOption.student_other)
        //                    {
        //                        if (columnNameList.Length != 8)
        //                        {
        //                            ModelState.AddModelError("format_error", "รูปแบบไฟล์ไม่ถูกต้อง");
        //                            return View(model);
        //                        }
        //                        imp.system_idm_user_types = IDMUserType.student;
        //                        imp.faculty_shot_name = columnNameList[j]; j++;
        //                        imp.cu_jobcode = columnNameList[j]; j++;
        //                        imp.basic_givenname = columnNameList[j]; j++;
        //                        imp.basic_sn = columnNameList[j]; j++;
        //                        imp.cu_thcn = columnNameList[j]; j++;
        //                        imp.cu_thsn = columnNameList[j]; j++;
        //                        imp.cu_pplid = columnNameList[j]; j++;
        //                        imp.basic_uid = columnNameList[j]; j++;

        //                        var fim_user = _context.table_visual_fim_user.Where(w => w.basic_uid.ToLower() == imp.basic_uid.ToLower()).FirstOrDefault();
        //                        if (fim_user != null)
        //                        {
        //                            imp.ImportVerify = false;
        //                            remark.AppendLine("ผู้ใช้ซ้ำในระบบ");
        //                        }
        //                    }
        //                    else if (import_option == ImportCreateOption.staff_hr)
        //                    {
        //                        if (columnNameList.Length != 11)
        //                        {
        //                            ModelState.AddModelError("format_error", "รูปแบบไฟล์ไม่ถูกต้อง");
        //                            return View(model);
        //                        }
        //                        imp.system_idm_user_types = IDMUserType.staff;
        //                        imp.cu_jobcode = columnNameList[j]; j++;
        //                        imp.cu_thcn = columnNameList[j]; j++;
        //                        imp.cu_thsn = columnNameList[j]; j++;
        //                        imp.basic_givenname = columnNameList[j]; j++;
        //                        imp.basic_sn = columnNameList[j]; j++;
        //                        imp.structure_1 = columnNameList[j]; j++;
        //                        imp.structure_2 = columnNameList[j]; j++;
        //                        imp.status = columnNameList[j]; j++;
        //                        imp.cu_pplid = columnNameList[j]; j++;
        //                        imp.basic_mobile = columnNameList[j]; j++;
        //                        imp.basic_telephonenumber = columnNameList[j]; j++;

        //                    }
        //                    else if (import_option == ImportCreateOption.staff_other)
        //                    {
        //                        if (columnNameList.Length != 12)
        //                        {
        //                            ModelState.AddModelError("format_error", "รูปแบบไฟล์ไม่ถูกต้อง");
        //                            return View(model);
        //                        }
        //                        imp.system_idm_user_types = IDMUserType.staff;
        //                        imp.cu_jobcode = columnNameList[j]; j++;
        //                        imp.cu_thcn = columnNameList[j]; j++;
        //                        imp.cu_thsn = columnNameList[j]; j++;
        //                        imp.basic_givenname = columnNameList[j]; j++;
        //                        imp.basic_sn = columnNameList[j]; j++;
        //                        imp.structure_1 = columnNameList[j]; j++;
        //                        imp.structure_2 = columnNameList[j]; j++;
        //                        imp.status = columnNameList[j]; j++;
        //                        imp.cu_pplid = columnNameList[j]; j++;
        //                        imp.cu_CUexpire = columnNameList[j]; j++;
        //                        imp.basic_mobile = columnNameList[j]; j++;
        //                        imp.basic_telephonenumber = columnNameList[j]; j++;
        //                    }
        //                    else if (import_option == ImportCreateOption.fixlogin)
        //                    {
        //                        if (columnNameList.Length != 3)
        //                        {
        //                            ModelState.AddModelError("format_error", "รูปแบบไฟล์ไม่ถูกต้อง");
        //                            return View(model);
        //                        }
        //                        imp.system_idm_user_types = IDMUserType.outsider;
        //                        imp.system_org = columnNameList[j]; j++;
        //                        imp.basic_givenname = columnNameList[j]; j++;
        //                        imp.basic_sn = columnNameList[j]; j++;

        //                        imp.cu_thcn = imp.basic_givenname;
        //                        imp.cu_thsn = imp.basic_sn;
        //                        imp.basic_uid = imp.basic_givenname;
        //                    }


        //                    if (string.IsNullOrEmpty(imp.basic_givenname))
        //                    {
        //                        imp.ImportVerify = false;
        //                        remark.AppendLine("First Name ไม่สามารถเป็นค่าว่าง");
        //                    }
        //                    if (string.IsNullOrEmpty(imp.basic_sn))
        //                    {
        //                        imp.ImportVerify = false;
        //                        remark.AppendLine("Last Name ไม่สามารถเป็นค่าว่าง");
        //                    }

        //                    if (import_option == ImportCreateOption.student
        //                        || import_option == ImportCreateOption.student_sasin
        //                        || import_option == ImportCreateOption.student_ppc
        //                        || import_option == ImportCreateOption.student_other
        //                        || import_option == ImportCreateOption.staff_hr
        //                        || import_option == ImportCreateOption.staff_other)
        //                    {
        //                        if (import_option != ImportCreateOption.staff_hr)
        //                        {
        //                            if (string.IsNullOrEmpty(imp.cu_jobcode))
        //                            {
        //                                imp.ImportVerify = false;
        //                                remark.AppendLine("jobcode ไม่สามารถเป็นค่าว่าง");
        //                            }
        //                        }
        //                        if (import_option == ImportCreateOption.staff_other || import_option == ImportCreateOption.staff_hr)
        //                        {
        //                            if (string.IsNullOrEmpty(imp.cu_pplid))
        //                            {
        //                                imp.ImportVerify = false;
        //                                remark.AppendLine("Citizen ID ไม่สามารถเป็นค่าว่าง");
        //                            }
        //                            else
        //                            {

        //                                var fim_user = _context.table_visual_fim_user
        //                                    .Where(w => w.cu_pplid.ToLower() == imp.cu_pplid.ToLower() & (w.system_idm_user_type == IDMUserType.staff | w.system_idm_user_type == IDMUserType.affiliate | w.system_idm_user_type == IDMUserType.outsider | w.system_idm_user_type == IDMUserType.temporary))
        //                                    .FirstOrDefault();
        //                                if (fim_user != null)
        //                                {
        //                                    imp.ImportVerify = false;
        //                                    remark.AppendLine("Citizen ID ซ้ำในระบบ");
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else if (import_option == ImportCreateOption.fixlogin)
        //                    {
        //                        var fim_user = _context.table_visual_fim_user
        //                                    .Where(w => w.basic_uid.ToLower() == imp.basic_uid.ToLower())
        //                                    .FirstOrDefault();
        //                        if (fim_user != null)
        //                        {
        //                            imp.ImportVerify = false;
        //                            remark.AppendLine("basic_uid ซ้ำในระบบ");
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
        //public IActionResult CreateAccountFromFile2()
        //{
        //     if (!checkrole())
        //        return RedirectToAction("Logout", "Auth");

        //    var userlogin = this._context.table_visual_fim_user.Where(w => w.basic_uid == this.HttpContext.User.Identity.Name).FirstOrDefault();
        //    if (userlogin == null)
        //        return RedirectToAction("Logout", "Auth");

        //    var msg = ReturnMessage.ImportFail;
        //    var code = ReturnCode.Error;

        //    var imports = _context.table_import.Where(w => w.import_Type == ImportType.create).OrderBy(o => o.ImportRow);
        //    foreach (var imp in imports.ToList())
        //    {
        //        var fim_user = _context.table_visual_fim_user.Where(w => w.basic_uid.ToLower() == imp.basic_uid.ToLower()).FirstOrDefault();
        //        if (fim_user != null)
        //        {
        //            imp.ImportVerify = false;
        //            imp.ImportRemark = "ผู้ใช้ซ้ำในระบบ";
        //            continue;
        //        }
        //        var model = new visual_fim_user();
        //        model.cu_jobcode = imp.cu_jobcode;
        //        model.basic_givenname = imp.basic_givenname;
        //        model.basic_sn = imp.basic_sn;
        //        model.cu_thcn = imp.cu_thcn;
        //        model.cu_thsn = imp.cu_thsn;
        //        model.cu_pplid = imp.cu_pplid;
        //        model.cu_CUexpire = imp.cu_CUexpire;
        //        model.basic_telephonenumber = imp.basic_telephonenumber;
        //        model.basic_mobile = imp.basic_mobile;
        //        if (string.IsNullOrEmpty(imp.ImportRemark))
        //            imp.ImportRemark = "";
        //        faculty faculty = null;

        //        if (imp.import_create_option == ImportCreateOption.student | imp.import_create_option == ImportCreateOption.student_sasin | imp.import_create_option == ImportCreateOption.student_ppc | imp.import_create_option == ImportCreateOption.student_other)
        //        {
        //            if (imp.import_create_option == ImportCreateOption.student)
        //            {
        //                var faculty_id = NumUtil.ParseInteger(imp.cu_jobcode.Substring(imp.cu_jobcode.Length - 2));
        //                faculty = _context.table_cu_faculty.Where(w => w.faculty_id == faculty_id).FirstOrDefault();
        //            }
        //            else if (imp.import_create_option == ImportCreateOption.student_sasin)
        //            {
        //                faculty = _context.table_cu_faculty.Where(w => w.faculty_shot_name.ToLower() == imp.faculty_shot_name.ToLower()).FirstOrDefault();
        //            }
        //            else if (imp.import_create_option == ImportCreateOption.student_ppc)
        //            {
        //                faculty = _context.table_cu_faculty.Where(w => w.faculty_shot_name.ToLower() == imp.faculty_shot_name.ToLower()).FirstOrDefault();
        //            }
        //            else if (imp.import_create_option == ImportCreateOption.student_other)
        //            {
        //                faculty = _context.table_cu_faculty.Where(w => w.faculty_shot_name.ToLower() == imp.faculty_shot_name.ToLower()).FirstOrDefault();
        //            }

        //        }
        //        else if (imp.import_create_option == ImportCreateOption.staff_hr | imp.import_create_option == ImportCreateOption.staff_other)
        //        {
        //            faculty = _context.table_cu_faculty.Where(w => w.faculty_name.ToLower() == imp.structure_1.ToLower() | w.faculty_name.ToLower() == imp.structure_2.ToLower() | w.faculty_shot_name.ToLower() == imp.structure_1.ToLower() | w.faculty_shot_name.ToLower() == imp.structure_2.ToLower()).FirstOrDefault();
        //            if (faculty == null)
        //            {
        //                var subfaculty = _context.table_cu_faculty_level2.Where(w => w.sub_office_name.ToLower() == imp.structure_1.ToLower() | w.sub_office_name.ToLower() == imp.structure_2.ToLower() | w.sub_office_shot_name.ToLower() == imp.structure_1.ToLower() | w.sub_office_shot_name.ToLower() == imp.structure_2.ToLower()).FirstOrDefault();
        //                if (subfaculty != null)
        //                {
        //                    faculty = _context.table_cu_faculty.Where(w => w.faculty_id == subfaculty.faculty_id).FirstOrDefault();
        //                }
        //            }
        //        }
        //        else if (imp.import_create_option == ImportCreateOption.fixlogin)
        //        {
        //            faculty = _context.table_cu_faculty.Where(w => w.faculty_name.ToLower() == imp.system_org.ToLower() | w.faculty_shot_name.ToLower() == imp.system_org.ToLower()).FirstOrDefault();
        //            if (faculty == null)
        //            {
        //                var subfaculty = _context.table_cu_faculty_level2.Where(w => w.sub_office_name.ToLower() == imp.system_org.ToLower() | w.sub_office_shot_name.ToLower() == imp.system_org.ToLower()).FirstOrDefault();
        //                if (subfaculty != null)
        //                {
        //                    faculty = _context.table_cu_faculty.Where(w => w.faculty_id == subfaculty.faculty_id).FirstOrDefault();
        //                }
        //            }
        //        }

        //        if (faculty != null)
        //        {
        //            model.system_faculty_id = (int)faculty.faculty_id;
        //            var distinguish_name = "";
        //            if (imp.import_create_option == ImportCreateOption.student | imp.import_create_option == ImportCreateOption.student_sasin | imp.import_create_option == ImportCreateOption.student_ppc | imp.import_create_option == ImportCreateOption.student_other)
        //            {
        //                distinguish_name = faculty.faculty_distinguish_name_student;
        //            }
        //            else if (imp.import_create_option == ImportCreateOption.staff_hr | imp.import_create_option == ImportCreateOption.staff_other)
        //            {
        //                if (imp.status.ToLower().Trim() == "student".ToLower())
        //                    distinguish_name = faculty.faculty_distinguish_name_student;
        //                else if (imp.status.ToLower().Trim() == "staff".ToLower() || imp.status.ToLower().Trim() == "พนักงานปกติ".ToLower())
        //                    distinguish_name = faculty.faculty_distinguish_name_staff;
        //                else if (imp.status.ToLower().Trim() == "outsider".ToLower())
        //                    distinguish_name = faculty.faculty_distinguish_name_outsider;
        //                else if (imp.status.ToLower().Trim() == "affiliate".ToLower())
        //                    distinguish_name = faculty.faculty_distinguish_name_affiliate;
        //            }
        //            if (imp.import_create_option == ImportCreateOption.fixlogin)
        //            {
        //                distinguish_name = faculty.faculty_distinguish_name_outsider;
        //                model.basic_uid = imp.basic_uid;
        //            }

        //            if (!string.IsNullOrEmpty(distinguish_name))
        //            {
        //                var ous = distinguish_name.Split(",");
        //                if (ous.Length > 3)
        //                {
        //                    for (var i = ous.Length - 1; i >= 0; i--)
        //                    {
        //                        var ou = ous[i];
        //                        if (i < 3)
        //                        {
        //                            if (ous.Length == 6)
        //                            {
        //                                if (i == 2)
        //                                    model.system_ou_lvl1 = ou;
        //                                else if (i == 1)
        //                                    model.system_ou_lvl2 = ou;
        //                                else if (i == 0)
        //                                    model.system_ou_lvl3 = ou;
        //                            }
        //                            else if (ous.Length == 5)
        //                            {
        //                                if (i == 1)
        //                                    model.system_ou_lvl1 = ou;
        //                                else if (i == 0)
        //                                    model.system_ou_lvl2 = ou;
        //                            }
        //                            else if (ous.Length == 4)
        //                            {
        //                                if (i == 0)
        //                                    model.system_ou_lvl1 = ou;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            try
        //            {

        //                genNewAccount(_context, model);
        //                _context.SaveChanges();

        //                var result_ldap = _providerldap.CreateUser(model, _context);
        //                model.ldap_created = result_ldap.result;
        //                if (result_ldap.result == true)
        //                    writelog(LogType.log_create_account_with_file, LogStatus.successfully, IDMSource.LDAP, model.basic_uid);
        //                else
        //                {
        //                    imp.ImportVerify = false;
        //                    imp.ImportRemark += writelog(LogType.log_create_account_with_file, LogStatus.failed, IDMSource.LDAP, model.basic_uid, log_exception: result_ldap.Message) + Environment.NewLine;

        //                }

        //                var result_ad = _provider.CreateUser(model, _context);
        //                model.ad_created = result_ad.result;
        //                if (result_ad.result == true)
        //                    writelog(LogType.log_create_account_with_file, LogStatus.successfully, IDMSource.AD, model.basic_uid);
        //                else
        //                {
        //                    imp.ImportVerify = false;
        //                    imp.ImportRemark += writelog(LogType.log_create_account_with_file, LogStatus.failed, IDMSource.AD, model.basic_uid, log_exception: result_ad.Message) + Environment.NewLine;
        //                }

        //                writelog(LogType.log_create_account_with_file, LogStatus.successfully, IDMSource.VisualFim, model.basic_uid);

        //                if (result_ldap.result == true && result_ad.result == true)
        //                    imp.ImportRemark = "สร้างบัญชีผู้ใช้สำเร็จ";
        //                _context.SaveChanges();
        //            }
        //            catch (Exception ex)
        //            {
        //                imp.ImportRemark += writelog(LogType.log_create_account_with_file, LogStatus.failed, IDMSource.VisualFim, model.basic_uid, log_exception: ex.Message) + Environment.NewLine;
        //            }
        //        }
        //    }
        //    //_context.table_import.RemoveRange(_context.table_import.Where(w => w.import_Type == ImportType.create));
        //    _context.SaveChanges();
        //    msg = ReturnMessage.ImportSuccess;
        //    code = ReturnCode.Success;
        //    return RedirectToAction("CreateAccountFromFile", new { code = code, msg = msg });
        //}


        //#endregion

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
                                var userType = getaUUserType(model.DistinguishedName);
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

                    var userType = getaUUserType(user.DistinguishedName);
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
                    var userType = getaUUserType(model.DistinguishedName);
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
