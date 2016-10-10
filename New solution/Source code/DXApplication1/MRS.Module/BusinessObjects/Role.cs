using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Data.Entity.Core.Objects.DataClasses;
using System.ComponentModel.DataAnnotations;

using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;

namespace MRS.Module.BusinessObjects {
    [MetadataType(typeof(RoleMetadata))]
    [ImageName("BO_Role")]
	public partial class Role : ISecurityRole, IOperationPermissionProvider {
		// ISecurityRole
		String ISecurityRole.Name {
			get { return Name; }
		}
		// IOperationPermissionProvider
		IEnumerable<IOperationPermissionProvider> IOperationPermissionProvider.GetChildren() {
			List<IOperationPermissionProvider> result = new List<IOperationPermissionProvider>();
			result.AddRange(new EnumerableConverter<IOperationPermissionProvider, Role>(ChildRoles));
			return result;
		}
		IEnumerable<IOperationPermission> IOperationPermissionProvider.GetPermissions() {
			List<IOperationPermission> result = new List<IOperationPermission>();
			foreach(TypePermissionObject persistentPermission in TypePermissions) {
				result.AddRange(persistentPermission.GetPermissions());
			}
			if(IsAdministrative) {
				result.Add(new IsAdministratorPermission());
			}
			if(CanEditModel) {
				result.Add(new ModelOperationPermission());
			}
			return result;
		}
	}

    public class RoleMetadata {
        [Browsable(false)]
        public Int32 ID { get; set; }
		[Aggregated]
		public EntityCollection<TypePermissionObject> TypePermissions { get; set; }
    }
}
