using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(menuName="MPTag/Heal Monster Tag")]
public class HealMonsterTag : MPTag {

    public override void Resolve(MonsterPoint source, List<MonsterPoint> targets){
        if(targets[0] && targets[0].mindex >-1){
            Monsters.GetMonster(targets[0].mindex).Heal();
        }
    }
}