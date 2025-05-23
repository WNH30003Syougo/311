using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Projectile : MonoBehaviour
{
    // �e�̈ړ����x�i1�b������̒P�ʋ����j
    public float speed = 10f;
    // �e�����������ۂɗ^����_���[�W��
    public int damage;
    // �U���Ώہi�ǔ���̓G�j
    private Transform target;
    void Awake() // Rigidbody���A�^�b�`����Ă���ꍇ�ɁA�d�͂𖳌��ɂ���
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;// �e���������Ȃ��悤�ɐݒ�
        }
    }
    public void SetTarget(Transform target) // �O������^�[�Q�b�g���Z�b�g���邽�߂̃��\�b�h
    {
        this.target = target;
        Debug.Log("SetTarget called. Target position: " + target.position);
    }

    void Update() // ���t���[���A�e���^�[�Q�b�g�̕����Ɉړ�������
    {
        // �^�[�Q�b�g�����ł܂��͖��ݒ�̏ꍇ�A�e���폜����
        if (target == null)
        {
            Debug.LogWarning("Projectile target is null. Destroying projectile.");
            Destroy(gameObject);// �������g���폜
            return;
        }
        // �^�[�Q�b�g�܂ł̕����x�N�g�����v�Z
        Vector3 dir = target.position - transform.position;
        // ���̃t���[���Ői�߂鋗�����v�Z�ispeed �~ deltaTime�j
        float distanceThisFrame = speed * Time.deltaTime;
        // �e���^�[�Q�b�g�ɂقړ͂��Ă���ꍇ�A�����������s��
        if (dir.magnitude <= distanceThisFrame)
        {
            // Enemy �X�N���v�g���擾���ă_���[�W��^����
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject); // ������A�����i�e�j���폜
            return;
        }
        // �^�[�Q�b�g�����Ɍ������Ĉ�苗�������O�i
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }
}

