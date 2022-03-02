using UnityEngine;
using System;

public class MPDamageBody : Strikeable {
    public int hp;
    int maxhp;
    public MPTag[] onKill;
    public MPTag[] onDamage;

    void Awake(){
        MonsterPart[] mrts = GetComponentsInChildren<MonsterPart>();
        if(mrts.Length>0){
            maxhp = 0;
            foreach(MonsterPart mr in mrts){
                maxhp+=mr.HP;
            }
        }
        hp = maxhp;
    }

    public void SleepHeal(){
        hp = Math.Min(hp+maxhp/2,maxhp);
    }

    MonsterPoint self=>GetComponent<MonsterPoint>();
    public override SRESP Strike(int damage, Vector3 position, Vector3 direction, int source)
    {
        if(hp > 0){
        hp-=damage;
        if(hp<=0){
            MPTag.ResolveTags(self, Monsters.GetMonster(source).body.monsterPoint,onKill);
        }
        else{
            MPTag.ResolveTags(self, Monsters.GetMonster(source).body.monsterPoint,onDamage);
        }
            return SRESP.HIT;
        }
        return SRESP.IGNORE;
    }
}