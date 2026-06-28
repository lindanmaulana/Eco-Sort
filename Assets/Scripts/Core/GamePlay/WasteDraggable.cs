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

    [Header("---- Audio Settings (BARU) ----")]
    [SerializeField] private AudioEvent correctSortSFX;
    [SerializeField] private AudioEvent wrongSortSFX;

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
        if (gameManager != null && gameManager.isGameOver)
        {
            isDragging = false; 
            return; 
        }

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
    }

    public void PlayFeedbackSFX(bool isCorrect)
    {
        bool isSFXOn = PlayerPrefs.GetInt(DataKeyPlayerPrefs.SETTING_SOUND_BACKGROUND, 1) == 1;
        if (isSFXOn && AudioManager.instance != null)
        {
            if (isCorrect && correctSortSFX != null)
            {
                AudioManager.instance.PlaySFX(correctSortSFX);
            }
            else if (!isCorrect && wrongSortSFX != null)
            {
                AudioManager.instance.PlaySFX(wrongSortSFX);
            }
        }

        if (!isCorrect)
        {
            bool isVibrationOn = PlayerPrefs.GetInt(DataKeyPlayerPrefs.SETTING_VIBRATE_BACKGROUND, 1) == 1;
            
            if (isVibrationOn)
            {
                TriggerWrongHaptic();
            }
        }
    }

    private void TriggerWrongHaptic()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator"))
            {
                if (vibrator != null)
                {
                    vibrator.Call("vibrate", (long)60); 
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Gagal memicu getar Android custom: " + e.Message);
            Handheld.Vibrate(); 
        }
        #else
        Handheld.Vibrate();
        #endif
    }
}