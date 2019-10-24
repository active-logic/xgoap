using System;

/*
The proverbial One Trick Poney.
This won't run for over a frame because the model state is unique
*/
namespace Activ.GOAP{
[Serializable] public class OTPoney : Agent{

    public float cost { get; set; }

    public Cost OneTrick() => true;

    Func<Cost>[] Agent.Actions() => new Func<Cost>[]{ OneTrick };

    override public bool Equals(object that)
    => that != null && that is OTPoney;

    override public int GetHashCode() => 0;

}}
