using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableHighlighter : MonoBehaviour
{
    public Material highlightMaterial; // ハイライト用マテリアル
    private Material originalMaterial;
    private Renderer objectRenderer;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalMaterial = objectRenderer.material;
    }

    public void Highlight()
    {
        objectRenderer.material = highlightMaterial;
    }

    public void RemoveHighlight()
    {
        objectRenderer.material = originalMaterial;
    }
}
