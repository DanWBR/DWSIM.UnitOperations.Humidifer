Imports DWSIM.Interfaces
Imports DWSIM.Thermodynamics
Imports DWSIM.Thermodynamics.Streams
Imports DWSIM.UnitOperations.UnitOperations

''' <summary>
''' Extension methods to get inlet/outlet material/energy streams connected from/to the unit operation
''' </summary>
Module StreamListExtender

    <System.Runtime.CompilerServices.Extension()>
    Public Function GetInletMaterialStream(ByVal unitop As UnitOpBaseClass, index As Integer) As MaterialStream

        If unitop.GraphicObject.InputConnectors(index).IsAttached Then
            Return unitop.FlowSheet.SimulationObjects(unitop.GraphicObject.InputConnectors(index).AttachedConnector.AttachedFrom.Name)
        Else
            Return Nothing
        End If

    End Function

    <System.Runtime.CompilerServices.Extension()>
    Public Function GetOutletMaterialStream(ByVal unitop As UnitOpBaseClass, index As Integer) As MaterialStream

        If unitop.GraphicObject.OutputConnectors(index).IsAttached Then
            Return unitop.FlowSheet.SimulationObjects(unitop.GraphicObject.OutputConnectors(index).AttachedConnector.AttachedTo.Name)
        Else
            Return Nothing
        End If

    End Function

    <System.Runtime.CompilerServices.Extension()>
    Public Function GetInletEnergyStream(ByVal unitop As UnitOpBaseClass, index As Integer) As DWSIM.UnitOperations.Streams.EnergyStream

        If unitop.GraphicObject.InputConnectors(index).IsAttached Then
            Return unitop.FlowSheet.SimulationObjects(unitop.GraphicObject.InputConnectors(index).AttachedConnector.AttachedFrom.Name)
        Else
            Return Nothing
        End If

    End Function

    <System.Runtime.CompilerServices.Extension()>
    Public Function GetOutletEnergyStream(ByVal unitop As UnitOpBaseClass, index As Integer) As DWSIM.UnitOperations.Streams.EnergyStream

        If unitop.GraphicObject.OutputConnectors(index).IsAttached Then
            Return unitop.FlowSheet.SimulationObjects(unitop.GraphicObject.OutputConnectors(index).AttachedConnector.AttachedTo.Name)
        Else
            Return Nothing
        End If

    End Function

    <System.Runtime.CompilerServices.Extension()>
    Public Function GetEnergyStream(ByVal unitop As UnitOpBaseClass) As DWSIM.UnitOperations.Streams.EnergyStream
        If unitop.GraphicObject.EnergyConnector.IsAttached Then
            If unitop.GraphicObject.EnergyConnector.AttachedConnector.AttachedTo.ObjectType = Enums.GraphicObjects.ObjectType.EnergyStream Then
                Return unitop.FlowSheet.SimulationObjects(unitop.GraphicObject.EnergyConnector.AttachedConnector.AttachedTo.Name)
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If
        Return Nothing
    End Function

End Module
