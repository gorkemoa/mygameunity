using UnityEngine;
using TMPro;   // 🔹 ÖNEMLİ: TextMeshPro için

public class HUDController : MonoBehaviour
{
    [Header("Referanslar")]
    public StackCarry stackCarry;    // Player'daki StackCarry
    public TMP_Text meatText;        // Et yazısı
    public TMP_Text moneyText;       // Para yazısı

    private void Awake()
    {
        // StackCarry inspector'dan atanmadıysa sahneden bul
        if (stackCarry == null)
        {
            stackCarry = FindObjectOfType<StackCarry>();
            if (stackCarry == null)
            {
                Debug.LogError("[HUD] StackCarry bulunamadı.");
            }
        }
    }

    private void Update()
    {
        if (stackCarry == null || meatText == null || moneyText == null)
            return;

        meatText.text = $"Et: {stackCarry.CurrentStack}/{stackCarry.maxStack}";
        moneyText.text = $"Para: {stackCarry.totalMoney}";
    }
}
