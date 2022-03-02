using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(menuName="MPTag/Init Sex Tag")]
public class InitSexTag : MPTag {
    public bool flipState;
    public override void Resolve(MonsterPoint source, List<MonsterPoint> targets){
            Monsters.GetMonster(source.mindex).InitSex(targets[0]);
        
    }
}