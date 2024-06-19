using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    public AudioMixer masterMixer;
    public Slider audioSlider;

    public void AudioControl()
    {
        float Sound = audioSlider.value;

        if(Sound == -40f)
        {
            masterMixer.SetFloat("BGM", -80);
            
        }
        else
        {
            masterMixer.SetFloat("BGM", Sound);
        }
    }

    public void ToggleAudioVolume()
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
