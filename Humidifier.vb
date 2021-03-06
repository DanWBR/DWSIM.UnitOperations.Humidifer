Public Class Humidifier

    Inherits DWSIM.SharedClasses.UnitOperations.BaseClass

    Implements DWSIM.Interfaces.IExternalUnitOperation

    Private Property UOName As String = "Humidifier"
    Private Property UODescription As String = "Humidifier Unit Operation"

    'tells DWSIM that this Unit Operation is compatible with mobile versions
    Public Overrides ReadOnly Property MobileCompatible As Boolean = False

    Private ReadOnly Property IExternalUnitOperation_Name As String Implements Interfaces.IExternalUnitOperation.Name
        Get
            Return UOName
        End Get
    End Property

    Public ReadOnly Property Description As String Implements Interfaces.IExternalUnitOperation.Description
        Get
            Return UODEscription
        End Get
    End Property

    Public ReadOnly Property Prefix As String Implements Interfaces.IExternalUnitOperation.Prefix
        Get
            Return "HMDF-"
        End Get
    End Property

    'display the editor on the classic user interface
    Public Overrides Sub DisplayEditForm()

    End Sub

    'this updates the editor window on classic ui
    Public Overrides Sub UpdateEditForm()

    End Sub

    'this closes the editor on classic ui
    Public Overrides Sub CloseEditForm()

    End Sub

    Public Overrides Function GetDisplayName() As String
        Return UOName
    End Function

    Public Overrides Function GetDisplayDescription() As String
        Return UODEscription
    End Function

    Public Overrides Function GetIconBitmap() As Object
        Return My.Resources.humidifier_32
    End Function

    'returns a new instance of humidifier, using XML cloning
    Public Overrides Function CloneXML() As Object

        Dim objdata = XMLSerializer.XMLSerializer.Serialize(Me)
        Dim newhumidifier As New Humidifer.Humidifier()
        newhumidifier.LoadData(objdata)

        Return newhumidifier

    End Function

    'returns a new instance of humidifer, using JSON cloning
    Public Overrides Function CloneJSON() As Object

        Dim jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(Me)
        Dim newhumidifier = Newtonsoft.Json.JsonConvert.DeserializeObject(Of Humidifer.Humidifier)(jsonstring)

        Return newhumidifier

    End Function

    'returns a new instance of this unit operation
    Public Function ReturnInstance(typename As String) As Object Implements Interfaces.IExternalUnitOperation.ReturnInstance
        Return New Humidifier()
    End Function



    'this function draws the object on the flowsheet
    Public Sub Draw(g As Object) Implements Interfaces.IExternalUnitOperation.Draw

    End Sub


    'this function creates the connection ports in the flowsheet object
    Public Sub CreateConnectors() Implements Interfaces.IExternalUnitOperation.CreateConnectors

    End Sub


    'this function display the properties on the croos-platfom user interface
    Public Sub PopulateEditorPanel(container As Object) Implements Interfaces.IExternalUnitOperation.PopulateEditorPanel

    End Sub


End Class
