using System;
//using System.Collections.Generic;
//using UnityEngine;

namespace Activ.GOAP{
[Serializable] public class Baker : Agent, Mapped{

    public enum Cooking{Raw, Cooked, Burned}
    //
    public const int Step    = 55;
    public const int MaxHeat = 200;
    public int   temperature = 0;
    public float bake;
    //
    [NonSerialized] (Option, Action)[] options;
    [NonSerialized] AI client;

    public Baker(){}

    public Baker(AI client) => this.client = client;

    public Cooking state => bake < 80  ? Cooking.Raw :
                            bake < 120 ? Cooking.Cooked :
                                         Cooking.Burned;

    public Cost Bake(){
        bake += (temperature / 2); return true;
    }

    public Cost SetTemperature(int degrees){
        temperature = degrees;
        return true;
    }

    Option[] Agent.Options()
    => state != Cooking.Burned ? new Option[]{ Bake } : null;

    (Option, Action)[] Mapped.Options()
    => state != Cooking.Burned ? CookingOptions() : null;

    (Option, Action)[] CookingOptions(){
        var n = MaxHeat/Step + 1;
        options = options ?? new (Option, Action)[n];
        for(int i = 0; i < n; i++){
            var t = i * Step;  // do not capture the iterator!
            options[i] = (() => SetTemperature(t),
                          () => client.SetTemperature(t));
        }
        return options;
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
