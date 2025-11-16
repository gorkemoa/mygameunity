using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Ayarları")]
    public int maxHealth = 5;
    public int currentHealth;

    [Header("HUD Referansı")]
    public HUDController hud;

    [Header("Animasyon")]
    public Animator anim;   // 👈 HumanMale_Character_FREE animator

    void Start()
    {
        currentHealth = maxHealth;

        if (hud != null)
        {
            hud.SetMaxHealth(maxHealth);
            hud.SetHealth(currentHealth);
        }

        if (anim == null)
        {
            // Player altındaki animatoru otomatik bul
            anim = GetComponentInChildren<Animator>();
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        if (hud != null)
            hud.SetHealth(currentHealth);

        // 🔥 Hasar animasyonu
        if (anim != null)
        {
            anim.SetTrigger("Hit");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Oyuncu öldü!");

        if (anim != null)
        {
            anim.SetTrigger("Die");
        }

        // İstersen burada hareketi kapat:
        // GetComponent<PlayerMovement>().enabled = false;
        // GetComponent<CharacterController>().enabled = false;
    }
}
