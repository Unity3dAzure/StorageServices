using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using System.Text;
using System;

namespace Unity3dAzure.StorageServices
{
	public static class Auth
	{
		/// <summary>
		/// Factory method to generate an authorized request URL using query params. (valid up to 15 minutes)
		/// </summary>
		/// <returns>The authorized request.</returns>
		/// <param name="client">StorageServiceClient</param>
		/// <param name="httpMethod">Http method.</param>
		public static StorageRequest CreateAuthorizedStorageRequest (StorageServiceClient client, Method method, string resourcePath = "", Dictionary<string, string> queryParams = null, Dictionary<string, string> headers = null, int contentLength = 0)
		{
			string baseUrl = client.PrimaryEndpoint ();
			string requestUrl = UrlHelper.BuildQuery (baseUrl, queryParams, resourcePath);
			StorageRequest request = new StorageRequest (requestUrl, method);
			request.AuthorizeRequest (client, method, resourcePath, queryParams, headers, contentLength);
			return request;
		}


	}
}
