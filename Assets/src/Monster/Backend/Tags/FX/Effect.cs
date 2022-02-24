using UnityEngine;
using System;

public abstract class Effect : NVComponent{
    public abstract void Trigger(Vector3 position, Vector3 direction);
}