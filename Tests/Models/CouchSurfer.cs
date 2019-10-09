using System;

/* Has one action, which costs nothing */
namespace Activ.GOAP{
[Serializable] public class CouchSurfer : Agent{

    public float cost { get; set; }

    public bool Sleep() => true;

    Func<bool>[] Agent.actions => new Func<bool>[]{ Sleep };

    override public bool Equals(object that)
    => that != null && that is CouchSurfer;

    override public int GetHashCode() => 0;

}}
