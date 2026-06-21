using UnityEngine;

[CreateAssetMenu(fileName = "NewAudioEvent", menuName = "Audio/Audio Event")]
public class AudioEvent : ScriptableObject
{
    public AudioClip[] clips; // Masukkan satu atau beberapa sound clip di sini

    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.5f, 1.5f)] public float pitch = 1f;
    public bool useRandomPitch = true; // Bikin suara sedikit variasi tiap bunyi

    // Fungsi untuk memainkan suara ini pada AudioSource yang dikirimkan
    public void Play(AudioSource source)
    {
        if (clips.Length == 0) return;

        // Pilih clip acak kalau isi array-nya lebih dari satu
        source.clip = clips[Random.Range(0, clips.Length)];
        source.volume = volume;
        
        // Kasih sedikit variasi pitch jika dicentang (bagus untuk koin/tembakan)
        source.pitch = useRandomPitch ? pitch + Random.Range(-0.1f, 0.1f) : pitch;
        
        source.Play();
    }
}