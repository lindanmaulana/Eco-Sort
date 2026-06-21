using UnityEngine;
using UnityEngine.UI;

public class SoundTrigger : MonoBehaviour
{
    [SerializeField] private AudioEvent soundToPlay;

    // Fungsi untuk memicu suara secara manual lewat code lain
    public void TriggerSound()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(soundToPlay);
        }
    }

    // OTOMATIS: Jika ditempel di Tombol UI, langsung nge-hook fungsi kliknya
    void Start()
    {
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(TriggerSound);
        }
    }

    // OTOMATIS: Jika ditempel di Koin (Trigger 2D)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TriggerSound();
        }
    }
}