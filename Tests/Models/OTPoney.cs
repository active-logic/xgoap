using System;

/* The proverbial one trick poney */
namespace Activ.GOAP{
[Serializable] public class OTPoney : Agent{

    public float cost { get; set; }

    public bool OneTrick(){ cost += 1; return true; }

    Func<bool>[] Agent.actions => new Func<bool>[]{ OneTrick };

    override public bool Equals(object that)
    => that != null && that is OTPoney;

    override public int GetHashCode() => 0;

}}
