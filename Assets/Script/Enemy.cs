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

    //�G�v���n�u
    public GameObject enemyPrefab;
    //�G�������ԊԊu
    private float interval;
    //�o�ߎ���
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
        //���ԊԊu�����肷��
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

        //���Ԍv��
        time += Time.deltaTime;

        //�o�ߎ��Ԃ��������ԂɂȂ����Ƃ�(�������Ԃ��傫���Ȃ����Ƃ�)
        if (time > interval)
        {
            //enemy���C���X�^���X������(��������)
            GameObject enemy = Instantiate(enemyPrefab);
            //���������G�̍��W�����肷��(����X=0,Y=10,Z=20�̈ʒu�ɏo��)
            enemy.transform.position = new Vector3(66f, 150f, 21f);
            //�o�ߎ��Ԃ����������čēx���Ԍv�����n�߂�
            time = 0f;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("�Փ˂��ꂽ�I�u�W�F�N�g�F" + collision.gameObject.name);

        Destroy(collision.gameObject);

    }

}
