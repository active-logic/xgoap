using System;

/* Has one action, which costs nothing */
namespace Activ.GOAP{
[Serializable] public class Freeloader : Agent, Parametric{

    public float cost { get; set; }

    public bool Load(int n) => true;

    Func<bool>[] Agent.actions => new Func<bool>[]{ };

    Action[] Parametric.methods => new Action[]{
        new Action( () => Load(1000), () => {} )
    };

    override public bool Equals(object that)
    => that != null && that is CouchSurfer;

    override public int GetHashCode() => 0;

}}
