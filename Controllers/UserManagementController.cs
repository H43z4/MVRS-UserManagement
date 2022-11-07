using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseModels.Authentication;
using Models.ViewModels.Identity;
using Models.ViewModels.VehicleRegistration.Core;
using SharedLib.APIs;
using SharedLib.Common;
using SharedLib.Security;
using UserManagement.Services;
using UserManagement.ViewModels;

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

        #region GetMethods

        [HttpGet(Name = "GetDropDownsCreateUser")]
        public async Task<ApiResponse> GetDropDownsCreateUser()
        {
            var ds = await this.userManagementService.GetDropDownsCreateUser();
            var data = new
            {
                Districts = ds.Tables[0],
                SiteOffices = ds.Tables[1],
                Roles = ds.Tables[2]
            };

            return ApiResponse.GetApiResponse(ApiResponseType.SUCCESS, data, Constants.RECORD_FOUND_MESSAGE);
        }

        [HttpGet(Name = "GetLineManager")]
        public async Task<ApiResponse> GetLineManager(long DistrictId = 0, long OfficeId = 0, long userRoleId = 0)
        {
            var ds = await this.userManagementService.GetLineManager(DistrictId, OfficeId, userRoleId);
            if (ds.Tables[0].Rows.Count > 0)
            {
                var data = new
                {
                    Roles = ds.Tables[0]
                };

                return ApiResponse.GetApiResponse(ApiResponseType.SUCCESS, data, Constants.RECORD_FOUND_MESSAGE);

            }
            else
            {

                return ApiResponse.GetApiResponse(ApiResponseType.NOT_FOUND, null, Constants.NOT_FOUND_MESSAGE);
            }

        }

        [HttpGet(Name = "GetUsersList")]
        public async Task<ApiResponse> GetUsersList()
        {
            var ds = await this.userManagementService.GetUserList();
            if (ds.Tables[0].Rows.Count > 0)
            {
                var data = new
                {
                    users = ds.Tables[0]
                };

                return ApiResponse.GetApiResponse(ApiResponseType.SUCCESS, data, Constants.RECORD_FOUND_MESSAGE);

            }
            else
            {

                return ApiResponse.GetApiResponse(ApiResponseType.NOT_FOUND, null, Constants.NOT_FOUND_MESSAGE);
            }

        }

        [HttpGet(Name = "GetAssignedSeriesCategories")]
        public async Task<ApiResponse> GetAssignedSeriesCategories(long userId, long RoleId)
        {
            var ds = await this.userManagementService.GetAssignedSeriesCategories(userId, RoleId);
            if (ds.Tables[0].Rows.Count > 0)
            {
                var data = new
                {
                    SeriesCategories = ds.Tables[0]
                };

                return ApiResponse.GetApiResponse(ApiResponseType.SUCCESS, data, Constants.RECORD_FOUND_MESSAGE);

            }
            else
            {

                return ApiResponse.GetApiResponse(ApiResponseType.NOT_FOUND, null, Constants.NOT_FOUND_MESSAGE);
            }

        }


        [HttpGet(Name = "GetUserByUserId")]
        public async Task<ApiResponse> GetUserByUserId(long userId)
        {
            var ds = await this.userManagementService.GetUserByUserId(userId);
            if (ds.Tables[0].Rows.Count > 0)
            {
                var data = new
                {
                    users = SharedLib.Common.Extentions.ToList<VwUserInfo>(ds.Tables[0]).FirstOrDefault()
                };

                return ApiResponse.GetApiResponse(ApiResponseType.SUCCESS, data, Constants.RECORD_FOUND_MESSAGE);

            }
            else
            {

                return ApiResponse.GetApiResponse(ApiResponseType.NOT_FOUND, null, Constants.NOT_FOUND_MESSAGE);
            }

        }



        [HttpGet(Name = "GetPersonByCNIC")]
        public async Task<ApiResponse> GetPersonByCNIC(string cnic)
        {

            var ds = await this.userManagementService.GetPersonByCNIC(cnic);

            var person = SharedLib.Common.Extentions.ToList<UserManagement.ViewModels.VwPerson>(ds.Tables[0]).FirstOrDefault();

            if (person is not null)
            {
                person.Addresses = SharedLib.Common.Extentions.ToList<UserManagement.ViewModels.VwAddress>(ds.Tables[1]).ToList();
                person.PhoneNumbers = SharedLib.Common.Extentions.ToList<UserManagement.ViewModels.VwPhoneNumber>(ds.Tables[2]).ToList();
            }

            var data = new { person };

            var status = person is not null ? Constants.RECORD_FOUND_MESSAGE : Constants.NOT_FOUND_MESSAGE;

            return ApiResponse.GetApiResponse(ApiResponseType.SUCCESS, data, status);
        }

        [HttpGet(Name = "GetUserPermission")]
        public async Task<ApiResponse> GetUserPermission(long userId, long RoleId)
        {

            var ds = await this.userManagementService.GetUserPermission(userId, RoleId);
            if (ds.Tables[0].Rows.Count > 0)
            {
                var data = new
                {
                    userPersmissions = ds.Tables[0]
                };

                return ApiResponse.GetApiResponse(ApiResponseType.SUCCESS, data, Constants.RECORD_FOUND_MESSAGE);

            }
            else
            {

                return ApiResponse.GetApiResponse(ApiResponseType.NOT_FOUND, null, Constants.NOT_FOUND_MESSAGE);
            }
        }




        #endregion



        #region PostMethods

        [HttpPost]
        public async Task<ApiResponse> CreateUser(VwCreateUser userObj)
        {
            this.userManagementService.VwUser = this.User;
            userObj.UserTypeId = 1;
            
            var passwordHasher = new PasswordHasher<User>();
            userObj.Password = passwordHasher.HashPassword(new Models.DatabaseModels.Authentication.User(), userObj.Password);

            var ds = await this.userManagementService.CreateUser(userObj);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows[0][0].ToString() == "0")
            {
                return ApiResponse.GetApiResponse(ApiResponseType.SUCCESS, null, Constants.DATA_SAVED_MESSAGE);
            }
            else
            {
                return ApiResponse.GetApiResponse(ApiResponseType.FAILED, null, Constants.DATA_NOT_SAVED_MESSAGE + ds.Tables[0].Rows[0][1].ToString());
            }
        }

        [HttpPost]
        public async Task<ApiResponse> UpdateUserRole(VwUserRole userObj)
        {
            this.userManagementService.VwUser = this.User;
            var ds = await this.userManagementService.UpdateUserRole(userObj);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows[0][0].ToString() == "0")
            {
                return ApiResponse.GetApiResponse(ApiResponseType.SUCCESS, null, Constants.DATA_SAVED_MESSAGE);
            }
            else
            {
                return ApiResponse.GetApiResponse(ApiResponseType.FAILED, null, Constants.DATA_NOT_SAVED_MESSAGE + ds.Tables[0].Rows[0][1].ToString());
            }

        }

        [HttpPost]
        public async Task<ApiResponse> SaveUserPermission(VwUserPermissions PermisionObj)
        {
            this.userManagementService.VwUser = this.User;
            var ds = await this.userManagementService.SaveUserPermission(PermisionObj);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows[0][0].ToString() == "0")
            {
                return ApiResponse.GetApiResponse(ApiResponseType.SUCCESS, null, Constants.DATA_SAVED_MESSAGE);
            }
            else
            {
                return ApiResponse.GetApiResponse(ApiResponseType.FAILED, null, Constants.DATA_NOT_SAVED_MESSAGE + ds.Tables[0].Rows[0][1].ToString());
            }

        }

        [HttpPost]
        public async Task<ApiResponse> CancellUserPermission(VwUserPermissionId obj)
        {
            this.userManagementService.VwUser = this.User;
            var ds = await this.userManagementService.CancellUserPermission(obj.UserId, obj.PermissionId);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows[0][0].ToString() == "0")
            {
                return ApiResponse.GetApiResponse(ApiResponseType.SUCCESS, null, Constants.DATA_SAVED_MESSAGE);
            }
            else
            {
                return ApiResponse.GetApiResponse(ApiResponseType.FAILED, null, Constants.DATA_NOT_SAVED_MESSAGE + ds.Tables[0].Rows[0][1].ToString());
            }

        }


        [HttpPost]
        public async Task<ApiResponse> SaveAssignedSeriesCategories(VwUserSeriesCategory obj)
        {
            this.userManagementService.VwUser = this.User;
            var ds = await this.userManagementService.SaveAssignedSeriesCategories(obj);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows[0][0].ToString() == "0")
            {
                return ApiResponse.GetApiResponse(ApiResponseType.SUCCESS, null, Constants.DATA_SAVED_MESSAGE);
            }
            else
            {
                return ApiResponse.GetApiResponse(ApiResponseType.FAILED, null, Constants.DATA_NOT_SAVED_MESSAGE + ds.Tables[0].Rows[0][1].ToString());
            }

        }


        #endregion
    }
}
