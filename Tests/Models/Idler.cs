using System;

namespace Activ.GOAP{
[Serializable] public class Idler : Agent{

    public float cost { get; set; }

    Func<ante>[] Agent.actions => new Func<ante>[]{};

    override public bool Equals(object that)
    => that != null && that is Idler;

    override public int GetHashCode() => 0;

}}
