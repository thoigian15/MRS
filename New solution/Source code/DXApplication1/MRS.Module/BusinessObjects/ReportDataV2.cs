using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.XtraReports.UI;

namespace MRS.Module.BusinessObjects {
    [MetadataType(typeof(ReportDataMetadata))]
    [PredefinedReportTypeMemberName("PredefinedReportTypeName")]
    [DefaultProperty("DisplayName")]
    public partial class ReportDataV2 : IReportDataV2Writable, IInplaceReportV2 {
        Type IReportDataV2.DataType {
            get {
                if(!string.IsNullOrEmpty(DataTypeName)) {
                    ITypeInfo typeInfo = XafTypesInfo.Instance.FindTypeInfo(DataTypeName);
                    if(typeInfo != null) {
                        return typeInfo.Type;
                    }
                }
                return null;
            }
        }
        [DisplayName("Data Type")]
        public string DataTypeCaption {
            get { return CaptionHelper.GetClassCaption(DataTypeName); }
        }
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public bool IsPredefined {
            get { return PredefinedReportType != null; }
        }
        [SettingsBindable(true)]
        [VisibleInListView(false)]
        [TypeConverter(typeof(ReportParametersObjectTypeConverter))]
        [Localizable(true)]
        public Type ParametersObjectType {
            get {
                if(!string.IsNullOrEmpty(ParametersObjectTypeName)) {
                    ITypeInfo typeInfo = XafTypesInfo.Instance.FindTypeInfo(ParametersObjectTypeName);
                    if(typeInfo != null) {
                        return typeInfo.Type;
                    }
                }
                return null;
            }
            set {
                ((IReportDataV2Writable)this).SetParametersObjectType(value);
            }
        }
        [Browsable(false)]
        public Type PredefinedReportType {
            get {
                if(!string.IsNullOrEmpty(PredefinedReportTypeName)) {
                    return ReflectionHelper.FindType(PredefinedReportTypeName);
                }
                return null;
            }
            set {
                PredefinedReportTypeName = value != null ? value.FullName : null;
            }
        }
        #region IReportDataV2Writable
        void IReportDataV2Writable.SetContent(byte[] content) {
            Content = content;
        }
        void IReportDataV2Writable.SetPredefinedReportType(Type reportType) {
            if(reportType != null) {
                Guard.TypeArgumentIs(typeof(XtraReport), reportType, "reportType");
            }
            PredefinedReportType = reportType;
        }
        void IReportDataV2Writable.SetParametersObjectType(Type parametersObjectType) {
            if(parametersObjectType != null) {
                Guard.TypeArgumentIs(typeof(ReportParametersObjectBase), parametersObjectType, "parametersObjectType");
            }
            ParametersObjectTypeName = parametersObjectType != null ? parametersObjectType.FullName : string.Empty;
        }
        void IReportDataV2Writable.SetDataType(Type newDataType) {
            DataTypeName = newDataType != null ? newDataType.FullName : string.Empty;
        }
        void IReportDataV2Writable.SetDisplayName(string displayName) {
            DisplayName = displayName;
        }
        #endregion
    }
    public class ReportDataMetadata {
        [Browsable(false)]
        public Int32 ID { get; set; }
        [VisibleInListView(false)]
        public Boolean IsInplaceReport { get; set; }
        [Browsable(false)]
        public String DataTypeName { get; set; }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public Byte[] Content { get; set; }
        [Browsable(false)]
        public String PredefinedReportTypeName { get; set; }
        [Browsable(false)]
        public string ParametersObjectTypeName { get; set; }
    }

    public class ReportParametersObjectTypeConverter : LocalizedClassInfoTypeConverter {
        public override List<Type> GetSourceCollection(ITypeDescriptorContext context) {
            ITypeInfo reportParametersObjectBaseTypeInfo = XafTypesInfo.Instance.FindTypeInfo(typeof(ReportParametersObjectBase));
            Guard.ArgumentNotNull(reportParametersObjectBaseTypeInfo, "reportParametersObjectBaseTypeInfo");

            return new List<Type>(Enumerator.Convert<ITypeInfo, Type>(reportParametersObjectBaseTypeInfo.Descendants, XafTypesInfo.CastTypeInfoToType));
        }
    }
}
