using System;

[Serializable]
public class Idler : Agent{

    public float cost { get; set; }

    Func<bool>[] Agent.actions => new Func<bool>[]{};

    override public bool Equals(object that)
    => that != null && that is Idler;

    override public int GetHashCode() => 0;

}
