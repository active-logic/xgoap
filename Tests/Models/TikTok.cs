using System;

[Serializable]
public class TikTok : Agent{

    public float cost { get; set; }

    public bool Tik(){ cost += 1; return true; }

    public bool Tok(){ cost += 1; return true; }

    Func<bool>[] Agent.actions => new Func<bool>[]{ Tik, Tok };

    override public bool Equals(object that)
    => that != null && that is TikTok;

    override public int GetHashCode() => 0;


}
