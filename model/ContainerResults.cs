// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Xml;
using System.Xml.Serialization;

namespace Azure.StorageServices {
  [Serializable]
  [XmlRoot("EnumerationResults")]
  public class ContainerResults {
    [XmlAttribute("ServiceEndpoint")]
    public string ServiceEndpoint;

    [XmlArray("Containers")]
    public Container[] Containers;
  }

  [Serializable]
  public class Container {
    public string Name;

    [XmlElement("Properties")]
    public ContainerProperties Properties;
  }

  [Serializable]
  public class ContainerProperties {
    [XmlElement("Last-Modified")]
    public string LastModified;

    public string Etag;
    public string LeaseStatus;
    public string LeaseState;
  }
}
