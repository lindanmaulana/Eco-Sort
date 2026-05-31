using UnityEngine;
using UnityEngine.InputSystem;

public class WasteDraggable : MonoBehaviour
{
    private AppGameManager gameManager;

    [HideInInspector] public bool wasDraggedByPlayer = false;
    private bool isDragging = false;
    private Rigidbody2D rb;
    private Camera mainCamera;
    private Collider2D myCollider;

    [Header("Settings")]
    public float constantSpeed = 3f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        mainCamera = Camera.main;
        
        if (rb != null) {
            rb.gravityScale = 0;
            rb.linearDamping = 0; 
        }
    }

    void Start()
    {
        Launch();
        gameManager = GameObject.FindAnyObjectByType<AppGameManager>();
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
        // 1. TARUH DI PALING ATAS & PAKSA RESET STATUS DRAG
        if (gameManager != null && gameManager.isGameOver)
        {
            isDragging = false; // Paksa lepas status seret!
            return; // Keluar dari fungsi, semua kode di bawah dicuekin
        }

        // 2. SISA KODE DRAG-DROP KAMU YANG LAMA (Ditaruh di bawahnya)
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (myCollider == Physics2D.OverlapPoint(mouseWorldPos))
            {
                isDragging = true;
                wasDraggedByPlayer = true;
                rb.linearVelocity = Vector2.zero;
            }
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && isDragging)
        {
            isDragging = false;
            Launch();
        }

        if (isDragging)
        {
            rb.MovePosition(mouseWorldPos);
        }

        // Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        // Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        // mouseWorldPos.z = 0;

        // if (gameManager != null && gameManager.isGameOver)
        // {
        //     return;
        // }

        // if (Mouse.current.leftButton.wasPressedThisFrame)
        // {
        //     if (myCollider == Physics2D.OverlapPoint(mouseWorldPos))
        //     {
        //         isDragging = true;
        //         wasDraggedByPlayer = true;
        //         rb.linearVelocity = Vector2.zero;
        //     }
        // }

        // if (Mouse.current.leftButton.wasReleasedThisFrame && isDragging)
        // {
        //     isDragging = false;
        //     Launch();
        // }

        // if (isDragging)
        // {
        //     rb.MovePosition(mouseWorldPos);
        // }
    }
}