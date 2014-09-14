using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Net;
using System.Text;
using System.Threading;
using ICSharpCode.SharpZipLib.Zip;
	
public class GUIUnzipper : MonoBehaviour{

	private static bool isUnzipped = false;	
		
	public static void unzipFolder() {

		if (!isUnzipped) {
			string docPath = Application.persistentDataPath;

			// Copy GUI.zip into application directory
			docPath = docPath + "/GUI.zip";

			// ALEX CHANGE - now loads from resources rather than WWW
			TextAsset textAsset = Resources.Load("backupGUI", typeof(TextAsset)) as TextAsset;
			System.IO.File.WriteAllBytes(docPath, textAsset.bytes);

			// Unzip the data within the app
			using (ZipInputStream s = new ZipInputStream(File.OpenRead(docPath))) {
				ZipEntry theEntry;
				while ((theEntry = s.GetNextEntry()) != null) {
					Console.WriteLine(theEntry.Name);
					
					string directoryName = Path.GetDirectoryName(theEntry.Name);
					string fileName      = Path.GetFileName(theEntry.Name);
					
					// Create directory
					if ((directoryName.Length > 0) && (!Directory.Exists(Application.persistentDataPath + "/Cached/Skins/" + directoryName))) {
						Directory.CreateDirectory(Application.persistentDataPath + "/Cached/Skins/" + directoryName);
					}
					// Unzip files into created cirectory
					if (fileName != String.Empty) {
						string filename = docPath.Substring(0, docPath.Length - 8);
						filename = filename +"/"+theEntry.Name;
						using (FileStream streamWriter = new FileStream(Application.persistentDataPath + "/Cached/Skins/" + theEntry.Name, FileMode.CreateNew)) {
							int size = 2048;
							byte[] fdata = new byte[2048];
							while (true) {
								size = s.Read(fdata, 0, fdata.Length);
								if (size > 0) {
									streamWriter.Write(fdata, 0, size);
								}
								else {
									break;
								}
							}
						}	
					}
				}
				isUnzipped = true;
				Debug.Log("Load of GUI.zip complete");
			}
		}	
	}
}
