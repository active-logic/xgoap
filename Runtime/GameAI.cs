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
public abstract partial class GameAI<T> : SolverOwner
                                                    where T : Agent{

    public bool verbose;
    public bool busy;
    public int frameBudget = 5;
    public int maxNodes = 1000;
    public Solver<T> solver;
    public ActionHandler<object> handler = new ActionMap();

    public SolverStats stats => solver;

    // Decide a goal and (optionally) a heuristic
    public abstract Goal<T> Goal();

    // Instantiates or updates the model using relevant world states
    public abstract T Model();

    // Override if you have a behavior while idle
    virtual public void Idle(){ }

    public virtual void Update(){
        if(busy) return;
        var model = Model();
        if(model == null) return;
        solver = solver ?? new Solver<T>();
        solver.maxNodes = maxNodes;
        if(handler is ActionMap m) m.verbose = verbose;
        handler.Effect( solver.isRunning
            ? solver.Iterate(frameBudget)?.Head()
            : solver.Next(model, Goal(), frameBudget)?.Head(),
            this);
    }

}}
