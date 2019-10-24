using System;

/* Has one action, which costs nothing */
namespace Activ.GOAP{
[Serializable] public class Freeloader : Parametric{

    public Cost Load(int n) => 0;

    //Func<Cost>[] Agent.Actions() => new Func<Cost>[]{ };

    Action[] Parametric.Functions() => new Action[]{
        new Action( () => Load(1000), () => {} )
    };

    override public bool Equals(object that)
    => that != null && that is CouchSurfer;

    override public int GetHashCode() => 0;

}}
