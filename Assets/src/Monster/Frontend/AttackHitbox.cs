using UnityEngine;
using System;
using System.Collections.Generic;

public class AttackHitbox : NVComponent{
    public int key;
    Collider _box;
    Collider box {get{if(!_box)_box=GetComponent<Collider>(); return _box;}}

    public int damage;
    public string hitSound;
    Dictionary<int, Collider> ignoreList;

    public Vector3 impulse;
    int index=>tform.root.GetComponent<MonsterBody>().monsterPoint.mindex;
    void Awake(){
        ignoreList = new Dictionary<int, Collider>();

    }
    public void SetActive(bool active){
        if(active)
        {    
            ignoreList = new Dictionary<int, Collider>();
        }
        box.enabled=active;
    }


    public void OnTriggerEnter(Collider c){
        Transform croot = c.transform.root;
        if(!ignoreList.ContainsKey(c.transform.root.GetHashCode())){
            Strikeable s = c.transform.root.GetComponent<Strikeable>();
            
            if(s)
            {
                MonsterBody mb = s.GetComponent<MonsterBody>();
                if(!(mb && mb.monsterKey==index)){
                ignoreList.Add(c.GetHashCode(), c);
        print(c);
                s.Strike(damage, tform.position, tform.TransformDirection(impulse),index);
            }
            }
        }
    }
}