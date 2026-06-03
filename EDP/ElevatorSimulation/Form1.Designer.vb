<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.pnlShaft = New System.Windows.Forms.Panel()
        Me.pnlCar = New System.Windows.Forms.Panel()
        Me.lblCarFloor = New System.Windows.Forms.Label()
        Me.lblMark1 = New System.Windows.Forms.Label()
        Me.lblMark2 = New System.Windows.Forms.Label()
        Me.lblMark3 = New System.Windows.Forms.Label()
        Me.lblMark4 = New System.Windows.Forms.Label()
        Me.GroupBoxgrpInside1 = New System.Windows.Forms.GroupBox()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnOpen = New System.Windows.Forms.Button()
        Me.btnFloor4 = New System.Windows.Forms.Button()
        Me.btnFloor3 = New System.Windows.Forms.Button()
        Me.btnFloor2 = New System.Windows.Forms.Button()
        Me.btnFloor1 = New System.Windows.Forms.Button()
        Me.grpStatus = New System.Windows.Forms.GroupBox()
        Me.lblActionCap = New System.Windows.Forms.Label()
        Me.lblPendingCap = New System.Windows.Forms.Label()
        Me.lblDirCap = New System.Windows.Forms.Label()
        Me.lblDoorCap = New System.Windows.Forms.Label()
        Me.lblFloorCap = New System.Windows.Forms.Label()
        Me.lblActionVal = New System.Windows.Forms.Label()
        Me.lblPendingVal = New System.Windows.Forms.Label()
        Me.lblDirVal = New System.Windows.Forms.Label()
        Me.lblDoorVal = New System.Windows.Forms.Label()
        Me.lblFloorVal = New System.Windows.Forms.Label()
        Me.grpControl = New System.Windows.Forms.GroupBox()
        Me.btnStop = New System.Windows.Forms.Button()
        Me.btnPause = New System.Windows.Forms.Button()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.btnObstruct = New System.Windows.Forms.Button()
        Me.lblKeys = New System.Windows.Forms.Label()
        Me.timerMove = New System.Windows.Forms.Timer(Me.components)
        Me.timerDoor = New System.Windows.Forms.Timer(Me.components)
        Me.btnExt4Down = New System.Windows.Forms.Button()
        Me.btnExt3Up = New System.Windows.Forms.Button()
        Me.btnExt3Down = New System.Windows.Forms.Button()
        Me.btnExt2Down = New System.Windows.Forms.Button()
        Me.btnExt2Up = New System.Windows.Forms.Button()
        Me.btnExt1Up = New System.Windows.Forms.Button()
        Me.lblDoorL = New System.Windows.Forms.Label()
        Me.lblDoorR = New System.Windows.Forms.Label()
        Me.pnlShaft.SuspendLayout()
        Me.pnlCar.SuspendLayout()
        Me.GroupBoxgrpInside1.SuspendLayout()
        Me.grpStatus.SuspendLayout()
        Me.grpControl.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlShaft
        '
        Me.pnlShaft.BackColor = System.Drawing.Color.DimGray
        Me.pnlShaft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlShaft.Controls.Add(Me.pnlCar)
        Me.pnlShaft.Controls.Add(Me.lblMark1)
        Me.pnlShaft.Controls.Add(Me.lblMark2)
        Me.pnlShaft.Controls.Add(Me.lblMark3)
        Me.pnlShaft.Controls.Add(Me.lblMark4)
        Me.pnlShaft.Location = New System.Drawing.Point(20, 20)
        Me.pnlShaft.Name = "pnlShaft"
        Me.pnlShaft.Size = New System.Drawing.Size(123, 566)
        Me.pnlShaft.TabIndex = 0
        '
        'pnlCar
        '
        Me.pnlCar.BackColor = System.Drawing.Color.LavenderBlush
        Me.pnlCar.BackgroundImage = CType(resources.GetObject("pnlCar.BackgroundImage"), System.Drawing.Image)
        Me.pnlCar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.pnlCar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlCar.Controls.Add(Me.lblCarFloor)
        Me.pnlCar.Controls.Add(Me.lblDoorR)
        Me.pnlCar.Controls.Add(Me.lblDoorL)
        Me.pnlCar.Location = New System.Drawing.Point(26, 458)
        Me.pnlCar.Name = "pnlCar"
        Me.pnlCar.Size = New System.Drawing.Size(70, 100)
        Me.pnlCar.TabIndex = 4
        '
        'lblCarFloor
        '
        Me.lblCarFloor.BackColor = System.Drawing.Color.Transparent
        Me.lblCarFloor.Font = New System.Drawing.Font("Palatino Linotype", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCarFloor.ForeColor = System.Drawing.Color.Lime
        Me.lblCarFloor.Location = New System.Drawing.Point(9, 7)
        Me.lblCarFloor.Name = "lblCarFloor"
        Me.lblCarFloor.Size = New System.Drawing.Size(53, 26)
        Me.lblCarFloor.TabIndex = 2
        Me.lblCarFloor.Text = "1"
        Me.lblCarFloor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblMark1
        '
        Me.lblMark1.BackColor = System.Drawing.Color.DimGray
        Me.lblMark1.Font = New System.Drawing.Font("Palatino Linotype", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMark1.ForeColor = System.Drawing.Color.White
        Me.lblMark1.Location = New System.Drawing.Point(5, 464)
        Me.lblMark1.Name = "lblMark1"
        Me.lblMark1.Size = New System.Drawing.Size(20, 20)
        Me.lblMark1.TabIndex = 3
        Me.lblMark1.Text = "1"
        '
        'lblMark2
        '
        Me.lblMark2.BackColor = System.Drawing.Color.DimGray
        Me.lblMark2.Font = New System.Drawing.Font("Palatino Linotype", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMark2.ForeColor = System.Drawing.Color.White
        Me.lblMark2.Location = New System.Drawing.Point(5, 303)
        Me.lblMark2.Name = "lblMark2"
        Me.lblMark2.Size = New System.Drawing.Size(20, 20)
        Me.lblMark2.TabIndex = 2
        Me.lblMark2.Text = "2"
        '
        'lblMark3
        '
        Me.lblMark3.BackColor = System.Drawing.Color.DimGray
        Me.lblMark3.Font = New System.Drawing.Font("Palatino Linotype", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMark3.ForeColor = System.Drawing.Color.White
        Me.lblMark3.Location = New System.Drawing.Point(5, 157)
        Me.lblMark3.Name = "lblMark3"
        Me.lblMark3.Size = New System.Drawing.Size(20, 20)
        Me.lblMark3.TabIndex = 1
        Me.lblMark3.Text = "3"
        '
        'lblMark4
        '
        Me.lblMark4.BackColor = System.Drawing.Color.DimGray
        Me.lblMark4.Font = New System.Drawing.Font("Palatino Linotype", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMark4.ForeColor = System.Drawing.Color.White
        Me.lblMark4.Location = New System.Drawing.Point(5, 10)
        Me.lblMark4.Name = "lblMark4"
        Me.lblMark4.Size = New System.Drawing.Size(20, 20)
        Me.lblMark4.TabIndex = 0
        Me.lblMark4.Text = "4"
        '
        'GroupBoxgrpInside1
        '
        Me.GroupBoxgrpInside1.BackColor = System.Drawing.Color.PaleVioletRed
        Me.GroupBoxgrpInside1.Controls.Add(Me.btnClose)
        Me.GroupBoxgrpInside1.Controls.Add(Me.btnOpen)
        Me.GroupBoxgrpInside1.Controls.Add(Me.btnFloor4)
        Me.GroupBoxgrpInside1.Controls.Add(Me.btnFloor3)
        Me.GroupBoxgrpInside1.Controls.Add(Me.btnFloor2)
        Me.GroupBoxgrpInside1.Controls.Add(Me.btnFloor1)
        Me.GroupBoxgrpInside1.Font = New System.Drawing.Font("Palatino Linotype", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBoxgrpInside1.Location = New System.Drawing.Point(220, 20)
        Me.GroupBoxgrpInside1.Name = "GroupBoxgrpInside1"
        Me.GroupBoxgrpInside1.Size = New System.Drawing.Size(340, 130)
        Me.GroupBoxgrpInside1.TabIndex = 2
        Me.GroupBoxgrpInside1.TabStop = False
        Me.GroupBoxgrpInside1.Text = "Inside Elevator – Floor Selection"
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(165, 88)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(135, 30)
        Me.btnClose.TabIndex = 5
        Me.btnClose.Text = "⊠ Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnOpen
        '
        Me.btnOpen.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOpen.Location = New System.Drawing.Point(15, 88)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(135, 30)
        Me.btnOpen.TabIndex = 4
        Me.btnOpen.Text = "⊡ Open"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnFloor4
        '
        Me.btnFloor4.Font = New System.Drawing.Font("Palatino Linotype", 13.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFloor4.Location = New System.Drawing.Point(240, 35)
        Me.btnFloor4.Name = "btnFloor4"
        Me.btnFloor4.Size = New System.Drawing.Size(60, 45)
        Me.btnFloor4.TabIndex = 3
        Me.btnFloor4.Text = "4"
        Me.btnFloor4.UseVisualStyleBackColor = True
        '
        'btnFloor3
        '
        Me.btnFloor3.Font = New System.Drawing.Font("Palatino Linotype", 13.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFloor3.Location = New System.Drawing.Point(165, 35)
        Me.btnFloor3.Name = "btnFloor3"
        Me.btnFloor3.Size = New System.Drawing.Size(60, 45)
        Me.btnFloor3.TabIndex = 2
        Me.btnFloor3.Text = "3"
        Me.btnFloor3.UseVisualStyleBackColor = True
        '
        'btnFloor2
        '
        Me.btnFloor2.Font = New System.Drawing.Font("Palatino Linotype", 13.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFloor2.Location = New System.Drawing.Point(90, 35)
        Me.btnFloor2.Name = "btnFloor2"
        Me.btnFloor2.Size = New System.Drawing.Size(60, 45)
        Me.btnFloor2.TabIndex = 1
        Me.btnFloor2.Text = "2"
        Me.btnFloor2.UseVisualStyleBackColor = True
        '
        'btnFloor1
        '
        Me.btnFloor1.Font = New System.Drawing.Font("Palatino Linotype", 13.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFloor1.Location = New System.Drawing.Point(15, 35)
        Me.btnFloor1.Name = "btnFloor1"
        Me.btnFloor1.Size = New System.Drawing.Size(60, 45)
        Me.btnFloor1.TabIndex = 0
        Me.btnFloor1.Text = "1"
        Me.btnFloor1.UseVisualStyleBackColor = True
        '
        'grpStatus
        '
        Me.grpStatus.BackColor = System.Drawing.Color.PaleVioletRed
        Me.grpStatus.Controls.Add(Me.lblActionCap)
        Me.grpStatus.Controls.Add(Me.lblPendingCap)
        Me.grpStatus.Controls.Add(Me.lblDirCap)
        Me.grpStatus.Controls.Add(Me.lblDoorCap)
        Me.grpStatus.Controls.Add(Me.lblFloorCap)
        Me.grpStatus.Controls.Add(Me.lblActionVal)
        Me.grpStatus.Controls.Add(Me.lblPendingVal)
        Me.grpStatus.Controls.Add(Me.lblDirVal)
        Me.grpStatus.Controls.Add(Me.lblDoorVal)
        Me.grpStatus.Controls.Add(Me.lblFloorVal)
        Me.grpStatus.Font = New System.Drawing.Font("Palatino Linotype", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpStatus.Location = New System.Drawing.Point(220, 156)
        Me.grpStatus.Name = "grpStatus"
        Me.grpStatus.Size = New System.Drawing.Size(340, 200)
        Me.grpStatus.TabIndex = 3
        Me.grpStatus.TabStop = False
        Me.grpStatus.Text = "Elevator Status"
        '
        'lblActionCap
        '
        Me.lblActionCap.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblActionCap.Location = New System.Drawing.Point(10, 145)
        Me.lblActionCap.Name = "lblActionCap"
        Me.lblActionCap.Size = New System.Drawing.Size(80, 22)
        Me.lblActionCap.TabIndex = 9
        Me.lblActionCap.Text = "Action:"
        '
        'lblPendingCap
        '
        Me.lblPendingCap.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPendingCap.Location = New System.Drawing.Point(10, 115)
        Me.lblPendingCap.Name = "lblPendingCap"
        Me.lblPendingCap.Size = New System.Drawing.Size(80, 22)
        Me.lblPendingCap.TabIndex = 8
        Me.lblPendingCap.Text = "Pending:"
        '
        'lblDirCap
        '
        Me.lblDirCap.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDirCap.Location = New System.Drawing.Point(10, 85)
        Me.lblDirCap.Name = "lblDirCap"
        Me.lblDirCap.Size = New System.Drawing.Size(120, 22)
        Me.lblDirCap.TabIndex = 7
        Me.lblDirCap.Text = "Direction:"
        '
        'lblDoorCap
        '
        Me.lblDoorCap.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDoorCap.Location = New System.Drawing.Point(10, 55)
        Me.lblDoorCap.Name = "lblDoorCap"
        Me.lblDoorCap.Size = New System.Drawing.Size(120, 22)
        Me.lblDoorCap.TabIndex = 6
        Me.lblDoorCap.Text = "Door Status:"
        '
        'lblFloorCap
        '
        Me.lblFloorCap.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFloorCap.Location = New System.Drawing.Point(10, 25)
        Me.lblFloorCap.Name = "lblFloorCap"
        Me.lblFloorCap.Size = New System.Drawing.Size(120, 22)
        Me.lblFloorCap.TabIndex = 5
        Me.lblFloorCap.Text = "Current Floor:"
        '
        'lblActionVal
        '
        Me.lblActionVal.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblActionVal.Location = New System.Drawing.Point(96, 140)
        Me.lblActionVal.Name = "lblActionVal"
        Me.lblActionVal.Size = New System.Drawing.Size(190, 22)
        Me.lblActionVal.TabIndex = 4
        Me.lblActionVal.Text = "Stopped – press Start"
        '
        'lblPendingVal
        '
        Me.lblPendingVal.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPendingVal.Location = New System.Drawing.Point(96, 115)
        Me.lblPendingVal.Name = "lblPendingVal"
        Me.lblPendingVal.Size = New System.Drawing.Size(172, 22)
        Me.lblPendingVal.TabIndex = 3
        Me.lblPendingVal.Text = "Int: –   Ext: –"
        '
        'lblDirVal
        '
        Me.lblDirVal.AutoSize = True
        Me.lblDirVal.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDirVal.ForeColor = System.Drawing.Color.Black
        Me.lblDirVal.Location = New System.Drawing.Point(140, 85)
        Me.lblDirVal.Name = "lblDirVal"
        Me.lblDirVal.Size = New System.Drawing.Size(50, 21)
        Me.lblDirVal.TabIndex = 2
        Me.lblDirVal.Text = "● Idle"
        '
        'lblDoorVal
        '
        Me.lblDoorVal.AutoSize = True
        Me.lblDoorVal.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDoorVal.Location = New System.Drawing.Point(140, 55)
        Me.lblDoorVal.Name = "lblDoorVal"
        Me.lblDoorVal.Size = New System.Drawing.Size(57, 21)
        Me.lblDoorVal.TabIndex = 1
        Me.lblDoorVal.Text = "Closed"
        '
        'lblFloorVal
        '
        Me.lblFloorVal.AutoSize = True
        Me.lblFloorVal.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFloorVal.Location = New System.Drawing.Point(140, 25)
        Me.lblFloorVal.Name = "lblFloorVal"
        Me.lblFloorVal.Size = New System.Drawing.Size(18, 21)
        Me.lblFloorVal.TabIndex = 0
        Me.lblFloorVal.Text = "1"
        '
        'grpControl
        '
        Me.grpControl.BackColor = System.Drawing.Color.PaleVioletRed
        Me.grpControl.Controls.Add(Me.btnStop)
        Me.grpControl.Controls.Add(Me.btnPause)
        Me.grpControl.Controls.Add(Me.btnStart)
        Me.grpControl.Location = New System.Drawing.Point(220, 456)
        Me.grpControl.Name = "grpControl"
        Me.grpControl.Size = New System.Drawing.Size(340, 70)
        Me.grpControl.TabIndex = 4
        Me.grpControl.TabStop = False
        Me.grpControl.Text = "Simulation Control"
        '
        'btnStop
        '
        Me.btnStop.Location = New System.Drawing.Point(220, 25)
        Me.btnStop.Name = "btnStop"
        Me.btnStop.Size = New System.Drawing.Size(95, 32)
        Me.btnStop.TabIndex = 3
        Me.btnStop.Text = "⏹ Stop"
        Me.btnStop.UseVisualStyleBackColor = True
        '
        'btnPause
        '
        Me.btnPause.Location = New System.Drawing.Point(115, 25)
        Me.btnPause.Name = "btnPause"
        Me.btnPause.Size = New System.Drawing.Size(95, 32)
        Me.btnPause.TabIndex = 2
        Me.btnPause.Text = "⏸ Pause"
        Me.btnPause.UseVisualStyleBackColor = True
        '
        'btnStart
        '
        Me.btnStart.Location = New System.Drawing.Point(10, 25)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(95, 32)
        Me.btnStart.TabIndex = 1
        Me.btnStart.Text = "▶ Start"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'btnObstruct
        '
        Me.btnObstruct.BackColor = System.Drawing.Color.MistyRose
        Me.btnObstruct.Location = New System.Drawing.Point(310, 394)
        Me.btnObstruct.Name = "btnObstruct"
        Me.btnObstruct.Size = New System.Drawing.Size(130, 35)
        Me.btnObstruct.TabIndex = 5
        Me.btnObstruct.Text = "⚠ Obstruct [X]"
        Me.btnObstruct.UseVisualStyleBackColor = False
        '
        'lblKeys
        '
        Me.lblKeys.Font = New System.Drawing.Font("Palatino Linotype", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblKeys.ForeColor = System.Drawing.Color.DimGray
        Me.lblKeys.Location = New System.Drawing.Point(202, 556)
        Me.lblKeys.Name = "lblKeys"
        Me.lblKeys.Size = New System.Drawing.Size(500, 30)
        Me.lblKeys.TabIndex = 6
        Me.lblKeys.Text = "Keys: 1-4 floor  U/D call  O open  C close  X obstruct  Q quit"
        '
        'timerMove
        '
        Me.timerMove.Interval = 1000
        '
        'timerDoor
        '
        Me.timerDoor.Interval = 3000
        '
        'btnExt4Down
        '
        Me.btnExt4Down.Location = New System.Drawing.Point(155, 31)
        Me.btnExt4Down.Name = "btnExt4Down"
        Me.btnExt4Down.Size = New System.Drawing.Size(35, 35)
        Me.btnExt4Down.TabIndex = 9
        Me.btnExt4Down.Text = "▼"
        Me.btnExt4Down.UseVisualStyleBackColor = True
        '
        'btnExt3Up
        '
        Me.btnExt3Up.Location = New System.Drawing.Point(155, 178)
        Me.btnExt3Up.Name = "btnExt3Up"
        Me.btnExt3Up.Size = New System.Drawing.Size(35, 35)
        Me.btnExt3Up.TabIndex = 10
        Me.btnExt3Up.Text = "▲"
        Me.btnExt3Up.UseVisualStyleBackColor = True
        '
        'btnExt3Down
        '
        Me.btnExt3Down.Location = New System.Drawing.Point(155, 218)
        Me.btnExt3Down.Name = "btnExt3Down"
        Me.btnExt3Down.Size = New System.Drawing.Size(35, 35)
        Me.btnExt3Down.TabIndex = 11
        Me.btnExt3Down.Text = "▼"
        Me.btnExt3Down.UseVisualStyleBackColor = True
        '
        'btnExt2Down
        '
        Me.btnExt2Down.Location = New System.Drawing.Point(155, 364)
        Me.btnExt2Down.Name = "btnExt2Down"
        Me.btnExt2Down.Size = New System.Drawing.Size(35, 35)
        Me.btnExt2Down.TabIndex = 13
        Me.btnExt2Down.Text = "▼"
        Me.btnExt2Down.UseVisualStyleBackColor = True
        '
        'btnExt2Up
        '
        Me.btnExt2Up.Location = New System.Drawing.Point(155, 324)
        Me.btnExt2Up.Name = "btnExt2Up"
        Me.btnExt2Up.Size = New System.Drawing.Size(35, 35)
        Me.btnExt2Up.TabIndex = 12
        Me.btnExt2Up.Text = "▲"
        Me.btnExt2Up.UseVisualStyleBackColor = True
        '
        'btnExt1Up
        '
        Me.btnExt1Up.Location = New System.Drawing.Point(155, 485)
        Me.btnExt1Up.Name = "btnExt1Up"
        Me.btnExt1Up.Size = New System.Drawing.Size(35, 35)
        Me.btnExt1Up.TabIndex = 14
        Me.btnExt1Up.Text = "▲"
        Me.btnExt1Up.UseVisualStyleBackColor = True
        '
        'lblDoorL
        '
        Me.lblDoorL.BackColor = System.Drawing.Color.Transparent
        Me.lblDoorL.Font = New System.Drawing.Font("Microsoft Sans Serif", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDoorL.ForeColor = System.Drawing.Color.Transparent
        Me.lblDoorL.Location = New System.Drawing.Point(8, 22)
        Me.lblDoorL.Name = "lblDoorL"
        Me.lblDoorL.Size = New System.Drawing.Size(24, 80)
        Me.lblDoorL.TabIndex = 0
        Me.lblDoorL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblDoorR
        '
        Me.lblDoorR.BackColor = System.Drawing.Color.Transparent
        Me.lblDoorR.Font = New System.Drawing.Font("Microsoft Sans Serif", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDoorR.ForeColor = System.Drawing.Color.Transparent
        Me.lblDoorR.Location = New System.Drawing.Point(35, 26)
        Me.lblDoorR.Name = "lblDoorR"
        Me.lblDoorR.Size = New System.Drawing.Size(27, 73)
        Me.lblDoorR.TabIndex = 1
        Me.lblDoorR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(615, 591)
        Me.Controls.Add(Me.btnExt1Up)
        Me.Controls.Add(Me.btnExt2Down)
        Me.Controls.Add(Me.btnExt2Up)
        Me.Controls.Add(Me.btnExt3Down)
        Me.Controls.Add(Me.btnExt3Up)
        Me.Controls.Add(Me.btnExt4Down)
        Me.Controls.Add(Me.lblKeys)
        Me.Controls.Add(Me.btnObstruct)
        Me.Controls.Add(Me.grpControl)
        Me.Controls.Add(Me.grpStatus)
        Me.Controls.Add(Me.GroupBoxgrpInside1)
        Me.Controls.Add(Me.pnlShaft)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MinimizeBox = False
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Elevator ni Gab"
        Me.pnlShaft.ResumeLayout(False)
        Me.pnlCar.ResumeLayout(False)
        Me.GroupBoxgrpInside1.ResumeLayout(False)
        Me.grpStatus.ResumeLayout(False)
        Me.grpStatus.PerformLayout()
        Me.grpControl.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlShaft As Panel
    Friend WithEvents lblMark4 As Label
    Friend WithEvents lblMark2 As Label
    Friend WithEvents lblMark3 As Label
    Friend WithEvents pnlCar As Panel
    Friend WithEvents lblMark1 As Label
    Friend WithEvents lblCarFloor As Label
    Friend WithEvents GroupBoxgrpInside1 As GroupBox
    Friend WithEvents btnFloor3 As Button
    Friend WithEvents btnFloor2 As Button
    Friend WithEvents btnFloor1 As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents btnOpen As Button
    Friend WithEvents btnFloor4 As Button
    Friend WithEvents grpStatus As GroupBox
    Friend WithEvents lblFloorVal As Label
    Friend WithEvents lblPendingVal As Label
    Friend WithEvents lblDirVal As Label
    Friend WithEvents lblDoorVal As Label
    Friend WithEvents lblActionVal As Label
    Friend WithEvents lblActionCap As Label
    Friend WithEvents lblPendingCap As Label
    Friend WithEvents lblDirCap As Label
    Friend WithEvents lblDoorCap As Label
    Friend WithEvents lblFloorCap As Label
    Friend WithEvents grpControl As GroupBox
    Friend WithEvents btnStop As Button
    Friend WithEvents btnPause As Button
    Friend WithEvents btnStart As Button
    Friend WithEvents btnObstruct As Button
    Friend WithEvents lblKeys As Label
    Friend WithEvents timerMove As Timer
    Friend WithEvents timerDoor As Timer
    Friend WithEvents btnExt4Down As Button
    Friend WithEvents btnExt3Up As Button
    Friend WithEvents btnExt3Down As Button
    Friend WithEvents btnExt2Down As Button
    Friend WithEvents btnExt2Up As Button
    Friend WithEvents btnExt1Up As Button
    Friend WithEvents lblDoorR As Label
    Friend WithEvents lblDoorL As Label
End Class
