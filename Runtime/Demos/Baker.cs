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

    public ante Bake(){
        bake += (temperature / 2); return true;
    }

    public ante SetTemperature(int degrees){
        temperature = degrees;
        return true;
    }

    Func<ante>[] Agent.actions
    => state != Cooking.Burned ? new Func<ante>[]{ Bake } : null;

    Action[] Parametric.methods
    => state != Cooking.Burned ? CookingOptions() : null;

    Action[] CookingOptions(){
        List<Action> actions = new List<Action>();
        for(int i = 0; i <= MaxHeat; i += Step){
            var j = i;  // Do not capture the iterator!
            actions.Add(new Action(
                () => SetTemperature(j),
                () => client.SetTemperature(j)
            ));
        }
        return actions.ToArray();
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
