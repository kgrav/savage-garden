using UnityEngine;

public class AffinityData{
    public string label;
    public Color color;
    public float value;

    public AffinityData(string label, float value, Color color){
        this.label = label;
        this.value = value;
        this.color = color;
    }
}