using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] private Image healthBarFill;
    [SerializeField] private Text meatText;
    [SerializeField] private Text moneyText;

    private PlayerHealth playerHealth;

    void Start()
    {
        // PlayerHealth bul
        playerHealth = FindFirstObjectByType<PlayerHealth>();

        if (playerHealth == null)
        {
            Debug.LogError("[HUDController] PlayerHealth bulunamadı!");
            return;
        }

        // Evente abone ol
        playerHealth.OnHealthChanged += HandleHealthChanged;

        // Başlangıç değeri
        HandleHealthChanged(playerHealth.CurrentHealth, playerHealth.maxHealth);
    }

    void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= HandleHealthChanged;
        }
    }

    private void HandleHealthChanged(float current, float max)
    {
        if (healthBarFill == null) return;

        float ratio = (max > 0f) ? current / max : 0f;
        healthBarFill.fillAmount = ratio;
        // Debug.Log($"[HUD] HP Ratio: {ratio}");
    }

    // Et / para için kullandığın update metodları aynen kalabilir,
    // sadece health kısmını event’e taşıdık.
    public void UpdateMeatText(int meatCount)
    {
        if (meatText != null)
            meatText.text = meatCount.ToString();
    }

    public void UpdateMoneyText(int money)
    {
        if (moneyText != null)
            moneyText.text = money.ToString();
    }
}
