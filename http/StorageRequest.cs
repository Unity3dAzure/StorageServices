using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Unity3dAzure.StorageServices
{
	public sealed class StorageRequest : RestRequest
	{
		public StorageRequest (string url, Method method) : base (url, method)
		{
		}

		public void AuthorizeRequest (StorageServiceClient client, Method method, string resourcePath = "", Dictionary<string, string> queryParams = null, Dictionary<string, string> headers = null, int contentLength = 0)
		{
			AuthorizationHeaders authHeaders = new AuthorizationHeaders (client, method, resourcePath, queryParams, headers, contentLength);
			string stringToSign = authHeaders.ToString ();
			string signature = GetSignature (client.Key, stringToSign);
			string authorization = GetAuthorizationHeader (client.Account, signature);

			this.AddHeader ("Authorization", authorization);
			this.AddHeader ("x-ms-date", authHeaders.MSDate ());
			this.AddHeader ("x-ms-version", authHeaders.MSVersion ());

			if (headers != null) {
				this.AddHeaders (headers);
			}

			Debug.Log ("Authorized request url:" + this.request.url + "\n\nauthorization: \"" + authorization + "\"\nx-ms-date: " + authHeaders.MSDate () + "\nstringToSign:'" + stringToSign + "'");
		}

		/// <summary>
		/// Creates Signature using format Base64(HMAC-SHA256(UTF8(StringToSign)))
		/// </summary>
		/// <returns>The signature.</returns>
		/// <param name="stringToSign">String to sign.</param>
		private string GetSignature (byte[] key, string stringToSign)
		{
			return SignatureHelper.Sign (key, stringToSign);
		}

		/// <summary>
		/// Authorization header value
		/// </summary>
		/// <param name="account">Account.</param>
		/// <param name="signature">Signature.</param>
		private string GetAuthorizationHeader (string account, string signature)
		{
			return string.Format ("SharedKey {0}:{1}", account, signature);
		}
	}
}

