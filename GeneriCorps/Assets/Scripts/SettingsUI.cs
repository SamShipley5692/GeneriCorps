using UnityEngine;
using UnityEngine.UI;


public class SettingsUI : MonoBehaviour
{

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    public Toggle masterToggle;
    public Toggle musicToggle;
    public Toggle sfxToggle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        var mgr = AudioSettingsManager.Instance;


        masterSlider.onValueChanged.AddListener(v => {
            AudioSettingsManager.Instance.SetVolume(AudioSettingsManager.Instance.masterKey, v);
            masterToggle.isOn = v > masterSlider.minValue;
        });
        musicSlider.onValueChanged.AddListener(v => {
            AudioSettingsManager.Instance.SetVolume(AudioSettingsManager.Instance.musicKey, v);
            musicToggle.isOn = v > musicSlider.minValue;
        });
        sfxSlider.onValueChanged.AddListener(v => {
            AudioSettingsManager.Instance.SetVolume(AudioSettingsManager.Instance.sfxKey, v);
            sfxToggle.isOn = v > sfxSlider.minValue;
        });

        masterToggle.onValueChanged.AddListener(on => {
            masterSlider.value = on ? masterSlider.maxValue : masterSlider.minValue;
        });
        musicToggle.onValueChanged.AddListener(on => {
            musicSlider.value = on ? musicSlider.maxValue : musicSlider.minValue;
        });
        sfxToggle.onValueChanged.AddListener(on => {
            sfxSlider.value = on ? sfxSlider.maxValue : sfxSlider.minValue;
        });


        masterSlider.value = mgr.GetVolume(mgr.masterKey);
        musicSlider.value = mgr.GetVolume(mgr.musicKey);
        sfxSlider.value = mgr.GetVolume(mgr.sfxKey);


        masterToggle.isOn = masterSlider.value > masterSlider.minValue;
        musicToggle.isOn = musicSlider.value > musicSlider.minValue;
        sfxToggle.isOn = sfxSlider.value > sfxSlider.minValue;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
