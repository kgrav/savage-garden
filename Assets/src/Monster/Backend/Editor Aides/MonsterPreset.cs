using UnityEngine;
using System;
[CreateAssetMenu(menuName="MonsterPreset")]
public class MonsterPreset : ScriptableObject {

    public Affinity affinity;

    public GameObject bodyPreset;

    public AppetiteConfig boredom;

    public AppetiteConfig hunger;
    public AppetiteConfig loneliness;
    public AppetiteConfig stamina;
    public AppetiteConfig survival;

    public string monsterName;
}