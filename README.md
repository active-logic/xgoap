# Extended GOAP planner

This package implements GOAP (goal oriented action planning).

Compared to similar packages available on Github, xGOAP is less verbose and lets you switch strategy depending on your model:

- With most APIs, actions must be defined as objects. In xGOAP, an action is a function of the model.
- Other GOAP packages typically define models using key-value pairs. In xGOAP a model is a class and you define the details of how this class implemented.
- The cost of an action (if used) is applied when the action is effected. This allows variable cost actions.
- If you have a cost function and a heuristic, A\* is used. However you needn't provide heuristics. Without heuristics, xGOAP defaults to breadth first search.

# Getting started

For planning you need a model (Agent), a goal and, if available a heuristic. I'll use Brent Owens' wood chopper agent as an example.

In GOAP an agent uses actions to update its model; each action has:
- Preconditions, used to decide whether the action can be performed.
- Effects, which determine the result(s) of an action
- A cost

In Owens example, a wood chopper has the *GetAxe*, *ChopLog* and *CollectBranches* actions. In xGoap, the ChopLog action is implemented as follows:

```cs
public bool ChopLog(){
    if(!hasAxe) return false;
    cost += 4;
    hasAxe = true;
    return true;
}
```

As you can see, xGOAP actions are easy to implement - Check the preconditions, pay the cost, apply the effects, and return `true` to indicate success or `false` to indicate failure. Here is the complete wood chopper model

```cs
[Serializable]  // must be serializable to clone model states
public class WoodChopper : Agent{

    bool hasAxe = false;
    bool hasFirewood = false;

    public bool GetAxe(){
        if(hasAxe) return false;
        cost += 2;
        return hasAxe = true;
    }

    public bool ChopLog(){
        if(!hasAxe) return false;
        cost += 4;
        return hasFirewood = true;
    }

    public bool CollectBranches(){
        cost += 8;
        return hasFirewood = true;
    }

    Func<bool>[] actions => new Func<bool>[]{
        ChopLog, GetAxe, CollectBranches
    };

    public float cost;
    public float est;

}
```

Then, to run the model and get the next planned action, create a new plan and call its `Eval` method passing the model and goal as argument.

```cs
var chopper = new WoodChooper();
var plan = new Planner();
string next = plan.Eval(chopper, x => x.hasFirewood);
```

The goal argument (here, `x => x.hasFirewood`) is just a function that takes a `WoodChopper` as argument and returns a bool to indicate whether the goal has been reached (true) or not (false).

# Unity integration

[Explain unity integrated example]

# Available methods

## Breadth First planner

[BrPlanner](Documentation/Breadth-First.md) implements planning using a breadth first search. In this case, cost functions are not supported so the underlying assumption is that every action has the same cost.

## Best First planner

[BfPlanner](Documentation/Best-First.md) implements cost minimization. This is similar to some GOAP implementations, for example []

## A* planner

[BfPlanner](Documentation/Best-First.md) implements A* cost minimization, with a goal distance estimate.
