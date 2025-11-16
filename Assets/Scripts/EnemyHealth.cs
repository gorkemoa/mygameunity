using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 50f;
    public float currentHealth;

    // Et prefab referansı
    public GameObject meatPrefab;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Ölünce et düşsün
        if (meatPrefab != null)
        {
            Instantiate(meatPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
