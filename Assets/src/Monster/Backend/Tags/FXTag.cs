using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName="MPTag/FX Tag")]
public class FXTag : MPTag {
    public int effectKey;
    public override void Resolve(MonsterPoint source, List<MonsterPoint> targets){
        EffectsTable.TriggerEffect(effectKey, source.tform.position, source.tform.forward);
    }
}