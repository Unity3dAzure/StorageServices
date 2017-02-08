using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

namespace Unity3dAzure.StorageServices
{
	public class BlobService
	{
		private StorageServiceClient client;

		public BlobService (StorageServiceClient client)
		{
			this.client = client;
		}

		/// <summary>
		/// Lists all of the containers in a storage account.
		/// </summary>
		/// <returns>The containers.</returns>
		/// <param name="">.</param>
		public IEnumerator ListContainers (Action<IRestResponse<ContainerResults>> callback)
		{
			Dictionary<string, string> queryParams = new Dictionary<string, string> ();
			queryParams.Add ("comp", "list");
			queryParams.Add ("restype", ResType.container.ToString ());

			StorageRequest request = Auth.CreateAuthorizedStorageRequest (client, Method.GET, "", queryParams);
			yield return request.request.Send ();
			request.ParseXML<ContainerResults> (callback);
		}

		public IEnumerator ListBlobs (Action<IRestResponse<BlobResults>> callback, string resourcePath = "")
		{
			Dictionary<string, string> queryParams = new Dictionary<string, string> ();
			queryParams.Add ("comp", "list");
			queryParams.Add ("restype", ResType.container.ToString ());

			StorageRequest request = Auth.CreateAuthorizedStorageRequest (client, Method.GET, resourcePath, queryParams);
			yield return request.request.Send ();
			request.ParseXML<BlobResults> (callback);
		}

		public IEnumerator GetTextBlob (Action<RestResponse> callback, string resourcePath = "")
		{
			// public request
			string url = UrlHelper.BuildQuery (client.SecondaryEndpoint (), "", resourcePath);
			StorageRequest request = new StorageRequest (url, Method.GET);
			yield return request.request.Send ();
			request.Result (callback);
		}

		public IEnumerator PutTextBlob (Action<RestResponse> callback, string text, string resourcePath, string filename, string contentType = "text/plain; charset=UTF-8")
		{
			byte[] bytes = Encoding.UTF8.GetBytes (text);
			return PutBlobBytes (callback, bytes, resourcePath, filename, contentType);
		}

		public IEnumerator PutImageBlob (Action<RestResponse> callback, byte[] bytes, string resourcePath, string filename, string contentType = "image/png")
		{
			return PutBlobBytes (callback, bytes, resourcePath, filename, contentType);
		}

		public IEnumerator PutAudioBlob (Action<RestResponse> callback, byte[] bytes, string resourcePath, string filename, string contentType = "audio/wav")
		{
			return PutBlobBytes (callback, bytes, resourcePath, filename, contentType);
		}

		public IEnumerator PutAssetBundle (Action<RestResponse> callback, byte[] bytes, string resourcePath, string filename, string contentType = "application/octet-stream")
		{
			return PutBlobBytes (callback, bytes, resourcePath, filename, contentType);
		}

		private IEnumerator PutBlobBytes (Action<RestResponse> callback, byte[] bytes, string resourcePath, string filename, string contentType)
		{
			int contentLength = bytes.Length;
			// TODO: check size is ok?
			Dictionary<string, string> headers = new Dictionary<string, string> ();

			string file = Path.GetFileName (filename);

			headers.Add ("Content-Type", contentType);
			headers.Add ("x-ms-blob-content-disposition", string.Format ("attachment; filename=\"{0}\"", file));
			headers.Add ("x-ms-blob-type", "BlockBlob");

			string filePath = resourcePath.Length > 0 ? resourcePath + "/" + file : file;

			StorageRequest request = Auth.CreateAuthorizedStorageRequest (client, Method.PUT, filePath, null, headers, contentLength);

			// add body
			request.AddBody (bytes, contentType);

			yield return request.request.Send ();
			request.Result (callback);
		}
	}
}