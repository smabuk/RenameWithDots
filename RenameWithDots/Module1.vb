Imports System.IO
Module Module1

    Function Main(ByVal args As String()) As Integer
        Dim filename As String
        Dim destinationName As String

        If args.Length <> 1 Then
            Console.WriteLine("Usage: RenameWithDots filename")
            Return 0
        End If

        filename = args(0)
        If File.Exists(filename) Then
            destinationName = GetDestinationName(filename)
            Try
                File.Move(filename, destinationName)
                Console.WriteLine($"Moved ""{filename}"" to ""{destinationName}""")
            Catch ex As Exception
                Console.WriteLine($"*** FAILED move of ""{filename}"" to ""{destinationName}"" ***")
                Console.WriteLine($"{ex.Message}")
                Return 255
            End Try
        Else
            Console.WriteLine($"*** FAILED filename ""{filename}"" does not exist")
        End If
        Return 0
    End Function

    Private Function GetDestinationName(filename As String) As String
        Dim destinationParts As String() = filename.Split("\")

        destinationParts(destinationParts.Length - 1) = destinationParts(destinationParts.Length - 1).Replace(" ", ".").Replace("..", ".").Replace(".-.", ".")

        Return String.Join("\", destinationParts)
    End Function

End Module
