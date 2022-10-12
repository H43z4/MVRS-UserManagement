using Database;
using Models.ViewModels.Identity;
using SharedLib.Interfaces;

namespace UserManagement.Services
{
    public interface IUserManagementService : ICurrentUser
    {
    }

    public class UserManagementService : IUserManagementService
    {
        readonly IDBHelper dbHelper;
        public VwUser VwUser { get; set; }
        public UserManagementService(IDBHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }
    }
}
