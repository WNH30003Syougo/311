//using System.Collections;
//using System.Collections.Generic;
//using System.Xml.Linq;
//using UnityEngine;

//public class Enemy : MonoBehaviour
//{
//    public GameObject enemyPrefab;         // �G�̃v���n�u�i��������G�̌��ƂȂ�I�u�W�F�N�g�j
//    public float moveSpeed = 3f;            // �ړ����x
//    public float spawnInterval = 5f;        // �X�|�[���Ԋu
//    public int maxEnemyCount = 3;           // �ő�o����
//    public int hp = 100;                    // �G���g��HP

//    private Transform baseTarget;           // �^�[�Q�b�g�iBase) �ړ��̖ڕW�ƂȂ�u��n�v��Transform
//    private List<GameObject> enemies = new List<GameObject>();// ���ݑ��݂��Ă���G�̃��X�g

//    void Start()
//    {
//        // �V�[�����́uBase�v�I�u�W�F�N�g���������ĖڕW�ɐݒ�
//        GameObject baseObj = GameObject.Find("Base");
//        if (baseObj != null)
//        {
//            baseTarget = baseObj.transform;
//        }


//        // �G�����J�n�i�R���[�`���j
//        StartCoroutine(SpawnEnemies());
//    }

//    void Update()
//    {
//        MoveEnemies(); // �G��ڕW�Ɍ������Ĉړ������鏈��
//        enemies.RemoveAll(e => e == null); // ���ł����G�̃��X�g����̏��O
//    }

//    // Start is called before the first frame update
//    public void TakeDamage(int amount) // ���̃X�N���v�g���A�^�b�`����Ă���I�u�W�F�N�g���g�̃_���[�W����
//    {
//        hp -= amount;


//        if (hp <= 0)
//        {
//            // HP��0�ȉ��Ȃ玩�g��j��i�e�ȂǂōU�����ꂽ�Ƃ��ɌĂ΂��j
//            Destroy(gameObject);
//        }
//    }
//    void MoveEnemies()// �������ꂽ�G����n�Ɍ������Ĉړ������鏈��
//    {
//        foreach (GameObject enemy in enemies)
//        {
//            if (enemy != null && baseTarget != null)
//            {
//                // ��n�Ɍ������Ĉړ�
//                enemy.transform.position = Vector3.MoveTowards(
//                    enemy.transform.position,
//                    baseTarget.position,
//                    moveSpeed * Time.deltaTime
//                );
//                // ��n�ɐڋ߂�����G��j��
//                if (Vector3.Distance(enemy.transform.position, baseTarget.position) < 0.5f)
//                {
//                    Destroy(enemy); // �ڐG���ɍ폜�i�����Ŋ�n��HP�������\�j
//                }
//            }
//        }
//    }

//    IEnumerator SpawnEnemies() // ����I�ɓG�𐶐�����R���[�`��
//    {
//        while (true)
//        {
//            // �ő吔�����Ȃ�G�𐶐����ă��X�g�ɒǉ�
//            if (enemies.Count < maxEnemyCount)
//            {
//                GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
//                enemies.Add(newEnemy);
//            }
//            // ��莞�ԑ҂��Ă��玟�̐�����
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
    public int hp = 100;                    // �G���g��HP


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

        // ���[�g�\���p LineRenderer ������
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

        // kyoten �������擾�iTag��"Kyoten"�̃I�u�W�F�N�g��T���j
        if (kyoten == null)
        {
            GameObject targetObj = GameObject.FindWithTag("kyoten");
            if (targetObj != null)
            {
                kyoten = targetObj.transform;
            }
            else
            {
                Debug.LogError("Kyoten �I�u�W�F�N�g��������܂���BTag���ݒ肳��Ă��܂����H");
            }
        }

    }

    void Update()
    {
        // ���_�ɒ���t�����̍U������
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

        // ���j�b�g�ɒ���t�����̍U������
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

        // �ړ��^�[�Q�b�g�̌���
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

        // �^�[�Q�b�g�Ɍ������Ĉړ�
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        // ���[�g�\��
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
