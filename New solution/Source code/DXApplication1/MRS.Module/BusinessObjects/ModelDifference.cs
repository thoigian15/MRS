using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.DC;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Objects.DataClasses;

namespace MRS.Module.BusinessObjects {
	[ImageName("ModelEditor_ModelMerge")]
	[MetadataType(typeof(ModelDifferenceMetadata))]
	[RuleCombinationOfPropertiesIsUnique(null, DefaultContexts.Save, "UserId, ContextId")]
	public partial class ModelDifference : IModelDifference, IObjectSpaceLink {
		private String userName;
		private IObjectSpace objectSpace;

		public String UserName {
			get {
				if (String.IsNullOrEmpty(UserId)) {
					userName = ModelDifferenceDbStore.SharedModelDifferenceName;
				}
				else if (String.IsNullOrEmpty(userName)) {
					List<DataViewExpression> expressions = new List<DataViewExpression>();
					expressions.Add(new DataViewExpression("A", ModelDifferenceDbStore.UserNamePropertyName));
					IList dataView = objectSpace.CreateDataView(ModelDifferenceDbStore.UserTypeInfo.Type, expressions,
						new BinaryOperator(
							ModelDifferenceDbStore.UserTypeInfo.KeyMember.Name,
							ModelDifferenceDbStore.UserIdTypeConverter.ConvertFromInvariantString(UserId)), null);
					if (dataView.Count > 0) {
						Object val = ((XafDataViewRecord)dataView[0])["A"];
						if ((val != null) && (val != DBNull.Value)) {
							userName = val.ToString();
						}
					}
				}
				return userName;
			}
		}
		// IDatabaseModelDifference
		IList<IModelDifferenceAspect> IModelDifference.Aspects {
			get { return Aspects.ToList<IModelDifferenceAspect>(); }
		}

		// IObjectSpaceLink
		IObjectSpace IObjectSpaceLink.ObjectSpace {
			get { return objectSpace; }
			set { objectSpace = value; }
		}
	}

	public class ModelDifferenceMetadata {
		[Browsable(false)]
		public Int32 ID { get; set; }

		[Browsable(false)]
		[RuleUniqueValue(null, DefaultContexts.Save)]
		[ModelDefault("AllowEdit", "False")]
		public String UserId { get; set; }

		[Browsable(false)]
		public Int32 Version { get; set; }

		[InverseProperty("Owner"), Aggregated]
        public EntityCollection<ModelDifferenceAspect> Aspects { get; set; }
	}
}
