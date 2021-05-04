Imports System.IO

Module Module1

    Function Main(ByVal args As String()) As Integer
        Dim destinationName As String

        If args.Length = 0 OrElse args(0) = "/h" OrElse args(0) = "/H" OrElse args(0) = "/?" Then
            Console.WriteLine("Usage: RenameWithDots filenames")
            Return 0
        End If

        Dim files As IEnumerable(Of String)
        files = GetFileList(args(0))
        For Each filename As String In files
            If File.Exists(filename) Then
                destinationName = GetDestinationName(filename)
                If destinationName <> filename Then
                    Try
                        File.Move(filename, destinationName)
                        Console.WriteLine($"Moved   ""{filename}""   =>   ""{destinationName}""")
                    Catch ex As Exception
                        Console.WriteLine($"*** FAILED move of ""{filename}""   =>   ""{destinationName}"" ***")
                        Console.Write($"    {ex.Message}")
                    End Try
                Else
                    Console.WriteLine($"Skipped ""{filename}""")
                End If
            Else
                Console.WriteLine($"*** FAILED filename ""{filename}"" does not exist")
            End If
        Next

        Return 0
    End Function

    Private Iterator Function GetFileList(ByVal pattern As String) As IEnumerable(Of String)

        Dim sourcePath As String
        Dim filePattern As String
        If Directory.Exists(pattern) Then
            sourcePath = pattern
            filePattern = "*"
        Else
            sourcePath = Path.GetDirectoryName(pattern)
            filePattern = Path.GetFileName(pattern)
            If String.IsNullOrWhiteSpace(sourcePath) Then
                sourcePath = Directory.GetCurrentDirectory()
            End If
            If String.IsNullOrWhiteSpace(filePattern) Then
                filePattern = "*"
            End If
        End If

        Dim files As IEnumerable(Of String)
        Try
            files = Directory.EnumerateFiles(sourcePath, filePattern)
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Return
        End Try

        For Each filename As String In files
            Yield filename
        Next
    End Function



    Private Function GetDestinationName(filename As String) As String
        Dim destinationParts As String() = filename.Split(Path.PathSeparator)

        destinationParts(destinationParts.Length - 1) = destinationParts(destinationParts.Length - 1) _
            .Replace("  ", " ") _
            .Replace(" ", ".") _
            .Replace("..", ".") _
            .Replace(".-.", ".")
        Return Path.Combine(destinationParts)
    End Function

End Module
