using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
using System.Text;
using System.Globalization;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using System.IO;

namespace ABAC.Controllers
{
    [Authorize]
    public class ReportController : ControllerBase
    {
        public MmsContext _contextmms;

        public ReportController(SpuContext context, MmsContext contextmms, ILogger<ReportController> logger, ILoginServices loginServices, IUserProvider provider, IOptions<SystemConf> conf) : base(context, logger, loginServices, conf, provider)
        {
            this._contextmms = contextmms;
        }
        public async Task<IActionResult> Log(SearchDTO model)
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.Helpdesk }))
                return RedirectToAction("Logout", "Auth");

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context, _conf.Env);
            if (userlogin == null)
                return RedirectToAction("Logout", "Auth");

            var lists = new List<system_log>();

            if (string.IsNullOrEmpty(model.dfrom))
                model.dfrom = DateUtil.ToDisplayDate(DateUtil.Now());
            if (string.IsNullOrEmpty(model.dto))
                model.dto = DateUtil.ToDisplayDate(DateUtil.Now());

            var dfrom = DateUtil.ToDate(model.dfrom);
            var dto = DateUtil.ToDate(model.dto);
            //if (!string.IsNullOrEmpty(model.id))
            //{
            //    var fim_user = _context.table_visual_fim_user.Where(w => w.basic_uid == model.id).FirstOrDefault();
            //    if (fim_user != null && fim_user.system_create_date.HasValue)
            //    {
            //        dfrom = fim_user.system_create_date;
            //        model.dfrom = DateUtil.ToDisplayDate(fim_user.system_create_date);
            //    }
            //}

            while (dfrom <= dto)
            {
                var datetime = dfrom.Value;
                var curdate = datetime.Year + "_" + datetime.Month.ToString("00") + "_" + datetime.Day.ToString("00");
                var tablename = "table_system_log_" + curdate;
                if (logTableIsExist(tablename))
                {
                    var sql = new StringBuilder();
                    sql.AppendLine("select [log_id],[log_username],[log_ip],[log_type_id],[log_type],[log_action],[log_status],[log_description],[log_target],[log_target_ip],[log_datetime],[log_exception] ");
                    sql.AppendLine(" from ");
                    sql.AppendLine(tablename);
                    sql.AppendLine(" where 1=1");
                    if (!string.IsNullOrEmpty(model.logstatus_search))
                    {
                        sql.AppendLine(" and log_status = '" + model.logstatus_search + "'");
                    }
                    if (!string.IsNullOrEmpty(model.log_type_search))
                    {
                        sql.AppendLine(" and log_type = '" + model.log_type_search + "'");
                    }
                    if (!string.IsNullOrEmpty(model.id))
                    {
                        sql.AppendLine(" and LOWER(log_username) = '" + model.id + "'");
                    }
                    if (!string.IsNullOrEmpty(model.text_search))
                    {
                        model.text_search = model.text_search.Trim().ToLower();
                        sql.AppendLine(" and (");
                        sql.AppendLine(" LOWER(log_username) like '%" + model.text_search + "%'");
                        sql.AppendLine(" or log_ip like '%" + model.text_search + "%'");
                        sql.AppendLine(" or LOWER(log_description) like '%" + model.text_search + "%'");
                        sql.AppendLine(" or log_target like '%" + model.text_search + "%'");
                        sql.AppendLine(" or log_target_ip like '%" + model.text_search + "%'");
                        sql.AppendLine(")");
                    }
                    sql.AppendLine(" order by log_datetime desc");

                    using (var command = _context.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = sql.ToString();
                        _context.Database.OpenConnection();
                        using (var result = command.ExecuteReader())
                        {
                            // do something with result
                            while (result.Read())
                            {
                                var j = 0;
                                var log = new system_log();
                                log.log_id = NumUtil.ParseInt64(AppUtil.ManageNull(result.GetValue(j))); j++;
                                log.log_username = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                                log.log_ip = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                                log.log_type_id = NumUtil.ParseInt64(AppUtil.ManageNull(result.GetValue(j))); j++;
                                log.log_type = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                                log.log_action = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                                log.log_status = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                                log.log_description = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                                log.log_target = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                                log.log_target_ip = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                                log.log_datetime = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                                log.log_exception = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                                lists.Add(log);
                            }
                        }
                    }
                }
                dfrom = dfrom.Value.AddDays(1);
            }


            //var lists = this._context.AuditLogs.Where(w => 1 == 1);

            //if (!string.IsNullOrEmpty(model.text_search))
            //    lists = lists.Where(w => w.Create_By.Contains(model.text_search) | w.FirstName.Contains(model.text_search) | w.LastName.Contains(model.text_search));

            //if (!string.IsNullOrEmpty(model.logaction))
            //    lists = lists.Where(w => w.Action == model.logaction);

            //if (!string.IsNullOrEmpty(model.dfrom))
            //{
            //    var dfrom = DateUtil.ToDate(model.dfrom);
            //    lists = lists.Where(w => w.Create_On >= dfrom);
            //}
            //if (!string.IsNullOrEmpty(model.dto))
            //{
            //    var dto = DateUtil.ToDate(model.dto);
            //    lists = lists.Where(w => w.Create_On <= dto);
            //}
            //if (!string.IsNullOrEmpty(model.logaction))
            //{
            //    lists = lists.Where(w => w.Action == model.logaction);
            //}

            //lists = lists.OrderByDescending(o => o.Create_On);
            //int skipRows = (model.pageno - 1) * 100;
            //var itemcnt = lists.Count();
            //var pagelen = itemcnt / 100;
            //if (itemcnt % 100 > 0)
            //    pagelen += 1;

            //model.itemcnt = itemcnt;
            //model.pagelen = pagelen;
            ////model.lists = lists.Skip(skipRows).Take(_pagelen).AsQueryable();

            model.lists = lists.OrderByDescending(o => o.log_datetime).AsQueryable();

            return View(model);
        }

        public async Task<IActionResult> BulkImport(SearchDTO model)
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.Helpdesk }))
                return RedirectToAction("Logout", "Auth");

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context, _conf.Env);
            if (userlogin == null)
                return RedirectToAction("Logout", "Auth");

            if (string.IsNullOrEmpty(model.dfrom))
                model.dfrom = DateUtil.ToDisplayDate(DateUtil.Now());
            if (string.IsNullOrEmpty(model.dto))
                model.dto = DateUtil.ToDisplayDate(DateUtil.Now());

            var dfrom = DateUtil.ToDate(model.dfrom);
            var dto = DateUtil.ToDate(model.dto);

            //if (this._loginServices.UserRole().Contains(roleType.Helpdesk) & !_loginServices.UserRole().Contains(roleType.Admin))
            if (string.IsNullOrEmpty(model.create_by))
                model.create_by = this.HttpContext.User.Identity.Name;


            var lists = this._context.User_Bulk_Import.Where(w => 1 == 1);

            if (!string.IsNullOrEmpty(model.text_search))
                lists = lists.Where(w => w.username.Contains(model.text_search) | w.firstname.Contains(model.text_search) | w.lastname.Contains(model.text_search) | w.Reference.Contains(model.text_search));

            if (!string.IsNullOrEmpty(model.create_by))
                lists = lists.Where(w => w.Create_By.ToLower().Contains(model.create_by.ToLower()) | w.adminname.ToLower().Contains(model.create_by.ToLower()));

            if (!string.IsNullOrEmpty(model.dfrom))
            {
                lists = lists.Where(w => w.Create_On >= dfrom);
            }
            if (!string.IsNullOrEmpty(model.dto))
            {
                lists = lists.Where(w => w.Create_On.Value.Date <= dto);
            }

            lists = lists.OrderByDescending(o => o.Create_On).ThenByDescending(o2 => o2.username);
            int skipRows = (model.pageno - 1) * 100;
            var itemcnt = lists.Count();
            var pagelen = itemcnt / 100;
            if (itemcnt % 100 > 0)
                pagelen += 1;

            model.itemcnt = itemcnt;
            model.pagelen = pagelen;
            //model.lists = lists.Skip(skipRows).Take(_pagelen).AsQueryable();

            model.lists = lists.AsQueryable();
            return View(model);
        }
        public async Task<IActionResult> BulkImportExcel(SearchDTO model)
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.Helpdesk }))
                return RedirectToAction("Logout", "Auth");

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context, _conf.Env);
            if (userlogin == null)
                return RedirectToAction("Logout", "Auth");

            if (string.IsNullOrEmpty(model.dfrom))
                model.dfrom = DateUtil.ToDisplayDate(DateUtil.Now());
            if (string.IsNullOrEmpty(model.dto))
                model.dto = DateUtil.ToDisplayDate(DateUtil.Now());

            var dfrom = DateUtil.ToDate(model.dfrom);
            var dto = DateUtil.ToDate(model.dto);

            //if (this._loginServices.UserRole().Contains(roleType.Helpdesk) & !_loginServices.UserRole().Contains(roleType.Admin))
            model.create_by = this.HttpContext.User.Identity.Name;

            var lists = this._context.User_Bulk_Import.Where(w => 1 == 1);

            if (!string.IsNullOrEmpty(model.text_search))
                lists = lists.Where(w => w.username.Contains(model.text_search) | w.firstname.Contains(model.text_search) | w.lastname.Contains(model.text_search) | w.Reference.Contains(model.text_search));

            if (!string.IsNullOrEmpty(model.create_by))
                lists = lists.Where(w => w.Create_By.ToLower().Contains(model.create_by.ToLower()) | w.adminname.ToLower().Contains(model.create_by.ToLower()));

            if (!string.IsNullOrEmpty(model.dfrom))
            {
                lists = lists.Where(w => w.Create_On >= dfrom);
            }
            if (!string.IsNullOrEmpty(model.dto))
            {
                lists = lists.Where(w => w.Create_On.Value.Date <= dto);
            }

            lists = lists.OrderByDescending(o => o.Create_On).ThenByDescending(o2 => o2.username);
            var date = DateUtil.ToInternalDate3(DateUtil.Now());
            var filePath = Directory.GetCurrentDirectory() + "\\wwwroot\\files\\temp\\bulkimport_" + date + ".xlsx";

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets.Add("Sheet1");
                var col = 1;
                var row = 1;
                worksheet.Cells[row, col].Value = "Username"; col++;
                worksheet.Cells[row, col].Value = "Password"; col++;
                worksheet.Cells[row, col].Value = "First Name"; col++;
                worksheet.Cells[row, col].Value = "Last Name"; col++;
                worksheet.Cells[row, col].Value = "Reference"; col++;
                worksheet.Cells[row, col].Value = "Create By"; col++;
                worksheet.Cells[row, col].Value = "Create Date"; col++;
                worksheet.Cells[row, col].Value = "Expiry Date"; col++;
                row++;
                foreach (User_Bulk_Import item in lists)
                {
                    col = 1;
                    worksheet.Cells[row, col].Value = item.username; col++;
                    worksheet.Cells[row, col].Value = item.password; col++;
                    worksheet.Cells[row, col].Value = item.firstname; col++;
                    worksheet.Cells[row, col].Value = item.lastname; col++;
                    worksheet.Cells[row, col].Value = item.Reference; col++;
                    worksheet.Cells[row, col].Value = item.Create_By; col++;
                    worksheet.Cells[row, col].Value = DateUtil.ToDisplayDateTime(item.Create_On); col++;
                    worksheet.Cells[row, col].Value = DateUtil.ToDisplayDate(item.expire_date); col++;
                    row++;
                }

                var filename = filePath;
                filePath = filePath.Replace("\\", "/");
                package.SaveAs(new FileInfo(filePath));
                if (System.IO.File.Exists(filename))
                {
                    var memory = new MemoryStream();
                    using (var stream = new FileStream(filename, FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }
                    memory.Position = 0;
                    var mimeType = "application/vnd.ms-excel";
                    return File(memory, mimeType, Path.GetFileName(filename));
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Bulk(SearchDTO model)
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.Helpdesk }))
                return RedirectToAction("Logout", "Auth");

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context, _conf.Env);
            if (userlogin == null)
                return RedirectToAction("Logout", "Auth");

            if (string.IsNullOrEmpty(model.dfrom))
                model.dfrom = DateUtil.ToDisplayDate(DateUtil.Now());
            if (string.IsNullOrEmpty(model.dto))
                model.dto = DateUtil.ToDisplayDate(DateUtil.Now());

            var dfrom = DateUtil.ToDate(model.dfrom);
            var dto = DateUtil.ToDate(model.dto);

            //if (this._loginServices.UserRole().Contains(roleType.Helpdesk) & !_loginServices.UserRole().Contains(roleType.Admin))
            if (string.IsNullOrEmpty(model.create_by))
                model.create_by = this.HttpContext.User.Identity.Name;

            var lists = this._context.User_Bulk.Where(w => 1 == 1);

            if (!string.IsNullOrEmpty(model.text_search))
                lists = lists.Where(w => w.username.Contains(model.text_search) | w.firstname.Contains(model.text_search) | w.lastname.Contains(model.text_search));

            if (!string.IsNullOrEmpty(model.create_by))
                lists = lists.Where(w => w.Create_By.ToLower().Contains(model.create_by.ToLower()) | w.adminname.ToLower().Contains(model.create_by.ToLower()));

            if (!string.IsNullOrEmpty(model.dfrom))
            {
                lists = lists.Where(w => w.Create_On >= dfrom);
            }
            if (!string.IsNullOrEmpty(model.dto))
            {
                lists = lists.Where(w => w.Create_On.Value.Date <= dto);
            }

            lists = lists.OrderByDescending(o => o.Create_On).ThenByDescending(o2 => o2.username);
            int skipRows = (model.pageno - 1) * 100;
            var itemcnt = lists.Count();
            var pagelen = itemcnt / 100;
            if (itemcnt % 100 > 0)
                pagelen += 1;

            model.itemcnt = itemcnt;
            model.pagelen = pagelen;
            //model.lists = lists.Skip(skipRows).Take(_pagelen).AsQueryable();

            model.lists = lists.AsQueryable();
            return View(model);
        }
        public async Task<IActionResult> BulkExcel(SearchDTO model)
        {
            if (!checkrole(new string[] { roleType.Admin, roleType.Helpdesk }))
                return RedirectToAction("Logout", "Auth");

            var userlogin = await _provider.GetAdUser2(this.HttpContext.User.Identity.Name, _context, _conf.Env);
            if (userlogin == null)
                return RedirectToAction("Logout", "Auth");

            if (string.IsNullOrEmpty(model.dfrom))
                model.dfrom = DateUtil.ToDisplayDate(DateUtil.Now());
            if (string.IsNullOrEmpty(model.dto))
                model.dto = DateUtil.ToDisplayDate(DateUtil.Now());

            var dfrom = DateUtil.ToDate(model.dfrom);
            var dto = DateUtil.ToDate(model.dto);

            var lists = this._context.User_Bulk.Where(w => 1 == 1);

            if (!string.IsNullOrEmpty(model.text_search))
                lists = lists.Where(w => w.username.Contains(model.text_search) | w.firstname.Contains(model.text_search) | w.lastname.Contains(model.text_search));

            if (!string.IsNullOrEmpty(model.create_by))
                lists = lists.Where(w => w.Create_By.ToLower().Contains(model.create_by.ToLower()) | w.adminname.ToLower().Contains(model.create_by.ToLower()));

            if (!string.IsNullOrEmpty(model.dfrom))
            {
                lists = lists.Where(w => w.Create_On >= dfrom);
            }
            if (!string.IsNullOrEmpty(model.dto))
            {
                lists = lists.Where(w => w.Create_On.Value.Date <= dto);
            }

            lists = lists.OrderByDescending(o => o.Create_On).ThenByDescending(o2 => o2.username);

            var date = DateUtil.ToInternalDate3(DateUtil.Now());
            var filePath = Directory.GetCurrentDirectory() + "\\wwwroot\\files\\temp\\bulk_" + date + ".xlsx";

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets.Add("Sheet1");
                var col = 1;
                var row = 1;
                worksheet.Cells[row, col].Value = "Username"; col++;
                worksheet.Cells[row, col].Value = "Password"; col++;
                worksheet.Cells[row, col].Value = "First Name"; col++;
                worksheet.Cells[row, col].Value = "Last Name"; col++;
                worksheet.Cells[row, col].Value = "Create By"; col++;
                worksheet.Cells[row, col].Value = "Create Date"; col++;
                worksheet.Cells[row, col].Value = "Expiry Date"; col++;
                row++;
                foreach(User_Bulk item in lists)
                {
                    col = 1;
                    worksheet.Cells[row, col].Value = item.username; col++;
                    worksheet.Cells[row, col].Value = item.password; col++;
                    worksheet.Cells[row, col].Value = item.firstname; col++;
                    worksheet.Cells[row, col].Value = item.lastname; col++;
                    worksheet.Cells[row, col].Value = item.Create_By; col++;
                    worksheet.Cells[row, col].Value = DateUtil.ToDisplayDateTime( item.Create_On); col++;
                    worksheet.Cells[row, col].Value = DateUtil.ToDisplayDate(item.expire_date); col++;
                    row++;
                }

                var filename = filePath;
                filePath = filePath.Replace("\\", "/");
                package.SaveAs(new FileInfo(filePath));
                if (System.IO.File.Exists(filename))
                {
                    var memory = new MemoryStream();
                    using (var stream = new FileStream(filename, FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }
                    memory.Position = 0;
                    var mimeType = "application/vnd.ms-excel";
                    return File(memory, mimeType, Path.GetFileName(filename));
                }
            }
            return View(model);
        }
        public IActionResult CreateUser(SearchDTO model)
        {
            if (string.IsNullOrEmpty(model.dfrom))
                model.dfrom = DateUtil.ToDisplayDate(DateUtil.Now());
            if (string.IsNullOrEmpty(model.dto))
                model.dto = DateUtil.ToDisplayDate(DateUtil.Now());

            var dfrom = DateUtil.ToDate(model.dfrom);
            var dto = DateUtil.ToDate(model.dto);
            var lists = new List<m_step_history>();
            var sql = new StringBuilder();
            sql.AppendLine(" select m.ma_name, r.run_profile_name, sh.step_history_id, sh.run_history_id, sh.step_number ,sh.step_result , sh.start_date, sh.end_date, ");
            sql.AppendLine(" sh.export_add, sh.export_update, sh.export_rename, sh.export_delete, sh.export_deleteadd, sh.export_failure ");
            sql.AppendLine(" from [mms_step_history] sh");
            sql.AppendLine(" inner join [mms_run_history] r on sh.run_history_id = r.run_history_id");
            sql.AppendLine(" inner join [mms_management_agent] m on m.ma_id = r.ma_id");
            sql.AppendLine(" where 1 = 1");
            if (dfrom.HasValue)
            {
                sql.AppendLine(" and sh.start_date >= convert(datetime, '" + DateUtil.ToInternalDate(dfrom) +"',110)");
            }
            if (dto.HasValue)
            {
                sql.AppendLine(" and sh.start_date <= convert(datetime, '" + DateUtil.ToInternalDate(dto.Value.AddDays(1)) + "',110)");
            }
            sql.AppendLine(" and (m.ma_name = 'STAFF-ADMA' or m.ma_name = 'STUDENT-ADMA')");
            sql.AppendLine(" and r.run_profile_name = 'Export'");
            sql.AppendLine(" and sh.export_add > 0");
            sql.AppendLine(" order by sh.start_date desc; ");
            using (var command = _contextmms.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql.ToString();
                _contextmms.Database.OpenConnection();
                using (var result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        var j = 0;
                        var row = new m_step_history();
                        row.ma_name = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                        row.run_profile_name = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                        row.step_history_id = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                        row.run_history_id = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                        row.step_number = NumUtil.ParseInteger(AppUtil.ManageNull(result.GetValue(j))); j++;
                        row.step_result = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                        row.start_date = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                        var start_date = DateUtil.ToDate(row.start_date, monthfirst: true);
                        if (start_date.HasValue)
                        {
                            //start_date = start_date.Value.AddHours(7);
                            row.start_date = DateUtil.ToDisplayDateTime(start_date);
                        }

                        row.end_date = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                        var end_date = DateUtil.ToDate(row.end_date, monthfirst: true);
                        if (end_date.HasValue)
                        {
                            //end_date = end_date.Value.AddHours(7);
                            row.end_date = DateUtil.ToDisplayDateTime(end_date);
                        }

                        row.export_add = NumUtil.ParseInteger(AppUtil.ManageNull(result.GetValue(j))); j++;
                        row.export_update = NumUtil.ParseInteger(AppUtil.ManageNull(result.GetValue(j))); j++;
                        row.export_rename = NumUtil.ParseInteger(AppUtil.ManageNull(result.GetValue(j))); j++;
                        row.export_delete = NumUtil.ParseInteger(AppUtil.ManageNull(result.GetValue(j))); j++;
                        row.export_deleteadd = NumUtil.ParseInteger(AppUtil.ManageNull(result.GetValue(j))); j++;
                        row.export_failure = NumUtil.ParseInteger(AppUtil.ManageNull(result.GetValue(j))); j++;
                        lists.Add(row);
                    }
                }
            }

            int skipRows = (model.pageno - 1) * 100;
            var itemcnt = lists.Count();
            var pagelen = itemcnt / 100;
            if (itemcnt % 100 > 0)
                pagelen += 1;

            model.itemcnt = itemcnt;
            model.pagelen = pagelen;

            model.lists = lists.AsQueryable();
            return View(model);
        }
        public IActionResult UpdateUser(SearchDTO model)
        {
            if (string.IsNullOrEmpty(model.dfrom))
                model.dfrom = DateUtil.ToDisplayDate(DateUtil.Now());
            if (string.IsNullOrEmpty(model.dto))
                model.dto = DateUtil.ToDisplayDate(DateUtil.Now());

            var dfrom = DateUtil.ToDate(model.dfrom);
            var dto = DateUtil.ToDate(model.dto);
            var lists = new List<m_step_history>();
            var sql = new StringBuilder();
            sql.AppendLine(" select m.ma_name, r.run_profile_name, sh.step_history_id, sh.run_history_id, sh.step_number ,sh.step_result , sh.start_date, sh.end_date, ");
            sql.AppendLine(" sh.export_add, sh.export_update, sh.export_rename, sh.export_delete, sh.export_deleteadd, sh.export_failure ");
            sql.AppendLine(" from [mms_step_history] sh");
            sql.AppendLine(" inner join [mms_run_history] r on sh.run_history_id = r.run_history_id");
            sql.AppendLine(" inner join [mms_management_agent] m on m.ma_id = r.ma_id");
            sql.AppendLine(" where 1 = 1");
            if (dfrom.HasValue)
            {
                sql.AppendLine(" and sh.start_date >= convert(datetime, '" + DateUtil.ToInternalDate(dfrom) + "',110)");
            }
            if (dto.HasValue)
            {
                sql.AppendLine(" and sh.start_date <= convert(datetime, '" + DateUtil.ToInternalDate(dto.Value.AddDays(1)) + "',110)");
            }
            sql.AppendLine(" and (m.ma_name = 'STAFF-ADMA' or m.ma_name = 'STUDENT-ADMA')");
            sql.AppendLine(" and r.run_profile_name = 'Export'");
            sql.AppendLine(" and (sh.export_update > 0 or sh.export_rename > 0 )");
            sql.AppendLine(" order by sh.start_date desc; ");
            using (var command = _contextmms.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql.ToString();
                _contextmms.Database.OpenConnection();
                using (var result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        var j = 0;
                        var row = new m_step_history();
                        row.ma_name = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                        row.run_profile_name = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                        row.step_history_id = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                        row.run_history_id = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                        row.step_number = NumUtil.ParseInteger(AppUtil.ManageNull(result.GetValue(j))); j++;
                        row.step_result = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                        row.start_date = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                        var start_date = DateUtil.ToDate(row.start_date, monthfirst: true);
                        if (start_date.HasValue)
                        {
                            //start_date = start_date.Value.AddHours(7);
                            row.start_date = DateUtil.ToDisplayDateTime(start_date);
                        }

                        row.end_date = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                        var end_date = DateUtil.ToDate(row.end_date, monthfirst: true);
                        if (end_date.HasValue)
                        {
                            //end_date = end_date.Value.AddHours(7);
                            row.end_date = DateUtil.ToDisplayDateTime(end_date);
                        }

                        row.export_add = NumUtil.ParseInteger(AppUtil.ManageNull(result.GetValue(j))); j++;
                        row.export_update = NumUtil.ParseInteger(AppUtil.ManageNull(result.GetValue(j))); j++;
                        row.export_rename = NumUtil.ParseInteger(AppUtil.ManageNull(result.GetValue(j))); j++;
                        row.export_delete = NumUtil.ParseInteger(AppUtil.ManageNull(result.GetValue(j))); j++;
                        row.export_deleteadd = NumUtil.ParseInteger(AppUtil.ManageNull(result.GetValue(j))); j++;
                        row.export_failure = NumUtil.ParseInteger(AppUtil.ManageNull(result.GetValue(j))); j++;
                        lists.Add(row);
                    }
                }
            }

            int skipRows = (model.pageno - 1) * 100;
            var itemcnt = lists.Count();
            var pagelen = itemcnt / 100;
            if (itemcnt % 100 > 0)
                pagelen += 1;

            model.itemcnt = itemcnt;
            model.pagelen = pagelen;

            model.lists = lists.AsQueryable();
            return View(model);
        }
        public IActionResult DeleteUser(SearchDTO model)
        {
            if (string.IsNullOrEmpty(model.dfrom))
                model.dfrom = DateUtil.ToDisplayDate(DateUtil.Now());
            if (string.IsNullOrEmpty(model.dto))
                model.dto = DateUtil.ToDisplayDate(DateUtil.Now());

            var dfrom = DateUtil.ToDate(model.dfrom);
            var dto = DateUtil.ToDate(model.dto);
            var lists = new List<m_step_history>();
            var sql = new StringBuilder();
            sql.AppendLine(" select m.ma_name, r.run_profile_name, sh.step_history_id, sh.run_history_id, sh.step_number ,sh.step_result , sh.start_date, sh.end_date, ");
            sql.AppendLine(" sh.export_add, sh.export_update, sh.export_rename, sh.export_delete, sh.export_deleteadd, sh.export_failure ");
            sql.AppendLine(" from [mms_step_history] sh");
            sql.AppendLine(" inner join [mms_run_history] r on sh.run_history_id = r.run_history_id");
            sql.AppendLine(" inner join [mms_management_agent] m on m.ma_id = r.ma_id");
            sql.AppendLine(" where 1 = 1");
            if (dfrom.HasValue)
            {
                sql.AppendLine(" and sh.start_date >= convert(datetime, '" + DateUtil.ToInternalDate(dfrom) + "',110)");
            }
            if (dto.HasValue)
            {
                sql.AppendLine(" and sh.start_date <= convert(datetime, '" + DateUtil.ToInternalDate(dto.Value.AddDays(1)) + "',110)");
            }
            sql.AppendLine(" and (m.ma_name = 'STAFF-ADMA' or m.ma_name = 'STUDENT-ADMA')");
            sql.AppendLine(" and r.run_profile_name = 'Export'");
            sql.AppendLine(" and sh.export_delete > 0");
            sql.AppendLine(" order by sh.start_date desc; ");
            using (var command = _contextmms.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql.ToString();
                _contextmms.Database.OpenConnection();
                using (var result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        var j = 0;
                        var row = new m_step_history();
                        row.ma_name = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                        row.run_profile_name = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                        row.step_history_id = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                        row.run_history_id = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                        row.step_number = NumUtil.ParseInteger(AppUtil.ManageNull(result.GetValue(j))); j++;
                        row.step_result = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                        row.start_date = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                        var start_date = DateUtil.ToDate(row.start_date, monthfirst: true);
                        if (start_date.HasValue)
                        {
                            //start_date = start_date.Value.AddHours(7);
                            row.start_date = DateUtil.ToDisplayDateTime(start_date);
                        }

                        row.end_date = AppUtil.ManageNull(result.GetValue(j)).ToString(); j++;
                        var end_date = DateUtil.ToDate(row.end_date, monthfirst: true);
                        if (end_date.HasValue)
                        {
                            //end_date = end_date.Value.AddHours(7);
                            row.end_date = DateUtil.ToDisplayDateTime(end_date);
                        }

                        row.export_add = NumUtil.ParseInteger(AppUtil.ManageNull(result.GetValue(j))); j++;
                        row.export_update = NumUtil.ParseInteger(AppUtil.ManageNull(result.GetValue(j))); j++;
                        row.export_rename = NumUtil.ParseInteger(AppUtil.ManageNull(result.GetValue(j))); j++;
                        row.export_delete = NumUtil.ParseInteger(AppUtil.ManageNull(result.GetValue(j))); j++;
                        row.export_deleteadd = NumUtil.ParseInteger(AppUtil.ManageNull(result.GetValue(j))); j++;
                        row.export_failure = NumUtil.ParseInteger(AppUtil.ManageNull(result.GetValue(j))); j++;
                        lists.Add(row);
                    }
                }
            }

            int skipRows = (model.pageno - 1) * 100;
            var itemcnt = lists.Count();
            var pagelen = itemcnt / 100;
            if (itemcnt % 100 > 0)
                pagelen += 1;

            model.itemcnt = itemcnt;
            model.pagelen = pagelen;

            model.lists = lists.AsQueryable();
            return View(model);
        }
    }
}
