Imports System.Collections.Generic
Imports System.Linq

Namespace ElevatorSim

    '── Enums ─────────────────────────────────────────────────────────
    Public Enum ElevatorDirection
        Up
        Down
        Idle
    End Enum

    Public Enum ElevatorDoorState
        Opened
        Closed
        Opening
        Closing
        Obstructed
    End Enum

    '── External call data ────────────────────────────────────────────
    Public Class ExternalCall
        Public Floor As Integer
        Public Dir As ElevatorDirection

        Public Sub New(f As Integer, d As ElevatorDirection)
            Floor = f
            Dir = d
        End Sub
    End Class

    '── Main elevator class ───────────────────────────────────────────
    Public Class ElevatorCar

        '── State ─────────────────────────────────────────────────────
        Public CurrentFloor As Integer = 1
        Public CurrentDirection As ElevatorDirection = ElevatorDirection.Idle
        Public CurrentDoorState As ElevatorDoorState = ElevatorDoorState.Closed
        Public IsMoving As Boolean = False
        Public IsPaused As Boolean = False
        Public IsRunning As Boolean = False
        Public StatusMessage As String = "Stopped – press Start"

        '── Queues ────────────────────────────────────────────────────
        Public InternalRequests As New HashSet(Of Integer)()
        Public ExternalCalls As New List(Of ExternalCall)()

        '── Add a request from inside the elevator ────────────────────
        Public Sub AddInternalRequest(floor As Integer)
            If floor < 1 OrElse floor > 4 Then
                StatusMessage = "Invalid floor"
                Exit Sub
            End If
            If floor = CurrentFloor AndAlso CurrentDoorState = ElevatorDoorState.Opened Then
                StatusMessage = $"Already at floor {floor} – doors open"
                Exit Sub
            End If
            InternalRequests.Add(floor)
            StatusMessage = $"Floor {floor} queued (internal)"
        End Sub

        '── Add a call from a floor panel ─────────────────────────────
        Public Sub AddExternalCall(floor As Integer, dir As ElevatorDirection)
            If floor < 1 OrElse floor > 4 Then Exit Sub
            If ExternalCalls.Any(Function(c) c.Floor = floor AndAlso c.Dir = dir) Then Exit Sub
            ExternalCalls.Add(New ExternalCall(floor, dir))
            StatusMessage = $"External call: floor {floor} {dir}"
        End Sub

        '── Any pending work? ─────────────────────────────────────────
        Public Function HasAnyRequests() As Boolean
            Return InternalRequests.Count > 0 OrElse ExternalCalls.Count > 0
        End Function

        '── Which floor to head for next? ─────────────────────────────
        Public Function GetNextFloor() As Integer
            Dim candidates As New List(Of Integer)()

            If CurrentDirection = ElevatorDirection.Up Then
                candidates.AddRange(InternalRequests.Where(Function(f) f > CurrentFloor))
                candidates.AddRange(ExternalCalls _
                    .Where(Function(c) c.Floor > CurrentFloor) _
                    .Select(Function(c) c.Floor))
            ElseIf CurrentDirection = ElevatorDirection.Down Then
                candidates.AddRange(InternalRequests.Where(Function(f) f < CurrentFloor))
                candidates.AddRange(ExternalCalls _
                    .Where(Function(c) c.Floor < CurrentFloor) _
                    .Select(Function(c) c.Floor))
            End If

            '── Nothing in current direction → take all remaining
            If candidates.Count = 0 Then
                candidates.AddRange(InternalRequests)
                candidates.AddRange(ExternalCalls.Select(Function(c) c.Floor))
            End If

            If candidates.Count = 0 Then Return -1

            If CurrentDirection = ElevatorDirection.Down Then Return candidates.Max()
            Return candidates.Min()
        End Function

        '── Remove all requests for a served floor ────────────────────
        Public Sub ClearRequestsAtFloor(floor As Integer)
            InternalRequests.Remove(floor)
            ExternalCalls.RemoveAll(Function(c) c.Floor = floor)
        End Sub

        '── Reset everything to floor 1, idle, closed ─────────────────
        Public Sub ResetToDefault()
            CurrentFloor = 1
            CurrentDirection = ElevatorDirection.Idle
            CurrentDoorState = ElevatorDoorState.Closed
            IsMoving = False
            IsPaused = False
            IsRunning = False
            StatusMessage = "Stopped – press Start"
            InternalRequests.Clear()
            ExternalCalls.Clear()
        End Sub

        '── Direction display string ───────────────────────────────────
        Public Function DirectionText() As String
            Select Case CurrentDirection
                Case ElevatorDirection.Up : Return "▲ Up"
                Case ElevatorDirection.Down : Return "▼ Down"
                Case Else : Return "● Idle"
            End Select
        End Function

        '── Pending queue display string ──────────────────────────────
        Public Function PendingText() As String
            Dim intPart As String = If(InternalRequests.Count > 0,
                "Int: " & String.Join(",", InternalRequests.OrderBy(Function(f) f)),
                "Int: –")

            Dim extPart As String = If(ExternalCalls.Count > 0,
                "Ext: " & String.Join(", ", ExternalCalls.Select(
                    Function(c) $"{c.Floor}{If(c.Dir = ElevatorDirection.Up, "↑", "↓")}")),
                "Ext: –")

            Return intPart & "   " & extPart
        End Function

    End Class

End Namespace