using UnityEngine;
using System;

public class Music : MonoBehaviour{
    AudioSource asrc=>GetComponent<AudioSource>();

    void Update(){
        asrc.pitch=0.9999f*asrc.pitch;
    }
}