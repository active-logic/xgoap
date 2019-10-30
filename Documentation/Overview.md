# A User Guide *Beyond GOAP* [DRAFT]

## What is GOAP?

GOAP (Goal oriented action planning) refers to a family of planning AIs inspired by [Jeff Orkin's GOAP](http://alumni.media.mit.edu/~jorkin/gdc2006_orkin_jeff_fear.pdf).

In GOAP, an agent are assigned a goal (escape a room, take cover, knock a target down, heal, ...) and utilize actions whose *preconditions*, *cost* and *effects* are known to fulfill the goal condition.

A search algorithm (such as A*) resolves the action sequence which constitutes a path to the goal.

A *heuristic*, estimating the extra cost to reach the goal, is often provided.

## While you GOAP

While reading, also check the [Sentinel demo](https://youtu.be/mbLNALyt5So) and associate [project files](https://github.com/active-logic/xgoap-demos).

## Planning Agent/Model

This library provides a solver and APIs to help you implement your own planning AIs.

The critical part of your AI is the model, aka 'agent'; the model represents an AI's knowledge of the environment they operate in, including itself.

- Your model is a class implementing `Agent` or `Mapped` (to express planning actions, aka 'options').

The solver needs to generate and organize copies of the model object, therefore cloning, hashing and comparing for equality are common operations applied many times over.

- Minimally, tag your model [*Serializable*](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/serialization/) or (much better) implement `Clonable`.
- Override `Equals()` and `GetHashCode()` (sloppy hashcodes decrease performance)

### Planning actions

Specify planning actions (options) via `Agent` and/or `Mapped` (or both!)

Usually, `Agent` suffices - easier to implement, and (currently) faster.

```cs
Option[] Agent.Options() => new Option[]{ JumpForward, Shoot, ... };

public Cost JumpForward{ ... }
```

Use `Mapped` when you need to parameterize planning actions:

```cs
(Option, Action)[] Mapped.Options(){
    var n   = inventory.Length;
    var opt = new (Option, Action)[n];
    for(int i = 0; i < n; i++){
        var j = i;  // don't capture the iterator!
        opt[i] = ( () => Use(j),
                   () => client.Use(inventory[j]) );
    }
    return opt;
}
```

In the above example:
- An inventory is considered
- An option is generated for each item in the inventory
- Options are mapped to game action via `(Option, System.Action)` tuples

NOTE: *In the above we are careful* not *to use the iterator `i` inside the lambda, otherwise all invocations of the lambda function would end up using a value of `n-1`*.

`Mapped` options are flexible and type safe, giving you complete control over how a planning action maps to a game action; `Agent` is much faster to implement.

### The Clonable interface

Implement `Allocate()` to create a model object instance. The purpose of this function is to perform all memory allocations upfront, not determine state.

Implement `Clone(T storage)` to copy model state. This function **must** assign all fields (to avoid leaking dirty state).

```cs
class MyModel : Clonable<MyModel>{

    T byRef;               // Assuming T : Clonable<T>; not required but handy
    int byValue;

    MyModel(){
        byRef = new T();  // allocate everything
        // byValue = 5;   // let's not do extra work
    }

    public MyModel Allocate() => new MyModel();

    public MyModel Clone(MyModel storage){
        this.byRef.Clone(storage.byRef);    // Don't shallow copy
        byValue = 5;                        // Set all fields
    }

}
```

Designed for instance reuse, this API enables optimizations, such as pooling.

Note: *The `Allocate` API is required because newing a `T : class, new()` object resolves to an `AlloceSlow` variant (the name says it all)*

### Test your model

Beyond GOAP cleanly separates your planning model from the game engine and/or actor (the object implementing actual game actions). This allows putting your model under test even before integrating an actual game actor.

## Integration

With a working model handy, you want to plug this into your game/simulation. The library provides a simple integration, mainly intended for (but not tied to) Unity3D.

The integration implements a two step *(planning -> action)* cycle:

1 - A plan is generated
2 - The *first* action in the plan is applied
(Rinse and repeat until the goal is attained)

We might plan once and step through all steps; however since world state changes dynamically, re-planning often keeps our agents on track.

NOTE: *In the future the integration will give you more control over how often replanning is applied*

To use the integration, subclass `GameAI`, as explained below.

### Subclassing `GameAI`

A `Goal` consists in a function, which verifies whether an instance of the model `T` satisfies the goal, and a heuristic function 'h', which measures the distance/cost between a model state designated as 'current' and the goal.
Sometimes you don't have a heuristic, or can't come up with anything just yet. That's okay (still, a heuristic dramatically speeds up planning).

[`GameAI`](../Runtime/GameAI.cs) specifies a handful of functions that you need to implement in order to get your game actors going:

- Supply a goal for the agent to strive towards.
- Link your planning model
- (optionally) implement an `Idle()` mode.
- Implement `IsActing()` to indicate when the actor are available for planning.

The `Goal()` method (assume `x` of type `T`):

```cs
override public Goal<T> Goal() => (
    x => cond,     // such as `x.someValue == true`
    x => heuristic // such as `x.DistTo(x.target)` or null if unavailable
);
```

Your implementation of `T Model()` should a model instance which represents the current state of the agent and their environment, for example:

```cs
// Model definition
class MyModel{ float x, float z; }

// inside MyAI : GameAI<MyModel>
override public MyModel Model(){
    return new MyModel(transform.position.x, transform.position.z);
}
```

While `IsActing()` returns false, the planner will be running and evaluating the next action; how you implement this (whether event based, or testing the state of the game actor...) is entirely up to you; likewise the `Idle()` function.

```cs
override public bool IsActing() => SomeCondition() && SomeOther();
```

### Providing counterparts for planning options

Since planning actions aren't 'real' game actions, your `GameAI` implementation must supply these.

- With `Agent`, all planning actions must have same-name, no-arg counterparts in `GameAI`.
- With `Mapped`, one approach consists in defining an interface, which specifies methods to be implemented both as planning actions, and as game actions. The [Baker](`../Tests/Models/Baker.cs`) example illustrates this approach.

## Running your AI (Unity 3D only)

Once you have implemented your `GameAI` subclass, it can be added to any game object (In Unity, `GameAI` derives from `Monobehaviour`).

Additionally, tweaks are available...

- *verbose* - gives you basic information (in the console) about what actions are applied to the game AI

Then, under 'solver params':

- *Frame budget* - max number of planning actions per game frame.
- *Max nodes* - max number of states that should exist within the planner at any given time.
- *Max iter* - the max number of iterations allowed to find a solution; after which the planner just bails out.
- *Tolerance* - represents how closely the heuristic should be followed. For example if you don't care about a $10 difference (if 'cost' represents money) or a 0.5 seconds delta (if 'time cost' is the heuristic), set this to $10 or 0.5 seconds.
Leaving this number to zero forces a full ordering, which significantly slows down the planner; but if you set this too high, you weaken the heuristic (which is also slower!) so there's no point in cranking it up.
- *Safe* - If your actions are cleanly implemented, a failing action won't mutate model state; then, uncheck this and get a small performance bonus. If unsure, leave unchecked.
