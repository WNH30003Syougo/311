using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbitZoom : MonoBehaviour
{
    public Transform kyoten; // 中心にするオブジェクト
    public float distance = 10.0f;
    public float zoomSpeed = 10f;
    public float minDistance = 5f;
    public float maxDistance = 50f;

    public float xSpeed = 250f;
    public float ySpeed = 250f;
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float panSpeed = 0.5f;

    private float x = 0.0f;
    private float y = 0.0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        if (kyoten == null)
        {
            GameObject go = new GameObject("CameraCenter");
            go.transform.position = Vector3.zero;
            kyoten = go.transform;
        }

        // === 中心点の視覚化 ===
        GameObject marker = new GameObject("KyotenMarker");
        marker.transform.SetParent(kyoten);
        marker.transform.localPosition = Vector3.zero;

        // スプライトを追加
        SpriteRenderer sr = marker.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("kyoten_icon"); // Resources フォルダ内のアイコンを使用
        sr.color = new Color(1f, 1f, 0f, 0.8f); // 半透明の黄色

        marker.transform.localScale = Vector3.one * 0.5f;

        // ビルボードスクリプトを追加
        marker.AddComponent<Billboard>();
    }

    void LateUpdate()
    {
        if (kyoten == null) return;

        bool isShiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // --------- 回転操作（右クリック） ---------
        if (Input.GetMouseButton(1))
        {
            x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
            y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
            y = Mathf.Clamp(y, yMinLimit, yMaxLimit);
        }

        // --------- ズーム操作（ホイール） ---------
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        // --------- パン操作（Shift + 中ボタン） ---------
        if (isShiftHeld && Input.GetMouseButton(2)) // 2 = 中ボタン
        {
            float panX = -Input.GetAxis("Mouse X") * panSpeed;
            float panY = -Input.GetAxis("Mouse Y") * panSpeed;

            Vector3 move = transform.right * panX + transform.up * panY;
            kyoten.position += move;
        }

        // --------- カメラ位置と回転を更新 ---------
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + kyoten.position;

        transform.rotation = rotation;
        transform.position = position;
    }
}
