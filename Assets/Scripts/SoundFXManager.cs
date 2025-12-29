using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] private AudioSource sfxObject;

    private void Awake()
    {
        instance = this;
        if (instance != null && instance != this) Destroy(this);

    }

    public void PlaySoundEffectClip(AudioClip aClip, Transform spawnTrans, float volume, float pitch = 1f, bool loop = false)
    {
        AudioSource audioSource = Instantiate(sfxObject, spawnTrans.position, Quaternion.identity);

        audioSource.clip = aClip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.loop = loop;

        audioSource.Play();

        if (!loop) Destroy(audioSource.gameObject, audioSource.clip.length);
    }
}
