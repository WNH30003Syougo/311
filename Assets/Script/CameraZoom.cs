using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Transform target;        // カメラが注視するターゲット
    public float zoomSpeed = 10f;   // ズームのスピード
    public float minDistance = 5f;  // 最小距離
    public float maxDistance = 20f; // 最大距離

    private float currentDistance;

    // Start is called before the first frame update
    void Start()
    {
        if (target == null)
        {
            Debug.LogError("ターゲットが設定されていません！");
            return;
        }

        // 初期距離を計算
        currentDistance = Vector3.Distance(transform.position, target.position);

    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        // ホイールの入力を取得
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // 距離を更新（ズームイン：正、ズームアウト：負）
        currentDistance -= scroll * zoomSpeed;
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);

        // ターゲットから一定距離の位置にカメラを配置
        transform.position = target.position - transform.forward * currentDistance;

    }
}
