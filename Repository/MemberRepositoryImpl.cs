using AliveStoreTemplate.Model;
using AliveStoreTemplate.Repositories;
using No2DB.Base;
using System;
using System.Linq;

namespace AliveStoreTemplate.Repository
{
    public class MemberRepositoryImpl : MemberRepository
    {
        public MemberInfo GetMemberInfoByAccount(string account)
        {
            try
            {
                var collection = new DRole("Member");
                var members = collection.Query<MemberInfo>("MemberInfo").AllDatas();
                if(members == null)
                {
                    throw new Exception("目前無人註冊");
                }
                var member = members.FirstOrDefault(x => x.Account == account);
                if (member == null)
                {
                    throw new Exception("此帳號未被註冊!");
                }
                return member;
            }
            catch
            {
                throw;
            }
        }

        public MemberInfo GetMemberInfoById(string MemberId)
        {
            try
            {
                var collection = new DRole("Member");
                var member = collection.Query<MemberInfo>("MemberInfo").DataByKey(MemberId);
                if (member == null)
                {
                    throw new Exception("此帳號未被註冊!");
                }
                return member;
            }
            catch
            {
                throw;
            }
        }

        public bool IsMemberExist(string account)
        {
            try
            {
                //開啟Member資料夾
                var collection = new DRole("Member");
                //Query member 是否存在
                var members = collection.Query<MemberInfo>("MemberInfo").AllDatas();
                if (members == null)
                {
                    return false;
                }
                var member = members.FirstOrDefault(x => x.Account == account);
                if(member == null)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                throw;
            }
        }

        public void PatchMemberInfo(MemberInfo member)
        {
            try
            {
                No2DB.Transaction.Operator op = new No2DB.Transaction.Operator("aaa");
                var collection = new DRole("Member");
                var user = collection.Query<MemberInfo>("MemberInfo").DataByKey(member.MemberId);

                if (user == null)
                {
                    throw new Exception("找不到該帳號資料");
                }

                //user.Password = member.Password;
                //user.NickName = member.NickName;
                //user.PhoneNumber = member.PhoneNumber;
                //user.Email = member.Email;
                //user.ProfilePhotoUrl = member.ProfilePhotoUrl;
                //user.UpdateTime = member.UpdateTime;

                op.Update(collection, "MemberInfo", member.MemberId, member);
                op.Done();
            }
            catch
            {
                throw;
            }
        }

        public void PostMemberRegister(MemberInfo member)
        {
            try
            {
                var collection = new DRole("Member");
                collection.GetOp("MemberInfo").Update(member.MemberId, member);
            }
            catch
            {
                throw;
            }
        }
    }
}
