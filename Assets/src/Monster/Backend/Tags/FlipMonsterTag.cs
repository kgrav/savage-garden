using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(menuName="MPTag/Flip Target Tag")]
public class FlipMonsterTag : MPTag {
    public bool flipState;
    public override void Resolve(MonsterPoint source, List<MonsterPoint> targets){
        if(targets[0] && targets[0].mindex >-1){
            Monsters.GetMonster(targets[0].mindex).FlipMonster(flipState);
        }
    }
}