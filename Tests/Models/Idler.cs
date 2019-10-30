using System;

namespace Activ.GOAP{
[Serializable] public class Idler : Agent{

    public float cost { get; set; }

    Option[] Agent.Options() => new Option[]{};

    override public bool Equals(object that)
    => that != null && that is Idler;

    override public int GetHashCode() => 0;

}}
