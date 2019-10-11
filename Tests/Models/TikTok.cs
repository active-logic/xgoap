using System;

namespace Activ.GOAP{
[Serializable] public class TikTok : Agent{

    public float cost { get; set; }

    public ante Tik(){ cost += 1; return true; }

    public ante Tok(){ cost += 1; return true; }

    Func<ante>[] Agent.actions => new Func<ante>[]{ Tik, Tok };

    override public bool Equals(object that)
    => that != null && that is TikTok;

    override public int GetHashCode() => 0;

}}
