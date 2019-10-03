using System;

/* A one trick poney that can actually trot */
[Serializable]
public class Inc : Agent{

    public float cost { get; set; }
    public int pos;

    public bool Step(){
        cost += 1;
        pos  += 1;
        return true;
    }

    Func<bool>[] Agent.actions => new Func<bool>[]{ Step };

    override public int GetHashCode() => pos;

    override public bool Equals(object other){
        if(other == null) return false;
        if(other is Inc that) return this.pos == that.pos;
        return false;
    }

}
