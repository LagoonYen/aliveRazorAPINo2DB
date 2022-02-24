using AliveStoreTemplate.Model;
using AliveStoreTemplate.Model.DTOModel;
using AliveStoreTemplate.Model.ResponseModel;
using AliveStoreTemplate.Repositories;
using AliveStoreTemplate.Services;
using System;
using System.Collections.Generic;
using System.Net;

namespace AliveStoreTemplate.Service
{
    public class MemberServiceImpl : MemberService
    {
        private readonly MemberRepository _memberRepository;

        public MemberServiceImpl(MemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public BaseResponseModel GetMemberInfoByAccount(string account)
        {
            try
            {
                var memberExist = _memberRepository.IsMemberExist(account);
                if (!memberExist)
                {
                    throw new Exception("帳號不存在");
                }

                return new BaseResponseModel
                {
                    Message = "帳號存在, 已寄出認證信件",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch
            {
                throw;
            }
        }

        public BaseQueryModel<MemberInfo> GetMemberInfoById(string MemberId)
        {
            try
            {
                var baseQueryModel = _memberRepository.GetMemberInfoById(MemberId);
                return new BaseQueryModel<MemberInfo>
                {
                    Results = new List<MemberInfo> { baseQueryModel },
                    Message = "讀取資料正確",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch
            {
                throw;
            }
        }

        public BaseResponseModel PatchMemberInfo(PatchMemberInfoReqModel Req)
        {
            try
            {
                //先取得帳號存在 及資料
                var memberInfo = _memberRepository.GetMemberInfoById(Req.UID);

                //欲修改資料
                memberInfo.NickName = Req.NickName;
                memberInfo.PhoneNumber = Req.PhoneNumber;
                memberInfo.Email = Req.Email;
                memberInfo.UpdateTime = DateTime.Now;

                //修改資料並回傳
                _memberRepository.PatchMemberInfo(memberInfo);
                return new BaseResponseModel
                {
                    Message = "資料修改完畢",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch
            {
                throw;
            }
        }

        public BaseQueryModel<MemberInfo> PostLogin(LoginReqModel Req)
        {
            try
            {
                var account = Req.Account;
                var password = Req.Password;

                //取得帳號存在與否
                var IsUserExist = _memberRepository.IsMemberExist(account);
                //查無帳號
                if (!IsUserExist)
                {
                    throw new Exception("此帳號尚未被註冊");
                }

                //取得其資料
                var UserInfo = _memberRepository.GetMemberInfoByAccount(account);

                //比對密碼正確
                if (UserInfo.Password != password)
                {
                    throw new Exception("密碼錯誤");
                }

                return new BaseQueryModel<MemberInfo>
                {
                    Results = new List<MemberInfo> { UserInfo },
                    Message = "登入成功",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch
            {
                throw;
            }
        }

        public BaseQueryModel<MemberInfo> PostLogin(string Account, string Password)
        {
            try
            {
                //取得帳號存在與否
                var IsUserExist = _memberRepository.IsMemberExist(Account);
                //查無帳號
                if (!IsUserExist)
                {
                    return new BaseQueryModel<MemberInfo>
                    {
                        Results = null,
                        Message = "此帳號尚未被註冊",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                //取得其資料
                var UserInfo = _memberRepository.GetMemberInfoByAccount(Account);

                //比對密碼
                if (UserInfo.Password != Password)
                {
                    return new BaseQueryModel<MemberInfo>
                    {
                        Results = null,
                        Message = "密碼錯誤",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                return new BaseQueryModel<MemberInfo>
                {
                    Results = new List<MemberInfo> { UserInfo },
                    Message = "登入成功",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public BaseResponseModel PostMemberRegister(LoginReqModel Req)
        {
            try
            {
                var account = Req.Account;
                var password = Req.Password;

                //取得帳號存在與否 bool
                var IsMemberExist = _memberRepository.IsMemberExist(account);
                if (IsMemberExist)
                {
                    throw new Exception("此帳號已被註冊過");
                }

                //開始建立新帳戶必要資料
                MemberInfo member = new MemberInfo();
                //隨機一個MemberId
                member.MemberId = "Member" + Guid.NewGuid().ToString("N");
                member.Account = member.Email = account;
                member.Password = password;
                member.RegisterTime = member.UpdateTime = DateTime.Now;


                //創建帳號
                _memberRepository.PostMemberRegister(member);
                return new BaseResponseModel
                {
                    Message = "註冊成功",
                    StatusCode = HttpStatusCode.OK,
                };
            }
            catch
            {
                throw;
            }
        }
    }
}
