using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class AppCarouselScroll : MonoBehaviour, IEndDragHandler, IDragHandler
{
    [Header("References")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform contentPanel;

    [Header("Settings")]
    [SerializeField] private float snapSpeed = 10f;

    private List<RectTransform> mapElements = new List<RectTransform>();
    private bool isSnapping = false;
    private Vector2 targetPosition;
    private int currentSelectedIndex = 0;

    void Start()
    {
        // Ambil semua elemen gambar map yang ada di dalam Content
        foreach (Transform child in contentPanel)
        {
            if (child.gameObject.activeInHierarchy)
            {
                mapElements.Add(child.GetComponent<RectTransform>());
            }
        }
    }

    void Update()
    {
        // Jika sedang mode snapping, geser content secara halus menuju posisi target
        if (isSnapping)
        {
            contentPanel.localPosition = Vector2.Lerp(contentPanel.localPosition, targetPosition, snapSpeed * Time.deltaTime);
            
            // Hentikan jika sudah sangat dekat dengan target
            if (Vector2.Distance(contentPanel.localPosition, targetPosition) < 0.1f)
            {
                contentPanel.localPosition = targetPosition;
                isSnapping = false;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Saat pemain sedang menggeser dengan jari, matikan efek snapping otomatisnya
        isSnapping = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Cari gambar mana yang posisinya paling dekat dengan titik tengah layar/viewport
        float closestDistance = float.MaxValue;
        int closestIndex = 0;

        // Titik tengah dari posisi carousel lokal
        float carouselCenterX = 0f; 

        for (int i = 0; i < mapElements.Count; i++)
        {
            // Hitung posisi absolut element terhadap parent-nya
            float elementGlobalX = mapElements[i].localPosition.x + contentPanel.localPosition.x;
            float distance = Mathf.Abs(carouselCenterX - elementGlobalX);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        currentSelectedIndex = closestIndex;
        SnapToElement(closestIndex);
    }

    private void SnapToElement(int index)
    {
        // Hitung koordinat X yang pas agar elemen target berada tepat di tengah-tengah
        float targetX = -mapElements[index].localPosition.x;
        targetPosition = new Vector2(targetX, contentPanel.localPosition.y);
        isSnapping = true;

        Debug.Log($"[Carousel] Sedang fokus di Map Index ke-{index}: {mapElements[index].name}");
    }

    // Fungsi bantuan untuk dipanggil oleh Tombol "MAIN" nanti
    public int GetCurrentSelectedIndex()
    {
        return currentSelectedIndex;
    }
}