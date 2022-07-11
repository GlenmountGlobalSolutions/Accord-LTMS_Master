Imports System.Runtime.Serialization

<DataContract()> _
Public Class NameValuePair

    Private mName As String
    Private mValue As String
    Private mSelected As String

    <DataMember()>
    Property Name As String
        Get
            Return mName
        End Get
        Set(value As String)
            mName = value
        End Set
    End Property

    <DataMember()>
    Property Value As String
        Get
            Return mValue
        End Get
        Set(value As String)
            mValue = value
        End Set
    End Property

    <DataMember()>
    Property Selected As String
        Get
            Return mSelected
        End Get
        Set(value As String)
            mSelected = value
        End Set
    End Property

    Public Sub New(ByVal newName As String, ByVal newValue As String, Optional ByVal isSelected As Boolean = False)
        Me.Name = newName
        Me.Value = newValue
        If isSelected Then
            Me.Selected = "1"
        Else
            Me.Selected = "0"
        End If

    End Sub


End Class
