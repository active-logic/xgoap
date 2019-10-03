using UnityEngine;

/*
To use this class:
1 - Create a subclass, assigning an agent on Start or Awake
2 - Implement the Goal() method
3 - Each action returned by the solver should match
a message (public, no-arg function call supported by at least
one component) to the game object
4 - When an action starts/ends set busy = true/false
*/
public abstract class UAgent : MonoBehaviour{

    public bool busy;
    Solver solver;
    Agent  agent = null;

    protected abstract Goal<Agent> Goal();

    void Update(){
        solver = solver ?? new Solver();
        if(!busy) SendMessage( solver.Eval<Agent>(agent, Goal()) );
    }

}
