using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp;

namespace MRS.Module.BusinessObjects {
    [MetadataType(typeof(TypePermissionObjectMetadata))]
    [ImageName("BO_Security_Permission_Type")]
    public partial class TypePermissionObject : ITypePermissionOperations, ICheckedListBoxItemsProvider {
        private Type targetType;
        public IEnumerable<IOperationPermission> GetPermissions() {
            List<IOperationPermission> result = new List<IOperationPermission>();
            if(TargetType != null) {
                if(AllowRead) {
                    result.Add(new TypeOperationPermission(TargetType, SecurityOperations.Read));
                }
                if(AllowWrite) {
                    result.Add(new TypeOperationPermission(TargetType, SecurityOperations.Write));
                }
                if(AllowCreate) {
                    result.Add(new TypeOperationPermission(TargetType, SecurityOperations.Create));
                }
                if(AllowDelete) {
                    result.Add(new TypeOperationPermission(TargetType, SecurityOperations.Delete));
                }
                if(AllowNavigate) {
                    result.Add(new TypeOperationPermission(TargetType, SecurityOperations.Navigate));
                }
                foreach(SecuritySystemMemberPermissionsObject memberPermissionObject in MemberPermissions) {
                    result.AddRange(memberPermissionObject.GetPermissions());
                }
                foreach(SecuritySystemObjectPermissionsObject objectPermissionObject in ObjectPermissions) {
                    result.AddRange(objectPermissionObject.GetPermissions());
                }
            }
            return result;
        }
        [ImmediatePostData]
        [RuleRequiredField]
        public Type TargetType {
            get {
                if((targetType == null) && !String.IsNullOrWhiteSpace(TargetTypeFullName)) {
                    targetType = ReflectionHelper.FindType(TargetTypeFullName);
                }
                return targetType;
            }
            set {
                targetType = value;
                if(targetType != null) {
                    TargetTypeFullName = targetType.FullName;
                }
                else {
                    TargetTypeFullName = "";
                }
                OnItemsChanged();
            }
        }
        public string Object {
            get {
                if(TargetType != null) {
                    string classCaption = CaptionHelper.GetClassCaption(TargetType.FullName);
                    return string.IsNullOrEmpty(classCaption) ? TargetType.Name : classCaption;
                }
                return string.Empty;
            }
        }
        Dictionary<Object, String> ICheckedListBoxItemsProvider.GetCheckedListBoxItems(string targetMemberName) {
            Dictionary<Object, String> result = new Dictionary<Object, String>();
            if(targetMemberName == "Members" && TargetType != null) {
                ITypeInfo typeInfo = XafTypesInfo.Instance.FindTypeInfo(TargetType);
                foreach(IMemberInfo memberInfo in typeInfo.Members) {
                    if(memberInfo.IsVisible) {
                        result.Add(memberInfo.Name, CaptionHelper.GetMemberCaption(typeInfo, memberInfo.Name));
                    }
                }
            }
            return result;
        }
        protected virtual void OnItemsChanged() {
            if(ItemsChanged != null) {
                ItemsChanged(this, new EventArgs());
            }
        }
        public event EventHandler ItemsChanged;
    }

    public class TypePermissionObjectMetadata {
        [Browsable(false)]
        public Int32 ID { get; set; }
        [Browsable(false)]
        public String TargetTypeFullName { get; set; }
        [Aggregated]
        public ICollection<SecuritySystemMemberPermissionsObject> MemberPermissions { get; set; }
        [Aggregated]
        public ICollection<SecuritySystemObjectPermissionsObject> ObjectPermissions { get; set; }
        [DisplayName("Read")]
        public bool AllowRead { get; set; }
        [DisplayName("Write")]
        public bool AllowWrite { get; set; }
        [DisplayName("Create")]
        public bool AllowCreate { get; set; }
        [DisplayName("Delete")]
        public bool AllowDelete { get; set; }
        [DisplayName("Navigate")]
        public bool AllowNavigate { get; set; }
    }
}
