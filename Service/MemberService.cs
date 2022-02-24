using AliveStoreTemplate.Model;
using AliveStoreTemplate.Model.DTOModel;
using AliveStoreTemplate.Model.ResponseModel;
using System.Threading.Tasks;

namespace AliveStoreTemplate.Services
{
    public interface MemberService
    {
        BaseResponseModel PostMemberRegister(LoginReqModel Req);

        BaseQueryModel<MemberInfo> PostLogin(LoginReqModel Req);

        BaseQueryModel<MemberInfo> PostLogin(string Account ,string Password);

        BaseQueryModel<MemberInfo> GetMemberInfoById(string MemberId);

        BaseResponseModel PatchMemberInfo(PatchMemberInfoReqModel Req);

        BaseResponseModel GetMemberInfoByAccount(string account);
    }
}
