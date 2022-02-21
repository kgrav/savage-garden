using UnityEngine;
using System;
using System.Collections.Generic;

public class UniversalAreaSense : MonsterSense {

    List<MonsterPoint> mpoints;
    
    void Start(){
        mpoints = new List<MonsterPoint>();
    }


    public override List<MonsterPoint> GetPoints(){
        return mpoints;
    }

    void OnTriggerEnter(Collider c){
        MonsterPoint mp = c.GetComponent<MonsterPoint>();
        if(mp){
            if(!mpoints.Contains(mp))
                mpoints.Add(mp);
        }
    }

    void OnTriggerExit(Collider c){
        MonsterPoint mp = c.GetComponent<MonsterPoint>();
        if(mp){
            mpoints.Remove(mp);
            
        }
    }
}