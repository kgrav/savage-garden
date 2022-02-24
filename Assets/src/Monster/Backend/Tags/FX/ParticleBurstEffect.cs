using UnityEngine;
using System;

public class ParticleBurstEffect : Effect{
    ParticleSystem _psys;
    public ParticleSystem psys {get{if(!_psys) _psys=GetComponent<ParticleSystem>(); return _psys;}}
    public int particles;
    public override void Trigger(Vector3 position, Vector3 direction)
    {
        tform.position = position;
        tform.LookAt(position + direction);
        psys.Emit(particles);
    }
}