using UnityEngine;
using System;
using System.Collections.Generic;

public class Monsters : MonoBehaviour
{

    
    public static Monsters monsters=>FindObjectOfType<Monsters>();

    
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

    public static List<AppetiteBarData> GetAppetiteData(int index){
        return GetMonster(index).GetAppetiteData();
    }


    public GameObject marker;
    public static Monster GetMonster(int indx){
        return monsterList[indx];
    }

    public static void DestroyMonster(int i){

    }

    public bool roll(int modifier, int check){
        return UnityEngine.Random.Range(1,21)+modifier > check;
    }

    
    
    
    public static Vector3 GetWanderPoint(){
        return monsters.wanderCenter + new Vector3((UnityEngine.Random.Range(0,monsters.wanderSize.x*2) - monsters.wanderSize.x),
                                            0,
                                            (UnityEngine.Random.Range(0,monsters.wanderSize.z*2)-monsters.wanderSize.z));
    }

    
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
                if(m.state!=MONSTATE.DEAD)
                m.Update();
            }
    }

    
}