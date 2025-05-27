using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform kyoten; // 中心にするオブジェクト
    public float distance = 300.0f; // 中心からカメラまでの距離
    public float xSpeed = 2500.0f;  // マウス横移動の回転速度
    public float ySpeed = 2500.0f;  // マウス縦移動の回転速度

    public float yMinLimit = -360f; // 縦回転の下限角度
    public float yMaxLimit = 360f;  // 縦回転の上限角度

    private float x = 0.0f;
    private float y = 0.0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        // マウスカーソルをゲームウィンドウ内に固定したい場合はここでLock
        // Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        if (kyoten != null)
        {
            if (Input.GetMouseButton(1)) // 左クリックを押しながら回す
            {
                x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
                y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
                //y = Mathf.Clamp(y, yMinLimit, yMaxLimit); // 上下角度を制限
            }

            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + kyoten.position;

            transform.rotation = rotation;
            transform.position = position;
        }
    }
}
