// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Xml;
using System.Xml.Serialization;

namespace Azure.StorageServices {
  [Serializable]
  [XmlRoot("EnumerationResults")]

  public class BlobResults {
    [XmlAttribute("ServiceEndpoint")]
    public string ServiceEndpoint;

    [XmlAttribute("ContainerName")]
    public string ContainerName;

    [XmlArray("Blobs")]
    public Blob[] Blobs;
  }

  [Serializable]
  public class Blob {
    public string Name;

    [XmlElement("Properties")]
    public BlobProperties Properties;
  }

  [Serializable]
  public class BlobProperties {
    [XmlElement("Last-Modified")]
    public string LastModified;

    public string Etag;

    [XmlElement("Content-Length")]
    public string ContentLength;

    [XmlElement("Content-Type")]
    public string ContentType;

    [XmlElement("Content-Encoding")]
    public string ContentEncoding;

    [XmlElement("Content-Language")]
    public string ContentLanguage;

    [XmlElement("Content-MD5")]
    public string ContentMD5;

    [XmlElement("Cache-Control")]
    public string CacheControl;

    [XmlElement("Content-Disposition")]
    public string ContentDisposition;

    public string BlobType;
    public string LeaseStatus;
    public string LeaseState;
    public bool ServerEncrypted;
  }
}
