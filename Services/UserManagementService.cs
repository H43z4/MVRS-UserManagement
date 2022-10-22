using Database;
using Models.ViewModels.Identity;
using SharedLib.Common;
using SharedLib.Interfaces;
using System.Data;
using UserManagement.ViewModels;
using UserManagement.ViewModels.udt;



namespace UserManagement.Services
{
    public interface IUserManagementService : ICurrentUser
    {
        Task<DataSet> GetDropDownsCreateUser();
        Task<DataSet> GetUserList();
        Task<DataSet> GetPersonByCNIC(string cnic);
        Task<DataSet> GetUserByUserId(long userId);
        Task<DataSet> GetAssignedSeriesCategories(long userId, long RoleId);
        Task<DataSet> GetUserPermission(long userId, long RoleId);
        Task<DataSet> UpdateUserRole(VwUserRole userObj);
        Task<DataSet> SaveUserPermission(VwUserPermissions PermisionObj);
        Task<DataSet> CancellUserPermission(long UserId, long PermissionId);
        Task<DataSet> GetLineManager(long DistrictId = 0, long OfficeId = 0, long userRoleId = 0);
        Task<DataSet> SaveAssignedSeriesCategories(VwUserSeriesCategory obj);
        Task<DataSet> CreateUser(VwCreateUser userObj);

    }

