using UnityEngine;

public class MonsterPart : NVComponent{
    
    protected MonsterBody parent;
    public Animator anim=>GetComponent<Animator>();
    public Affinity affinity;
    public int HP;
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


    public virtual MAction BuildAttackAction(MonsterPoint target,Monster self,MONAPP reason,float priority){
        return null;
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