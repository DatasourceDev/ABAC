#pragma checksum "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8d21768b08863d9c71319dcda2ea3220e0c54889"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Account_InternetAccess), @"mvc.1.0.view", @"/Views/Account/InternetAccess.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Work\ABAC\ABAC\Views\_ViewImports.cshtml"
using ABAC;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Work\ABAC\ABAC\Views\_ViewImports.cshtml"
using ABAC.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
using ABAC.Identity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
using ABAC.Extensions;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8d21768b08863d9c71319dcda2ea3220e0c54889", @"/Views/Account/InternetAccess.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"820b982f49bd50402add54639bfc6c94fbc3cac4", @"/Views/_ViewImports.cshtml")]
    public class Views_Account_InternetAccess : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ABAC.DTO.SearchDTO>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("form-control"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", "text", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("placeholder", new global::Microsoft.AspNetCore.Html.HtmlString("รหัสผู้ใช้, ชื่อ, นามสกุล, เลขประจำตัวประชาชน, รหัสพนักงาน, โทรศัพท์, อีเมล์"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("value", "", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("form-horizontal form-parsley"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "InternetAccess", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Account", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_7 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "get", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.SelectTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 4 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
  
    ViewData["Title"] = "แก้ไข Internet access";


#line default
#line hidden
#nullable disable
            WriteLiteral(@"<div class=""container-fluid"">
    <div class=""row"">
        <div class=""col-sm-12"">
            <div aria-live=""polite"" aria-atomic=""true"" style=""position: relative;"">
                <div class=""page-title-box"">
                    <div class=""float-right"" style=""width:50%"">
");
#nullable restore
#line 14 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
                         if (ViewBag.ReturnCode != null)
                        {
                            

#line default
#line hidden
#nullable disable
#nullable restore
#line 16 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
                             if (ViewBag.ReturnCode == ReturnCode.Success)
                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                <div class=\"alert alert-success  border-0\" role=\"alert\">\r\n                                    <i class=\"mdi mdi-check-circle-outline font-18 mr-1\"></i>\r\n                                    <strong class=\"font-18\">");
#nullable restore
#line 20 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
                                                       Write(ViewBag.Message);

#line default
#line hidden
#nullable disable
            WriteLiteral("</strong>\r\n                                </div>\r\n");
#nullable restore
#line 22 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
                            }
                            else
                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                <div class=\"alert alert-danger border-0\" role=\"alert\">\r\n                                    <i class=\"mdi mdi-close-circle-outline font-18 mr-1\"></i>\r\n                                    <strong class=\"font-18\">");
#nullable restore
#line 27 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
                                                       Write(ViewBag.Message);

#line default
#line hidden
#nullable disable
            WriteLiteral("</strong>\r\n                                </div>\r\n");
#nullable restore
#line 29 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
                            }

