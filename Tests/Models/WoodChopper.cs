using System;

[Serializable]
public class WoodChopper : Agent{

    public bool hasAxe = false;
    public bool hasFirewood = false;

    public bool GetAxe(){
        if(hasAxe) return false;
        cost += 2;
        return hasAxe = true;
    }

    public bool ChopLog(){
        if(!hasAxe) return false;
        cost += 4;
        return hasFirewood = true;
    }

    public bool CollectBranches(){
        cost += 8;
        return hasFirewood = true;
    }

    public Func<bool>[] actions => new Func<bool>[]{
        ChopLog, GetAxe, CollectBranches
    };

    public float cost { get; set; }
    public float est { get; set; }

}
