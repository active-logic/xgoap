using UnityEngine;

/*
To use this class:
1 - Implement the Goal() and Model() methods
2 - Each action returned by the solver should match
a public, no-arg function call supported by at least
one component.
3 - When an action starts/ends set busy = true/false
*/
public abstract class GameAI<T> : MonoBehaviour
                                     where T : Agent{

    public bool busy;
    Solver<T> solver;

    // Decide a goal and (optionally) a heuristic
    protected abstract Goal<T> Goal();

    // Instantiates or updates the model using relevant world states
    protected abstract T Model();

    // Action mapping may be overriden
    // TODO - handle other action type
    protected virtual void Effect(object action)
    => SendMessage(action.ToString());

    void Update(){
        if(busy) return;
        solver = solver ?? new Solver<T>();
        Effect(solver.Eval(Model(), Goal()));
        busy = true;
    }

}
