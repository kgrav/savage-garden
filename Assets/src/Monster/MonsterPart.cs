using UnityEngine;

public class MonsterPart : NVComponent{
    
    protected MonsterBody parent;
    public Animator anim=>GetComponent<Animator>();

    public float attackTime;
    public float damage;
    public bool hasAttack;

    void Awake(){
        if(anim){
            foreach(NVAnimationState a in anim.GetBehaviours<NVAnimationState>()){
                a.SetRoot(this);
            }
        }
        Init();
    }

    public virtual void TriggerAttack(){
        anim.SetTrigger("Attack");

    }

    public void CallAnimationMethod(string method){
        Invoke(method,0f);
    }

    public void SetBody(MonsterBody parent){
        this.parent = parent;
    }
    public void Sound(string sound){

    }

    protected virtual void Init(){}
}