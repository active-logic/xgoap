using System;

namespace Activ.GOAP{
[Serializable] public class WoodChopper : Agent{

    public bool hasAxe, hasFirewood;

    public Func<ante>[] actions => new Func<ante>[]{
        ChopLog, GetAxe, CollectBranches
    };

    public ante GetAxe(){
        if(hasAxe) return false;
        hasAxe = true;
        return 2;
    }

    public ante ChopLog()
    => hasAxe ? (ante)(hasFirewood = true, 4f) : (ante)false;

    public ante CollectBranches() => (hasFirewood = true, 8);

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

}}
