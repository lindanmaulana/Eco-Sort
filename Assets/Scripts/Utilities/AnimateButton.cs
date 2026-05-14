using UnityEngine;
using UnityEngine.EventSystems;

public class AnimateButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Settings")]
    public float scaleFactor = 0.9f;
    public float speed = 15f;
    private Vector3 initialScale;
    private Vector3 targetScale;

    private bool isDestroyed = false;

    void Awake()
    {
        initialScale = transform.localScale;
        targetScale = initialScale;
    }
    
    void Update()
    {
        if (isDestroyed) return;

        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * speed);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        targetScale = initialScale * scaleFactor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        targetScale = initialScale;
    }

    void OnDestroy()
    {
        isDestroyed = true;
    }

    void OnDisable()
    {
        targetScale = initialScale;
        transform.localScale = initialScale;
    }
}
