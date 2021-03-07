Public Class Editor

    Inherits WeifenLuo.WinFormsUI.Docking.DockContent

    Public Property HObject As Humidifier

    Private Sub Editor_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.Text = HObject.GraphicObject.Tag

        UpdateInfo()

    End Sub

    Sub UpdateInfo()

        If HObject.IsAdiabatic Then
            ComboBox1.SelectedIndex = 0
        Else
            ComboBox1.SelectedIndex = 1
        End If

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

End Class