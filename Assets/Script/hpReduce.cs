using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hpReduce : MonoBehaviour
{
    //public Slider hpSlider;
    // Start is called before the first frame update
    void Start()
    {
        //hpSlider.value = 100;
    }

    void OnTriggerExit(Collider col)
    {
        //if (col.gameObject.tag == "playerAttack")
        //{
        //    hpSlider.value -= 10;
        //    Debug.Log("プレイヤーの攻撃、10のダメージ");
        //}

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
