// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using RESTClient;
using System.Text;
using System.Collections.Generic;
using System;

namespace Azure.StorageServices {
  public class AuthorizationHeaders {
    private string method;
    private CanonicalizedHeaders canonicalizedHeaders;
    private string canonicalizedResource;

    private Dictionary<string, string> authHeaders = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) {
      { "Content-Encoding", "" },
      { "Content-Language", "" },
      { "Content-Length", "" },
      { "Content-MD5", "" },
      { "Content-Type", "" },
      { "Date", "" },
      { "If-Modified-Since", "" },
      { "If-Match", "" },
      { "If-None-Match", "" },
      { "If-Unmodified-Since", "" },
      { "Range", "" }
    };

    public AuthorizationHeaders(StorageServiceClient client, Method method, string resourcePath = "", Dictionary<string, string> queryParams = null, Dictionary<string, string> headers = null, int contentLength = 0) {
      string path = resourcePath;
      this.method = method.ToString();
      this.canonicalizedHeaders = new CanonicalizedHeaders(client.Version, headers);

      if (queryParams != null) {
        path = resourcePath + BuildQueryString(queryParams);
      }

      if (headers != null) {
        UpdateHeaderValues(headers);
      }

      if (contentLength > 0) {
        authHeaders["Content-Length"] = contentLength.ToString();
      }

      // account followed by url encoded resource path, and query params
      this.canonicalizedResource = string.Format("/{0}/{1}", client.Account, path);
    }

    private string BuildQueryString(Dictionary<string, string> queryParams) {
      StringBuilder q = new StringBuilder();
      foreach (KeyValuePair<string, string> param in queryParams) {
        q.Append("\n" + param.Key + ":" + param.Value);
      }
      return q.ToString();
    }

    private void UpdateHeaderValues(Dictionary<string, string> headers) {
      foreach (KeyValuePair<string, string> header in headers) {
        if (authHeaders.ContainsKey(header.Key)) {
          authHeaders[header.Key] = header.Value;
        }
      }
    }

    public string MSDate() {
      return canonicalizedHeaders.MSDate;
    }

    public string MSVersion() {
      return canonicalizedHeaders.MSVersion;
    }

    /// <summary>
    /// Returns string to sign
    /// https://docs.microsoft.com/en-us/rest/api/storageservices/fileservices/authentication-for-the-azure-storage-services
    /// </summary>
    public override string ToString() {
      StringBuilder sb = new StringBuilder();
      sb.Append(method + "\n");
      foreach (KeyValuePair<string, string> authHeader in authHeaders) {
        sb.Append(authHeader.Value + "\n");
      }
      sb.Append(canonicalizedHeaders);
      sb.Append(canonicalizedResource);
      return sb.ToString();
    }
  }
}
