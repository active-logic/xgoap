using System;

namespace Activ.GOAP{
[Serializable] public class TikTok : Agent{

    public float cost { get; set; }

    public Cost Tik(){ cost += 1; return true; }

    public Cost Tok(){ cost += 1; return true; }

    Option[] Agent.Options() => new Option[]{ Tik, Tok };

    override public bool Equals(object that)
    => that != null && that is TikTok;

    override public int GetHashCode() => 0;

}}
