using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBody : NVComponent
{
    public int monsterKey {get; private set;}

    public MonsterSense sense =>GetComponentInChildren<MonsterSense>();

    public void Setup(Monster m){
        monsterKey = m.index;
        monsterPoint.Setup(monsterKey);
    }

    public Transform rarm,larm,torso,head,mouth,rleg,lleg;
    public int rArmPrefab,lArmPrefab,torsoPrefab,headPrefab,mouthPrefab,rLegPrefab,lLegPrefab;

    public void Build(){
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

    void InstantiatePart(int prefabKey,Transform prefabAttachPt){
        GameObject q = GameObject.Instantiate(Monsters.monsters.partPrefabs[rArmPrefab], rarm.position,rarm.rotation);
        q.transform.SetParent(rarm);
        q.transform.localPosition = Vector3.zero;
    }
    Rigidbody rig=>GetComponent<Rigidbody>();
    public MonsterPoint monsterPoint=>GetComponent<MonsterPoint>();
    public MonsterLeg[] legs=>GetComponentsInChildren<MonsterLeg>();
    
    public float maxVelocity;
    public bool actionMsg;
    public bool appetitesMsg;
    public bool motiveOrderMsg;
    

    Vector3 ClampedVelocity(Vector3 velocity){
        float vmag = velocity.magnitude;
        Vector3 r = velocity;
        if(vmag > movespd*maxVelocity){
            r = movespd*maxVelocity*velocity.normalized;
        }
        return r;
    }

    public bool hasLegs {get{
        bool one=false;
        foreach(MonsterLeg l in legs){
            if(l!=null){
                one=true;
            }
        }
        return one;
    }}
    float movespd=0;
    public float moveSpeed {get{return movespd;}}
    void Awake(){
        foreach(MonsterLeg leg in GetComponentsInChildren<MonsterLeg>()){
            leg.SetBody(this);
        }
    }

    // Update is called once per frame

    public void SetFaceDirection(Vector3 direction){
        tform.LookAt(tform.position + direction,Vector3.up);
        float vmag = rig.velocity.magnitude;
        rig.velocity=direction*vmag;
    }

    public void SetMoveSpeed(float movespdn){
        if(movespdn < 0.05f && movespd > 0.05f){
            foreach(MonsterLeg leg in legs){
                leg.StopWalkCycle();
            }
            rig.velocity=Vector3.zero;
        }
        else if(movespdn > 0.05f && movespd < 0.05f){
            float delay = 0;
            foreach(MonsterLeg leg in legs){
                leg.SetWalkCycle(delay);
                delay += 0.5f;
            }
        }
        movespd=movespdn;
    }

    protected override void NVUpdate()
    {
        rig.velocity=ClampedVelocity(rig.velocity);
    }


    public void AddLegForce(MonsterLeg leg){
        rig.AddForce(leg.legforce*movespd*tform.forward);
    }

    public void AddLegForce(float f){
        rig.AddForce(f*movespd*tform.forward);
    }


    public MonsterPart GetBestAttack(){
        MonsterPart best = null;
        foreach(MonsterPart mp in GetComponentsInChildren<MonsterPart>()){
            if(mp.hasAttack){
                if(best == null || mp.damage > best.damage)
                    best = mp;
            }
        }
        return best;
    }
}
