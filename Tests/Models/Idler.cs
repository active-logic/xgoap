using System;

namespace Activ.GOAP{
[Serializable] public class Idler : Agent{

    public float cost { get; set; }

    Func<Cost>[] Agent.Actions() => new Func<Cost>[]{};

    override public bool Equals(object that)
    => that != null && that is Idler;

    override public int GetHashCode() => 0;

}}
