using System;

[Serializable]
public class WoodChopper : Agent{

    public bool hasAxe = false;
    public bool hasFirewood = false;

    public float cost { get; set; }

    public Func<bool>[] actions => new Func<bool>[]{
        ChopLog, GetAxe, CollectBranches
    };

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

    override public bool Equals(object other){
        if(other == null) return false;
        if(other is WoodChopper that){
            return this.hasAxe == that.hasAxe
                && this.hasFirewood == that.hasFirewood;
        } else return false;
    }

    override public int GetHashCode()
    => (hasAxe ? 1 : 0) + (hasFirewood ? 2 : 0);

    override public string ToString()
    => $"WoodChopper[axe:{hasAxe} f.wood:{hasFirewood} ]";

}
