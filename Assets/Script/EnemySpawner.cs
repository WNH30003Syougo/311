using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class EnemySpawner : MonoBehaviour
//{
//    public GameObject enemyPrefab;
//    public Transform spawnPoint;
//    public Transform kyoten;
//    public float spawnInterval = 3f;
//    public int maxEnemies = 10;  // 最大スポーン数

//    private float timer;
//    private int spawnedEnemies = 0;

//    void Update()
//    {
//        // 拠点が存在しない or 非アクティブならスポーンしない
//        if (kyoten == null || !kyoten.gameObject.activeInHierarchy)
//            return;

//        timer += Time.deltaTime;

//        if (timer >= spawnInterval && spawnedEnemies < maxEnemies)
//        {
//            SpawnEnemy();
//            timer = 0f;
//        }
//    }

//    void SpawnEnemy()
//    {
//        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
//        //Instantiate(enemyPrefab, transform.position, Quaternion.identity);
//        Enemy enemyScript = enemy.GetComponent<Enemy>();
//        if (enemyScript != null)
//        {
//            enemyScript.kyoten = kyoten;
//        }
//        spawnedEnemies++;
//    }
//}

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;   // 生成する敵のプレハブ
    public float spawnInterval = 5f; // 敵を生成する間隔（秒）

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        // このGameObject（スポーンブロック）の位置に敵を生成
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }
}