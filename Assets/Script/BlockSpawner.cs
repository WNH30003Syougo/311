using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    //生成されるブロック(ユニット)のプレハブ
    public GameObject InterceptorPrefab;
    //クリック対象となる[地面(Torus)]のレイヤー
    public LayerMask groundLayer;
    //ブロックの移動速度
    public float moveSpeed = 50f;
    //現在クリックし操作対象になっているトーラスオブジェクト
    private Transform currentTorus;
    // トーラスの大円半径（外側円）、小円半径（内側円）位置計算に必要
    private float bigRadius;//トーラスの大円(外側円)の半径　
    private float smallRadius;//トーラスの小円の半径(内側円)
    // ブロックの位置を決めるための2つの角度（トーラス上の位置）
    private float angleTheta;//トーラス上の角度位置　 大円方向（周方向）
    private float anglePhi;//トーラス上の角度位置// 小円方向（断面方向）
    //現在シーン内に生成されたブロックオブジェクトの参照
    private GameObject placedBlock;
    void Update()
    {
        // 左クリックでブロックを配置（既存のブロックがある場合は削除）
        if (Input.GetMouseButtonDown(0))
        {
            if (placedBlock != null)
            {
                Destroy(placedBlock);
            }
            SpawnBlock();
        }
        // ブロックが存在する場合は、移動と回転を受け付ける
        if (placedBlock != null)
        {
            MoveBlockOnTorus();// ←→ キーで移動
            RotateBlock();// WASD キーで回転
        }
    }

    void SpawnBlock()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // トーラスのレイヤーに対して Raycast を撃つ
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            // クリックされたトーラスを取得
            currentTorus = hit.transform;
            // トーラスの半径（大円・小円）を更新
            UpdateTorusRadii(); // ← クリックごとにラジアン更新
            // ヒットしたワールド位置をトーラスのローカル座標に変換
            Vector3 localHitPoint = currentTorus.InverseTransformPoint(hit.point);
            // 大円方向（theta）を求める（XZ平面上の角度
            angleTheta = Mathf.Atan2(localHitPoint.z, localHitPoint.x) * Mathf.Rad2Deg;
            // 大円の円周上の中心点（小円が回転する中心）を計算
            float circleCenterX = Mathf.Cos(angleTheta * Mathf.Deg2Rad) * bigRadius;
            float circleCenterZ = Mathf.Sin(angleTheta * Mathf.Deg2Rad) * bigRadius;
            Vector3 circleCenter = new Vector3(circleCenterX, 0, circleCenterZ);
            // ヒット点から中心点へのオフセット方向（断面方向）
            Vector3 offset = localHitPoint - circleCenter;
            Vector3 dir = offset.normalized;
            // 小円方向（phi）を求める（上下方向の角度）
            anglePhi = Mathf.Asin(dir.y) * Mathf.Rad2Deg;
            // 角度から最終的なブロックのワールド位置を求めて生成
            Vector3 torusPos = CalculateTorusPosition(angleTheta, anglePhi);
            placedBlock = Instantiate(InterceptorPrefab, torusPos, Quaternion.identity);
            // ブロックをトーラスの子オブジェクトに設定（位置・回転はワールド基準）
            placedBlock.transform.SetParent(currentTorus, true);

        }

    }

    void UpdateTorusRadii() // トーラスのサイズから大円・小円の半径を更新
    {
        MeshFilter meshFilter = currentTorus.GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            Bounds bounds = meshFilter.sharedMesh.bounds;
            Vector3 size = Vector3.Scale(bounds.size, currentTorus.localScale); // 実際のワールドサイズ考慮

            bigRadius = (size.x + size.z) * 0.25f; // XとZの平均の半分、平均で 0.25
            smallRadius = size.y * 0.5f; // Y軸の半分が小円半径


        }
    }
    // 指定された角度から、トーラス上のワールド座標を返す
    Vector3 CalculateTorusPosition(float theta, float phi)
    {
        float radTheta = theta * Mathf.Deg2Rad;
        float radPhi = phi * Mathf.Deg2Rad;

        float x = (bigRadius + smallRadius * Mathf.Cos(radPhi)) * Mathf.Cos(radTheta);
        float y = smallRadius * Mathf.Sin(radPhi);
        float z = (bigRadius + smallRadius * Mathf.Cos(radPhi)) * Mathf.Sin(radTheta);
        Vector3 localPos = new Vector3(x, y, z);
        return currentTorus.TransformPoint(localPos);
    }
    void RotateBlock()// WASD キーによるブロックの回転
    {
        if (placedBlock == null) return;

        float rotationSpeed = 100f;

        if (Input.GetKey(KeyCode.W))
            placedBlock.transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.S))
            placedBlock.transform.Rotate(Vector3.left * rotationSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.A))
            placedBlock.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.D))
            placedBlock.transform.Rotate(Vector3.down * rotationSpeed * Time.deltaTime);
    }
    //void MoveBlockOnTorus()
    //{
    //    if (currentTorus == null) return;

    //    float deltaTheta = 0f;

    //    if (Input.GetKey(KeyCode.LeftArrow)) deltaTheta -= 1f;
    //    if (Input.GetKey(KeyCode.RightArrow)) deltaTheta += 1f;

    //    if (deltaTheta != 0f)
    //    {
    //        angleTheta += deltaTheta * moveSpeed * Time.deltaTime;
    //        Vector3 newPos = CalculateTorusPosition(angleTheta, anglePhi);
    //        placedBlock.transform.position = newPos;

    //        /* Vector3 axis = currentTorus.up * Mathf.Sign(deltaTheta);
    //         float angle = moveSpeed * Time.deltaTime;
    //         currentTorus.Rotate(axis, angle, Space.World);*/
    //    }
    //}
    void MoveBlockOnTorus()// トーラス表面に沿ってブロックを移動する処理（←→キーで操作）
    {
        if (placedBlock == null || currentTorus == null) return;
        // 矢印キーによって角度を変更（今回はThetaのみ）
        float deltaTheta = 0f;
        float deltaPhi = 0f;
        // ↑↓を使いたい場合はdeltaPhiも有効化
        if (Input.GetKey(KeyCode.UpArrow)) deltaPhi += 1f;
        if (Input.GetKey(KeyCode.DownArrow)) deltaPhi -= 1f;
        if (Input.GetKey(KeyCode.LeftArrow)) deltaTheta -= 1f;
        if (Input.GetKey(KeyCode.RightArrow)) deltaTheta += 1f;

        bool moved = (deltaTheta != 0f || deltaPhi != 0f);
        // 角度を更新
        angleTheta += deltaTheta * moveSpeed * Time.deltaTime;
        anglePhi += deltaPhi * moveSpeed * Time.deltaTime;
        // 新しい位置を計算してブロックを移動
        Vector3 newPos = CalculateTorusPosition(angleTheta, anglePhi);
        placedBlock.transform.position = newPos;
        // トーラス自体も回転させることで、表面に沿っているような印象を作る
        if (moved)
        {
            Vector3 axis = Vector3.zero;
            //float angle = moveSpeed * Time.deltaTime;

            if (deltaTheta != 0f)
            {
                // 大円方向の回転（Y軸周り）
                //axis = currentTorus.up * Mathf.Sign(deltaTheta);
            }
            else if (deltaPhi != 0f)
            {
                // 小円方向の回転（表面法線に基づいた回転軸）
                Vector3 localPos = currentTorus.InverseTransformPoint(placedBlock.transform.position);
                Vector3 center = new Vector3(
                    Mathf.Cos(angleTheta * Mathf.Deg2Rad) * bigRadius,
                    0,
                    Mathf.Sin(angleTheta * Mathf.Deg2Rad) * bigRadius
                );
                Vector3 normal = (localPos - center).normalized;
                Vector3 worldNormal = currentTorus.TransformDirection(normal);
                axis = Vector3.Cross(worldNormal, Vector3.up).normalized * Mathf.Sign(deltaPhi);
            }
            // トーラスをワールド空間で回転
            //currentTorus.Rotate(axis, angle, Space.World);
        }
    }
}