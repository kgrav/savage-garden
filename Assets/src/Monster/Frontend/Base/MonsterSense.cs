using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class MonsterSense : MonsterPart {

    public abstract List<MonsterPoint> GetPoints();
}