#line default
#line hidden
#nullable disable
#nullable restore
#line 29 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
                             
                        }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                    </div>
                    <h4 class=""page-title"">แก้ไข Internet access</h4>
                    <p class=""text-muted"">
                        Edit internet access
                    </p>
                </div><!--end page-title-box-->
            </div>
        </div><!--end col-->
    </div>
    <!-- end page title end breadcrumb -->
    <!--end row-->
    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "8d21768b08863d9c71319dcda2ea3220e0c548899680", async() => {
                WriteLiteral(@"

        <div class=""row"">
            <div class=""col-lg-12"">
                <div class=""card"">
                    <div class=""card-body"">
                        <h4 class=""mt-0 header-title"">ค้นหา</h4>
                        <div class=""form-group row"">
                            <div class=""col-lg-9"">
                                <label class=""col-form-label"">คำค้น</label>
                                ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagOnly, "8d21768b08863d9c71319dcda2ea3220e0c5488910381", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_1.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
#nullable restore
#line 52 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.text_search);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                BeginWriteTagHelperAttribute();
                __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                __tagHelperExecutionContext.AddHtmlAttribute("required", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n                                ");
#nullable restore
#line 53 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
                           Write(Html.ValidationMessageFor(m => m.text_search));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\r\n                            </div>\r\n                            <div class=\"col-lg-2\">\r\n                                <label class=\"col-form-label\">ประเภทผู้ใช้</label>\r\n                                ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("select", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "8d21768b08863d9c71319dcda2ea3220e0c5488913066", async() => {
                    WriteLiteral("\r\n                                    ");
                    __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "8d21768b08863d9c71319dcda2ea3220e0c5488913371", async() => {
                        WriteLiteral("All");
                    }
                    );
                    __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                    __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                    __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = (string)__tagHelperAttribute_3.Value;
                    __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
                    await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                    if (!__tagHelperExecutionContext.Output.IsContentModified)
                    {
                        await __tagHelperExecutionContext.SetOutputContentAsync();
                    }
                    Write(__tagHelperExecutionContext.Output);
                    __tagHelperExecutionContext = __tagHelperScopeManager.End();
                    WriteLiteral("\r\n\r\n                                    ");
                    __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "8d21768b08863d9c71319dcda2ea3220e0c5488914696", async() => {
#nullable restore
#line 61 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
                                                                      Write(IDMUserType.affiliate.toUserTypeName());

#line default
#line hidden
#nullable disable
                    }
                    );
                    __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                    __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                    BeginWriteTagHelperAttribute();
#nullable restore
#line 61 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
                                       WriteLiteral(IDMUserType.affiliate);

#line default
#line hidden
#nullable disable
                    __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                    __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = __tagHelperStringValueBuffer;
                    __tagHelperExecutionContext.AddTagHelperAttribute("value", __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                    await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                    if (!__tagHelperExecutionContext.Output.IsContentModified)
                    {
                        await __tagHelperExecutionContext.SetOutputContentAsync();
                    }
                    Write(__tagHelperExecutionContext.Output);
                    __tagHelperExecutionContext = __tagHelperScopeManager.End();
                    WriteLiteral("\r\n                                    ");
                    __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "8d21768b08863d9c71319dcda2ea3220e0c5488916693", async() => {
#nullable restore
#line 62 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
                                                                     Write(IDMUserType.outsider.toUserTypeName());

#line default
#line hidden
#nullable disable
                    }
                    );
                    __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                    __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                    BeginWriteTagHelperAttribute();
#nullable restore
#line 62 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
                                       WriteLiteral(IDMUserType.outsider);

#line default
#line hidden
#nullable disable
                    __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                    __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = __tagHelperStringValueBuffer;
                    __tagHelperExecutionContext.AddTagHelperAttribute("value", __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                    await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                    if (!__tagHelperExecutionContext.Output.IsContentModified)
                    {
                        await __tagHelperExecutionContext.SetOutputContentAsync();
                    }
                    Write(__tagHelperExecutionContext.Output);
                    __tagHelperExecutionContext = __tagHelperScopeManager.End();
                    WriteLiteral("\r\n                                    ");
                    __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "8d21768b08863d9c71319dcda2ea3220e0c5488918687", async() => {
#nullable restore
#line 63 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
                                                                  Write(IDMUserType.staff.toUserTypeName());

#line default
#line hidden
#nullable disable
                    }
                    );
                    __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                    __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                    BeginWriteTagHelperAttribute();
#nullable restore
#line 63 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
                                       WriteLiteral(IDMUserType.staff);

#line default
#line hidden
#nullable disable
                    __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                    __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = __tagHelperStringValueBuffer;
                    __tagHelperExecutionContext.AddTagHelperAttribute("value", __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                    await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                    if (!__tagHelperExecutionContext.Output.IsContentModified)
                    {
                        await __tagHelperExecutionContext.SetOutputContentAsync();
                    }
                    Write(__tagHelperExecutionContext.Output);
                    __tagHelperExecutionContext = __tagHelperScopeManager.End();
                    WriteLiteral("\r\n                                    ");
                    __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "8d21768b08863d9c71319dcda2ea3220e0c5488920672", async() => {
#nullable restore
#line 64 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
                                                                    Write(IDMUserType.student.toUserTypeName());

#line default
#line hidden
#nullable disable
                    }
                    );
                    __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                    __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                    BeginWriteTagHelperAttribute();
#nullable restore
#line 64 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
                                       WriteLiteral(IDMUserType.student);

#line default
#line hidden
#nullable disable
                    __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                    __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = __tagHelperStringValueBuffer;
                    __tagHelperExecutionContext.AddTagHelperAttribute("value", __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                    await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                    if (!__tagHelperExecutionContext.Output.IsContentModified)
                    {
                        await __tagHelperExecutionContext.SetOutputContentAsync();
                    }
                    Write(__tagHelperExecutionContext.Output);
                    __tagHelperExecutionContext = __tagHelperScopeManager.End();
                    WriteLiteral("\r\n                                    ");
                    __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "8d21768b08863d9c71319dcda2ea3220e0c5488922663", async() => {
#nullable restore
#line 65 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
                                                                      Write(IDMUserType.temporary.toUserTypeName());

#line default
#line hidden
#nullable disable
                    }
                    );
                    __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                    __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                    BeginWriteTagHelperAttribute();
#nullable restore
#line 65 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
                                       WriteLiteral(IDMUserType.temporary);

#line default
#line hidden
#nullable disable
                    __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                    __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = __tagHelperStringValueBuffer;
                    __tagHelperExecutionContext.AddTagHelperAttribute("value", __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                    await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                    if (!__tagHelperExecutionContext.Output.IsContentModified)
                    {
                        await __tagHelperExecutionContext.SetOutputContentAsync();
                    }
                    Write(__tagHelperExecutionContext.Output);
                    __tagHelperExecutionContext = __tagHelperScopeManager.End();
                    WriteLiteral("\r\n                                ");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.SelectTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
#nullable restore
#line 58 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.usertype_search);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"
                            </div>
                            <div class=""col-lg-1"">
                                <label class=""col-form-label""><br /></label>
                                <button class=""btn btn-dark btn-block"" type=""submit"">ค้นหา</button>
                            </div>
                        </div>
                        <div class=""form-group row"">
                            <div class=""col-lg-12"">
                                <div class=""table-rep-plugin"">
                                    <div class=""table-responsive mb-0"" data-pattern=""priority-columns"">
                                        <table id=""tech-companies-1"" class=""table table-striped mb-0"">
                                            <thead>
                                                <tr>
                                                    <th>Login Name</th>
                                                    <th>Display Name</th>
                                                    ");
                WriteLiteral(@"<th>Mail</th>
                                                    <th>Mobile</th>
                                                    <th>Jobcode</th>
                                                    <th>Citizen ID</th>
                                                    <th></th>
                                                </tr>
                                            </thead>
                                            <tbody>
");
#nullable restore
#line 90 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
                                                 if (Model.lists != null)
                                                {
                                                    foreach (visual_fim_user item in Model.lists)
                                                    {

#line default
#line hidden
#nullable disable
                WriteLiteral("                                                        <tr>\r\n                                                            <td>");
#nullable restore
#line 95 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
                                                           Write(item.basic_uid);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                                                            <td>");
#nullable restore
#line 96 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
                                                           Write(item.basic_displayname);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                                                            <td>");
#nullable restore
#line 97 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
                                                           Write(item.basic_mail);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                                                            <td>");
#nullable restore
#line 98 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
                                                           Write(item.basic_mobile);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                                                            <td>");
#nullable restore
#line 99 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
                                                           Write(item.cu_jobcode);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                                                            <td>");
#nullable restore
#line 100 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
                                                           Write(item.cu_pplid);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                                                            <td>\r\n                                                                <a");
                BeginWriteAttribute("href", " href=\"", 6023, "\"", 6091, 1);
#nullable restore
#line 102 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
WriteAttributeValue("", 6030, Url.Action("InternetAccessInfo","Account", new{id=item.id }), 6030, 61, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral("><i class=\"far fa-edit font-16\"></i></a>\r\n                                                            </td>\r\n                                                        </tr>\r\n");
#nullable restore
#line 105 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
                                                    }

                                                }

#line default
#line hidden
#nullable disable
#nullable restore
#line 108 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
                                                 if (Model.lists == null || Model.lists.Count() == 0)
                                                {

#line default
#line hidden
#nullable disable
                WriteLiteral("                                                    <tr>\r\n                                                        <td colspan=\"7\" class=\"text-center\">ไม่พบข้อมูล</td>\r\n                                                    </tr>\r\n");
#nullable restore
#line 113 "C:\Work\ABAC\ABAC\Views\Account\InternetAccess.cshtml"
                                                }

#line default
#line hidden
#nullable disable
                WriteLiteral(@"
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>


                    </div> <!-- end card-body -->
                </div> <!-- end card -->
            </div> <!-- end col -->
            <!-- end col -->
        </div>
    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_5.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_5);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Controller = (string)__tagHelperAttribute_6.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_6);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_7.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_7);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n</div><!-- container -->\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ABAC.DTO.SearchDTO> Html { get; private set; }
    }
}
#pragma warning restore 1591
