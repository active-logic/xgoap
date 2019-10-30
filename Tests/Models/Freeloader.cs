using System;

/* Has one action, which costs nothing */
namespace Activ.GOAP{
[Serializable] public class Freeloader : Mapped{

    public Cost Load(int n) => 0;

    (Option, Action)[] Mapped.Options()
    => new (Option, Action)[]{ (() => Load(1000), () => {}) };

    override public bool Equals(object that)
    => that != null && that is CouchSurfer;

    override public int GetHashCode() => 0;

}}
