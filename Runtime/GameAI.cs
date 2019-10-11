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
// NOTE: derives from MonoBehaviour when used with Unity3D
// (see Runtime/Unity/GameAI.cs)
public abstract partial class GameAI<T> where T : Agent{

    public bool verbose;
    public bool busy;
    public int frameBudget = 5;
    public Solver<T> solver;
    public ActionHandler<object> handler = new ActionMap();

    // Decide a goal and (optionally) a heuristic
    public abstract Goal<T> Goal();

    // Instantiates or updates the model using relevant world states
    public abstract T Model();

    // Override if you have a behavior while idle
    virtual public void Idle(){ }

    public void Update(){
        if(busy) return;
        solver = solver ?? new Solver<T>();
        if(handler is ActionMap m) m.verbose = verbose;
        handler.Effect( solver.isRunning
            ? solver.Iterate(frameBudget)?.Head()
            : solver.Next(Model(), Goal(), frameBudget)?.Head(),
            this);
    }

}}
