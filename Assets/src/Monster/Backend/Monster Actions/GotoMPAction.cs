using UnityEngine;
using System;

public class GotoMPAction : MAction {
    public MonsterPoint approachPoint {get; private set;}

    public GotoMPAction(Monster self, MONAPP reason, MonsterPoint approachPoint, float priority){
        this.self=self;
        this.reason=reason;
        this.approachPoint=approachPoint;
        this.priority=priority;
    }

    public override bool EndCondition()
    {
        float dist = MathUtils.xzdist(self.body.tform.position, approachPoint.tform.position);
        return dist < 10 || !self.body.hasLegs;
    }

    
    public override void DoAction()
    {
        self.body.SetMoveSpeed(1);
        Vector3 direction = approachPoint.tform.position - self.body.tform.position;
        direction.y = 0;
        direction = direction.normalized;
        self.body.SetFaceDirection(direction);
        self.appetites[(int)MONAPP.BOREDOM].Restore(0.5f*Time.deltaTime);
    }
    protected override void StartAction()
    { 
        Vector3 direction = approachPoint.tform.position - self.body.tform.position;
        direction.y = 0;
        direction = direction.normalized;
        self.body.SetFaceDirection(direction);
    }

    public override void EndAction()
    {
        self.body.SetMoveSpeed(0);
    }
}