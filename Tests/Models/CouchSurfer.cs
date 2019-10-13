using System;

/* Has one action, which costs nothing */
namespace Activ.GOAP{
[Serializable] public class CouchSurfer : Agent{

    public Cost Sleep() => 0;

    Func<Cost>[] Agent.actions => new Func<Cost>[]{ Sleep };

    override public bool Equals(object that)
    => that != null && that is CouchSurfer;

    override public int GetHashCode() => 0;

}}
