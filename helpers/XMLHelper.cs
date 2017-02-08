using System;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;

namespace Unity3dAzure.StorageServices
{
	public class XMLHelper
	{
		public static XmlDocument LoadResourceDocument (string filename)
		{
			string xml = LoadResourceText (filename);
			XmlDocument doc = new XmlDocument ();
			doc.LoadXml (xml);
			return doc;
		}

		public static string LoadResourceText (string filename)
		{
			TextAsset contents = (TextAsset)Resources.Load (filename);
			return contents.text;
		}

		public static string LoadAsset (string filepath, string extension = ".xml")
		{
			string path = Path.Combine (Application.dataPath, filepath + extension);
			return File.ReadAllText (path);
		}

		public static T FromXml<T> (string xml) where T : class
		{
			XmlSerializer serializer = new XmlSerializer (typeof(T));
			using (var stream = new MemoryStream (Encoding.UTF8.GetBytes (xml))) {
				return (T)serializer.Deserialize (stream);
			}
		}
	}
}
