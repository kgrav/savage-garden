using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(menuName="MPTag/Kill Self Tag")]
public class KillSelfTag : MPTag {

    public override void Resolve(MonsterPoint source, List<MonsterPoint> targets){
        if(source.mindex >-1){
            Monsters.GetMonster(source.mindex).KillMonster("killed by tag effect");
        }
    }
}