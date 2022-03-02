using UnityEngine;
using System;
using System.Collections.Generic;

public class Monsters : MonoBehaviour
{
    public GameObject[] partPrefabs;

    public int[] torsos,arms,heads,legs;
    
    public static Monsters monsters=>FindObjectOfType<Monsters>();

    public float affinityRscale;
    public static float affinityScale {get{return monsters.affinityRscale;}}
    static bool listInit = false;
    static List<Monster> _monsterList;
    static List<Monster> monsterList {get{if(!listInit){
        listInit=true;
        _monsterList = new List<Monster>(); 
    }
    return _monsterList;
    }}
    public static List<AffinityData> GetAffinityData(int index){
        return GetMonster(index).GetAffinityData();
    }

    public static bool Check(float dc){
        float d20i = UnityEngine.Random.Range(0f,20f);
        return d20i > dc;
    }

    public static bool CheckInverse(float dc){
        float d20i = UnityEngine.Random.Range(0f,20f);
        return d20i > (20-dc);
    }

    public static GameObject CreateBedPoint(Vector3 pos){
        return Instantiate(monsters.AdHocBedPoint, pos, Quaternion.identity);
    }
    public GameObject AdHocBedPoint;

    public static List<AppetiteBarData> GetAppetiteData(int index){
        return GetMonster(index).GetAppetiteData();
    }


    public GameObject marker;
    public static Monster GetMonster(int indx){
        return monsterList[indx];
    }

    public static void DestroyMonster(int i){
        Destroy(GetMonster(i).body);
    }

    public bool roll(int modifier, int check){
        return UnityEngine.Random.Range(1,21)+modifier > check;
    }


    public void CreateRandomMonster(){
        int torso = torsos[UnityEngine.Random.Range(0,torsos.Length)];
        int arm1 = Check(19f) ? -1 : arms[UnityEngine.Random.Range(0,arms.Length)];
        int arm2 = Check(19f) || arm1 > -1 ? -1 : arms[UnityEngine.Random.Range(0,arms.Length)];
        int leg = legs[UnityEngine.Random.Range(0,legs.Length)];
        int head = heads[UnityEngine.Random.Range(0,heads.Length)];
        MonsterTorso q = Instantiate(partPrefabs[torso]).GetComponent<MonsterTorso>();
        q.rArmPrefab = arm1;
        q.lArmPrefab = arm2;
        q.rLegPrefab = leg;
        q.headPrefab = head;
        q.Build();
    }

    public GameObject egg;
    
    public static Vector3 GetWanderPoint(Vector3 near, float range){
        Vector3 ptadjust = new Vector3((UnityEngine.Random.Range(0,range*2) - range),
                                            0,
                                            (UnityEngine.Random.Range(0,range*2)-range));
        ptadjust.x = Mathf.Clamp(ptadjust.x,monsters.wanderCenter.x-monsters.wanderSize.x,monsters.wanderCenter.x+monsters.wanderSize.x);
        ptadjust.z = Mathf.Clamp(ptadjust.z,monsters.wanderCenter.z-monsters.wanderSize.z,monsters.wanderCenter.z+monsters.wanderSize.z);
        return near + ptadjust;
    }
    public static Vector3 GetWanderPoint(){
        return monsters.wanderCenter + new Vector3((UnityEngine.Random.Range(0,monsters.wanderSize.x*2) - monsters.wanderSize.x),
                                            0,
                                            (UnityEngine.Random.Range(0,monsters.wanderSize.z*2)-monsters.wanderSize.z));
    }

    public static bool rbool {get{return UnityEngine.Random.Range(0f,1f) > 0.5f;}}
    public static float QualityScale {get{return Monsters.monsters.qualityScale;}}
    static MAction BuildHungerAction(Monster m){
        return null;
    }

    public static bool pause = false;

    public Vector3 wanderSize, wanderCenter;
    public Vector2 rstepInterval;

    public static float rtime {
        get{
            return UnityEngine.Random.Range(monsters.rstepInterval.x,monsters.rstepInterval.y);
        }
    }

public int criticalityDeathThreshold;

public float PriorityDecay;
public float qualityScale;

    public MonsterPreset[] presets;

    void Awake(){
        foreach(MonsterPreset m in presets){
            int indx = monsterList.Count;
            monsterList.Add(new Monster(m, indx));
        }
    }

    void Update(){
            foreach(Monster m in monsterList){
                if(m.body && m.state!=MONSTATE.DEAD)
                m.Update();
            }
    }

    
}