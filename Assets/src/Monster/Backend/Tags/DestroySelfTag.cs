using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName="MPTag/Destroy Self")]
public class DestroySelfTag : MPTag{

    public override void Resolve(MonsterPoint source, List<MonsterPoint> targets){
        if(source.mindex >= 0){
            Monsters.DestroyMonster(source.mindex);
        }
        else{
            GameObject.Destroy(source.gameObject);
        }
    }

}