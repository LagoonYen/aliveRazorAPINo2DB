using AliveStoreTemplate.Model;

namespace AliveStoreTemplate.Repositories
{
    public interface MemberRepository
    {
        public bool IsMemberExist(string account);

        public MemberInfo GetMemberInfoByAccount(string account);

        public void PostMemberRegister(MemberInfo member);

        public MemberInfo GetMemberInfoById(string MemberId);

        public void PatchMemberInfo(MemberInfo member);
    }
}
