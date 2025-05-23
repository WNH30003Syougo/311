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
        Debug.Log("TestUnit がダメージを受けた: " + amount);
        if (currentHp <= 0)
        {
            Debug.Log("TestUnit が破壊された");
            Destroy(gameObject);
        }
    }
}
