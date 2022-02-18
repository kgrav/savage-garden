using UnityEngine;
using System;
public class NVAnimationState : StateMachineBehaviour {
    MonsterPart controller;
    public void SetRoot(MonsterPart p){
        controller=p;
    }

     public string enterStateMethod, exitStateMethod;
 
    public bool loopPercentFunctions;
    public PercentFunction[] percentFunctions;
    public PercentFunction[] soundClips;
    [Serializable]
    public class PercentFunction{
        public float pct;
        public string method;
    }
    int pertptr=0,audtptr=0;
    float lastTime = 0;
    

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        pertptr = 0;
        audtptr=0;
        if(enterStateMethod.Length > 0)
            controller.CallAnimationMethod(enterStateMethod);
        ChStart(animator,stateInfo,layerIndex);
    }

    protected virtual void ChStart(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(exitStateMethod.Length > 0)
            controller.CallAnimationMethod(exitStateMethod);
        ChExit(animator, stateInfo,layerIndex);
    }
    protected virtual void ChExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float newTime = stateInfo.normalizedTime - Mathf.Floor(stateInfo.normalizedTime);
        if(soundClips.Length > 0){

            if(audtptr < soundClips.Length && stateInfo.normalizedTime > soundClips[audtptr].pct){
                controller.Sound(soundClips[audtptr].method);
                audtptr++;
            }
            else if(loopPercentFunctions && audtptr >= soundClips.Length && stateInfo.normalizedTime < lastTime){
                audtptr = 0;
            }
        }
        if(percentFunctions.Length > 0){

            if(pertptr < percentFunctions.Length && stateInfo.normalizedTime > percentFunctions[pertptr].pct){
                controller.CallAnimationMethod(percentFunctions[pertptr].method);
                pertptr++;
            }
            else if(loopPercentFunctions && pertptr >= percentFunctions.Length && newTime < lastTime){
                pertptr = 0;
            }
        }
        lastTime = newTime;
        ChUpdate(animator, stateInfo, layerIndex);
    }

    protected virtual void ChUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}