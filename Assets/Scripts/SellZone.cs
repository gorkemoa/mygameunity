    using UnityEngine;

public class SellZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Sadece Player içeri girerse
        if (other.CompareTag("Player"))
        {
            // Player'daki StackCarry scriptini bul
            StackCarry stack = other.GetComponent<StackCarry>();

            if (stack != null)
            {
                // Tüm etleri sat
                stack.SellAllMeat();
            }
        }
    }
}
