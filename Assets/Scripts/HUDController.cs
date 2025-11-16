using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("Health Bar")]
    public Image healthBarFill;

    [Header("Texts")]
    public TMP_Text meatText;   // Üstündeki et sayısı
    public TMP_Text moneyText;  // Para miktarı

    private int _maxHealth = 1;

    void Awake()
    {
        // Oyuna girer girmez “New Text” yerine temiz başlangıç yazsın
        SetMeat(0);
        SetMoney(0);
    }

    // -------- CAN --------
    public void SetMaxHealth(int max)
    {
        _maxHealth = Mathf.Max(1, max);

        if (healthBarFill != null)
            healthBarFill.fillAmount = 1f;
    }

    public void SetHealth(int current)
    {
        if (healthBarFill == null) return;

        float t = Mathf.Clamp01((float)current / _maxHealth);
        healthBarFill.fillAmount = t;
    }

    // -------- ET --------
    public void SetMeat(int value)
    {
        if (meatText == null) return;

        // Örn: 🦴 Et: x3   /   🦴 Et yok
        if (value <= 0)
            meatText.text = "Et: yok";
        else
            meatText.text = $"Et: x{value}";
    }

    // -------- PARA --------
    public void SetMoney(int value)
    {
        if (moneyText == null) return;

        // Binlik ayırıcıyla yaz (1.250 gibi) ve ₺ sembolü ekle
        string formatted = value.ToString("N0"); // 1250 -> 1.250
        moneyText.text = $"Para: {formatted} TL";
    }
}
