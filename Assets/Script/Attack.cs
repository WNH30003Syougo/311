using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Attack;

public class Attack : MonoBehaviour
{
    public int hp;

    public float speed;

    public int gold;

    public class Route : MonoBehaviour
    {
        public List<Transform> points;
    }

    // Start is called before the first frame update
    void Start()
    {
        //transform.position = route.points[0].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var v = Vector3.right; //移動方向 今回は右
        transform.position += v * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("衝突されたオブジェクト：" + collision.gameObject.name);

        Destroy(collision.gameObject);
    }

}
