using UnityEngine;

public class KyotenHealth : MonoBehaviour
{
    public int maxHp = 100;
    private int currentHp;
    private bool isDead = false;

    void Start()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHp -= amount;
        Debug.Log("‹’“_ HP: " + currentHp);
        if (currentHp <= 0)
        {
            isDead = true;
            Debug.Log("‹’“_‚ª”j‰ó‚³‚ê‚½I");
            Destroy(gameObject);
        }
    }
}
