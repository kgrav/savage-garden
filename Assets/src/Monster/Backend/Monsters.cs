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

    public GameObject marker;
    public static Monster GetMonster(int indx){
        return monsterList[indx];
    }

    public bool roll(int modifier, int check){
        return UnityEngine.Random.Range(1,21)+modifier > check;
    }

    public static Vector3 xzdir(Vector3 a, Vector3 b){
        Vector3 a1 = a;
        a1.y=0;
        Vector3 b1 = b;
        b1.y=0;
        return b1-a1;
    }
    public static float actionUpdateInterval {get{return 1/monsters.actionUpdatesPerSecond;}}
    
    public static Vector3 GetWanderPoint(){
        return monsters.wanderCenter + new Vector3((UnityEngine.Random.Range(0,monsters.wanderSize.x*2) - monsters.wanderSize.x),
                                            0,
                                            (UnityEngine.Random.Range(0,monsters.wanderSize.z*2)-monsters.wanderSize.z));
    }

    public static MAction BuildAction(Monster m, MONMOV reason){
        switch(reason){
            case MONMOV.NONE:
                return null;
            break;
            case MONMOV.BOREDOM:
                return BuildBoredomAction(m);
            break;
        }
        return null;
    }

    static MAction BuildBoredomAction(Monster m){
        Vector3 wanderpt = GetWanderPoint();
        Instantiate(monsters.marker, wanderpt,Quaternion.identity);
        return new ApproachAction(m, MONMOV.BOREDOM,wanderpt);
    }

    public static bool pause = false;

    public Vector3 wanderSize, wanderCenter;
    public Vector2 rstepInterval;

    public static float rtime {
        get{
            return UnityEngine.Random.Range(monsters.rstepInterval.x,monsters.rstepInterval.y);
        }
    }

    public float actionUpdatesPerSecond;

    public MonsterPreset[] presets;

    void Awake(){
        foreach(MonsterPreset m in presets){
            int indx = monsterList.Count;
            monsterList.Add(new Monster(m, indx));
        }
    }

    void Update(){
            foreach(Monster m in monsterList){
                m.Update();
            }
    }
}