    public class UserManagementService : IUserManagementService
    {
        readonly IDBHelper dbHelper;
        public VwUser VwUser { get; set; }
        public UserManagementService(IDBHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public async Task<DataSet> GetDropDownsCreateUser()
        {
            Dictionary<string, object> paramDict = new Dictionary<string, object>();
            var ds = await this.dbHelper.GetDataSetByStoredProcedure("[Auth].[GetDropDownsCreateUser]", paramDict);
            return ds;
        }


        public async Task<DataSet> GetUserList()
        {
            Dictionary<string, object> paramDict = new Dictionary<string, object>();
            var ds = await this.dbHelper.GetDataSetByStoredProcedure("[Auth].[GetUserList]", paramDict);
            return ds;
        }

        public async Task<DataSet> GetPersonByCNIC(string cnic)
        {
            //SqlParameter[] sql = new SqlParameter[2];
            //sql[0] = new SqlParameter("@PersonId", DBNull.Value);
            //sql[1] = new SqlParameter("@CNIC", cnic);

            //var ds = await this.adoNet.ExecuteUsingDataAdapter("[Core].[GetPerson]", sql);

            Dictionary<string, object> paramDict = new Dictionary<string, object>();
            paramDict.Add("@CNIC", cnic);
            var ds = await this.dbHelper.GetDataSetByStoredProcedure("[Auth].[GetPerson]", paramDict);


            return ds;
        }

        public async Task<DataSet> GetUserByUserId(long userId)
        {
            Dictionary<string, object> paramDict = new Dictionary<string, object>();
            paramDict.Add("@UserId", userId);
            var ds = await this.dbHelper.GetDataSetByStoredProcedure("[Auth].[GetUserByUserId]", paramDict);
            return ds;
        }

        public async Task<DataSet> GetAssignedSeriesCategories(long userId, long RoleId)
        {
            Dictionary<string, object> paramDict = new Dictionary<string, object>();
            paramDict.Add("@UserId", userId);
            paramDict.Add("@RoleId", RoleId);
            var ds = await this.dbHelper.GetDataSetByStoredProcedure("[Auth].[GetAssignedSeriesCategories]", paramDict);
            return ds;
        }


        public async Task<DataSet> GetUserPermission(long userId, long RoleId)
        {
            Dictionary<string, object> paramDict = new Dictionary<string, object>();
            paramDict.Add("@UserId", userId);
            paramDict.Add("@RoleId", RoleId);
            var ds = await this.dbHelper.GetDataSetByStoredProcedure("[Auth].[GetUserPermission]", paramDict);
            return ds;
        }


        public async Task<DataSet> GetLineManager(long DistrictId = 0, long OfficeId = 0, long userRoleId = 0)
        {

            Dictionary<string, object> paramDict = new Dictionary<string, object>();
            paramDict.Add("@DistrictId", DistrictId);
            paramDict.Add("@SiteOfficeId", OfficeId);
            paramDict.Add("@RoleId", userRoleId);
            var ds = await this.dbHelper.GetDataSetByStoredProcedure("[Auth].[GetLineManager]", paramDict);


            return ds;
        }


        public async Task<DataSet> UpdateUserRole(VwUserRole userObj)
        {
            Dictionary<string, object> paramDict = new Dictionary<string, object>();
            paramDict.Add("@UserId", userObj.UserId);
            paramDict.Add("@Roleid", userObj.RoleId);
            paramDict.Add("@Createdby", this.VwUser.UserId);
            paramDict.Add("@DistrictId", userObj.DistrictId);
            paramDict.Add("@SiteOfficeId", userObj.SiteOfficeId);
            if (userObj.LineManagerId > 0)
            {
                paramDict.Add("@LineManagerId", userObj.LineManagerId);
            }
            else
            {
                paramDict.Add("@LineManagerId", DBNull.Value);
            }

            var ds = await this.dbHelper.GetDataSetByStoredProcedure("[Auth].[UpdateUserRole]", paramDict);
            return ds;
        }


        public async Task<DataSet> SaveUserPermission(VwUserPermissions PermisionObj)
        {
            Dictionary<string, object> paramDict = new Dictionary<string, object>();
            paramDict.Add("@UserId", PermisionObj.UserId);
            paramDict.Add("@Roleid", PermisionObj.RoleId);
            paramDict.Add("@WorkingPermissionId", PermisionObj.WorkingPermissionId);
            paramDict.Add("@MinDateTime", PermisionObj.MinDateTime);
            paramDict.Add("@MaxDateTime", PermisionObj.MaxDateTime);
            paramDict.Add("@Createdby", this.VwUser.UserId);

            var ds = await this.dbHelper.GetDataSetByStoredProcedure("[Auth].[SaveUserPermission]", paramDict);
            return ds;
        }

        public async Task<DataSet> CancellUserPermission(long UserId, long PermissionId)
        {
            Dictionary<string, object> paramDict = new Dictionary<string, object>();
            paramDict.Add("@UserId", UserId);
            paramDict.Add("@UserWorkingPermissionId", PermissionId);
            paramDict.Add("@Createdby", this.VwUser.UserId);

            var ds = await this.dbHelper.GetDataSetByStoredProcedure("[Auth].[CancellUserPermission]", paramDict);
            return ds;
        }

        public async Task<DataSet> SaveAssignedSeriesCategories(VwUserSeriesCategory obj)
        {
            Dictionary<string, object> paramDict = new Dictionary<string, object>();
            long[] IdArrays = obj.SeriesCategoryId;

            VwIdOnly newId;
            var Id = new List<VwIdOnly>();
            for (int i = 0; i < IdArrays.Length; i++)
            {
                newId = new VwIdOnly();
                newId.Id = IdArrays[i];
                Id.Add(newId);

            }

            paramDict.Add("@Ids", Id.ToDataTable());
            paramDict.Add("@UserId", obj.UserId);
            paramDict.Add("@Roleid", obj.RoleId);
            paramDict.Add("@Createdby", this.VwUser.UserId);


            var ds = await this.dbHelper.GetDataSetByStoredProcedure("[Auth].[SaveAssignedSeriesCategories]", paramDict);
            return ds;
        }


        public async Task<DataSet> CreateUser(VwCreateUser userObj)
        {
            Dictionary<string, object> paramDict = new Dictionary<string, object>();


            var person = userObj.Person;
            var personDataModel = new Person();
            EntityMapper<VwPerson, Person>.CopyByPropertyNameAndType(person, personDataModel);

            var personList = new List<Person>();
            personList.Add(personDataModel);

            var addresses = new List<Address>();

            person.Addresses.ForEach(x =>
            {
                var address = new Address();
                EntityMapper<VwAddress, Address>.CopyByPropertyNameAndType(x, address);
                addresses.Add(address);
            });

            var phoneNumbers = new List<PhoneNumber>();

            person.PhoneNumbers.ForEach(x =>
            {
                var phoneNumber = new PhoneNumber();
                EntityMapper<VwPhoneNumber, PhoneNumber>.CopyByPropertyNameAndType(x, phoneNumber);
                phoneNumbers.Add(phoneNumber);
            });


            paramDict.Add("@Person", personList.ToDataTable());
            paramDict.Add("@Address", addresses.ToDataTable());
            paramDict.Add("@PhoneNumber", phoneNumbers.ToDataTable());
            paramDict.Add("@UserName", userObj.UserName);
            paramDict.Add("@Password", userObj.Password);
            paramDict.Add("@Roleid", userObj.RoleId);
            paramDict.Add("@Createdby", this.VwUser.UserId);
            paramDict.Add("@UserDistrictId", userObj.DistrictId);
            paramDict.Add("@SiteOfficeId", userObj.SiteOfficeId);
            paramDict.Add("@UserTypeId", userObj.UserTypeId);
            if (userObj.LineManagerId > 0)
            {
                paramDict.Add("@LineManagerId", userObj.LineManagerId);
            }
            else
            {
                paramDict.Add("@LineManagerId", DBNull.Value);
            }

            var ds = await this.dbHelper.GetDataSetByStoredProcedure("[Auth].[CreateUser]", paramDict);
            return ds;
        }
    }
}
