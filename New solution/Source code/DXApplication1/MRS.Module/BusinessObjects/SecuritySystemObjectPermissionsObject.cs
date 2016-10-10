using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;

namespace MRS.Module.BusinessObjects {
    [System.ComponentModel.DisplayName("Object Operation Permissions")]
    [ImageName("BO_Security_Permission_Object")]
    [DefaultListViewOptions(true, NewItemRowPosition.Top)]
    [MetadataType(typeof(ObjectPermissionsObjectMetadata))]
    public partial class SecuritySystemObjectPermissionsObject : IOwnerInitializer {
        public IList<IOperationPermission> GetPermissions() {
            IList<IOperationPermission> result = new List<IOperationPermission>();
            if(Owner == null) {
                Tracing.Tracer.LogWarning("Cannot create OperationPermission objects: Owner property returns null. {0} class, {1} Id", GetType(), ID);
            }
            else if(Owner.TargetType == null) {
                Tracing.Tracer.LogWarning("Cannot create OperationPermission objects: Owner.TargetType property returns null. {0} class, {1} Id", GetType(), ID);
            }
            else {
                if(AllowRead) {
                    result.Add(new ObjectOperationPermission(Owner.TargetType, Criteria, SecurityOperations.Read));
                }
                if(AllowWrite) {
                    result.Add(new ObjectOperationPermission(Owner.TargetType, Criteria, SecurityOperations.Write));
                }
                if(AllowDelete) {
                    result.Add(new ObjectOperationPermission(Owner.TargetType, Criteria, SecurityOperations.Delete));
                }
                if(AllowNavigate) {
                    result.Add(new ObjectOperationPermission(Owner.TargetType, Criteria, SecurityOperations.Navigate));
                }
            }
            return result;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DisplayName("Read")]
        public bool? EffectiveRead {
            get { return AllowRead ? true : (Owner != null && Owner.AllowRead) ? (bool?)null : false; }
            set { AllowRead = value.HasValue ? value.Value : false; }
        }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DisplayName("Write")]
        //This property is introduced to show 'Owner' setitngs in UI. In code, use AllowWrite instead.
        public bool? EffectiveWrite {
            get { return AllowWrite ? true : (Owner != null && Owner.AllowWrite) ? (bool?)null : false; }
            set { AllowWrite = value.HasValue ? value.Value : false; }
        }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DisplayName("Delete")]
        //This property is introduced to show 'Owner' setitngs in UI. In code, use AllowDelete instead.
        public bool? EffectiveDelete {
            get { return AllowDelete ? true : (Owner != null && Owner.AllowDelete) ? (bool?)null : false; }
            set { AllowDelete = value.HasValue ? value.Value : false; }
        }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DisplayName("Navigate")]
        //This property is introduced to show 'Owner' setitngs in UI. In code, use AllowNavigate instead.
        public bool? EffectiveNavigate {
            get { return AllowNavigate ? true : (Owner != null && Owner.AllowNavigate) ? (bool?)null : false; }
            set { AllowNavigate = value.HasValue ? value.Value : false; }
        }

        public string InheritedFrom {
            get {
                string result = "";
                if(Owner != null) {
                    if(Owner.AllowRead) {
                        result = string.Concat(result, string.Format(CaptionHelper.GetLocalizedText("Messages", "Read") + CaptionHelper.GetLocalizedText("Messages", "IsInheritedFrom"), CaptionHelper.GetClassCaption(Owner.TargetType.FullName)));
                    }
                    if(Owner.AllowWrite) {
                        result = string.Concat(result, string.Format(CaptionHelper.GetLocalizedText("Messages", "Write") + CaptionHelper.GetLocalizedText("Messages", "IsInheritedFrom"), CaptionHelper.GetClassCaption(Owner.TargetType.FullName)));
                    }
                    if(Owner.AllowDelete) {
                        result = string.Concat(result, string.Format(CaptionHelper.GetLocalizedText("Messages", "Delete") + CaptionHelper.GetLocalizedText("Messages", "IsInheritedFrom"), CaptionHelper.GetClassCaption(Owner.TargetType.FullName)));
                    }
                    if(Owner.AllowNavigate) {
                        result = string.Concat(result, string.Format(CaptionHelper.GetLocalizedText("Messages", "Navigate") + CaptionHelper.GetLocalizedText("Messages", "IsInheritedFrom"), CaptionHelper.GetClassCaption(Owner.TargetType.FullName)));
                    }
                }
                return result;
            }
        }

        #region IOwnerInitializer Members
        void IOwnerInitializer.SetMasterObject(object masterObject) {
            TypePermissionObject typePermission = masterObject as TypePermissionObject;
            if(typePermission != null) {
                Owner = typePermission;
            }
        }
        #endregion
    }
    public class ObjectPermissionsObjectMetadata {
        [Browsable(false)]
        public Int32 ID { get; set; }
        [FieldSize(FieldSizeAttribute.Unlimited)]
        [CriteriaOptions("Owner.TargetType")]
        [VisibleInListView(true)]
        [EditorAlias(EditorAliases.PopupCriteriaPropertyEditor)]
        public string Criteria { get; set; }
        [VisibleInListView(false), VisibleInDetailView(false)]
        public bool AllowRead { get; set; }
        [VisibleInListView(false), VisibleInDetailView(false)]
        public bool AllowWrite { get; set; }
        [VisibleInListView(false), VisibleInDetailView(false)]
        public bool AllowDelete { get; set; }
        [VisibleInListView(false), VisibleInDetailView(false)]
        public bool AllowNavigate { get; set; }
        [VisibleInListView(false), VisibleInDetailView(false)]
        public TypePermissionObject Owner { get; set; }
    }
}
