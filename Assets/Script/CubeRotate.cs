using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRotate : MonoBehaviour
{
    private int speed;                //オブジェクトのスピード
    private int radius;               //円を描く半径
    private Vector3 defPosition;      //defPositionをVector3で定義する。
    float x;
    float z;

    // Start is called before the first frame update
    void Start()
    {
        speed = 1;
        radius = 150;

        defPosition = transform.position;    //defPositionを自分のいる位置に設定する。

    }

    // Update is called once per frame
    void Update()
    {
        x = radius * Mathf.Sin(Time.time * speed);      //X軸の設定
        z = radius * Mathf.Cos(Time.time * speed);      //Z軸の設定

        transform.position = new Vector3(x + defPosition.x, defPosition.y, z + defPosition.z);  //自分のいる位置から座標を動かす。

    }
}
