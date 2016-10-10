using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.EF;
using MRS.Module.BusinessObjects;

namespace MRS.Module.DatabaseUpdate {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppUpdatingModuleUpdatertopic.aspx
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            User sampleUser = ObjectSpace.FindObject<User>(new BinaryOperator("UserName", "User"));
            if(sampleUser == null) {
                sampleUser = ObjectSpace.CreateObject<User>();
                sampleUser.UserName = "User";
                sampleUser.SetPassword("");
            }
            Role defaultRole = CreateDefaultRole();
            sampleUser.Roles.Add(defaultRole);

            User userAdmin = ObjectSpace.FindObject<User>(new BinaryOperator("UserName", "Admin"));
            if(userAdmin == null) {
                userAdmin = ObjectSpace.CreateObject<User>();
                userAdmin.UserName = "Admin";
                // Set a password if the standard authentication type is used
                userAdmin.SetPassword("");
            }
			// If a role with the Administrators name doesn't exist in the database, create this role
            Role adminRole = ObjectSpace.FindObject<Role>(new BinaryOperator("Name", "Administrators"));
            if(adminRole == null) {
               adminRole = ObjectSpace.CreateObject<Role>();
                adminRole.Name = "Administrators";
            }
            adminRole.IsAdministrative = true;
			userAdmin.Roles.Add(adminRole);
     		ObjectSpace.CommitChanges();
        }
        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
        }
        private Role CreateDefaultRole() {
            Role defaultRole = ObjectSpace.FindObject<Role>(new BinaryOperator("Name", "Default"));
            if(defaultRole == null) {
                defaultRole = ObjectSpace.CreateObject<Role>();
                defaultRole.Name = "Default";
                defaultRole.TypePermissions.Add(CreateUserPermissions());
                defaultRole.TypePermissions.Add(CreateTypePermission<Role>(true, false));
                defaultRole.TypePermissions.Add(CreateTypePermission<ModelDifference>(true, true));
                defaultRole.TypePermissions.Add(CreateTypePermission<ModelDifferenceAspect>(true, true));
            }
            return defaultRole;
        }
        private TypePermissionObject CreateUserPermissions() {
            TypePermissionObject userPermissions = CreateTypePermission<User>(false, false);
            userPermissions.ObjectPermissions.Add(CreateUserObjectPermission());
            userPermissions.MemberPermissions.Add(CreateUserMemberPermission("StoredPassword"));
            userPermissions.MemberPermissions.Add(CreateUserMemberPermission("ChangePasswordOnFirstLogon"));
            return userPermissions;
        }
        private TypePermissionObject CreateTypePermission<T>(bool allowRead, bool allowWrite) {
            TypePermissionObject typePermissions = ObjectSpace.CreateObject<TypePermissionObject>();
            typePermissions.TargetType = typeof(T);
            typePermissions.AllowWrite = allowWrite;
            typePermissions.AllowRead = allowRead;
            return typePermissions;
        }
        private SecuritySystemMemberPermissionsObject CreateUserMemberPermission(string member) {
            SecuritySystemMemberPermissionsObject memberPermission = ObjectSpace.CreateObject<SecuritySystemMemberPermissionsObject>();
            memberPermission.Members = member;
            memberPermission.AllowWrite = true;
            return memberPermission;
        }
        private SecuritySystemObjectPermissionsObject CreateUserObjectPermission() {
            SecuritySystemObjectPermissionsObject objectPermission = ObjectSpace.CreateObject<SecuritySystemObjectPermissionsObject>();
            objectPermission.Criteria = "[ID] = CurrentUserId()";
            objectPermission.AllowRead = true;
            objectPermission.AllowNavigate = true;
            return objectPermission;
        }
    }
}
