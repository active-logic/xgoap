using System;

/* A one trick poney that can actually trot */
namespace Activ.GOAP{
[Serializable] public class Inc : Agent{

    public int pos;

    public ante Step(){
        pos  += 1;
        return true;
    }

    Func<ante>[] Agent.actions => new Func<ante>[]{ Step };

    override public int GetHashCode() => pos;

    override public bool Equals(object other){
        if(other == null) return false;
        if(other is Inc that) return this.pos == that.pos;
        return false;
    }

}}
