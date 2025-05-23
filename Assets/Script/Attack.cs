using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEngine.GraphicsBuffer;

public class Attack : MonoBehaviour
{
    public float attackRadius = 50f;// 攻撃範囲の半径
    public float attackInterval = 1.0f; // 攻撃の間隔（秒）
    public int attackPower = 10;// 攻撃力（ダメージ量）
    private float timer; // 攻撃間隔用のタイマー
    private bool showRange = false;// 攻撃範囲の可視化のON/OFF
    public GameObject projectilePrefab; // 弾（Projectile）プレハブ
    private GameObject rangeVisualizer;// 攻撃範囲を表す可視化オブジェクト
    public Material rangeMaterial;// 攻撃範囲に使うマテリアル

    void Start()
    {
        // 攻撃範囲の可視化球体を生成
        CreateRangeSphere();

    }

    void Update()
    {
        timer += Time.deltaTime;

        // Vキーで可視化のON/OFFを切り替える
        if (Input.GetKeyDown(KeyCode.V))
        {
            ToggleRangeVisualizer();
        }
        // 一定時間経過ごとに攻撃処理を行う
        if (timer >= attackInterval)
        {
            AttackEnemies();
            timer = 0f;
        }
    }

    void AttackEnemies()// 攻撃処理（範囲内の敵を探し、HPの低い順→基地に近い順でターゲット選定し攻撃）
    {
        // 攻撃範囲内のコライダーを取得
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRadius);
        Debug.Log("Found colliders: " + colliders.Length); // ← 追加
        List<Enemy> enemies = new List<Enemy>();

        foreach (var collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy == null)
            {
                enemy = collider.GetComponentInParent<Enemy>();  // ← 追加
            }
            if (enemy != null)
            {
                enemies.Add(enemy);
            }

        }
        // 範囲内に敵がいない場合は何もしない
        if (enemies.Count == 0) return;
        // 基地オブジェクトを探す（"Base" という名前のオブジェクトが対象）
        GameObject baseObj = GameObject.Find("kyoten");
        if (baseObj == null)
        {
            Debug.LogWarning("Base が見つかりません");
            return;
        }
        // HPの低い順 → 基地に近い順に並び替え
        enemies = enemies.OrderBy(e => e.hp)
                         .ThenBy(e => Vector3.Distance(e.transform.position, baseObj.transform.position))
                         .ToList();

        // 最も優先度の高い敵に対して弾を発射
        FireProjectile(enemies[0].transform);
    }

    void CreateRangeSphere()// 攻撃範囲を可視化する球体を生成
    {
        rangeVisualizer = GameObject.CreatePrimitive(PrimitiveType.Sphere);// 球体を生成
        rangeVisualizer.transform.SetParent(transform);// このオブジェクトの子に設定
        rangeVisualizer.transform.localPosition = Vector3.zero; // 中心に配置
        // 攻撃範囲に合わせたスケール設定（ワールド座標ベース）
        rangeVisualizer.transform.localScale = Vector3.one * attackRadius * 2f;
        float worldScale = attackRadius * 2f;
        Vector3 lossyScale = transform.lossyScale;
        rangeVisualizer.transform.localScale = new Vector3(
            worldScale / lossyScale.x,
            worldScale / lossyScale.y,
            worldScale / lossyScale.z
        );
        //Debug.Log("Adjusted rangeVisualizer scale: " + rangeVisualizer.transform.localScale);
        //Debug.Log("rangeVisualizer scale: " + rangeVisualizer.transform.localScale);
        //Debug.Log("Parent scale: " + transform.lossyScale);
        // 可視化マテリアルの設定（指定がない場合は赤半透明）
        if (rangeMaterial != null)
        {
            rangeVisualizer.GetComponent<Renderer>().material = rangeMaterial;
        }
        else
        {
            //ebug.LogWarning("rangeMaterial が未設定なので、デフォルト赤色を設定します");
            rangeVisualizer.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.3f); // 半透明赤
        }
        // コライダーは不要なので削除
        Destroy(rangeVisualizer.GetComponent<Collider>());
        // 初期状態では非表示
        rangeVisualizer.SetActive(false);

        //Debug.Log("Range Visualizer Created at: " + rangeVisualizer.transform.position);
    }

    void ToggleRangeVisualizer()// 可視化ON/OFFの切り替え処理
    {
        if (rangeVisualizer != null)
        {
            showRange = !showRange;
            rangeVisualizer.SetActive(showRange);
        }
    }
    void FireProjectile(Transform enemyTransform)// 弾を生成し、指定した敵に向かわせる処理
    {
        Debug.Log("FireProjectile called");
        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile prefab が未設定です！");
            return;
        }
        // 弾プレハブを生成
        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        //Debug.Log("Projectile generated at: " + transform.position);
        // Projectile スクリプトを取得
        Projectile projectile = proj.GetComponent<Projectile>();
        if (projectile == null)
        {
            //Debug.LogError("Projectile スクリプトが弾に付いていません！");
            return;
        }

        if (enemyTransform == null)
        {
            //Debug.LogError("enemyTransform is null!");
        }
        else
        {
            //Debug.Log("enemyTransform position: " + enemyTransform.position);
        }
        // 敵をターゲットに設定
        projectile.SetTarget(enemyTransform);
        //Debug.Log("Target set for projectile");
        // 弾のダメージ値を設定
        projectile.damage = attackPower;
    }

}
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Linq;
//using static UnityEngine.GraphicsBuffer;

//public class Attack : MonoBehaviour
//{
//    public int damage = 10;
//    public float speed = 5f;
//    public float attackRadius = 50f;// 攻撃範囲の半径
//    public float attackInterval = 1.0f; // 攻撃の間隔（秒）
//    public int attackPower = 10;// 攻撃力（ダメージ量）
//    private float timer; // 攻撃間隔用のタイマー
//    private bool showRange = false;// 攻撃範囲の可視化のON/OFF
//    public GameObject projectilePrefab; // 弾（Projectile）プレハブ
//    private GameObject rangeVisualizer;// 攻撃範囲を表す可視化オブジェクト
//    public Material rangeMaterial;// 攻撃範囲に使うマテリアル

//    void Start()
//    {
//        // 攻撃範囲の可視化球体を生成
//        CreateRangeSphere();

//    }

//    void Update()
//    {
//        var v = Vector3.right; // 右方向に進む（必要に応じて変更）
//        transform.position += v * speed * Time.deltaTime;
//        timer += Time.deltaTime;

//        // Vキーで可視化のON/OFFを切り替える
//        if (Input.GetKeyDown(KeyCode.V))
//        {
//            ToggleRangeVisualizer();
//        }
//        // 一定時間経過ごとに攻撃処理を行う
//        if (timer >= attackInterval)
//        {
//            AttackEnemies();
//            timer = 0f;
//        }
//    }

//    void OnTriggerEnter(Collider collision)
//    {
//        Debug.Log("衝突されたオブジェクト：" + collision.gameObject.name);

//        // Enemyに当たったときだけ処理
//        if (collision.CompareTag("Enemy"))
//        {
//            Enemy enemy = collision.GetComponent<Enemy>();
//            if (enemy != null)
//            {
//                enemy.TakeDamage(damage); // ダメージを与える
//            }

//            Destroy(gameObject); // 攻撃オブジェクトを削除（弾）
//        }
//        else if (!collision.CompareTag("Attack")) // ほかの攻撃と衝突した場合は無視
//        {
//            Destroy(gameObject); // 敵以外に当たったら消す
//        }
//    }
//}
