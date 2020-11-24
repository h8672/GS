using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object icon link.
/// Use IconFiller to receive Icon for each preview?
/// </summary>
[System.Serializable]
public struct ObjectIconLink
{
    public GS.Data.ObjectData data;
    public Texture iconImage;
    public Texture borderImage;
}
