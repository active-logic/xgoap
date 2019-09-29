using System;

[Serializable]
public class Eradicator : Agent{

    public int rats = 3;

    public bool Eradicate(){ rats--; return true; }

    public bool Dawdle() => true;

    public bool Wipeout() => false;

    Func<bool>[] Agent.actions
    => new Func<bool>[]{Eradicate, Dawdle, Wipeout};

}
