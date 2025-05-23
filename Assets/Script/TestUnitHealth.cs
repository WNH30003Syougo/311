using UnityEngine;

public class TestUnitHealth : MonoBehaviour
{
    public int maxHp = 50;
    private int currentHp;

    void Start()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(int amount)
    {
        currentHp -= amount;
        Debug.Log("TestUnit ���_���[�W���󂯂�: " + amount);
        if (currentHp <= 0)
        {
            Debug.Log("TestUnit ���j�󂳂ꂽ");
            Destroy(gameObject);
        }
    }
}
