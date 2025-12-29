using UnityEngine;

public class Watersplash : MonoBehaviour
{
    [SerializeField] private AudioClip splashSFX;

    void Start()
    {
        float pitchVariance = 0.5f + UnityEngine.Random.Range(0f, 1f);
        SoundFXManager.instance.PlaySoundEffectClip(splashSFX, transform, 1f, pitchVariance);
    }
}
