using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplySkyTexture : MonoBehaviour
{
    public Material skyMaterial;      // SkyInteriorMaterial
    public Texture skyTexture;        // ‰F’ˆ‰æ‘œ

    void Start()
    {
        if (skyMaterial != null && skyTexture != null)
        {
            skyMaterial.SetTexture("_MainTex", skyTexture);
        }
    }
}