using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp;

    public float speed;

    public int gold;

    //public Route route;

    public GameObject enemy;

    int num = 0;

    //敵プレハブ
    public GameObject enemyPrefab;
    //敵生成時間間隔
    private float interval;
    //経過時間
    private float time = 0f;

    public class Route : MonoBehaviour
    {
        public List<Transform> points;
    }

    GameObject Kiti;
    GameObject Keiro;

    // Start is called before the first frame update
    void Start()
    {
        //transform.position = route.points[0].transform.position;
        Kiti = GameObject.Find("Sphere");
        Keiro = GameObject.Find("Cylinder");
        //時間間隔を決定する
        interval = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        var v = Kiti.transform.position - Keiro.transform.position;
        transform.position += v * speed * Time.deltaTime;

        //num++;
        //if (num % 40 == 0)
        //{
        //    Instantiate(enemy, new Vector3(0f, 100f, -1f), Quaternion.identity);
        //}

        //時間計測
        time += Time.deltaTime;

        //経過時間が生成時間になったとき(生成時間より大きくなったとき)
        if (time > interval)
        {
            //enemyをインスタンス化する(生成する)
            GameObject enemy = Instantiate(enemyPrefab);
            //生成した敵の座標を決定する(現状X=0,Y=10,Z=20の位置に出力)
            enemy.transform.position = new Vector3(66f, 150f, 21f);
            //経過時間を初期化して再度時間計測を始める
            time = 0f;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("衝突されたオブジェクト：" + collision.gameObject.name);

        Destroy(collision.gameObject);

    }

}
