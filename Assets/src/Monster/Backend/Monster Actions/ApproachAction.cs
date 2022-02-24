using UnityEngine;
using System;

public class ApproachAction : MAction{
    public Vector3 approachPoint {get; private set;}

    public ApproachAction(Monster self, MONAPP reason, Vector3 approachPoint,float priority){
        
        next=null;
        this.self=self;
        this.reason=reason;
        this.approachPoint=approachPoint;
    }

    public override bool EndCondition()
    {
        float dist = MathUtils.xzdist(self.body.tform.position, approachPoint);
        return dist < 10 || !self.body.hasLegs;
    }

    
    public override void DoAction()
    {
        self.body.SetMoveSpeed(1);
        self.appetites[(int)MONAPP.BOREDOM].Restore(0.5f*Time.deltaTime);
    }
    protected override void StartAction()
    { 
        Vector3 direction = approachPoint - self.body.tform.position;
        direction.y = 0;
        direction = direction.normalized;
        self.body.SetFaceDirection(direction);
    }

    public override void EndAction()
    {
        self.body.SetMoveSpeed(0);
    }
}