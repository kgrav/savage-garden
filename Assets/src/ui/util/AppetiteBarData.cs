public class AppetiteBarData{
    public float lowThreshhold;
    public float highThreshold;
    public float value;
    public MONAPP key;

    public AppetiteBarData(float lowThreshold, float highThreshold, float value, MONAPP key){
        this.lowThreshhold=lowThreshold;
        this.highThreshold=highThreshold;
        this.value=value;
        this.key=key;
    }
}