using UnityEngine;
using UnityEngine.EventSystems;

public class AnimateButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Settings")]
    public float scaleFactor = 0.9f;
    public float speed = 15f;
    private Vector3 initialScale;
    private Vector3 targetScale;

    void Awake()
    {
        initialScale = transform.localScale;
        targetScale = initialScale;
    }

    void Update()
    {
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
}
