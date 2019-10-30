using System;

namespace Activ.GOAP{
[Serializable] public class Eradicator : Agent{

    public float baseCost = 1;
    public float cost { get; set; }

    public int rats = 3;

    public Cost Eradicate(){
        cost += baseCost;
        if(rats>0) rats--;
        return true;
    }

    public Cost Dawdle(){
        cost += baseCost;
        return true;
    }

    public Cost Wipeout() => false;

    Option[] Agent.Options()
    => new Option[]{Eradicate, Dawdle, Wipeout};

    override public bool Equals(object other){
        if(other is Eradicator that){
            return this.rats == that.rats;
        } else return false;
    }

    override public int GetHashCode() => rats;

    override public string ToString()
    => $"Eradicator[rats:{rats} ]";

}}
