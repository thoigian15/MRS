using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;

using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base.Security;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Utils;

namespace MRS.Module.BusinessObjects {
    [MetadataType(typeof(UserMetadata))]
    [ImageName("BO_User"), System.ComponentModel.DefaultProperty("UserName")]
    public partial class User : ISecurityUser, IAuthenticationActiveDirectoryUser, IAuthenticationStandardUser, IOperationPermissionProvider, ISecurityUserWithRoles {
        // ISecurityUser
        Boolean ISecurityUser.IsActive {
            get { return IsActive; }
        }
        String ISecurityUser.UserName {
            get { return UserName; }
        }

        // IAuthenticationActiveDirectoryUser
        String IAuthenticationActiveDirectoryUser.UserName {
            get { return UserName; }
            set { UserName = value; }
        }
        // IAuthenticationStandardUser
        Boolean IAuthenticationStandardUser.ComparePassword(String password) {
            PasswordCryptographer passwordCryptographer = new PasswordCryptographer();
            return passwordCryptographer.AreEqual(StoredPassword, password);
        }
        public void SetPassword(String password) {
            PasswordCryptographer passwordCryptographer = new PasswordCryptographer();
            StoredPassword = passwordCryptographer.GenerateSaltedPassword(password);
        }
        Boolean IAuthenticationStandardUser.ChangePasswordOnFirstLogon {
            get { return ChangePasswordOnFirstLogon; }
            set { ChangePasswordOnFirstLogon = value; }
        }
        String IAuthenticationStandardUser.UserName {
            get { return UserName; }
        }

        // IOperationPermissionProvider
        IEnumerable<IOperationPermissionProvider> IOperationPermissionProvider.GetChildren() {
            return new EnumerableConverter<IOperationPermissionProvider, Role>(Roles);
        }
        IEnumerable<IOperationPermission> IOperationPermissionProvider.GetPermissions() {
            return new IOperationPermission[0];
        }

        // ISecurityUserWithRoles
        IList<ISecurityRole> ISecurityUserWithRoles.Roles {
            get {
                IList<ISecurityRole> result = new List<ISecurityRole>();
                foreach(Role role in Roles) {
                    result.Add(role);
                }
                return new ReadOnlyCollection<ISecurityRole>(result);
            }
        }
    }

    public class UserMetadata {
        [Browsable(false)]
        public Int32 ID { get; set; }
        [Browsable(false), SecurityBrowsable]
        protected String StoredPassword { get; set; }
    }
}
