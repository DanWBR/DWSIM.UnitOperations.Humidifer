Imports DWSIM.Interfaces.Enums
Imports SkiaSharp.Views.Desktop.Extensions

Namespace ExternalUnitOperations

    <System.Serializable()> Public Class Humidifier

        Inherits DWSIM.UnitOperations.UnitOperations.UnitOpBaseClass

        Implements DWSIM.Interfaces.IExternalUnitOperation

        Private Property UOName As String = "Humidifier"
        Private Property UODescription As String = "Humidifier Unit Operation"

        Public Overrides Property ObjectClass As SimulationObjectClass = SimulationObjectClass.MixersSplitters

        Public Property IsAdiabatic As Boolean = True

        'tells DWSIM that this Unit Operation is compatible with mobile versions
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

        Private editwindow As Editor

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

            If editwindow.InvokeRequired Then

                editwindow.Invoke(Sub()
                                      editwindow?.UpdateInfo()
                                  End Sub)

            Else

                editwindow?.UpdateInfo()

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

            Dim canvas = DirectCast(g, SkiaSharp.SKCanvas)

            If Image Is Nothing Then

                Using bitmap = My.Resources.humidifier_512.ToSKBitmap()
                    Image = SkiaSharp.SKImage.FromBitmap(bitmap)
                End Using

            End If

            Dim x = Me.GraphicObject.X
            Dim y = Me.GraphicObject.Y
            Dim w = Me.GraphicObject.Width
            Dim h = Me.GraphicObject.Height

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


        'this function display the properties on the croos-platfom user interface
        Public Sub PopulateEditorPanel(container As Object) Implements Interfaces.IExternalUnitOperation.PopulateEditorPanel

        End Sub

        Public Overrides Sub Calculate(Optional args As Object = Nothing)

            Dim i1 = GraphicObject.InputConnectors(0).AttachedConnector.AttachedFrom.Name
            Dim i2 = GraphicObject.InputConnectors(1).AttachedConnector.AttachedFrom.Name
            Dim o1 = GraphicObject.OutputConnectors(0).AttachedConnector.AttachedTo.Name

            Dim gas_stream As DWSIM.Thermodynamics.Streams.MaterialStream = FlowSheet.SimulationObjects(i1)
            Dim water_stream As DWSIM.Thermodynamics.Streams.MaterialStream = FlowSheet.SimulationObjects(i2)

            Dim mixedstream As DWSIM.Thermodynamics.Streams.MaterialStream = gas_stream.Clone()

            mixedstream = mixedstream.Add(water_stream.Clone())

            Dim Pgas = gas_stream.GetPressure() 'Pa
            Dim Pwater = water_stream.GetPressure() 'Pa

            mixedstream.SetPressure((Pgas + Pwater) / 2)

            Dim Hg = gas_stream.GetMassEnthalpy() 'kJ/kg
            Dim Hw = water_stream.GetMassEnthalpy() 'kJ/kg

            Dim Wg = gas_stream.GetMassFlow() 'kg/s
            Dim Ww = water_stream.GetMassFlow() 'kg/s

            If IsAdiabatic Then

                Dim Ho = (Wg * Hg + Ww * Hw) / (Wg + Ww) 'kJ/kg

                mixedstream.SetMassEnthalpy(Ho)
                mixedstream.SetFlashSpec("PH") 'Pressure/Enthalpy

            Else

                'isothermic mode means outlet temperature = gas temperature

                Dim Tg = gas_stream.GetTemperature() 'K

                mixedstream.SetTemperature(Tg)
                mixedstream.SetFlashSpec("PT") 'Pressure/Temperature

                Me.PropertyPackage.CurrentMaterialStream = mixedstream
                mixedstream.Calculate()

                Dim Wo = mixedstream.GetMassFlow() 'kg/s
                Dim Ho = mixedstream.GetMassEnthalpy() 'kJ/kg

                Dim Eb = Wo * Ho - (Wg * Hg + Ww * Hw) 'kJ/s = KW

                Dim o2 = GraphicObject.OutputConnectors(1).AttachedConnector.AttachedTo.Name

                Dim energystream As DWSIM.UnitOperations.Streams.EnergyStream = FlowSheet.SimulationObjects(o2)

                energystream.EnergyFlow = Eb 'kW

            End If

            Dim outletstream = FlowSheet.SimulationObjects(o1)

            outletstream.Assign(mixedstream)

        End Sub


    End Class

End Namespace

