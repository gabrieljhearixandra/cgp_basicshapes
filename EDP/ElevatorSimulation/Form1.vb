Imports ElevatorSim
Imports ElevatorSim2.ElevatorSim

Public Class Form1

    '── Single elevator instance ───────────────────────────────────────
    Private elevator As New ElevatorCar()

    '══════════════════════════════════════════════════════════════════
    '  FORM LOAD
    '══════════════════════════════════════════════════════════════════
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PositionCar()
        UpdateUI()
    End Sub

    '══════════════════════════════════════════════════════════════════
    '  CAR POSITIONING
    '══════════════════════════════════════════════════════════════════
    Private Function FloorToCarTop(floor As Integer) As Integer
        Select Case floor
            Case 4 : Return 10
            Case 3 : Return 130
            Case 2 : Return 250
            Case 1 : Return 380
            Case Else : Return 380
        End Select
    End Function

    Private Sub PositionCar()
        pnlCar.Top = FloorToCarTop(elevator.CurrentFloor)
        lblCarFloor.Text = elevator.CurrentFloor.ToString()
    End Sub

    '══════════════════════════════════════════════════════════════════
    '  UI UPDATE
    '══════════════════════════════════════════════════════════════════
    Private Sub UpdateUI()

        '── Status labels ─────────────────────────────────────────────
        lblFloorVal.Text = elevator.CurrentFloor.ToString()
        lblDirVal.Text = elevator.DirectionText()
        lblDoorVal.Text = elevator.CurrentDoorState.ToString()
        lblPendingVal.Text = elevator.PendingText()
        lblActionVal.Text = elevator.StatusMessage

        '── Car position ──────────────────────────────────────────────
        PositionCar()

        '── Door visual ───────────────────────────────────────────────
        Select Case elevator.CurrentDoorState
            Case ElevatorDoorState.Opened, ElevatorDoorState.Obstructed
                lblDoorL.Left = 6
                lblDoorR.Left = 54
            Case Else
                lblDoorL.Left = 8
                lblDoorR.Left = 48
        End Select

        '── Floor button highlight when request is pending ────────────
        btnFloor1.BackColor = If(elevator.InternalRequests.Contains(1), Color.Goldenrod, SystemColors.Control)
        btnFloor2.BackColor = If(elevator.InternalRequests.Contains(2), Color.Goldenrod, SystemColors.Control)
        btnFloor3.BackColor = If(elevator.InternalRequests.Contains(3), Color.Goldenrod, SystemColors.Control)
        btnFloor4.BackColor = If(elevator.InternalRequests.Contains(4), Color.Goldenrod, SystemColors.Control)

        '── External call button highlight when call is pending ───────
        btnExt1Up.BackColor = If(elevator.ExternalCalls.Any(
            Function(c) c.Floor = 1 AndAlso c.Dir = ElevatorDirection.Up),
            Color.Goldenrod, SystemColors.Control)

        btnExt2Up.BackColor = If(elevator.ExternalCalls.Any(
            Function(c) c.Floor = 2 AndAlso c.Dir = ElevatorDirection.Up),
            Color.Goldenrod, SystemColors.Control)

        btnExt2Down.BackColor = If(elevator.ExternalCalls.Any(
            Function(c) c.Floor = 2 AndAlso c.Dir = ElevatorDirection.Down),
            Color.Goldenrod, SystemColors.Control)

        btnExt3Up.BackColor = If(elevator.ExternalCalls.Any(
            Function(c) c.Floor = 3 AndAlso c.Dir = ElevatorDirection.Up),
            Color.Goldenrod, SystemColors.Control)

        btnExt3Down.BackColor = If(elevator.ExternalCalls.Any(
            Function(c) c.Floor = 3 AndAlso c.Dir = ElevatorDirection.Down),
            Color.Goldenrod, SystemColors.Control)

        btnExt4Down.BackColor = If(elevator.ExternalCalls.Any(
            Function(c) c.Floor = 4 AndAlso c.Dir = ElevatorDirection.Down),
            Color.Goldenrod, SystemColors.Control)

        '── Enable/disable interaction buttons ────────────────────────
        Dim canInteract As Boolean = elevator.IsRunning AndAlso Not elevator.IsPaused

        btnFloor1.Enabled = canInteract
        btnFloor2.Enabled = canInteract
        btnFloor3.Enabled = canInteract
        btnFloor4.Enabled = canInteract

        btnExt1Up.Enabled = canInteract
        btnExt2Up.Enabled = canInteract
        btnExt2Down.Enabled = canInteract
        btnExt3Up.Enabled = canInteract
        btnExt3Down.Enabled = canInteract
        btnExt4Down.Enabled = canInteract

        btnObstruct.Enabled = canInteract AndAlso
                              (elevator.CurrentDoorState = ElevatorDoorState.Opened OrElse
                               elevator.CurrentDoorState = ElevatorDoorState.Obstructed)

        btnOpen.Enabled = canInteract AndAlso
                           elevator.CurrentDoorState = ElevatorDoorState.Closed AndAlso
                           Not elevator.IsMoving

        btnClose.Enabled = canInteract AndAlso
                           (elevator.CurrentDoorState = ElevatorDoorState.Opened OrElse
                            elevator.CurrentDoorState = ElevatorDoorState.Obstructed)

        btnStart.Enabled = Not elevator.IsRunning OrElse elevator.IsPaused
        btnPause.Enabled = elevator.IsRunning AndAlso Not elevator.IsPaused
        btnStop.Enabled = elevator.IsRunning OrElse elevator.IsPaused

        '── Direction label colour ────────────────────────────────────
        Select Case elevator.CurrentDirection
            Case ElevatorDirection.Up
                lblDirVal.ForeColor = Color.Honeydew

            Case ElevatorDirection.Down
                lblDirVal.ForeColor = Color.Crimson

            Case Else
                lblDirVal.ForeColor = SystemColors.ControlText
        End Select

    End Sub

    '══════════════════════════════════════════════════════════════════
    '  SIMULATION CONTROLS
    '══════════════════════════════════════════════════════════════════
    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        If elevator.IsPaused Then
            elevator.IsPaused = False
            elevator.StatusMessage = "Resumed"
            If elevator.IsMoving Then timerMove.Start()
        Else
            elevator.IsRunning = True
            elevator.StatusMessage = "Running – waiting for requests"
        End If
        UpdateUI()
    End Sub

    Private Sub btnPause_Click(sender As Object, e As EventArgs) Handles btnPause.Click
        elevator.IsPaused = True
        timerMove.Stop()
        timerDoor.Stop()
        elevator.StatusMessage = "Paused – press Start to resume"
        UpdateUI()
    End Sub

    Private Sub btnStop_Click(sender As Object, e As EventArgs) Handles btnStop.Click
        timerMove.Stop()
        timerDoor.Stop()
        elevator.ResetToDefault()
        UpdateUI()
    End Sub

    '══════════════════════════════════════════════════════════════════
    '  DOOR LOGIC
    '══════════════════════════════════════════════════════════════════
    Private Sub DoOpenDoors()
        If elevator.IsMoving Then Exit Sub
        timerDoor.Stop()
        elevator.CurrentDoorState = ElevatorDoorState.Opened
        elevator.StatusMessage = $"Doors open – Floor {elevator.CurrentFloor}"
        UpdateUI()
        timerDoor.Start()
    End Sub

    Private Sub DoCloseDoors()
        If elevator.CurrentDoorState <> ElevatorDoorState.Opened AndAlso
           elevator.CurrentDoorState <> ElevatorDoorState.Obstructed Then Exit Sub
        timerDoor.Stop()
        elevator.CurrentDoorState = ElevatorDoorState.Closed
        elevator.StatusMessage = "Doors closed"
        UpdateUI()
        If elevator.HasAnyRequests() Then StartMoving()
    End Sub

    Private Sub DoObstruct()
        If elevator.CurrentDoorState <> ElevatorDoorState.Opened AndAlso
           elevator.CurrentDoorState <> ElevatorDoorState.Obstructed Then Exit Sub
        timerDoor.Stop()
        elevator.CurrentDoorState = ElevatorDoorState.Obstructed
        elevator.StatusMessage = "⚠ Obstruction! Doors held open…"
        UpdateUI()
        timerDoor.Interval = 3000
        timerDoor.Start()
    End Sub

    Private Sub timerDoor_Tick(sender As Object, e As EventArgs) Handles timerDoor.Tick
        If elevator.IsPaused Then Exit Sub
        If elevator.CurrentDoorState = ElevatorDoorState.Obstructed Then
            timerDoor.Stop()
            timerDoor.Start()
            Exit Sub
        End If
        timerDoor.Stop()
        DoCloseDoors()
    End Sub

    '══════════════════════════════════════════════════════════════════
    '  MOVEMENT
    '══════════════════════════════════════════════════════════════════
    Private Sub StartMoving()
        If elevator.IsMoving Then Exit Sub
        If elevator.CurrentDoorState <> ElevatorDoorState.Closed Then Exit Sub
        If Not elevator.IsRunning OrElse elevator.IsPaused Then Exit Sub

        Dim nextFloor As Integer = elevator.GetNextFloor()
        If nextFloor = -1 Then Exit Sub

        If nextFloor > elevator.CurrentFloor Then
            elevator.CurrentDirection = ElevatorDirection.Up
        ElseIf nextFloor < elevator.CurrentFloor Then
            elevator.CurrentDirection = ElevatorDirection.Down
        Else
            elevator.ClearRequestsAtFloor(elevator.CurrentFloor)
            DoOpenDoors()
            Return
        End If

        elevator.IsMoving = True
        elevator.StatusMessage = $"Departing → floor {nextFloor}…"
        UpdateUI()
        timerMove.Start()
    End Sub

    Private Sub timerMove_Tick(sender As Object, e As EventArgs) Handles timerMove.Tick
        If elevator.IsPaused OrElse Not elevator.IsRunning Then
            timerMove.Stop()
            Exit Sub
        End If

        If elevator.CurrentDoorState <> ElevatorDoorState.Closed Then
            timerMove.Stop()
            elevator.IsMoving = False
            Exit Sub
        End If

        Dim nextFloor As Integer = elevator.GetNextFloor()

        If nextFloor = -1 Then
            timerMove.Stop()
            elevator.IsMoving = False
            elevator.CurrentDirection = ElevatorDirection.Idle
            elevator.StatusMessage = "Idle – waiting for requests"
            UpdateUI()
            Exit Sub
        End If

        If nextFloor > elevator.CurrentFloor Then
            elevator.CurrentDirection = ElevatorDirection.Up
            elevator.CurrentFloor += 1
        ElseIf nextFloor < elevator.CurrentFloor Then
            elevator.CurrentDirection = ElevatorDirection.Down
            elevator.CurrentFloor -= 1
        End If

        elevator.StatusMessage = $"Moving {elevator.DirectionText()}… Floor {elevator.CurrentFloor}"

        Dim shouldStop As Boolean =
            elevator.InternalRequests.Contains(elevator.CurrentFloor) OrElse
            elevator.ExternalCalls.Any(Function(c) c.Floor = elevator.CurrentFloor)

        If shouldStop OrElse elevator.CurrentFloor = nextFloor Then
            timerMove.Stop()
            elevator.IsMoving = False
            elevator.StatusMessage = $"Ding! – Floor {elevator.CurrentFloor}"
            elevator.ClearRequestsAtFloor(elevator.CurrentFloor)
            DoOpenDoors()
        End If

        UpdateUI()
    End Sub

    '══════════════════════════════════════════════════════════════════
    '  INTERNAL FLOOR BUTTONS
    '══════════════════════════════════════════════════════════════════
    Private Sub HandleInternalRequest(floor As Integer)
        If Not elevator.IsRunning OrElse elevator.IsPaused Then Exit Sub
        elevator.AddInternalRequest(floor)
        If Not elevator.IsMoving AndAlso
           elevator.CurrentDoorState = ElevatorDoorState.Closed Then
            StartMoving()
        End If
        UpdateUI()
    End Sub

    Private Sub btnFloor1_Click(s As Object, e As EventArgs) Handles btnFloor1.Click
        HandleInternalRequest(1)
    End Sub
    Private Sub btnFloor2_Click(s As Object, e As EventArgs) Handles btnFloor2.Click
        HandleInternalRequest(2)
    End Sub
    Private Sub btnFloor3_Click(s As Object, e As EventArgs) Handles btnFloor3.Click
        HandleInternalRequest(3)
    End Sub
    Private Sub btnFloor4_Click(s As Object, e As EventArgs) Handles btnFloor4.Click
        HandleInternalRequest(4)
    End Sub

    '══════════════════════════════════════════════════════════════════
    '  EXTERNAL CALL BUTTONS  (per floor, on shaft wall)
    '══════════════════════════════════════════════════════════════════
    Private Sub HandleExternalCall(floor As Integer, dir As ElevatorDirection)
        If Not elevator.IsRunning OrElse elevator.IsPaused Then Exit Sub
        elevator.AddExternalCall(floor, dir)
        If Not elevator.IsMoving AndAlso
           elevator.CurrentDoorState = ElevatorDoorState.Closed Then
            StartMoving()
        End If
        UpdateUI()
    End Sub

    '── Floor 1 – Up only
    Private Sub btnExt1Up_Click(s As Object, e As EventArgs) Handles btnExt1Up.Click
        HandleExternalCall(1, ElevatorDirection.Up)
    End Sub

    '── Floor 2 – Up and Down
    Private Sub btnExt2Up_Click(s As Object, e As EventArgs) Handles btnExt2Up.Click
        HandleExternalCall(2, ElevatorDirection.Up)
    End Sub
    Private Sub btnExt2Down_Click(s As Object, e As EventArgs) Handles btnExt2Down.Click
        HandleExternalCall(2, ElevatorDirection.Down)
    End Sub

    '── Floor 3 – Up and Down
    Private Sub btnExt3Up_Click(s As Object, e As EventArgs) Handles btnExt3Up.Click
        HandleExternalCall(3, ElevatorDirection.Up)
    End Sub
    Private Sub btnExt3Down_Click(s As Object, e As EventArgs) Handles btnExt3Down.Click
        HandleExternalCall(3, ElevatorDirection.Down)
    End Sub

    '── Floor 4 – Down only
    Private Sub btnExt4Down_Click(s As Object, e As EventArgs) Handles btnExt4Down.Click
        HandleExternalCall(4, ElevatorDirection.Down)
    End Sub

    '══════════════════════════════════════════════════════════════════
    '  DOOR CONTROL BUTTONS
    '══════════════════════════════════════════════════════════════════
    Private Sub btnOpen_Click(s As Object, e As EventArgs) Handles btnOpen.Click
        If elevator.IsRunning AndAlso Not elevator.IsPaused Then DoOpenDoors()
    End Sub

    Private Sub btnClose_Click(s As Object, e As EventArgs) Handles btnClose.Click
        If elevator.IsRunning AndAlso Not elevator.IsPaused Then DoCloseDoors()
    End Sub

    Private Sub btnObstruct_Click(s As Object, e As EventArgs) Handles btnObstruct.Click
        If elevator.IsRunning AndAlso Not elevator.IsPaused Then DoObstruct()
    End Sub

    '══════════════════════════════════════════════════════════════════
    '  KEYBOARD SHORTCUTS
    '══════════════════════════════════════════════════════════════════
    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If Not elevator.IsRunning OrElse elevator.IsPaused Then Exit Sub

        Select Case e.KeyCode
            Case Keys.D1, Keys.NumPad1 : HandleInternalRequest(1)
            Case Keys.D2, Keys.NumPad2 : HandleInternalRequest(2)
            Case Keys.D3, Keys.NumPad3 : HandleInternalRequest(3)
            Case Keys.D4, Keys.NumPad4 : HandleInternalRequest(4)
            Case Keys.U : HandleExternalCall(elevator.CurrentFloor, ElevatorDirection.Up)
            Case Keys.D : HandleExternalCall(elevator.CurrentFloor, ElevatorDirection.Down)
            Case Keys.O
                If elevator.CurrentDoorState = ElevatorDoorState.Closed AndAlso
                   Not elevator.IsMoving Then DoOpenDoors()
            Case Keys.C : DoCloseDoors()
            Case Keys.X : DoObstruct()
            Case Keys.Q : Application.Exit()
        End Select
    End Sub

    Private Sub lblMark1_Click(sender As Object, e As EventArgs) Handles lblMark1.Click

    End Sub

    Private Sub GroupBoxgrpInside1_Enter(sender As Object, e As EventArgs) Handles GroupBoxgrpInside1.Enter

    End Sub

    Private Sub lblDirVal_Click(sender As Object, e As EventArgs) Handles lblDirVal.Click

    End Sub

    Private Sub lblDoorR_Click(sender As Object, e As EventArgs) Handles lblDoorR.Click

    End Sub
End Class