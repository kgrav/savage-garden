using UnityEngine;
using System;

[Serializable]
public class AppetiteConfig{
    public MONMOV movKey;
    public float lowPercent;
    public float highPercent;
    public float value;
    public float max;
    public float decreaseRate;
}