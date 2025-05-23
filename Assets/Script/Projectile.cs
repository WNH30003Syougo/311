using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Projectile : MonoBehaviour
{
    // 弾の移動速度（1秒あたりの単位距離）
    public float speed = 10f;
    // 弾が命中した際に与えるダメージ量
    public int damage;
    // 攻撃対象（追尾先の敵）
    private Transform target;
    void Awake() // Rigidbodyがアタッチされている場合に、重力を無効にする
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;// 弾が落下しないように設定
        }
    }
    public void SetTarget(Transform target) // 外部からターゲットをセットするためのメソッド
    {
        this.target = target;
        Debug.Log("SetTarget called. Target position: " + target.position);
    }

    void Update() // 毎フレーム、弾をターゲットの方向に移動させる
    {
        // ターゲットが消滅または未設定の場合、弾も削除する
        if (target == null)
        {
            Debug.LogWarning("Projectile target is null. Destroying projectile.");
            Destroy(gameObject);// 自分自身を削除
            return;
        }
        // ターゲットまでの方向ベクトルを計算
        Vector3 dir = target.position - transform.position;
        // このフレームで進める距離を計算（speed × deltaTime）
        float distanceThisFrame = speed * Time.deltaTime;
        // 弾がターゲットにほぼ届いている場合、命中処理を行う
        if (dir.magnitude <= distanceThisFrame)
        {
            // Enemy スクリプトを取得してダメージを与える
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject); // 命中後、自分（弾）を削除
            return;
        }
        // ターゲット方向に向かって一定距離だけ前進
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }
}

