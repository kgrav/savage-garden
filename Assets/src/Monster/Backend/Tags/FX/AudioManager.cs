using UnityEngine;
using System;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour{

    static AudioManager _aman;
    public static AudioManager aman {
        get{if(!_aman) _aman = FindObjectOfType<AudioManager>(); return _aman;}
    }

    AudioSource[] worldSfx;

    public int worldSfxSize;
    public AudioListListNode[] nodes;

    bool dictInit = false;


    AudioSource _uiSound;
    AudioSource uiSound {get{if(!_uiSound) _uiSound = GetComponent<AudioSource>(); return _uiSound;}}

    Dictionary<string, Dictionary<string, AudioClip>> _soundTable;

    public void PlayUISound(string label){
        uiSound.PlayOneShot(soundTable["UI"][label]);
        
    }

    public Dictionary<string, Dictionary<string, AudioClip>> soundTable{
        get{
            if(!dictInit)
            {
                _soundTable = new Dictionary<string, Dictionary<string, AudioClip>>();
                dictInit=true;
                foreach(AudioListListNode alln in nodes){
                    Dictionary<string, AudioClip> x = new Dictionary<string, AudioClip>();
                    foreach(AudioList.AudioListNode aln in alln.list.list){
                        x.Add(aln.label, aln.clip);
                    }
                    _soundTable.Add(alln.label, x);
                }
            }
            return _soundTable;
        }
    }

    [Serializable]
    public class AudioListListNode {
        public AudioList list;
        public string label;
    }
}