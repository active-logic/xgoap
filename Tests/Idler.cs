using System;

[Serializable]
public class Idler : Agent{

    public float cost { get; set; }
    public float est  { get; set; }

    Func<bool>[] Agent.actions => new Func<bool>[]{};

}
