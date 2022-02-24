using AliveStoreTemplate.Model;
using AliveStoreTemplate.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace AliveStoreTemplate.Pages
{
    public class LoginModel : PageModel
    {

        [BindProperty]
        public string Account { get; set; }
        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string Notice { get; set; }


        private readonly MemberService _memberService;

        public LoginModel(MemberService memberService)
        {
            _memberService = memberService;
        }

        public void OnGet()
        {

        }

        public void OnPostMyLogin()
        {
            var result = _memberService.PostLogin(Account, Password);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var user = result.Results.FirstOrDefault();
                
                if (user != null)
                {
                    //�]�wsession
                    Common.CommonUtil.SessionSetObject<MemberInfo>(HttpContext.Session, Common.SessionKeys.LoginSession, user);
                    Response.Redirect("Home");
                }
            }
            Notice = result.Message;
        }
    }
}
