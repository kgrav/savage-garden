using UnityEngine;
using System.Collections.Generic;


public abstract class MPTag : ScriptableObject {
    public static void ResolveTags(MonsterPoint source, MonsterPoint target, MPTag[] tags){
        
        List<MonsterPoint> mplist = null;
        if(target)
        {
            mplist=new List<MonsterPoint>();
            mplist.Add(target);
        }
        ResolveTags(source,mplist,tags);
    }
    public static void ResolveTags(MonsterPoint source, List<MonsterPoint> targets, MPTag[] tags){
        foreach(MPTag t in tags){
                t.Resolve(source,targets);
        }
    }

    public static void GetConditions(MPTag[] tags){
        
    }

    public abstract void Resolve(MonsterPoint source, List<MonsterPoint> targets);
     
}  