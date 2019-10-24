using System;
using System.Collections.Generic;

namespace Activ.GOAP{
[Serializable] public class Baker : Agent, Parametric{

    public const int Step    = 55;
    public const int MaxHeat = 200;
    public enum Cooking{Raw, Cooked, Burned}
    public int   temperature = 0;
    public float bake;

    public Cooking state => bake < 80  ? Cooking.Raw :
                            bake < 120 ? Cooking.Cooked :
                                         Cooking.Burned;

    [NonSerialized] AI client;

    public Baker(AI client = null) => this.client = client;

    public Cost Bake(){
        bake += (temperature / 2); return true;
    }

    public Cost SetTemperature(int degrees){
        temperature = degrees;
        return true;
    }

    Func<Cost>[] Agent.Actions()
    => state != Cooking.Burned ? new Func<Cost>[]{ Bake } : null;

    Action[] Parametric.Functions()
    => state != Cooking.Burned ? CookingOptions() : null;

    Action[] CookingOptions(){
        List<Action> elems = new List<Action>();
        for(int i = 0; i <= MaxHeat; i += Step){
            var j = i;  // Do not capture the iterator!
            elems.Add(new Action(
                () => SetTemperature(j),
                () => client.SetTemperature(j)
            ));
        }
        return elems.ToArray();
    }

    override public bool Equals(object other){
        var that = other as Baker;
        return this.bake == that.bake
            && this.temperature == that.temperature;
    }

    override public int GetHashCode()
    => temperature + (int)(bake * 1000);

    override public string ToString()
    => $"Baker[ {state} at {temperature}℃ ]";

    public interface AI{
        void Bake();
        void SetTemperature(int i);
    }

}}
