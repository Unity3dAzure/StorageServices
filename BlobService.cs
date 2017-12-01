// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using RESTClient;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

namespace Azure.StorageServices {
  public class BlobService {
    private StorageServiceClient client;

    public BlobService(StorageServiceClient client) {
      this.client = client;
    }

    /// <summary>
    /// Lists all of the containers in a storage account.
    /// </summary>
    /// <returns>The containers.</returns>
    /// <param name="">.</param>
    public IEnumerator ListContainers(Action<IRestResponse<ContainerResults>> callback) {
      Dictionary<string, string> queryParams = new Dictionary<string, string>();
      queryParams.Add("comp", "list");
      queryParams.Add("restype", ResType.container.ToString());
      StorageRequest request = Auth.CreateAuthorizedStorageRequest(client, Method.GET, "", queryParams);
      yield return request.Send();
      request.ParseXML<ContainerResults>(callback);
    }

    /// <summary>
    /// Lists all of the blobs in a container.
    /// </summary>
    /// <returns>The containers.</returns>
    /// <param name="">.</param>
    public IEnumerator ListBlobs(Action<IRestResponse<BlobResults>> callback, string resourcePath = "") {
      Dictionary<string, string> queryParams = new Dictionary<string, string>();
      queryParams.Add("comp", "list");
      queryParams.Add("restype", ResType.container.ToString());
      StorageRequest request = Auth.CreateAuthorizedStorageRequest(client, Method.GET, resourcePath, queryParams);
      yield return request.Send();
      request.ParseXML<BlobResults>(callback);
    }

    #region Download blobs

    public IEnumerator GetTextBlob(Action<RestResponse> callback, string resourcePath = "") {
      StorageRequest request = Auth.GetAuthorizedStorageRequest(client, resourcePath);
      yield return request.Send();
      request.GetText(callback);
    }

    public IEnumerator GetJsonBlob<T>(Action<IRestResponse<T>> callback, string resourcePath = "") {
      StorageRequest request = Auth.GetAuthorizedStorageRequest(client, resourcePath);
      yield return request.Send();
      request.ParseJson(callback);
    }

    public IEnumerator GetJsonArrayBlob<T>(Action<IRestResponse<T[]>> callback, string resourcePath = "") {
      StorageRequest request = Auth.GetAuthorizedStorageRequest(client, resourcePath);
      yield return request.Send();
      request.ParseJsonArray(callback);
    }

    public IEnumerator GetXmlBlob<T>(Action<IRestResponse<T>> callback, string resourcePath = "") {
      StorageRequest request = Auth.GetAuthorizedStorageRequest(client, resourcePath);
      yield return request.Send();
      request.ParseXML<T>(callback);
    }

    public IEnumerator GetImageBlob(Action<IRestResponse<Texture>> callback, string resourcePath = "") {
      StorageRequest request = Auth.GetAuthorizedStorageRequestTexture(client, resourcePath);
      yield return request.Send();
      request.GetTexture(callback);
    }

    public IEnumerator GetAudioBlob(Action<IRestResponse<AudioClip>> callback, string resourcePath = "") {
      StorageRequest request = Auth.GetAuthorizedStorageRequestAudioClip(client, resourcePath);
      yield return request.Send();
      request.GetAudioClip(callback);
    }

    public IEnumerator GetAssetBundle(Action<IRestResponse<AssetBundle>> callback, string resourcePath = "") {
      StorageRequest request = Auth.GetAuthorizedStorageRequestAssetBundle(client, resourcePath);
      yield return request.Send();
      request.GetAssetBundle(callback);
    }

    public IEnumerator GetBlob(Action<IRestResponse<byte[]>> callback, string resourcePath = "") {
      StorageRequest request = Auth.GetAuthorizedStorageRequest(client, resourcePath);
      yield return request.Send();
      request.GetBytes(callback);
    }

    #endregion

    #region Upload blobs

    public IEnumerator PutTextBlob(Action<RestResponse> callback, string text, string resourcePath, string filename, string contentType = "text/plain; charset=UTF-8") {
      byte[] bytes = Encoding.UTF8.GetBytes(text);
      return PutBlob(callback, bytes, resourcePath, filename, contentType);
    }

    public IEnumerator PutImageBlob(Action<RestResponse> callback, byte[] bytes, string resourcePath, string filename, string contentType = "image/png") {
      return PutBlob(callback, bytes, resourcePath, filename, contentType);
    }

    public IEnumerator PutAudioBlob(Action<RestResponse> callback, byte[] bytes, string resourcePath, string filename, string contentType = "audio/wav") {
      return PutBlob(callback, bytes, resourcePath, filename, contentType);
    }

    public IEnumerator PutAssetBundle(Action<RestResponse> callback, byte[] bytes, string resourcePath, string filename, string contentType = "application/octet-stream") {
      return PutBlob(callback, bytes, resourcePath, filename, contentType);
    }

    public IEnumerator PutBlob(Action<RestResponse> callback, byte[] bytes, string resourcePath, string filename, string contentType, Method method = Method.PUT) {
      int contentLength = bytes.Length; // TODO: check size is ok?
      Dictionary<string, string> headers = new Dictionary<string, string>();
      string file = Path.GetFileName(filename);

      headers.Add("Content-Type", contentType);
      headers.Add("x-ms-blob-content-disposition", string.Format("attachment; filename=\"{0}\"", file));
      headers.Add("x-ms-blob-type", "BlockBlob");

      string filePath = resourcePath.Length > 0 ? resourcePath + "/" + file : file;
      StorageRequest request = Auth.CreateAuthorizedStorageRequest(client, method, filePath, null, headers, contentLength);
      request.AddBody(bytes, contentType);
      yield return request.Send();
      request.Result(callback);
    }

    #endregion

    public IEnumerator DeleteBlob(Action<RestResponse> callback, string resourcePath, string filename) {
      string filePath = resourcePath.Length > 0 ? resourcePath + "/" + filename : filename;
      StorageRequest request = Auth.CreateAuthorizedStorageRequest(client, Method.DELETE, filePath);
      yield return request.Send();
      request.Result(callback);
    }
  }
}
