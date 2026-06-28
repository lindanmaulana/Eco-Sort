using UnityEngine;

public class ShopSoundTrigger : MonoBehaviour
{
    [Header("Asset Suara Validasi Toko")]
    [SerializeField] private AudioEvent soundSuccess;
    [SerializeField] private AudioEvent soundFailed;

    public void TriggerSuccess()
    {
        if (AudioManager.instance != null && soundSuccess != null)
        {
            AudioManager.instance.PlaySFX(soundSuccess);
        }
    }

    public void TriggerFailed()
    {
        if (AudioManager.instance != null && soundFailed != null)
        {
            AudioManager.instance.PlaySFX(soundFailed);
        }
    }
}