using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Utils;

namespace MRS.Module.BusinessObjects {
	[ImageName("ModelEditor_ModelMerge")]
	[DefaultProperty("DispayName")]
	[ModelDefault("Caption", "User Settings Aspect")]
	[MetadataType(typeof(ModelDifferenceAspectMetadata))]
	public partial class ModelDifferenceAspect : IModelDifferenceAspect {
		// IDatabaseModelDifferenceAspect
		IModelDifference IModelDifferenceAspect.Owner {
			get { return Owner; }
			set { Owner = value as ModelDifference; }
		}
		public String DisplayName {
            get {
                return string.IsNullOrEmpty(Name) ? CaptionHelper.GetLocalizedText("Texts", "DefaultAspectText", "(Default language)") : Name;
            }
        }
	}

	public class ModelDifferenceAspectMetadata {
		[Browsable(false)]
		public Int32 ID { get; set; }

		[FieldSize(FieldSizeAttribute.Unlimited)]
		public String Xml { get; set; }

		[VisibleInListView(false), VisibleInLookupListView(false)]
        public String Name { get; set; }

		[RuleRequiredField(null, DefaultContexts.Save)]
		public ModelDifference Owner { get; set; }
	}
}
