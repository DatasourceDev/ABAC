#pragma checksum "C:\Source Code\ABAC\Views\Shared\MailRequestApproveResetPassword.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8567152166e7d82d871a7f23d67a39feb366aa5a"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_MailRequestApproveResetPassword), @"mvc.1.0.view", @"/Views/Shared/MailRequestApproveResetPassword.cshtml")]
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
#line 1 "C:\Source Code\ABAC\Views\_ViewImports.cshtml"
using ABAC;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Source Code\ABAC\Views\_ViewImports.cshtml"
using ABAC.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Source Code\ABAC\Views\Shared\MailRequestApproveResetPassword.cshtml"
using ABAC.Services;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Source Code\ABAC\Views\Shared\MailRequestApproveResetPassword.cshtml"
using ABAC.Extensions;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8567152166e7d82d871a7f23d67a39feb366aa5a", @"/Views/Shared/MailRequestApproveResetPassword.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"820b982f49bd50402add54639bfc6c94fbc3cac4", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_MailRequestApproveResetPassword : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ABAC.Models.visual_fim_user>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 4 "C:\Source Code\ABAC\Views\Shared\MailRequestApproveResetPassword.cshtml"
  
    ViewData["Title"] = "MailRequestApproveResetPassword";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"<div>
    ระบบยืนยันการร้องขอเปลี่ยนรหัสผ่านของท่าน
    <br />
    <br />
    หากท่านไม่ได้ดำเนินการร้องขอเปลี่ยนรหัสผ่านโปรดติดต่อกลับ สำนักบริหารเทคโนโลยีสารสนเทศ เพื่อตรวจสอบ
    <br />
    เบอร์โทร 02-2183314 (ในเวลาทำการ)
    <br />
    หรือ email Help@chula.ac.th
    <table>
        <tr>
            <td style=""padding-right:15px"">
                <img src=""http://idm3.it.chula.ac.th/images/logo.png"" height=""30""  />
            </td>
            <td>
                <p>
                    ศูนย์ ICT มหาวิทยาลัย<br />
                    email: ictcenter@cu.ac.th<br />
                </p>
            </td>
        </tr>
    </table>

</div>

");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ABAC.Models.visual_fim_user> Html { get; private set; }
    }
}
#pragma warning restore 1591
