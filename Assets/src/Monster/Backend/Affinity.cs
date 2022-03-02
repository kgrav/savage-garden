using System;
using UnityEngine;

[Serializable]
public class Affinity {

    public float sloth;

    public float gluttony;

    public float pride;

    public float wrath;

    public float lust;

    public Affinity(){
        
        pride=0;
        sloth=0;
        gluttony=0;
        wrath=0;
        lust=0;
    }

    public Affinity Randomize(){
        Affinity r = new Affinity();
        float aS = Monsters.affinityScale;
        r.sloth = Mathf.Clamp(sloth + (UnityEngine.Random.Range(0f,aS) - (aS/2)), 0, 20);
        r.gluttony = Mathf.Clamp(gluttony + (UnityEngine.Random.Range(0f,aS) - (aS/2)), 0, 20);
        r.pride = Mathf.Clamp(pride + (UnityEngine.Random.Range(0f,aS) - (aS/2)), 0, 20);
        r.wrath = Mathf.Clamp(wrath + (UnityEngine.Random.Range(0f,aS) - (aS/2)), 0, 20);
        r.lust = Mathf.Clamp(lust + (UnityEngine.Random.Range(0f,aS) - (aS/2)), 0, 20);
        return r;
    }


    public static float AffinityDist(Affinity a, Affinity b){
        float r = (Mathf.Abs(a.sloth-b.sloth) +
                   Mathf.Abs(a.gluttony-b.gluttony) + 
                   Mathf.Abs(a.pride-b.pride) +
                   Mathf.Abs(a.wrath-b.wrath) +
                   Mathf.Abs(a.lust-b.lust))/5f;
        return r;
    }




    public static Affinity Average(WeightedAffinity[] affinities){
        Affinity r = new Affinity();
        float q = 0;
        foreach(WeightedAffinity wa in affinities){
            r.pride += wa.affinity.pride*wa.weight;
            r.sloth += wa.affinity.sloth*wa.weight;
            r.gluttony += wa.affinity.gluttony*wa.weight;
            r.wrath += wa.affinity.wrath*wa.weight;
            r.lust += wa.affinity.lust*wa.weight;
            q += wa.weight;
        }
        r.pride/=q;
        r.sloth/=q;
        r.gluttony/=q;
        r.wrath/=q;
        r.lust/=q;
        return r;
    }


    public static Affinity BreedAffinities(Affinity a, Affinity b){
        Affinity r = new Affinity();
        return r;
    }
}