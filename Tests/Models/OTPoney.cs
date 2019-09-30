using System;

/* The proverbial one trick poney */
[Serializable]
public class OTPoney : Agent{

    public float cost { get; set; }
    public float est  { get; set; }

    public bool OneTrick(){ cost += 1; return true; }

    Func<bool>[] Agent.actions => new Func<bool>[]{ OneTrick };

}
