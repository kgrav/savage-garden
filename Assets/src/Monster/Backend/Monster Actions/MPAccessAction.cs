using UnityEngine;
using System;

public class MPAccessAction : MAction {
    public MonsterPoint goalPoint {get; private set;}
    public MPAccessAction(Monster self, MonsterPoint goalPoint, MONMOV reason){
        this.self=self;
        this.goalPoint=goalPoint;
        this.reason=reason;
    }

    public override void DoAction()
    {
        goalPoint.Access(self,reason);
    }

    public override void EndAction()
    {
        goalPoint.Disengage(self);
    }

    public override bool EndCondition()
    {
        if(!goalPoint)
            return true;
        MPSTATE accessCode = goalPoint.Availability(reason);
        bool end = false;
        end |= Vector3.Distance(goalPoint.tform.position, self.body.tform.position) > 10;
        end |= self.AppetiteIsHigh(reason);
        end |= accessCode == MPSTATE.AVAILABLE || accessCode==MPSTATE.BADTYPE;
        return false;
    }

    protected override void StartAction()
    {
        goalPoint.Engage(self, reason);
    }
}