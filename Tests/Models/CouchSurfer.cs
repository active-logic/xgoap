using System;

/* Has one action, which costs nothing */
namespace Activ.GOAP{
[Serializable] public class CouchSurfer : Agent{

    public ante Sleep() => 0;

    Func<ante>[] Agent.actions => new Func<ante>[]{ Sleep };

    override public bool Equals(object that)
    => that != null && that is CouchSurfer;

    override public int GetHashCode() => 0;

}}
