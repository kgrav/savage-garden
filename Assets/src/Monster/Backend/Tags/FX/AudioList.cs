using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "AudioList")]
public class AudioList : ScriptableObject{
    
    public AudioListNode[] list;


    [Serializable]
    public class AudioListNode{
        public string label;
        public AudioClip clip;
    }
}