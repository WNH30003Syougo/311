//using System.Collections;
//using System.Collections.Generic;
//using System.Xml.Linq;
//using UnityEngine;

//public class Enemy : MonoBehaviour
//{
//    public GameObject enemyPrefab;         // 敵のプレハブ（生成する敵の元となるオブジェクト）
//    public float moveSpeed = 3f;            // 移動速度
//    public float spawnInterval = 5f;        // スポーン間隔
//    public int maxEnemyCount = 3;           // 最大出現数
//    public int hp = 100;                    // 敵自身のHP

//    private Transform baseTarget;           // ターゲット（Base) 移動の目標となる「基地」のTransform
//    private List<GameObject> enemies = new List<GameObject>();// 現在存在している敵のリスト

//    void Start()
//    {
//        // シーン内の「Base」オブジェクトを検索して目標に設定
//        GameObject baseObj = GameObject.Find("Base");
//        if (baseObj != null)
//        {
//            baseTarget = baseObj.transform;
//        }


//        // 敵生成開始（コルーチン）
//        StartCoroutine(SpawnEnemies());
//    }

//    void Update()
//    {
//        MoveEnemies(); // 敵を目標に向かって移動させる処理
//        enemies.RemoveAll(e => e == null); // 消滅した敵のリストからの除外
//    }

//    // Start is called before the first frame update
//    public void TakeDamage(int amount) // このスクリプトがアタッチされているオブジェクト自身のダメージ処理
//    {
//        hp -= amount;


//        if (hp <= 0)
//        {
//            // HPが0以下なら自身を破壊（弾などで攻撃されたときに呼ばれる）
//            Destroy(gameObject);
//        }
//    }
//    void MoveEnemies()// 生成された敵を基地に向かって移動させる処理
//    {
//        foreach (GameObject enemy in enemies)
//        {
//            if (enemy != null && baseTarget != null)
//            {
//                // 基地に向かって移動
//                enemy.transform.position = Vector3.MoveTowards(
//                    enemy.transform.position,
//                    baseTarget.position,
//                    moveSpeed * Time.deltaTime
//                );
//                // 基地に接近したら敵を破壊
//                if (Vector3.Distance(enemy.transform.position, baseTarget.position) < 0.5f)
//                {
//                    Destroy(enemy); // 接触時に削除（ここで基地のHP減少も可能）
//                }
//            }
//        }
//    }

//    IEnumerator SpawnEnemies() // 定期的に敵を生成するコルーチン
//    {
//        while (true)
//        {
//            // 最大数未満なら敵を生成してリストに追加
//            if (enemies.Count < maxEnemyCount)
//            {
//                GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
//                enemies.Add(newEnemy);
//            }
//            // 一定時間待ってから次の生成へ
//            yield return new WaitForSeconds(spawnInterval);
//        }
//    }
//}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform kyoten;
    public Transform testUnit;
    public float moveSpeed = 2f;
    public float slowSpeed = 0.5f;
    public float unitDetectRange = 3f;
    public int damage = 10;
    public int maxHp = 30;
    public float attackInterval = 1.0f;
    public int hp = 100;                    // 敵自身のHP


    private int currentHp;

    private LineRenderer line;

    private bool isAttackingKyoten = false;
    private bool isAttackingUnit = false;
    private float attackTimer = 0f;

    private KyotenHealth kyotenHealth;
    private TestUnitHealth testUnitHealth;

    void Awake()
    {
        currentHp = maxHp;

        // ルート表示用 LineRenderer 初期化
        //line = gameObject.AddComponent<LineRenderer>();
        line = GetComponent<LineRenderer>();
        if (line == null)
        {
            line = gameObject.AddComponent<LineRenderer>();
        }

        line.positionCount = 2;
        line.startWidth = 0.5f;
        line.endWidth = 0.5f;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = Color.blue;
        line.endColor = Color.blue;

        // kyoten を自動取得（Tagが"Kyoten"のオブジェクトを探す）
        if (kyoten == null)
        {
            GameObject targetObj = GameObject.FindWithTag("kyoten");
            if (targetObj != null)
            {
                kyoten = targetObj.transform;
            }
            else
            {
                Debug.LogError("Kyoten オブジェクトが見つかりません。Tagが設定されていますか？");
            }
        }

    }

    void Update()
    {
        // 拠点に張り付き中の攻撃処理
        if (isAttackingKyoten)
        {
            if (kyotenHealth == null)
            {
                isAttackingKyoten = false;
                return;
            }

            attackTimer += Time.deltaTime;
            if (attackTimer >= attackInterval)
            {
                kyotenHealth.TakeDamage(damage);
                attackTimer = 0f;
            }
            return;
        }

        // ユニットに張り付き中の攻撃処理
        if (isAttackingUnit)
        {
            if (testUnitHealth == null)
            {
                isAttackingUnit = false;
                return;
            }

            attackTimer += Time.deltaTime;
            if (attackTimer >= attackInterval)
            {
                testUnitHealth.TakeDamage(damage);
                attackTimer = 0f;
            }
            return;
        }

        if (kyoten == null) return;

        // 移動ターゲットの決定
        Transform target = kyoten;
        float speed = moveSpeed;

        if (testUnit != null)
        {
            float distToUnit = Vector3.Distance(transform.position, testUnit.position);
            if (distToUnit <= unitDetectRange)
            {
                target = testUnit;
                speed = slowSpeed;
            }
        }

        // ターゲットに向かって移動
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        // ルート表示
        line.SetPosition(0, transform.position);
        line.SetPosition(1, target.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("kyoten") && !isAttackingKyoten)
        {
            kyotenHealth = other.GetComponent<KyotenHealth>();
            if (kyotenHealth != null)
            {
                isAttackingKyoten = true;
                attackTimer = 0f;
            }
        }

        if (other.CompareTag("Testunit") && !isAttackingUnit)
        {
            testUnitHealth = other.GetComponent<TestUnitHealth>();
            if (testUnitHealth != null)
            {
                isAttackingUnit = true;
                attackTimer = 0f;
            }
        }

        if (other.CompareTag("Attack"))
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHp -= amount;
        if (currentHp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
