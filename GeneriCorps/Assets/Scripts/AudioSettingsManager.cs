using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingsManager : MonoBehaviour
{

    public static AudioSettingsManager Instance { get; private set; }
    public AudioMixer mixer;

    public string masterKey = "MasterVolume";
    public string musicKey = "MusicVolume";
    public string sfxKey = "SFXVolume";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject); return;
        }

        ApplyKey(masterKey);
        ApplyKey(musicKey);
        ApplyKey(sfxKey);

    }

    void Start()
    {
        ApplyKey(masterKey);
        ApplyKey(musicKey);
        ApplyKey(sfxKey);

        var src = GetComponent<AudioSource>();
        if (src != null && !src.isPlaying)
            src.Play();
    }

    void ApplyKey(string key)
        {
            float val = PlayerPrefs.GetFloat(key, 1f);
            float db = Mathf.Log10(Mathf.Max(val, 0.0001f)) * 20f;
            mixer.SetFloat(key, db);
        }


         public void SetVolume(string key, float linearValue)
    {
        // drive the mixer
        float db = Mathf.Log10(Mathf.Max(linearValue, 0.0001f)) * 20f;
        mixer.SetFloat(key, db);

        // save it
        PlayerPrefs.SetFloat(key, linearValue);
    }

    public float GetVolume(string key) 
        
        => PlayerPrefs.GetFloat(key, 1f);
    
}
