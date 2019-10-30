using System;

namespace Activ.GOAP{
public class WoodChopper : Agent, Clonable<WoodChopper>{

    public bool hasAxe, hasFirewood;
    Option[] opt;

    public Cost GetAxe(){
        if(hasAxe) return false;
        hasAxe = true;
        return 2;
    }

    public Cost ChopLog()
    => hasAxe ? (Cost)(hasFirewood = true, 4f) : (Cost)false;

    public Cost CollectBranches() => (hasFirewood = true, 8);

    public Option[] Options()
    => opt = opt ?? new Option[]{ChopLog, GetAxe, CollectBranches};

    public WoodChopper Allocate() => new WoodChopper();

    public WoodChopper Clone(WoodChopper x){
        x.hasAxe      = hasAxe;
        x.hasFirewood = hasFirewood;
        return x;
    }

    override public bool Equals(object other)
    => other is WoodChopper that
       && hasAxe == that.hasAxe && hasFirewood == that.hasFirewood;

    override public int GetHashCode()
    => (hasAxe ? 1 : 0) + (hasFirewood ? 2 : 0);

    override public string ToString()
    => $"WoodChopper[axe:{hasAxe} f.wood:{hasFirewood} ]";

}}
