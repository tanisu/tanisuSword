using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField] Slider BGMSlider, SESlider;

    void Start()
    {
        BGMSlider.value = SoundManager.I.bgmVolume;
        SESlider.value = SoundManager.I.seVolume;
        BGMSlider.onValueChanged.AddListener((value) => {
            SoundManager.I.bgmVolume = value;
            SoundManager.I.ChangeBGMVolumes();
        });
        SESlider.onValueChanged.AddListener((value) => {
            SoundManager.I.seVolume = value;
            SoundManager.I.ChangeSEVolumes();
        });
    }

    

}
