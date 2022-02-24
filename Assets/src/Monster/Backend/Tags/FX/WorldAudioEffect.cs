using UnityEngine;
using System;

public class WorldAudioEffect : Effect{
    public string sound;
    public override void Trigger(Vector3 position, Vector3 direction)
    {
        WorldAudioManager.PlayClipInWorld(sound, position);
    }
}