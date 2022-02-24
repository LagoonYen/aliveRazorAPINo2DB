using AliveStoreTemplate.Model.DTOModel;
using AliveStoreTemplate.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

namespace AliveStoreTemplate.Pages
{
    public class RegisterModel : PageModel
    {

        [BindProperty]
        public string Account { get; set; }

        [BindProperty]
        public string Password { get; set; }

        private readonly MemberService _memberService;

        public RegisterModel(MemberService memberService)
        {
            _memberService = memberService;
        }

        public void OnGet()
        {

        }

        public void OnPostRegister()
        {
            var Req = new LoginReqModel
            {
                Account = Account,
                Password = Password
            };

            var result = _memberService.PostMemberRegister(Req);

            if(result.StatusCode == HttpStatusCode.OK)
            {
                Response.Redirect("Home");
            }
            ViewData["Message"] = string.Format("Register Error");
        }
    }
}
