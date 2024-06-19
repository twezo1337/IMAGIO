using IMAGIO.Services;
using IMAGIO.ViewModels;
using System.Security.Claims;

namespace IMAGIO.Interfaces
{
    public interface IAccountService
    {
        Task<BaseResponse<ClaimsIdentity>> Register(RegisterViewModel model);

        Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model);

        //Task<BaseResponse<bool>> ChangePassword(ChangePasswordViewModel model);
    }
}
