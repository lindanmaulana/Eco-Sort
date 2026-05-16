using UnityEngine;
using UnityEngine.InputSystem; // Tambahkan ini agar sistem baru terbaca

public class WasteDraggable : MonoBehaviour
{

    [HideInInspector] public bool wasDraggedByPlayer = false;
    private bool isDragging = false;
    private Rigidbody2D rb;
    private Camera mainCamera;

    [Header("Settings")]
    public float constantSpeed = 3f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        
        if (rb != null) {
            rb.gravityScale = 0;
            rb.linearDamping = 0; 
        }
    }

    void Start()
    {
        Launch();
    }

    void Launch()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        rb.linearVelocity = new Vector2(x, y).normalized * constantSpeed;
    }

    void FixedUpdate()
    {
        if (!isDragging && rb.linearVelocity.magnitude > 0)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * constantSpeed;
        }
    }

    void Update()
    {
        if (isDragging)
        {
            // Cara baru mengambil posisi mouse di Input System Package
            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
            mouseWorldPos.z = 0;
            
            rb.MovePosition(mouseWorldPos);
        }
    }

    // Fungsi ini tetap jalan selama kamu punya Physics 2D Raycaster di Camera
    private void OnMouseDown()
    {
        isDragging = true;
        wasDraggedByPlayer = true;
        rb.linearVelocity = Vector2.zero;
    }

    private void OnMouseUp()
    {
        isDragging = false;
        Launch();
    }
}