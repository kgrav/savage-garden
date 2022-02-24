using System;
using UnityEngine;

public enum SRESP {HIT,IGNORE}
public abstract class Strikeable : NVComponent{

    
    public abstract SRESP Strike(int damage, Vector3 position, Vector3 direction, int source);

    public virtual void AddForce(Vector3 force, bool stunForce){}
}