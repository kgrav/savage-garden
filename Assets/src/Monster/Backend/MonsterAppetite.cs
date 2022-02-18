using UnityEngine;
using System;

public class MonsterAppetite {

    public float lowPercent {get; private set;}
    public float highPercent {get; private set;}
    public float value {get; private set;}
    public float max {get; private set;}
    public float decreaseRate {get; private set;}

    public float priority {get; private set;}

    public MONMOV movKey {get; private set;}

    public float pctValue {get{
        return value/max;
    }}

    public float criticality {get{
        return pctValue < lowPercent ? Mathf.Abs(lowPercent-pctValue) : 0;
    }}

    public bool low {
        get{
            return pctValue <= lowPercent;
        }
    }

    public bool high {
        get{
            return pctValue >= highPercent;
        }
    }

    public MonsterAppetite(MONMOV movKey, float lowPercent, float highPercent, float value, float max, float decreaseRate, float priority){
        this.lowPercent=lowPercent;
        this.highPercent=highPercent;
        this.value=value;
        this.max=max;
        this.decreaseRate=decreaseRate;
        this.priority = priority;
    }

    public MonsterAppetite(AppetiteConfig preset, float priority){
        this.lowPercent=preset.lowPercent;
        this.highPercent=preset.highPercent;
        this.value=preset.value;
        this.max=preset.max;
        this.decreaseRate=preset.decreaseRate;
        this.priority=priority;
    }

    public void Restore(float f){
        if(value < 0){
            value=0;
        }
        value = Mathf.Min(value+f,max);
    }

    public void Update(){
        value -= decreaseRate*Time.deltaTime;
    }
}