using UnityEngine;
using System;

public class MPAccessAction : MAction {
    public MonsterPoint goalPoint {get; private set;}
    public MPAccessAction(Monster self, MonsterPoint goalPoint, MONAPP reason,float priority){
        this.self=self;
        this.goalPoint=goalPoint;
        this.reason=reason;
        this.priority=priority;
    }

    public override void DoAction()
    {
        goalPoint.Access(self,reason);
    }

    public override void EndAction()
    {
        self.SetDecreasePause(reason,false);
        if(goalPoint)
        goalPoint.Disengage(self, reason);
    }

    public override bool EndCondition()
    {
        if(!goalPoint)
            return true;
        MPSTATE accessCode = goalPoint.Availability(reason);
        bool end = false;
        end |= MathUtils.xzdist(goalPoint.tform.position, self.body.tform.position) > 10;
        end |= self.AppetiteIsHigh(reason);
        end |= accessCode == MPSTATE.INACTIVE || accessCode==MPSTATE.BADTYPE;
        return end;
    }

    protected override void StartAction()
    {
        self.SetDecreasePause(reason,true);
        goalPoint.Engage(self, reason);
    }
}