using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Net;
using System.Collections.Generic;
using System.Text;

namespace Unity3dAzure.StorageServices
{
	public abstract class RestRequest : IDisposable
	{
		public UnityWebRequest request { get; private set; }

		public RestRequest (string url, Method method)
		{
			request = new UnityWebRequest (url, method.ToString ());
			request.downloadHandler = new DownloadHandlerBuffer ();
		}

		public void AddHeader (string key, string value)
		{
			request.SetRequestHeader (key, value);
		}

		public void AddHeaders (Dictionary<string, string> headers)
		{
			foreach (KeyValuePair<string, string> header in headers) {
				AddHeader (header.Key, header.Value);
			}
		}

		public void AddBody (byte[] bytes, string contentType)
		{
			if (request.uploadHandler != null) {
				Debug.LogWarning ("Request body can only be set once");
				return;
			}
			request.uploadHandler = new UploadHandlerRaw (bytes);
			request.uploadHandler.contentType = contentType;
		}

		public void AddBody (string text, string contentType = "text/plain; charset=UTF-8")
		{
			byte[] bytes = Encoding.UTF8.GetBytes (text);
			this.AddBody (bytes, contentType);
		}

		public virtual void AddBody<T> (T data, string contentType = "application/json; charset=utf-8") where T : new()
		{
			string jsonString = JsonUtility.ToJson (data);
			byte[] bytes = Encoding.UTF8.GetBytes (jsonString);
			this.AddBody (bytes, contentType);
		}

		#region Response and XML object parsing

		private RestResult GetRestResult (bool expectedBodyContent = true)
		{
			HttpStatusCode statusCode = (HttpStatusCode)Enum.Parse (typeof(HttpStatusCode), request.responseCode.ToString ());
			RestResult result = new RestResult (statusCode);

			if (result.IsError) {
				result.ErrorMessage = "Response failed with status: " + statusCode.ToString ();
				return result;
			}

			if (expectedBodyContent && string.IsNullOrEmpty (request.downloadHandler.text)) {
				result.IsError = true;
				result.ErrorMessage = "Response has empty body";
				return result;
			}

			return result;
		}

		private RestResult<T> GetRestResult<T> () where T : class
		{
			HttpStatusCode statusCode = (HttpStatusCode)Enum.Parse (typeof(HttpStatusCode), request.responseCode.ToString ());
			RestResult<T> result = new RestResult<T> (statusCode);

			if (result.IsError) {
				result.ErrorMessage = "Response failed with status: " + statusCode.ToString ();
				return result;
			}

			if (string.IsNullOrEmpty (request.downloadHandler.text)) {
				result.IsError = true;
				result.ErrorMessage = "Response has empty body";
				return result;
			}

			return result;
		}

		private RestResult<T> TrySerializeXml<T> () where T : class
		{
			RestResult<T> result = GetRestResult<T> ();
			// return early if there was a status / data error other than Forbidden
			if (result.IsError && result.StatusCode == HttpStatusCode.Forbidden) {
				try {
					ErrorResult error = XMLHelper.FromXml<ErrorResult> (request.downloadHandler.text);
					Debug.LogWarning ("Authentication Failed: " + error.AuthenticationErrorDetail);
				} catch (Exception e) {
					Debug.LogWarning ("Authentication Failed: " + e.Message);
				}
				return result;
			} else if (result.IsError) {
				return result;
			}
			// otherwise try and serialize XML response text to an object
			try {
				result.AnObject = XMLHelper.FromXml<T> (request.downloadHandler.text);
			} catch (Exception e) {
				result.IsError = true;
				result.ErrorMessage = "Failed to parse object of type: " + typeof(T).ToString () + " Exception message: " + e.Message;
			}
			return result;
		}

		public void ParseXML<T> (Action<IRestResponse<T>> callback = null) where T : class
		{
			RestResult<T> result = TrySerializeXml<T> ();

			if (result.IsError) {
				Debug.LogWarning ("Response error status:" + result.StatusCode + " code:" + request.responseCode + " error:" + result.ErrorMessage + " request url:" + request.url);
				callback (new RestResponse<T> (result.ErrorMessage, result.StatusCode, request.url, request.downloadHandler.text));
			} else {
				callback (new RestResponse<T> (result.StatusCode, request.url, request.downloadHandler.text, result.AnObject));
			}
			this.Dispose ();
		}

		/// <summary>
		/// To be used with a callback which passes the response with result including status success or error code, request url and any body text.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public void Result (Action<RestResponse> callback = null)
		{
			RestResult result = GetRestResult (false);
			if (result.IsError) {
				Debug.LogWarning ("Response error status:" + result.StatusCode + " code:" + request.responseCode + " error:" + result.ErrorMessage + " request url:" + request.url);
				callback (new RestResponse (result.ErrorMessage, result.StatusCode, request.url, request.downloadHandler.text));
			} else {
				callback (new RestResponse (result.StatusCode, request.url, request.downloadHandler.text));
			}
		}

		#endregion

		public void Dispose ()
		{
			request.Dispose (); // request completed, clean-up resources
		}

	}
}
