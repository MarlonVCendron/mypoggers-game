using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsController : MonoBehaviour
{
    public AudioMixer mainMixer;

    public void SetFullscreen(bool isFullscreen){
        if(isFullscreen){
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        } else {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }
    //gráficos
    public void SetVolume(float volume){
        mainMixer.SetFloat("volume", volume);
    }
}
