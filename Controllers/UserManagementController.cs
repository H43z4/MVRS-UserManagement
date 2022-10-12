using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels.Identity;
using SharedLib.APIs;
using SharedLib.Common;
using SharedLib.Security;
using UserManagement.Services;

namespace UserManagement.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = AuthenticationSchemes.JWT_BEARER_TOKEN_STATELESS)]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService userManagementService;
        public VwUser User
        {
            get
            {
                return (VwUser)this.Request.HttpContext.Items["User"];
            }
        }

        public UserManagementController(IUserManagementService userManagementService)
        {
            this.userManagementService = userManagementService;
        }


        //[HttpGet(Name = "GetPostalAddress")]
        //public async Task<ApiResponse> GetPostalAddress()
        //{
        //    this.userManagementService.VwUser = this.User;

        //    var ds = await this.userManagementService.GetPostalAddress();

        //    if (ds.Tables[0].Rows.Count == 0)
        //    {
        //        return ApiResponse.GetApiResponse(ApiResponseType.FAILED, null, Constants.NOT_FOUND_MESSAGE);
        //    }

        //    var data = ds.Tables[0].ToList<PostalAddressInfo>();

        //    return ApiResponse.GetApiResponse(ApiResponseType.SUCCESS, data, Constants.RECORD_FOUND_MESSAGE);
        //}
    }
}
