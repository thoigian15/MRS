using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using DevExpress.Persistent.Validation;
using DevExpress.Persistent.Base.General;

namespace MRS.Module.BusinessObjects {
    [MetadataType(typeof(HCategoryMetadata))]
    public partial class HCategory : IHCategory {
        [RuleFromBoolProperty("HCategoryCircularReferences", DefaultContexts.Save, "Circular refrerence detected. To correct this error, set the Parent property to another value.", UsedProperties = "Parent")]
        [Browsable(false)]
        public Boolean IsValid {
            get {
                HCategory currentObj = Parent;
                while(currentObj != null) {
                    if(currentObj == this) {
                        return false;
                    }
                    currentObj = currentObj.Parent;
                }
                return true;
            }
        }

        IBindingList ITreeNode.Children {
            get { return ((IListSource)Children).GetList() as IBindingList; }
        }
        ITreeNode IHCategory.Parent {
            get { return Parent as IHCategory; }
            set { Parent = value as HCategory; }
        }
        ITreeNode ITreeNode.Parent {
            get { return Parent as ITreeNode; }
        }
    }

    public class HCategoryMetadata {
        [Browsable(false)]
        public Int32 ID { get; set; }
    }
}
