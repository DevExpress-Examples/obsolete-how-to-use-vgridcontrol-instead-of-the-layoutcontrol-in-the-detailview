using System;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp;
using DevExpress.XtraVerticalGrid;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using DevExpress.ExpressApp.NodeWrappers;
using System.Collections;
using DevExpress.XtraEditors.Repository;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraVerticalGrid.Rows;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;

namespace WinSolution.Module.Win {
	public interface IModelVGridControlDetailViewItem : IModelDetailViewItem {
	}
	[DetailViewItemAttribute(typeof(IModelVGridControlDetailViewItem))]
    public class VGridControlDetailViewItem : ViewItem, IComplexPropertyEditor {
		public VGridControlDetailViewItem(IModelVGridControlDetailViewItem model, Type classType) : base(classType, model.Id) { }
        private VGridControl vGridControlCore = null;
        public VGridControl VGridControl {
            get {
                return vGridControlCore;
            }
        }
        private XafApplication applicationCore = null;
        public XafApplication Application {
            get {
                return applicationCore;
            }
        }
        protected override object CreateControlCore() {
            vGridControlCore = new VGridControl();
            vGridControlCore.DataSource = GetDataSource();
            RepositoryEditorsFactory factory = new RepositoryEditorsFactory(Application, View.ObjectSpace);
			IModelDetailView dw = (IModelDetailView)View.Model;
            foreach (IModelDetailViewItem item in dw.Items) {
				IModelPropertyEditor modelPropertyEditor = item as IModelPropertyEditor;
				if(modelPropertyEditor != null) {
					bool isGranted = DataManipulationRight.CanRead(ObjectType, modelPropertyEditor.PropertyName, View.CurrentObject, null);
					RepositoryItem repositoryItem = factory.CreateRepositoryItem(!isGranted, modelPropertyEditor, ObjectType);
					if(repositoryItem != null) {
						IMemberInfo mi = ObjectTypeInfo.FindMember(modelPropertyEditor.PropertyName);
						if(mi != null) {
							vGridControlCore.RepositoryItems.Add(repositoryItem);
							EditorRow row = new EditorRow(mi.BindingName);
							row.Properties.Caption = item.Caption;
							row.Properties.RowEdit = repositoryItem;
							vGridControlCore.Rows.Add(row);
						}
					}
				}
            }
            vGridControlCore.CellValueChanged += OnVGridControlCellValueChanged;
            View.CurrentObjectChanged += OnViewCurrentObjectChanged;
            View.ObjectSpace.Reloaded += OnObjectSpaceReloaded;
            return vGridControlCore;
        }
        protected override void Dispose(bool disposing) {
            if (disposing && (vGridControlCore != null) && (View != null)) {
                View.CurrentObjectChanged -= OnViewCurrentObjectChanged;
                vGridControlCore.CellValueChanged -= OnVGridControlCellValueChanged;
                View.ObjectSpace.Reloaded -= OnObjectSpaceReloaded;
            }
            base.Dispose(disposing);
        }
        private CriteriaOperator GetCriteria() {
            if (View.CurrentObject != null) {
                return CriteriaOperator.Parse(string.Format("{0}=?", View.ObjectSpace.GetKeyPropertyName(ObjectType)), View.ObjectSpace.GetKeyValue(View.CurrentObject));
            } else {
                return null;
            }
        }
        private IList GetDataSource() {
            XPCollection ds = new XPCollection(View.ObjectSpace.Session, ObjectType, false);
            ds.Add(View.CurrentObject);
            return ds;
        }
        private void OnViewCurrentObjectChanged(object sender, EventArgs e) {
            VGridControl.DataSource = GetDataSource();
        }
        private void OnObjectSpaceReloaded(object sender, EventArgs e) {
            VGridControl.DataSource = GetDataSource();
        }
        private void OnVGridControlCellValueChanged(object sender, DevExpress.XtraVerticalGrid.Events.CellValueChangedEventArgs e) {
            VGridControl.BindingContext[View.CurrentObject].EndCurrentEdit();
        }
        #region IComplexPropertyEditor Members
        public void Setup(ObjectSpace objectSpace, XafApplication application) {
            applicationCore = application;
        }
        #endregion
    }
}