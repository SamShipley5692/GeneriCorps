using UnityEngine;

public class SoundModulator : MonoBehaviour
{

    [Range(0f, 0.5f)] public float pitchVariance = 0.05f;
    [Range(0f, 0.5f)] public float volumeVariance = 0.1f;

    AudioSource _src;
    void Awake()
    {
        _src = GetComponent<AudioSource>();
        
        _src.loop = false;
    }


    public void PlayOneShotModulated(AudioClip clip, float baseVolume = 1f)
    {
        
        _src.pitch = 1f + Random.Range(-pitchVariance, +pitchVariance);
        
        float vol = baseVolume * (1f + Random.Range(-volumeVariance, +volumeVariance));
        _src.PlayOneShot(clip, vol);
        
        _src.pitch = 1f;
    }

}
