using UnityEngine;
using System.IO;
using System.Text;
using DevCore.Core;

namespace DevCore.Core {
	public static class SaveUtility {
		#region Properties
		public static char fileSeparator => Path.DirectorySeparatorChar;

		public static string defaultSavePath {
			get { return Application.persistentDataPath; }
		}
		#endregion


		#region Json
		/// <summary>
		/// Write text content in the target specified file
		/// </summary>
		/// <param name="fileInfo">The target file informations</param>
		/// <param name="content">The text content to write</param>
		public static void WriteText(FileInfo fileInfo, string content) {
			byte[] data = new UTF8Encoding(true).GetBytes(content);
			WriteBytes(fileInfo, data);
		}

		/// <summary>
		/// Write byte content in the target specified file
		/// </summary>
		/// <param name="fileInfo">The target file informations</param>
		/// <param name="content">The byte content to write</param>
		public static void WriteBytes(FileInfo fileInfo, byte[] content) {
			string fullPath = fileInfo.fullFilePath;
			if (!Directory.Exists(fileInfo.path)) {
				Directory.CreateDirectory(fileInfo.path);
			}

			FileStream fs;
			if (!File.Exists(fullPath)) {
				fs = File.Create(fullPath);
			} else {
				fs = File.OpenWrite(fullPath);
			}
			
			fs.SetLength(0); //Clear content
			fs.Write(content, 0, content.Length);
			fs.Close();
		}

		/// <summary>
		/// Read text content from a source file
		/// </summary>
		/// <param name="fileInfo">The source file informations</param>
		/// <returns></returns>
		/// <exception cref="FileNotFoundException"></exception>
		public static string ReadText(FileInfo fileInfo) {
			byte[] data = ReadBytes(fileInfo);
			return new UTF8Encoding(true).GetString(data, 0, data.Length);
		}
		
		/// <summary>
		/// Read byte content from a source file
		/// </summary>
		/// <param name="fileInfo">The source file informations</param>
		/// <returns></returns>
		/// <exception cref="FileNotFoundException"></exception>
		public static byte[] ReadBytes(FileInfo fileInfo) {
			string path = fileInfo.fullFilePath;
			if (!File.Exists(path)) {
				throw new FileNotFoundException($"[Save Utility] The file you want to read [{path}] doesn't exist");
			}

			var fs = new FileStream(path, FileMode.Open);
			byte[] data = new byte[fs.Length];
			fs.Read(data, 0, data.Length);
			fs.Close();
			return data;
		}
		#endregion


		#region File Utility
		public static string ConcatFilePath(string name, string extension) {
			return ConcatFilePath(defaultSavePath, name, extension);
		}

		public static string ConcatFilePath(string path, string name, string extension) {
			return path + fileSeparator + name + '.' + extension;
		}
		#endregion
	}
}

public struct FileInfo {
	#region Datas
	public string fileName;
	public string path;
	public string extension;
	#endregion


	#region Properties
	public string fullFilePath => SaveUtility.ConcatFilePath(path, fileName, extension);
	#endregion


	#region Construction
	public FileInfo(string fileName, string extension) {
		path = SaveUtility.defaultSavePath;
		this.fileName = fileName;
		this.extension = extension;
	}

	public FileInfo(string path, string fileName, string extension) {
		this.path = path;
		this.fileName = fileName;
		this.extension = extension;
	}
	#endregion
}