// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using RESTClient;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using System.Text;
using System;
using UnityEngine.Networking;

namespace Azure.StorageServices {
  public static class Auth {
    /// <summary>
    /// Factory method to generate an authorized request URL using query params. (valid up to 15 minutes)
    /// </summary>
    /// <returns>The authorized request.</returns>
    /// <param name="client">StorageServiceClient</param>
    /// <param name="httpMethod">Http method.</param>
    public static StorageRequest CreateAuthorizedStorageRequest(StorageServiceClient client, Method method, string resourcePath = "", Dictionary<string, string> queryParams = null, Dictionary<string, string> headers = null, int contentLength = 0) {
      string requestUrl = RequestUrl(client, queryParams, resourcePath);
      StorageRequest request = new StorageRequest(requestUrl, method);
      request.AuthorizeRequest(client, method, resourcePath, queryParams, headers, contentLength);
      return request;
    }

    public static StorageRequest GetAuthorizedStorageRequest(StorageServiceClient client, string resourcePath = "", Dictionary<string, string> queryParams = null, Dictionary<string, string> headers = null, int contentLength = 0) {
      return CreateAuthorizedStorageRequest(client, Method.GET, resourcePath, queryParams, headers, contentLength);
    }

    public static StorageRequest GetAuthorizedStorageRequestTexture(StorageServiceClient client, string resourcePath = "", Dictionary<string, string> queryParams = null, Dictionary<string, string> headers = null, int contentLength = 0) {
      string requestUrl = RequestUrl(client, queryParams, resourcePath);
      StorageRequest request = new StorageRequest(UnityWebRequestTexture.GetTexture(requestUrl));
      request.AuthorizeRequest(client, Method.GET, resourcePath, queryParams, headers, contentLength);
      return request;
    }

    public static StorageRequest GetAuthorizedStorageRequestAudioClip(StorageServiceClient client, string resourcePath = "", Dictionary<string, string> queryParams = null, Dictionary<string, string> headers = null, int contentLength = 0, AudioType audioType = AudioType.WAV) {
      string requestUrl = RequestUrl(client, queryParams, resourcePath);
      StorageRequest request = new StorageRequest(UnityWebRequestMultimedia.GetAudioClip(requestUrl, audioType));
      request.AuthorizeRequest(client, Method.GET, resourcePath, queryParams, headers, contentLength);
      return request;
    }

    public static StorageRequest GetAuthorizedStorageRequestAssetBundle(StorageServiceClient client, string resourcePath = "", Dictionary<string, string> queryParams = null, Dictionary<string, string> headers = null, int contentLength = 0) {
      string requestUrl = RequestUrl(client, queryParams, resourcePath);
      StorageRequest request = new StorageRequest(UnityWebRequestAssetBundle.GetAssetBundle(requestUrl));
      request.AuthorizeRequest(client, Method.GET, resourcePath, queryParams, headers, contentLength);
      return request;
    }

    private static string RequestUrl(StorageServiceClient client, Dictionary<string, string> queryParams = null, string resourcePath = "") {
      string baseUrl = client.PrimaryEndpoint();
      return UrlHelper.BuildQuery(baseUrl, queryParams, resourcePath);
    }

  }
}
