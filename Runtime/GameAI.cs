using System;
#if UNITY_2018_1_OR_NEWER
using UnityEngine;
#endif

/*
How-to:
1 - Implement the Goal() and Model() methods
2 - Each action returned by the solver should match
a public, no-arg function call supported by at least
one component.
3 - When an action starts/ends set busy = true/false
*/
namespace Activ.GOAP{
public abstract partial class GameAI<T> where T : Agent{

    readonly object[] NoArg = new object[0];
    public bool verbose;
    public bool busy;
    public int frameBudget = 5;
    public Solver<T> solver;

    int frameCount{get{
        #if UNITY_2018_1_OR_NEWER
        return Time.frameCount;
        #else
        return 0;
        #endif
    }}

    public void Update(){
        if(busy) return;
        solver = solver ?? new Solver<T>();
        Effect( solver.isRunning
            ? solver.Iterate(frameBudget)?.Head()
            : solver.Next(Model(), Goal(), frameBudget)?.Head() );
    }

    // Decide a goal and (optionally) a heuristic
    public abstract Goal<T> Goal();

    // Instantiates or updates the model using relevant world states
    public abstract T Model();

    // Override if you have a behavior while idle
    virtual protected void Idle(){ }

    // Action mapping may be overriden
    protected virtual void Effect(object action){
        switch(action){
        case string noArg:
            if(noArg == Solver<Agent>.INIT) return;
            Print($"No-arg message: {noArg} @{frameCount}");
            this.GetType().GetMethod(noArg).Invoke(this, NoArg);
            return;
        case System.Action method:
            Print($"Delegate: {method.Method.Name} @{frameCount}");
            method();
            return;
        case null:
            Idle();
            return;
        default:
            throw new ArgumentException($"Bar arg: " + action);
        }
    }

    void Print(object arg){
        if(!verbose) return;
        #if UNITY_2018_1_OR_NEWER
        print(arg);
        #else
        System.Console.WriteLine(arg);
        #endif
    }

}}
