using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEngine.GraphicsBuffer;

public class Attack : MonoBehaviour
{
    public float attackRadius = 50f;// �U���͈͂̔��a
    public float attackInterval = 1.0f; // �U���̊Ԋu�i�b�j
    public int attackPower = 10;// �U���́i�_���[�W�ʁj
    private float timer; // �U���Ԋu�p�̃^�C�}�[
    private bool showRange = false;// �U���͈͂̉�����ON/OFF
    public GameObject projectilePrefab; // �e�iProjectile�j�v���n�u
    private GameObject rangeVisualizer;// �U���͈͂�\�������I�u�W�F�N�g
    public Material rangeMaterial;// �U���͈͂Ɏg���}�e���A��

    void Start()
    {
        // �U���͈͂̉������̂𐶐�
        CreateRangeSphere();

    }

    void Update()
    {
        timer += Time.deltaTime;

        // V�L�[�ŉ�����ON/OFF��؂�ւ���
        if (Input.GetKeyDown(KeyCode.V))
        {
            ToggleRangeVisualizer();
        }
        // ��莞�Ԍo�߂��ƂɍU���������s��
        if (timer >= attackInterval)
        {
            AttackEnemies();
            timer = 0f;
        }
    }

    void AttackEnemies()// �U�������i�͈͓��̓G��T���AHP�̒Ⴂ������n�ɋ߂����Ń^�[�Q�b�g�I�肵�U���j
    {
        // �U���͈͓��̃R���C�_�[���擾
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRadius);
        Debug.Log("Found colliders: " + colliders.Length); // �� �ǉ�
        List<Enemy> enemies = new List<Enemy>();

        foreach (var collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy == null)
            {
                enemy = collider.GetComponentInParent<Enemy>();  // �� �ǉ�
            }
            if (enemy != null)
            {
                enemies.Add(enemy);
            }

        }
        // �͈͓��ɓG�����Ȃ��ꍇ�͉������Ȃ�
        if (enemies.Count == 0) return;
        // ��n�I�u�W�F�N�g��T���i"Base" �Ƃ������O�̃I�u�W�F�N�g���Ώہj
        GameObject baseObj = GameObject.Find("kyoten");
        if (baseObj == null)
        {
            Debug.LogWarning("Base ��������܂���");
            return;
        }
        // HP�̒Ⴂ�� �� ��n�ɋ߂����ɕ��ёւ�
        enemies = enemies.OrderBy(e => e.hp)
                         .ThenBy(e => Vector3.Distance(e.transform.position, baseObj.transform.position))
                         .ToList();

        // �ł��D��x�̍����G�ɑ΂��Ēe�𔭎�
        FireProjectile(enemies[0].transform);
    }

    void CreateRangeSphere()// �U���͈͂��������鋅�̂𐶐�
    {
        rangeVisualizer = GameObject.CreatePrimitive(PrimitiveType.Sphere);// ���̂𐶐�
        rangeVisualizer.transform.SetParent(transform);// ���̃I�u�W�F�N�g�̎q�ɐݒ�
        rangeVisualizer.transform.localPosition = Vector3.zero; // ���S�ɔz�u
        // �U���͈͂ɍ��킹���X�P�[���ݒ�i���[���h���W�x�[�X�j
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
        // �����}�e���A���̐ݒ�i�w�肪�Ȃ��ꍇ�͐Ԕ������j
        if (rangeMaterial != null)
        {
            rangeVisualizer.GetComponent<Renderer>().material = rangeMaterial;
        }
        else
        {
            //ebug.LogWarning("rangeMaterial �����ݒ�Ȃ̂ŁA�f�t�H���g�ԐF��ݒ肵�܂�");
            rangeVisualizer.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.3f); // ��������
        }
        // �R���C�_�[�͕s�v�Ȃ̂ō폜
        Destroy(rangeVisualizer.GetComponent<Collider>());
        // ������Ԃł͔�\��
        rangeVisualizer.SetActive(false);

        //Debug.Log("Range Visualizer Created at: " + rangeVisualizer.transform.position);
    }

    void ToggleRangeVisualizer()// ����ON/OFF�̐؂�ւ�����
    {
        if (rangeVisualizer != null)
        {
            showRange = !showRange;
            rangeVisualizer.SetActive(showRange);
        }
    }
    void FireProjectile(Transform enemyTransform)// �e�𐶐����A�w�肵���G�Ɍ����킹�鏈��
    {
        Debug.Log("FireProjectile called");
        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile prefab �����ݒ�ł��I");
            return;
        }
        // �e�v���n�u�𐶐�
        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        //Debug.Log("Projectile generated at: " + transform.position);
        // Projectile �X�N���v�g���擾
        Projectile projectile = proj.GetComponent<Projectile>();
        if (projectile == null)
        {
            //Debug.LogError("Projectile �X�N���v�g���e�ɕt���Ă��܂���I");
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
        // �G���^�[�Q�b�g�ɐݒ�
        projectile.SetTarget(enemyTransform);
        //Debug.Log("Target set for projectile");
        // �e�̃_���[�W�l��ݒ�
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
//    public float attackRadius = 50f;// �U���͈͂̔��a
//    public float attackInterval = 1.0f; // �U���̊Ԋu�i�b�j
//    public int attackPower = 10;// �U���́i�_���[�W�ʁj
//    private float timer; // �U���Ԋu�p�̃^�C�}�[
//    private bool showRange = false;// �U���͈͂̉�����ON/OFF
//    public GameObject projectilePrefab; // �e�iProjectile�j�v���n�u
//    private GameObject rangeVisualizer;// �U���͈͂�\�������I�u�W�F�N�g
//    public Material rangeMaterial;// �U���͈͂Ɏg���}�e���A��

//    void Start()
//    {
//        // �U���͈͂̉������̂𐶐�
//        CreateRangeSphere();

//    }

//    void Update()
//    {
//        var v = Vector3.right; // �E�����ɐi�ށi�K�v�ɉ����ĕύX�j
//        transform.position += v * speed * Time.deltaTime;
//        timer += Time.deltaTime;

//        // V�L�[�ŉ�����ON/OFF��؂�ւ���
//        if (Input.GetKeyDown(KeyCode.V))
//        {
//            ToggleRangeVisualizer();
//        }
//        // ��莞�Ԍo�߂��ƂɍU���������s��
//        if (timer >= attackInterval)
//        {
//            AttackEnemies();
//            timer = 0f;
//        }
//    }

//    void OnTriggerEnter(Collider collision)
//    {
//        Debug.Log("�Փ˂��ꂽ�I�u�W�F�N�g�F" + collision.gameObject.name);

//        // Enemy�ɓ��������Ƃ���������
//        if (collision.CompareTag("Enemy"))
//        {
//            Enemy enemy = collision.GetComponent<Enemy>();
//            if (enemy != null)
//            {
//                enemy.TakeDamage(damage); // �_���[�W��^����
//            }

//            Destroy(gameObject); // �U���I�u�W�F�N�g���폜�i�e�j
//        }
//        else if (!collision.CompareTag("Attack")) // �ق��̍U���ƏՓ˂����ꍇ�͖���
//        {
//            Destroy(gameObject); // �G�ȊO�ɓ������������
//        }
//    }
//}
