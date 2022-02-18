    using UnityEngine;
    using System;
    
    public class Appetite{
        public static float criticalityMultiplier = 5;
        public MONMOV type;
        protected int _motiveKey;
        public int motiveKey {get{return _motiveKey;}}
        protected float _value;
        public float value {get{return _value;}}
        protected float _loAppetite, _hiAppetite;
        public float loAppetite {get{return _loAppetite;}}
        public float hiAppetite {get{return _hiAppetite;}}
        protected float _priority;
        public float priority {get{return _priority;}}
        protected float _max;
        public float max {get{return _max;}}

        public Appetite(){
            _value=0;
            _loAppetite=0;
            _hiAppetite=0;
            _priority=0;
            _max=0;
        }

        public Appetite(int key, float valuepct, float loAppetitepct, float hiAppetitepct, float max, float priority){
            _value = valuepct*max;
            _loAppetite = loAppetitepct*max;
            _hiAppetite = hiAppetitepct*max;
            _priority = priority;
            _max = max;
        }

        public void Update(float rate, float time){
            _value += rate*time;
            _value = Mathf.Min(value,max);
        }

        public bool isLow(){
            return value <=loAppetite;

        }

        public bool isFull(){
            return value >=hiAppetite;
        }

        public bool halfFull(){
            return value >= 0.5f*max;
        }

        public float Criticality(){
            return criticalityMultiplier*((loAppetite-value)/loAppetite);
        }

        public bool isPositive(){
            return value > 0;
        }

        public void SetValuePct(float pct){
            _value = pct*max;
        }

        public bool isCritical(){
            return value/loAppetite <=0.33f;
        }

        public virtual void rstep(Monster m, MonsterPoint mp){
            
        }
    } 