Imports System.Windows.Forms
Imports DWSIM.Interfaces.Enums.GraphicObjects

Public Class Editor

    Inherits WeifenLuo.WinFormsUI.Docking.DockContent

    Public Property HObject As Humidifier

    Public Loaded As Boolean = False

    Private Sub Editor_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        UpdateInfo()

    End Sub

    Sub UpdateInfo()

        Loaded = False

        With HObject

            Me.Text = .GraphicObject.Tag & " (" & .GetDisplayName() & ")"

            lblTag.Text = .GraphicObject.Tag
            If .Calculated Then
                lblStatus.Text = .FlowSheet.GetTranslatedString("Calculado") & " (" & .LastUpdated.ToString & ")"
                lblStatus.ForeColor = System.Drawing.Color.Blue
            Else
                If Not .GraphicObject.Active Then
                    lblStatus.Text = .FlowSheet.GetTranslatedString("Inativo")
                    lblStatus.ForeColor = System.Drawing.Color.Gray
                ElseIf .ErrorMessage <> "" Then
                    lblStatus.Text = .FlowSheet.GetTranslatedString("Erro")
                    lblStatus.ForeColor = System.Drawing.Color.Red
                Else
                    lblStatus.Text = .FlowSheet.GetTranslatedString("NoCalculado")
                    lblStatus.ForeColor = System.Drawing.Color.Black
                End If
            End If

            lblConnectedTo.Text = ""

            If .IsSpecAttached Then lblConnectedTo.Text = .FlowSheet.SimulationObjects(.AttachedSpecId).GraphicObject.Tag
            If .IsAdjustAttached Then lblConnectedTo.Text = .FlowSheet.SimulationObjects(.AttachedAdjustId).GraphicObject.Tag

            Dim mslist As String() = .FlowSheet.GraphicObjects.Values.Where(Function(x) x.ObjectType = ObjectType.MaterialStream).Select(Function(m) m.Tag).ToArray

            cbInlet1.Items.Clear()
            cbInlet2.Items.Clear()

            cbInlet1.Items.AddRange(mslist)
            cbInlet2.Items.AddRange(mslist)

            cbOutlet1.Items.Clear()

            cbOutlet1.Items.AddRange(mslist)

            If Not .GetInletMaterialStream(0) Is Nothing Then cbInlet1.SelectedItem = .GetInletMaterialStream(0).GraphicObject.Tag
            If Not .GetInletMaterialStream(1) Is Nothing Then cbInlet2.SelectedItem = .GetInletMaterialStream(1).GraphicObject.Tag

            If Not .GetOutletMaterialStream(0) Is Nothing Then cbOutlet1.SelectedItem = .GetOutletMaterialStream(0).GraphicObject.Tag

            Dim eslist As String() = .FlowSheet.GraphicObjects.Values.Where(Function(x) x.ObjectType = ObjectType.EnergyStream).Select(Function(m) m.Tag).ToArray

            cbEnergy.Items.Clear()
            cbEnergy.Items.AddRange(eslist)

            If Not .GetOutletEnergyStream(1) Is Nothing Then cbEnergy.SelectedItem = .GetOutletEnergyStream(1).GraphicObject.Tag

            'parameters

            If .IsAdiabatic Then
                ComboBox1.SelectedIndex = 0
            Else
                ComboBox1.SelectedIndex = 1
            End If

        End With

        Loaded = True


    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

        HObject.FlowSheet.RequestCalculation()

        Select Case ComboBox1.SelectedIndex
            Case 0
                HObject.IsAdiabatic = True
            Case 1
                HObject.IsAdiabatic = False
        End Select

    End Sub


    Sub HandleInletConnections(sender As Object, e As EventArgs) Handles cbInlet1.SelectedIndexChanged, cbInlet2.SelectedIndexChanged
        If Loaded Then UpdateInletConnection(sender)
    End Sub

    Sub HandleOutletConnections(sender As Object, e As EventArgs) Handles cbOutlet1.SelectedIndexChanged, cbEnergy.SelectedIndexChanged
        If Loaded Then UpdateOutletConnection(sender)
    End Sub

    Sub UpdateInletConnection(cb As ComboBox)

        Dim text As String = cb.Text

        If text <> "" Then

            Dim index As Integer = Convert.ToInt32(cb.Name.Substring(cb.Name.Length - 1)) - 1

            Dim gobj = HObject.GraphicObject
            Dim flowsheet = HObject.FlowSheet

            If flowsheet.GetFlowsheetSimulationObject(text).GraphicObject.OutputConnectors(0).IsAttached Then
                MessageBox.Show(flowsheet.GetTranslatedString("Todasasconexespossve"), flowsheet.GetTranslatedString("Erro"), MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                If gobj.InputConnectors(index).IsAttached Then flowsheet.DisconnectObjects(gobj.InputConnectors(index).AttachedConnector.AttachedFrom, gobj)
                Try
                    flowsheet.ConnectObjects(flowsheet.GetFlowsheetSimulationObject(text).GraphicObject, gobj, 0, index)
                Catch ex As Exception
                    MessageBox.Show(ex.Message, flowsheet.GetTranslatedString("Erro"), MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
            UpdateInfo()

        End If

    End Sub

    Sub UpdateOutletConnection(cb As ComboBox)

        Dim text As String = cb.Text

        If text <> "" Then

            Dim index As Integer = Convert.ToInt32(cb.Name.Substring(cb.Name.Length - 1)) - 1

            Dim gobj = HObject.GraphicObject
            Dim flowsheet = HObject.FlowSheet

            If flowsheet.GetFlowsheetSimulationObject(text).GraphicObject.InputConnectors(0).IsAttached Then
                MessageBox.Show(flowsheet.GetTranslatedString("Todasasconexespossve"), flowsheet.GetTranslatedString("Erro"), MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                If gobj.OutputConnectors(index).IsAttached Then flowsheet.DisconnectObjects(gobj, gobj.OutputConnectors(index).AttachedConnector.AttachedTo)
                Try
                    flowsheet.ConnectObjects(gobj, flowsheet.GetFlowsheetSimulationObject(text).GraphicObject, index, 0)
                Catch ex As Exception
                    MessageBox.Show(ex.Message, flowsheet.GetTranslatedString("Erro"), MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
            UpdateInfo()

        End If

    End Sub

    Private Sub cbPropPack_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbPropPack.SelectedIndexChanged
        If Loaded Then
            HObject.PropertyPackage = HObject.FlowSheet.PropertyPackages.Values.Where(Function(x) x.Tag = cbPropPack.SelectedItem.ToString).SingleOrDefault
            RequestCalc()
        End If
    End Sub

    Private Sub rtbAnnotations_RtfChanged(sender As Object, e As EventArgs) Handles rtbAnnotations.RtfChanged
        If Loaded Then HObject.Annotation = rtbAnnotations.Rtf
    End Sub

    Private Sub lblTag_KeyPress(sender As Object, e As KeyEventArgs) Handles lblTag.KeyUp

        If e.KeyCode = Keys.Enter Then

            If Loaded Then HObject.GraphicObject.Tag = lblTag.Text
            If Loaded Then HObject.FlowSheet.UpdateOpenEditForms()
            Me.Text = HObject.GraphicObject.Tag & " (" & HObject.GetDisplayName() & ")"
            DirectCast(HObject.FlowSheet, Interfaces.IFlowsheetGUI).UpdateInterface()

        End If

    End Sub

    Sub RequestCalc()

        HObject.FlowSheet.RequestCalculation(HObject)

    End Sub

End Class