Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Xpo
Imports System.Collections
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.DC
Imports DevExpress.Data.Filtering
Imports DevExpress.ExpressApp.Model
Imports DevExpress.XtraVerticalGrid
Imports DevExpress.ExpressApp.Editors
Imports DevExpress.XtraVerticalGrid.Rows
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.ExpressApp.Win.Editors

Namespace WinSolution.Module.Win
	Public Interface IModelVGridControlViewItem
	Inherits IModelViewItem
	End Interface
	<DetailViewItemAttribute(GetType(IModelVGridControlViewItem))> _
	Public Class VGridControlViewItem
		Inherits ViewItem
		Implements IComplexPropertyEditor
		Public Sub New(ByVal model As IModelVGridControlViewItem, ByVal classType As Type)
			MyBase.New(classType, model.Id)
		End Sub
		Private vGridControlCore As VGridControl
		Public ReadOnly Property VGridControl() As VGridControl
			Get
				Return vGridControlCore
			End Get
		End Property
		Private applicationCore As XafApplication
		Public ReadOnly Property Application() As XafApplication
			Get
				Return applicationCore
			End Get
		End Property
		Protected Overrides Function CreateControlCore() As Object
			vGridControlCore = New VGridControl()
			vGridControlCore.DataSource = GetDataSource()
			Dim factory As New RepositoryEditorsFactory(Application, View.ObjectSpace)
			Dim dw As IModelDetailView = CType(View.Model, IModelDetailView)
			For Each item As IModelViewItem In dw.Items
				Dim modelPropertyEditor As IModelPropertyEditor = TryCast(item, IModelPropertyEditor)
				If modelPropertyEditor IsNot Nothing Then
					Dim isGranted As Boolean = DataManipulationRight.CanRead(ObjectType, modelPropertyEditor.PropertyName, View.CurrentObject, Nothing)
					Dim repositoryItem As RepositoryItem = factory.CreateRepositoryItem((Not isGranted), modelPropertyEditor, ObjectType)
					If repositoryItem IsNot Nothing Then
						Dim mi As IMemberInfo = ObjectTypeInfo.FindMember(modelPropertyEditor.PropertyName)
						If mi IsNot Nothing Then
							vGridControlCore.RepositoryItems.Add(repositoryItem)
							Dim row As New EditorRow(mi.BindingName)
							row.Properties.Caption = (CType(modelPropertyEditor, IModelCommonMemberViewItem)).Caption
							row.Properties.RowEdit = repositoryItem
							vGridControlCore.Rows.Add(row)
						End If
					End If
				End If
			Next item
			AddHandler vGridControlCore.CellValueChanged, AddressOf OnVGridControlCellValueChanged
			AddHandler View.CurrentObjectChanged, AddressOf OnViewCurrentObjectChanged
			AddHandler View.ObjectSpace.Reloaded, AddressOf OnObjectSpaceReloaded
			Return vGridControlCore
		End Function
		Protected Overrides Overloads Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (vGridControlCore IsNot Nothing) AndAlso (View IsNot Nothing) Then
				RemoveHandler View.CurrentObjectChanged, AddressOf OnViewCurrentObjectChanged
				RemoveHandler vGridControlCore.CellValueChanged, AddressOf OnVGridControlCellValueChanged
				RemoveHandler View.ObjectSpace.Reloaded, AddressOf OnObjectSpaceReloaded
			End If
			MyBase.Dispose(disposing)
		End Sub
		Private Function GetDataSource() As IList
			Dim ds As New XPCollection((CType(View.ObjectSpace, ObjectSpace)).Session, ObjectType, False)
			ds.Add(View.CurrentObject)
			Return ds
		End Function
		Private Sub OnViewCurrentObjectChanged(ByVal sender As Object, ByVal e As EventArgs)
			VGridControl.DataSource = GetDataSource()
		End Sub
		Private Sub OnObjectSpaceReloaded(ByVal sender As Object, ByVal e As EventArgs)
			VGridControl.DataSource = GetDataSource()
		End Sub
		Private Sub OnVGridControlCellValueChanged(ByVal sender As Object, ByVal e As DevExpress.XtraVerticalGrid.Events.CellValueChangedEventArgs)
			VGridControl.BindingContext(View.CurrentObject).EndCurrentEdit()
		End Sub
		#Region "IComplexPropertyEditor Members"
		Public Sub Setup(ByVal objectSpace As IObjectSpace, ByVal application As XafApplication) Implements IComplexPropertyEditor.Setup
			applicationCore = application
		End Sub
		#End Region
	End Class
End Namespace