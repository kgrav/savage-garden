using UnityEngine;
using System;

public class WorldAudioManager : MonoBehaviour{

    static WorldAudioManager _wam;
    public static WorldAudioManager wam {get{if(!_wam) _wam = FindObjectOfType<WorldAudioManager>(); return _wam;}}

    public string audioKey;
    public int size;
    int current;
    
    AudioSource[] _audioSources;
    AudioSource[] audioSources{
        get{
            if(!sourcesInit){
                sourcesInit=true;
                _audioSources = new AudioSource[size];
                for(int i = 0; i < size; ++i){
                    _audioSources[i] = Instantiate(new GameObject()).AddComponent<AudioSource>();
                    _audioSources[i].volume=0.1f;
                    _audioSources[i].pitch=0.5f;
                }
            }
            return _audioSources;
        }
    }
    bool sourcesInit = false;

    public static void PlayClipInWorld(string clip, Vector3 position){
        wam.audioSources[wam.current].transform.position = position;
        wam.audioSources[wam.current].PlayOneShot(AudioManager.aman.soundTable[wam.audioKey][clip]);
        wam.current++;
        if(wam.current >= wam.size){
            wam.current = 0;
        }
    }

}