using UnityEngine;
using UnityEngine.UI;

public class SoundTrigger : MonoBehaviour
{
    [SerializeField] private AudioEvent soundToPlay;

    public void TriggerSound()
    {
        bool isSFXOn = PlayerPrefs.GetInt(DataKeyPlayerPrefs.SETTING_SOUND_BACKGROUND, 1) == 1;

        if (!isSFXOn) return;

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(soundToPlay);
        }
    }

    void Start()
    {
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(TriggerSound);
        }
    }

    private void OnEnable()
    {
        Button btn = GetComponent<Button>();
        Collider2D col = GetComponent<Collider2D>();
        
        if (btn == null && col == null)
        {
            TriggerSound();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TriggerSound();
        }
    }
}