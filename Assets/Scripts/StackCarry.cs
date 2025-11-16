using UnityEngine;

public class StackCarry : MonoBehaviour
{
    [Header("Stack Ayarları")]
    public Transform stackRoot;          // Player içindeki StackRoot
    public GameObject carriedMeatPrefab; // Sırtta taşıdığın et prefab'ı
    public float stackHeight = 0.25f;    // Her etin yukarı doğru mesafe farkı
    public float pickupRadius = 1.5f;    // Yerden et toplama mesafesi
    public int maxStack = 20;            // Taşınabilecek maksimum et

    [Header("Satış Ayarları")]
    public int moneyPerMeat = 10;        // Her et kaç para ediyor
    public int totalMoney = 0;           // Toplam kazandığın para

    [Header("HUD")]
    public HUDController hud;            // Canvas'taki HUDController

    private int stackCount = 0;

    void Start()
    {
        // Oyuna ilk girince “New Text” yerine 0 gözüksün
        if (hud != null)
        {
            hud.SetMeat(stackCount);
            hud.SetMoney(totalMoney);
        }
        else
        {
            Debug.LogError("StackCarry: HUDController atanmadı!");
        }
    }

    void Update()
    {
        // Her frame et var mı diye etrafı kontrol et
        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRadius);

        foreach (var hit in hits)
        {
            // Sadece tag'i "Meat" olanları al
            if (hit.CompareTag("Meat"))
            {
                PickUpMeat(hit.gameObject);
                break; // Aynı frame'de 1 tane al, yeter
            }
        }
    }

    void PickUpMeat(GameObject worldMeat)
    {
        if (stackCount >= maxStack) return;

        // Yerdeki eti yok et
        Destroy(worldMeat);

        // Yeni taşıma etini sırtta oluştur
        GameObject carried = Instantiate(carriedMeatPrefab, stackRoot);

        // Sırtta üst üste dizilmesi için local pozisyon ver
        carried.transform.localPosition = new Vector3(
            0f,
            stackHeight * stackCount,
            0f
        );

        stackCount++;

        // Et sayısını HUD'de güncelle
        if (hud != null)
        {
            hud.SetMeat(stackCount);
        }
    }

    // TÜM ETLERİ SAT
    public void SellAllMeat()
    {
        if (stackCount <= 0) return;

        // Toplam para hesapla
        int earned = stackCount * moneyPerMeat;
        totalMoney += earned;

        Debug.Log("Etleri sattın! +" + earned + " para. Toplam: " + totalMoney);

        // Sırttaki tüm child objeleri (taşınan etleri) yok et
        for (int i = stackRoot.childCount - 1; i >= 0; i--)
        {
            Transform child = stackRoot.GetChild(i);
            Destroy(child.gameObject);
        }

        // Stack sayısını sıfırla
        stackCount = 0;

        // HUD'de et ve parayı güncelle
        if (hud != null)
        {
            hud.SetMeat(stackCount);
            hud.SetMoney(totalMoney);
        }
    }

    public int CurrentStack => stackCount;

    // Editörde toplama alanını görmek için
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}
