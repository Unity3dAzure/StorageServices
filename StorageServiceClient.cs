// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using RESTClient;
using System;
using System.Text.RegularExpressions;

namespace Azure.StorageServices {
  public sealed class StorageServiceClient : RestClient {
    private string account;

    public string Account {
      get {
        return account;
      }
    }

    private byte[] key;

    public byte[] Key {
      get {
        return key;
      }
    }

    // https://docs.microsoft.com/en-us/rest/api/storageservices/fileservices/versioning-for-the-azure-storage-services
    private string version;

    public string Version {
      get {
        return version;
      }
    }

    public StorageServiceClient(string url, string accessKey, string version = "2017-04-17", string account = "") : base(url) {
      this.version = version;
      if (!string.IsNullOrEmpty(accessKey)) {
        this.key = Convert.FromBase64String(accessKey);
      }
      if (!string.IsNullOrEmpty(account)) {
        this.account = account;
      } else {
        this.account = GetAccountName(url);
      }
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Unity3dAzure.StorageServices.StorageServiceClient"/> class.
    /// </summary>
    /// <param name="account">Storage account name.</param>
    /// <param name="accessKey">Access key.</param>
    public static StorageServiceClient Create(string account, string accessKey, string version = "2017-04-17") {
      string url = GetPrimaryEndpoint(account);
      return new StorageServiceClient(url, accessKey, version, account);
    }

    public BlobService GetBlobService() {
      return new BlobService(this);
    }

    public string PrimaryEndpoint() {
      return GetPrimaryEndpoint(account);
    }

    public string SecondaryEndpoint() {
      return GetSecondaryEndpoint(account);
    }

    private static string GetPrimaryEndpoint(string account) {
      return "https://" + account + ".blob.core.windows.net/";
    }

    private static string GetSecondaryEndpoint(string account) {
      return "https://" + account + ".blob.core.windows.net/";
    }

    private string GetAccountName(string url) {
      var match = Regex.Match(url, @"^https?:\/\/([a-z0-9]+)", RegexOptions.IgnoreCase);
      if (match.Groups.Count == 2 && match.Groups[1].Value.Length > 0) {
        return match.Groups[1].Value;
      }
      return url;
    }

  }
}
