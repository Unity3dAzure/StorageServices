// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Xml;
using System.Xml.Serialization;

namespace Azure.StorageServices {
  [Serializable]
  [XmlRoot("Error")]
  public class ErrorResult {
    public string Code;
    public string Message;
    public string AuthenticationErrorDetail;
  }
}
