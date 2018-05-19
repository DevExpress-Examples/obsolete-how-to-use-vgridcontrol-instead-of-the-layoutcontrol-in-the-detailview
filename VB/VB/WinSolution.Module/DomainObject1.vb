Imports Microsoft.VisualBasic
Imports System

Imports DevExpress.Xpo

Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Persistent.Validation
Imports System.ComponentModel

Namespace WinSolution.Module
	<DefaultClassOptions> _
	Public Class DomainObject1
		Inherits BaseObject
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		Private _name As String
		Public Property Name() As String
			Get
				Return _name
			End Get
			Set(ByVal value As String)
				SetPropertyValue("Name", _name, value)
			End Set
		End Property

		Private _age As Integer
		Public Property Age() As Integer
			Get
				Return _age
			End Get
			Set(ByVal value As Integer)
				SetPropertyValue("Age", _age, value)
			End Set
		End Property
		Private _description As String
		Public Property Description() As String
			Get
				Return _description
			End Get
			Set(ByVal value As String)
				SetPropertyValue("Description", _description, value)
			End Set
		End Property
		Private _ReferencedProperty As DomainObject1
		Public Property ReferencedProperty() As DomainObject1
			Get
				Return _ReferencedProperty
			End Get
			Set(ByVal value As DomainObject1)
				SetPropertyValue("ReferencedProperty", _ReferencedProperty, value)
			End Set
		End Property
	End Class
End Namespace
