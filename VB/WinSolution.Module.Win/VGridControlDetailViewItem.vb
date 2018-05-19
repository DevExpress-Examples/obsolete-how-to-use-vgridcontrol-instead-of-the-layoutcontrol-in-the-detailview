Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.ExpressApp.Editors
Imports DevExpress.ExpressApp
Imports DevExpress.XtraVerticalGrid
Imports DevExpress.Data.Filtering
Imports DevExpress.Xpo
Imports DevExpress.ExpressApp.NodeWrappers
Imports System.Collections
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.ExpressApp.Win.Editors
Imports DevExpress.XtraVerticalGrid.Rows
Imports DevExpress.ExpressApp.DC

Namespace WinSolution.Module.Win
	<DetailViewItemName("VGridControlDetailViewItem")> _
	Public Class VGridControlDetailViewItem
		Inherits DetailViewItem
		Implements IComplexPropertyEditor
		Public Sub New(ByVal classType As Type, ByVal info As DictionaryNode)
			MyBase.New(info, classType)
		End Sub
		Private vGridControlCore As VGridControl = Nothing
		Public ReadOnly Property VGridControl() As VGridControl
			Get
				Return vGridControlCore
			End Get
		End Property
		Private applicationCore As XafApplication = Nothing
		Public ReadOnly Property Application() As XafApplication
			Get
				Return applicationCore
			End Get
		End Property
		Protected Overrides Function CreateControlCore() As Object
			vGridControlCore = New VGridControl()
			vGridControlCore.DataSource = GetDataSource()
			Dim factory As New RepositoryEditorsFactory(Application, View.ObjectSpace)
			Dim dw As New DetailViewInfoNodeWrapper(View.Info)
			For Each item As DetailViewItemInfoNodeWrapper In dw.Editors.Items
				Dim isGranted As Boolean = DataManipulationRight.CanRead(ObjectType, item.PropertyName, View.CurrentObject, Nothing)
				Dim repositoryItem As RepositoryItem = factory.CreateRepositoryItem((Not isGranted), item, ObjectType)
				If repositoryItem IsNot Nothing Then
					Dim mi As IMemberInfo = ObjectTypeInfo.FindMember(item.PropertyName)
					If mi IsNot Nothing Then
						vGridControlCore.RepositoryItems.Add(repositoryItem)
						Dim row As New EditorRow(mi.BindingName)
						row.Properties.Caption = item.Caption
						row.Properties.RowEdit = repositoryItem
						vGridControlCore.Rows.Add(row)
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
		Private Function GetCriteria() As CriteriaOperator
			If View.CurrentObject IsNot Nothing Then
				Return CriteriaOperator.Parse(String.Format("{0}=?", View.ObjectSpace.GetKeyPropertyName(ObjectType)), View.ObjectSpace.GetKeyValue(View.CurrentObject))
			Else
				Return Nothing
			End If
		End Function
		Private Function GetDataSource() As IList
			Dim ds As New XPCollection(View.ObjectSpace.Session, ObjectType, False)
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
		Public Sub Setup(ByVal objectSpace As ObjectSpace, ByVal application As XafApplication) Implements IComplexPropertyEditor.Setup
			applicationCore = application
		End Sub
		#End Region
	End Class
End Namespace