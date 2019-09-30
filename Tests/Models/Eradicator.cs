using System;

[Serializable]
public class Eradicator : Agent{

    public float baseCost = 1;
    public float cost { get; set; }
    public float est  { get; set; }

    public int rats = 3;

    public bool Eradicate(){
        cost += baseCost;
        if(rats>0) rats--;
        return true;
    }

    public bool Dawdle(){
        cost += baseCost;
        return true;
    }

    public bool Wipeout() => false;

    Func<bool>[] Agent.actions
    => new Func<bool>[]{Eradicate, Dawdle, Wipeout};

}
