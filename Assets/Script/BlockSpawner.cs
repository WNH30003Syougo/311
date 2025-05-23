using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    //���������u���b�N(���j�b�g)�̃v���n�u
    public GameObject InterceptorPrefab;
    //�N���b�N�ΏۂƂȂ�[�n��(Torus)]�̃��C���[
    public LayerMask groundLayer;
    //�u���b�N�̈ړ����x
    public float moveSpeed = 50f;
    //���݃N���b�N������ΏۂɂȂ��Ă���g�[���X�I�u�W�F�N�g
    private Transform currentTorus;
    // �g�[���X�̑�~���a�i�O���~�j�A���~���a�i�����~�j�ʒu�v�Z�ɕK�v
    private float bigRadius;//�g�[���X�̑�~(�O���~)�̔��a�@
    private float smallRadius;//�g�[���X�̏��~�̔��a(�����~)
    // �u���b�N�̈ʒu�����߂邽�߂�2�̊p�x�i�g�[���X��̈ʒu�j
    private float angleTheta;//�g�[���X��̊p�x�ʒu�@ ��~�����i�������j
    private float anglePhi;//�g�[���X��̊p�x�ʒu// ���~�����i�f�ʕ����j
    //���݃V�[�����ɐ������ꂽ�u���b�N�I�u�W�F�N�g�̎Q��
    private GameObject placedBlock;
    void Update()
    {
        // ���N���b�N�Ńu���b�N��z�u�i�����̃u���b�N������ꍇ�͍폜�j
        if (Input.GetMouseButtonDown(0))
        {
            if (placedBlock != null)
            {
                Destroy(placedBlock);
            }
            SpawnBlock();
        }
        // �u���b�N�����݂���ꍇ�́A�ړ��Ɖ�]���󂯕t����
        if (placedBlock != null)
        {
            MoveBlockOnTorus();// ���� �L�[�ňړ�
            RotateBlock();// WASD �L�[�ŉ�]
        }
    }

    void SpawnBlock()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // �g�[���X�̃��C���[�ɑ΂��� Raycast ������
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            // �N���b�N���ꂽ�g�[���X���擾
            currentTorus = hit.transform;
            // �g�[���X�̔��a�i��~�E���~�j���X�V
            UpdateTorusRadii(); // �� �N���b�N���ƂɃ��W�A���X�V
            // �q�b�g�������[���h�ʒu���g�[���X�̃��[�J�����W�ɕϊ�
            Vector3 localHitPoint = currentTorus.InverseTransformPoint(hit.point);
            // ��~�����itheta�j�����߂�iXZ���ʏ�̊p�x
            angleTheta = Mathf.Atan2(localHitPoint.z, localHitPoint.x) * Mathf.Rad2Deg;
            // ��~�̉~����̒��S�_�i���~����]���钆�S�j���v�Z
            float circleCenterX = Mathf.Cos(angleTheta * Mathf.Deg2Rad) * bigRadius;
            float circleCenterZ = Mathf.Sin(angleTheta * Mathf.Deg2Rad) * bigRadius;
            Vector3 circleCenter = new Vector3(circleCenterX, 0, circleCenterZ);
            // �q�b�g�_���璆�S�_�ւ̃I�t�Z�b�g�����i�f�ʕ����j
            Vector3 offset = localHitPoint - circleCenter;
            Vector3 dir = offset.normalized;
            // ���~�����iphi�j�����߂�i�㉺�����̊p�x�j
            anglePhi = Mathf.Asin(dir.y) * Mathf.Rad2Deg;
            // �p�x����ŏI�I�ȃu���b�N�̃��[���h�ʒu�����߂Đ���
            Vector3 torusPos = CalculateTorusPosition(angleTheta, anglePhi);
            placedBlock = Instantiate(InterceptorPrefab, torusPos, Quaternion.identity);
            // �u���b�N���g�[���X�̎q�I�u�W�F�N�g�ɐݒ�i�ʒu�E��]�̓��[���h��j
            placedBlock.transform.SetParent(currentTorus, true);

        }

    }

    void UpdateTorusRadii() // �g�[���X�̃T�C�Y�����~�E���~�̔��a���X�V
    {
        MeshFilter meshFilter = currentTorus.GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            Bounds bounds = meshFilter.sharedMesh.bounds;
            Vector3 size = Vector3.Scale(bounds.size, currentTorus.localScale); // ���ۂ̃��[���h�T�C�Y�l��

            bigRadius = (size.x + size.z) * 0.25f; // X��Z�̕��ς̔����A���ς� 0.25
            smallRadius = size.y * 0.5f; // Y���̔��������~���a


        }
    }
    // �w�肳�ꂽ�p�x����A�g�[���X��̃��[���h���W��Ԃ�
    Vector3 CalculateTorusPosition(float theta, float phi)
    {
        float radTheta = theta * Mathf.Deg2Rad;
        float radPhi = phi * Mathf.Deg2Rad;

        float x = (bigRadius + smallRadius * Mathf.Cos(radPhi)) * Mathf.Cos(radTheta);
        float y = smallRadius * Mathf.Sin(radPhi);
        float z = (bigRadius + smallRadius * Mathf.Cos(radPhi)) * Mathf.Sin(radTheta);
        Vector3 localPos = new Vector3(x, y, z);
        return currentTorus.TransformPoint(localPos);
    }
    void RotateBlock()// WASD �L�[�ɂ��u���b�N�̉�]
    {
        if (placedBlock == null) return;

        float rotationSpeed = 100f;

        if (Input.GetKey(KeyCode.W))
            placedBlock.transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.S))
            placedBlock.transform.Rotate(Vector3.left * rotationSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.A))
            placedBlock.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.D))
            placedBlock.transform.Rotate(Vector3.down * rotationSpeed * Time.deltaTime);
    }
    //void MoveBlockOnTorus()
    //{
    //    if (currentTorus == null) return;

    //    float deltaTheta = 0f;

    //    if (Input.GetKey(KeyCode.LeftArrow)) deltaTheta -= 1f;
    //    if (Input.GetKey(KeyCode.RightArrow)) deltaTheta += 1f;

    //    if (deltaTheta != 0f)
    //    {
    //        angleTheta += deltaTheta * moveSpeed * Time.deltaTime;
    //        Vector3 newPos = CalculateTorusPosition(angleTheta, anglePhi);
    //        placedBlock.transform.position = newPos;

    //        /* Vector3 axis = currentTorus.up * Mathf.Sign(deltaTheta);
    //         float angle = moveSpeed * Time.deltaTime;
    //         currentTorus.Rotate(axis, angle, Space.World);*/
    //    }
    //}
    void MoveBlockOnTorus()// �g�[���X�\�ʂɉ����ău���b�N���ړ����鏈���i�����L�[�ő���j
    {
        if (placedBlock == null || currentTorus == null) return;
        // ���L�[�ɂ���Ċp�x��ύX�i�����Theta�̂݁j
        float deltaTheta = 0f;
        float deltaPhi = 0f;
        // �������g�������ꍇ��deltaPhi���L����
        if (Input.GetKey(KeyCode.UpArrow)) deltaPhi += 1f;
        if (Input.GetKey(KeyCode.DownArrow)) deltaPhi -= 1f;
        if (Input.GetKey(KeyCode.LeftArrow)) deltaTheta -= 1f;
        if (Input.GetKey(KeyCode.RightArrow)) deltaTheta += 1f;

        bool moved = (deltaTheta != 0f || deltaPhi != 0f);
        // �p�x���X�V
        angleTheta += deltaTheta * moveSpeed * Time.deltaTime;
        anglePhi += deltaPhi * moveSpeed * Time.deltaTime;
        // �V�����ʒu���v�Z���ău���b�N���ړ�
        Vector3 newPos = CalculateTorusPosition(angleTheta, anglePhi);
        placedBlock.transform.position = newPos;
        // �g�[���X���̂���]�����邱�ƂŁA�\�ʂɉ����Ă���悤�Ȉ�ۂ����
        if (moved)
        {
            Vector3 axis = Vector3.zero;
            //float angle = moveSpeed * Time.deltaTime;

            if (deltaTheta != 0f)
            {
                // ��~�����̉�]�iY������j
                //axis = currentTorus.up * Mathf.Sign(deltaTheta);
            }
            else if (deltaPhi != 0f)
            {
                // ���~�����̉�]�i�\�ʖ@���Ɋ�Â�����]���j
                Vector3 localPos = currentTorus.InverseTransformPoint(placedBlock.transform.position);
                Vector3 center = new Vector3(
                    Mathf.Cos(angleTheta * Mathf.Deg2Rad) * bigRadius,
                    0,
                    Mathf.Sin(angleTheta * Mathf.Deg2Rad) * bigRadius
                );
                Vector3 normal = (localPos - center).normalized;
                Vector3 worldNormal = currentTorus.TransformDirection(normal);
                axis = Vector3.Cross(worldNormal, Vector3.up).normalized * Mathf.Sign(deltaPhi);
            }
            // �g�[���X�����[���h��Ԃŉ�]
            //currentTorus.Rotate(axis, angle, Space.World);
        }
    }
}