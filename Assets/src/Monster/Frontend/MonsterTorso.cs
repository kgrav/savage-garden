using UnityEngine;
using System;

public class MonsterTorso : MonsterPart {
    public Transform rarm,larm,head,mouth,rleg,lleg;
    public int rArmPrefab,lArmPrefab,headPrefab,mouthPrefab,rLegPrefab,lLegPrefab;

    public Affinity Build(){

        if(rArmPrefab>-1){
            InstantiatePart(rArmPrefab,rarm);
        }
        if(lArmPrefab>-1){
            InstantiatePart(lArmPrefab,larm);
        }
        if(headPrefab>-1){
            InstantiatePart(headPrefab,head);
        }
        if(mouthPrefab>-1){
            InstantiatePart(mouthPrefab,mouth);
        }
        if(rLegPrefab>-1){
            InstantiatePart(rLegPrefab,rleg);
        }
        if(lLegPrefab>-1){
            InstantiatePart(lLegPrefab,lleg);
        }
    }

    Affinity InstantiatePart(int prefabKey,Transform prefabAttachPt){
        GameObject q = GameObject.Instantiate(Monsters.monsters.partPrefabs[rArmPrefab], rarm.position,rarm.rotation);
        q.transform.SetParent(rarm);
        q.transform.localPosition = Vector3.zero;
        return q.GetComponentInChildren<MonsterPart>().affinity;
    }
}