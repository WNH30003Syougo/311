using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRotate : MonoBehaviour
{
    private int speed;                //�I�u�W�F�N�g�̃X�s�[�h
    private int radius;               //�~��`�����a
    private Vector3 defPosition;      //defPosition��Vector3�Œ�`����B
    float x;
    float z;

    // Start is called before the first frame update
    void Start()
    {
        speed = 1;
        radius = 150;

        defPosition = transform.position;    //defPosition�������̂���ʒu�ɐݒ肷��B

    }

    // Update is called once per frame
    void Update()
    {
        x = radius * Mathf.Sin(Time.time * speed);      //X���̐ݒ�
        z = radius * Mathf.Cos(Time.time * speed);      //Z���̐ݒ�

        transform.position = new Vector3(x + defPosition.x, defPosition.y, z + defPosition.z);  //�����̂���ʒu������W�𓮂����B

    }
}
