# Azure Storage Services for Unity3d

Save and load image textures, audio files, json or xml data files for use in
Unity. You can also store Unity Asset Bundles to load Prefabs with referenced
scripts to work in your game / application.

## External dependencies

**First download the shared
[REST Client library for Unity](https://github.com/Unity3dAzure/RESTClient) and
extract the contents into your Unity project "Assets" folder.**

* [RESTClient](https://github.com/Unity3dAzure/RESTClient)

## Requirements

Unity 2017.2 recommended. Unity v5.3 or greater required as
[UnityWebRequest](https://docs.unity3d.com/Manual/UnityWebRequest.html) and
[JsonUtility](https://docs.unity3d.com/ScriptReference/JsonUtility.html)
features are used. Unity will be extending platform support for UnityWebRequest
so keep Unity up to date if you need to support these additional platforms.

## Azure Blob Storage Demos for Unity 2017.2

Try the
[Azure Storage Services Demos](https://github.com/Unity3dAzure/StorageServicesDemo)
project for Unity on Mac / Windows. (The demo project has got everything already
bundled in and does not require any additional assets to work. Just wire it up
with your Azure Storage Service and public Blob container and run it right
inside the Unity Editor.)

## How to setup Storage Services with a new Unity project

1. [Download StorageServices](https://github.com/Unity3dAzure/StorageServices/archive/master.zip)
   and
   [REST Client](https://github.com/Unity3dAzure/RESTClient/archive/master.zip)
   for Unity.
   * Copy 'StorageServices' and 'RESTClient' into your Unity project's `Assets`
     folder.
2. Create [Azure](https://portal.azure.com) Storage Service

## Supported platforms

Intended to work on all the platforms
[UnityWebRequest](https://docs.unity3d.com/Manual/UnityWebRequest.html) supports
including:

* Unity Editor and Standalone players
* iOS
* Android
* Windows

## Notice

This library is in beta so not all APIs are supported yet and some things may
change.

Questions or tweet [@deadlyfingers](https://twitter.com/deadlyfingers)
