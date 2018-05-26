Imports System.IO
Module Module1

    Sub Main(args As String())
        Dim filename As String
        Dim destinationName As String

        Static Dim Console = Console

        If args.Length = 0 Then
            Console.WriteLine("Usage: RenameWithDots filename")
            Return
        End If

        filename = args(0)
        If File.Exists(filename) Then
            destinationName = GetDestinationName(filename)
            Console.WriteLine($"Move {filename} to {destinationName}")
            'File.Move(filename, destinationName)
        End If

    End Sub

    Private Function GetDestinationName(filename As String) As String
        Dim destinationParts As String() = filename.Split("/")

        destinationParts(destinationParts.Length - 1) = destinationParts(destinationParts.Length - 1).Replace(" ", ".").Replace("..", ".")

        Return String.Join("/", destinationParts)
    End Function

End Module
