using System;
using System.Xml;
using System.Xml.Serialization;

namespace Unity3dAzure.StorageServices
{
	[Serializable]
	[XmlRoot ("Error")]
	public class ErrorResult
	{
		public string Code;
		public string Message;
		public string AuthenticationErrorDetail;
	}
}