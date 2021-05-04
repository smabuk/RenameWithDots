using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace dots {
	class Program {
		static int Main(string[] args) {
			string destinationFilename;

			if (args.Length == 0 || new string[] { "/?", "/h", "/H", "--help" }.Contains(args[0])  ) {
				Console.WriteLine("Usage: dots [path]filename");
				return 0;
			}

			IEnumerable<string> filenames = GetFileList(args[0]);
			foreach (string filename in filenames) {
				if (File.Exists(filename)) {
					destinationFilename = GetDestinationFilename(filename);
					if (destinationFilename != filename) {
						try {
							File.Move(filename, destinationFilename);
							Console.WriteLine($"Moved   \"{filename}\"   =>   \"{destinationFilename}\"");
						} catch (Exception ex) {
							Console.WriteLine($"*** FAILED *** move of \"{filename}\"   =>   \"{destinationFilename}\"");
							Console.WriteLine($"    {ex.Message}");
						}
					} else {
						Console.WriteLine($"Skipped \"{filename}\"");
					}
				} else {
					Console.WriteLine($"*** FAILED *** filename \"{filename}\" does not exist");
				}
			}

			return 0;
		}

		private static IEnumerable<string> GetFileList(string pattern) {
			string sourcePath = pattern;
			string filePattern = "";

			if (!Directory.Exists(pattern)) {
				if (String.IsNullOrWhiteSpace(Path.GetDirectoryName(pattern))) {
					sourcePath = Directory.GetCurrentDirectory();
				} else {
					sourcePath = Path.GetDirectoryName(pattern) ?? "";
				}
				filePattern = Path.GetFileName(pattern) ?? "";
			}

			IEnumerable<string> filenames = new List<string>();
			try {
				filenames = Directory.EnumerateFiles(sourcePath, filePattern);
			} catch (Exception ex) {
				Console.Write(ex.Message);
			}

			foreach (string filename in filenames) {
				yield return filename;
			}

		}

		private static string GetDestinationFilename(string filename) {
			string[] destinationParts = filename.Split(Path.DirectorySeparatorChar);
			destinationParts[^1] = destinationParts[^1]
				.Replace("  ", " ")
				.Replace(" ", ".")
				.Replace("..", ".")
				.Replace(".-.", ".");
			return Path.Combine(destinationParts);
		}
	}
}
