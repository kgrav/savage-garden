using UnityEngine;
using System;
using System.Collections.Generic;

public class UniversalAreaSense : MonsterSense {

    List<long> keys => new List<long>();
    Dictionary<long,MonsterPoint> senses => new Dictionary<long, MonsterPoint>();


    protected override void NVUpdate(){
        foreach(long l in keys){
            
            if(!senses[l]){
                senses.Remove(l);
                keys.Remove(l);
            }
        }
    }

    public override List<MonsterPoint> GetPoints(){
        List<MonsterPoint> r = new List<MonsterPoint>();
        foreach(long l in keys){
            r.Add(senses[l]);
        }
        return r;
    }

    void OnTriggerEnter(Collider c){
        MonsterPoint mp = c.GetComponent<MonsterPoint>();
        if(mp){
            if(!senses.ContainsKey(mp.GetHashCode())){
                senses.Add(mp.GetHashCode(), mp);
                keys.Add(mp.GetHashCode());
            }
        }
    }

    void OnTriggerExit(Collider c){
        MonsterPoint mp = c.GetComponent<MonsterPoint>();
        if(mp){
            if(senses.ContainsKey(mp.GetHashCode())){
                senses.Remove(mp.GetHashCode());
                keys.Remove(mp.GetHashCode());
            }
        }
    }
}