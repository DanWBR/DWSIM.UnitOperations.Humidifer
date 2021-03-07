Imports DWSIM.Interfaces.Enums
Imports SkiaSharp.Views.Desktop.Extensions
Imports DWSIM.UI.Shared

''' <summary>
''' Simple Humidifer Model.
''' </summary>
<System.Serializable()> Public Class Humidifier

    Inherits DWSIM.UnitOperations.UnitOperations.UnitOpBaseClass

    Implements DWSIM.Interfaces.IExternalUnitOperation

    Private Property UOName As String = "Humidifier"
    Private Property UODescription As String = "Humidifier Unit Operation"

    Public Overrides Property ComponentName As String = UOName

    Public Overrides Property ComponentDescription As String = UODescription

    Public Overrides Property ObjectClass As SimulationObjectClass = SimulationObjectClass.MixersSplitters

    'proeprty to define the calculation mode (Adiabatic or Isothermic)

    Public Property IsAdiabatic As Boolean = True

    'tells DWSIM that this Unit Operation is or isn't compatible with mobile versions

    Public Overrides ReadOnly Property MobileCompatible As Boolean = False

    Private ReadOnly Property IExternalUnitOperation_Name As String Implements Interfaces.IExternalUnitOperation.Name
        Get
            Return UOName
        End Get
    End Property

    Public ReadOnly Property Description As String Implements Interfaces.IExternalUnitOperation.Description
        Get
            Return UODescription
        End Get
    End Property

    Public ReadOnly Property Prefix As String Implements Interfaces.IExternalUnitOperation.Prefix
        Get
            Return "HMDF-"
        End Get
    End Property

    Public Sub New(ByVal Name As String, ByVal Description As String)

        MyBase.CreateNew()
        Me.ComponentName = Name
        Me.ComponentDescription = Description

    End Sub

    Public Sub New()

        MyBase.New()

    End Sub

    <NonSerialized> <Xml.Serialization.XmlIgnore> Private editwindow As Editor

    'display the editor on the classic user interface
    Public Overrides Sub DisplayEditForm()

        If editwindow Is Nothing Then

            editwindow = New Editor() With {.HObject = Me}

        Else

            If editwindow.IsDisposed Then
                editwindow = New Editor() With {.HObject = Me}
            End If

        End If

        FlowSheet.DisplayForm(editwindow)

    End Sub

    'this updates the editor window on classic ui
    Public Overrides Sub UpdateEditForm()

        If editwindow IsNot Nothing Then

            If editwindow.InvokeRequired Then

                editwindow.Invoke(Sub()
                                      editwindow?.UpdateInfo()
                                  End Sub)

            Else

                editwindow?.UpdateInfo()

            End If

        End If

    End Sub

    'this closes the editor on classic ui
    Public Overrides Sub CloseEditForm()

        editwindow?.Close()

    End Sub

    Public Overrides Function GetDisplayName() As String
        Return UOName
    End Function

    Public Overrides Function GetDisplayDescription() As String
        Return UODescription
    End Function

    Public Overrides Function GetIconBitmap() As Object
        Return My.Resources.humidifier_32
    End Function

    'returns a new instance of humidifier, using XML cloning
    Public Overrides Function CloneXML() As Object

        Dim objdata = XMLSerializer.XMLSerializer.Serialize(Me)
        Dim newhumidifier As New Humidifier()
        newhumidifier.LoadData(objdata)

        Return newhumidifier

    End Function

    'returns a new instance of humidifer, using JSON cloning
    Public Overrides Function CloneJSON() As Object

        Dim jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(Me)
        Dim newhumidifier = Newtonsoft.Json.JsonConvert.DeserializeObject(Of Humidifier)(jsonstring)

        Return newhumidifier

    End Function

    'returns a new instance of this unit operation
    Public Function ReturnInstance(typename As String) As Object Implements Interfaces.IExternalUnitOperation.ReturnInstance

        Return New Humidifier()

    End Function

    Private Image As SkiaSharp.SKImage

    'this function draws the object on the flowsheet
    Public Sub Draw(g As Object) Implements Interfaces.IExternalUnitOperation.Draw

        'get the canvas object
        Dim canvas = DirectCast(g, SkiaSharp.SKCanvas)

        'load the icon image on memory
        If Image Is Nothing Then

            Using bitmap = My.Resources.humidifier_512.ToSKBitmap()
                Image = SkiaSharp.SKImage.FromBitmap(bitmap)
            End Using

        End If

        Dim x = Me.GraphicObject.X
        Dim y = Me.GraphicObject.Y
        Dim w = Me.GraphicObject.Width
        Dim h = Me.GraphicObject.Height

        'draw the image into the flowsheet inside the object's reserved rectangle area
        Using p As New SkiaSharp.SKPaint With {.FilterQuality = SkiaSharp.SKFilterQuality.High}
            canvas.DrawImage(Image, New SkiaSharp.SKRect(GraphicObject.X, GraphicObject.Y, GraphicObject.X + GraphicObject.Width, GraphicObject.Y + GraphicObject.Height), p)
        End Using

    End Sub


    'this function creates the connection ports in the flowsheet object
    Public Sub CreateConnectors() Implements Interfaces.IExternalUnitOperation.CreateConnectors

        If GraphicObject.InputConnectors.Count = 0 Then

            Dim port1 As New Drawing.SkiaSharp.GraphicObjects.ConnectionPoint()

            port1.IsEnergyConnector = False
            port1.Type = Interfaces.Enums.GraphicObjects.ConType.ConIn
            port1.Position = New DWSIM.DrawingTools.Point.Point(GraphicObject.X, GraphicObject.Y + GraphicObject.Height / 3)
            port1.ConnectorName = "Gas Stream Inlet Port"

            GraphicObject.InputConnectors.Add(port1)

            Dim port2 As New Drawing.SkiaSharp.GraphicObjects.ConnectionPoint()

            port2.IsEnergyConnector = False
            port2.Type = Interfaces.Enums.GraphicObjects.ConType.ConIn
            port2.Position = New DWSIM.DrawingTools.Point.Point(GraphicObject.X, GraphicObject.Y + GraphicObject.Height * 2 / 3)
            port2.ConnectorName = "Water Stream Inlet Port"

            GraphicObject.InputConnectors.Add(port2)

        Else

            GraphicObject.InputConnectors(0).Position = New DWSIM.DrawingTools.Point.Point(GraphicObject.X, GraphicObject.Y + GraphicObject.Height / 3)

            GraphicObject.InputConnectors(1).Position = New DWSIM.DrawingTools.Point.Point(GraphicObject.X, GraphicObject.Y + GraphicObject.Height * 2 / 3)

        End If

        If GraphicObject.OutputConnectors.Count = 0 Then

            Dim port3 As New Drawing.SkiaSharp.GraphicObjects.ConnectionPoint()

            port3.IsEnergyConnector = False
            port3.Type = Interfaces.Enums.GraphicObjects.ConType.ConOut
            port3.Position = New DWSIM.DrawingTools.Point.Point(GraphicObject.X + GraphicObject.Width, GraphicObject.Y + GraphicObject.Height / 2)
            port3.ConnectorName = "Mixed Stream Outlet Port"

            GraphicObject.OutputConnectors.Add(port3)

            Dim port4 As New Drawing.SkiaSharp.GraphicObjects.ConnectionPoint()
            port4.IsEnergyConnector = True
            port4.Type = Interfaces.Enums.GraphicObjects.ConType.ConEn
            port4.Position = New DWSIM.DrawingTools.Point.Point(GraphicObject.X + GraphicObject.Width / 2, GraphicObject.Y + GraphicObject.Height)
            port4.ConnectorName = "Energy Stream Outlet Port"

            GraphicObject.OutputConnectors.Add(port4)

        Else

            GraphicObject.OutputConnectors(0).Position = New DWSIM.DrawingTools.Point.Point(GraphicObject.X + GraphicObject.Width, GraphicObject.Y + GraphicObject.Height / 2)
            GraphicObject.OutputConnectors(1).Position = New DWSIM.DrawingTools.Point.Point(GraphicObject.X + GraphicObject.Width / 2, GraphicObject.Y + GraphicObject.Height)

        End If

        GraphicObject.EnergyConnector.Active = False

    End Sub

    'this function display the properties on the croos-platform user interface
    Public Sub PopulateEditorPanel(container As Object) Implements Interfaces.IExternalUnitOperation.PopulateEditorPanel

        'using extension methods from DWSIM.ExtensionMethods.Eto (DWISM.UI.Shared namespace)

        Dim propertiespanel = DirectCast(container, Eto.Forms.DynamicLayout)

        Dim options = New List(Of String)({"Adiabatic", "Isothermic"})

        'adds a dropdown to select between adiabatic and isothermic mode 

        propertiespanel.CreateAndAddDropDownRow("Calculation Mode", options, IIf(IsAdiabatic, 0, 1),
                                                Sub(dd, e)
                                                    Select Case dd.SelectedIndex
                                                        Case 0
                                                            IsAdiabatic = True
                                                        Case 1
                                                            IsAdiabatic = False
                                                    End Select
                                                End Sub, Nothing)

    End Sub

    Public Overrides Sub Calculate(Optional args As Object = Nothing)

        If GetInletMaterialStream(0) Is Nothing Then
            Throw New Exception("No stream connected to inlet gas port")
        End If

        If GetInletMaterialStream(1) Is Nothing Then
            Throw New Exception("No stream connected to inlet water port")
        End If

        If GetOutletMaterialStream(0) Is Nothing Then
            Throw New Exception("No stream connected to outlet gas port")
        End If

        Dim gas_stream As DWSIM.Thermodynamics.Streams.MaterialStream = GetInletMaterialStream(0)
        Dim water_stream As DWSIM.Thermodynamics.Streams.MaterialStream = GetInletMaterialStream(1)

        'check if water stream is really made of liquid water only.

        If Not water_stream.GetPhase("Overall").Compounds.Keys.Contains("Water") Then
            Throw New Exception("This Unit Operation needs Water in the list of added compounds.")
        End If

        'get water compound amount in liquid phase.

        Dim liquidphase_water = water_stream.GetPhase("Liquid1")

        Dim water = liquidphase_water.Compounds("Water")

        If liquidphase_water.Properties.molarfraction < 0.9999 And water.MoleFraction < 0.9999 Then
            Throw New Exception("The inlet water stream must be 100% liquid water.")
        End If

        Dim mixedstream As DWSIM.Thermodynamics.Streams.MaterialStream = gas_stream.Clone()

        mixedstream = mixedstream.Add(water_stream.Clone())

        Dim Pgas = gas_stream.GetPressure() 'Pa
        Dim Pwater = water_stream.GetPressure() 'Pa

        'set outlet stream pressure as (Pg + Pw)/2

        mixedstream.SetPressure((Pgas + Pwater) / 2)

        Dim Hg = gas_stream.GetMassEnthalpy() 'kJ/kg
        Dim Hw = water_stream.GetMassEnthalpy() 'kJ/kg

        Dim Wg = gas_stream.GetMassFlow() 'kg/s
        Dim Ww = water_stream.GetMassFlow() 'kg/s

        If IsAdiabatic Then

            'outlet stream enthalpy

            Dim Ho = (Wg * Hg + Ww * Hw) / (Wg + Ww) 'kJ/kg

            'the stream will be calculated by the flowsheet solver, we just need to set its properties.

            mixedstream.SetMassEnthalpy(Ho)
            mixedstream.SetFlashSpec("PH") 'Pressure/Enthalpy

            'if the energy stream is connected, set its value to zero (adiabatic)

            If GetOutletEnergyStream(1) IsNot Nothing Then

                Dim energystream As DWSIM.UnitOperations.Streams.EnergyStream = GetOutletEnergyStream(1)

                energystream.EnergyFlow = 0.0 'kW

            End If

        Else

            If GetOutletEnergyStream(1) Is Nothing Then
                Throw New Exception("No stream connected to outlet energy port")
            End If

            'isothermic mode means outlet temperature = gas temperature

            Dim Tg = gas_stream.GetTemperature() 'K

            mixedstream.SetTemperature(Tg)
            mixedstream.SetFlashSpec("PT") 'Pressure/Temperature

            'calculate the stream to get its enthalpy and close the energy balance.

            Me.PropertyPackage.CurrentMaterialStream = mixedstream
            mixedstream.Calculate()

            Dim Wo = mixedstream.GetMassFlow() 'kg/s
            Dim Ho = mixedstream.GetMassEnthalpy() 'kJ/kg

            Dim Eb = (Wg * Hg + Ww * Hw) - Wo * Ho 'kJ/s = KW

            Dim energystream As DWSIM.UnitOperations.Streams.EnergyStream = GetOutletEnergyStream(1)

            energystream.EnergyFlow = Eb 'kW

        End If

        'get a reference to the outlet stream

        Dim outletstream = GetOutletMaterialStream(0)

        'copy the properties from mixedstream

        outletstream.Assign(mixedstream)

    End Sub


End Class