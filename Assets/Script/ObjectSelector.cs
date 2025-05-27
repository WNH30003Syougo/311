using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    private SelectableHighlighter selectedObject;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 左クリック
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (selectedObject != null)
                {
                    selectedObject.RemoveHighlight(); // 以前の選択を解除
                }

                selectedObject = hit.collider.GetComponent<SelectableHighlighter>();
                if (selectedObject != null)
                {
                    selectedObject.Highlight(); // 新しいオブジェクトをハイライト
                }
            }
        }
    }
}
