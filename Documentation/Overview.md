# A user guide *beyond GOAP*

## What is GOAP?

GOAP (Goal oriented action planning) refers to a family of planning AIs inspired by [Jeff Orkin's GOAP](http://alumni.media.mit.edu/~jorkin/gdc2006_orkin_jeff_fear.pdf).

In GOAP, an agent are assigned a goal (escape a room, take cover, down a target, heal, ...). Utilizing actions with known *preconditions*, *cost* and *effects* a search then resolves a path to the goal.

To speed things up, a *heuristic* is often provided.

## While you GOAP

While reading, also check the [Sentinel demo](https://youtu.be/mbLNALyt5So) and associate [project files](https://github.com/active-logic/xgoap-demos).

## Planning Agent/Model

The critical part of your AI is the model, aka 'agent'; the model represents the AI's knowledge of the environment they operate in, including itself.

- Your model is a class implementing `Agent` and/or `Mapped` (to express planning actions, aka 'options').

The solver maintains multiple copies of the model object, therefore cloning, hashing and comparing for equality are common operations applied many times over. Searches are fast (A\*) provided the model is concise and effective.

- Minimally, tag your model [*Serializable*](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/serialization/) or (much better) implement `Clonable`
- Override `Equals()` and `GetHashCode()` (sloppy hashcodes decrease performance)

### Options

*Beyond GOAP*, planning actions are known as 'options'; the solver doesn't directly care about how you update your model. It does, however, need to know whether an option is available, and at what cost.
as a simple example consider the following (incomplete) model:

```cs
class Scout{

    Vector2 pos;   // map coord
    int     mood;  // current mood

    public Cost Greet() => (mood += 1, 0.1f);  // success with cost = 0.1  (1)

    public Cost Move(Vector2 dir){
        var p = pos + dir;
        if(!CanReach(p)){
            return false;  // auto-converted to 'Cost' with failed status  (2)
        }else{
            pos = p;
            return 2;  // auto-converted to Cost with success and cost = 2 (3)
        }
    }

}
```

- In (1), a tuple is used to quickly modify the model and return a cost. This is a shorthand, handy when an option is always available.
- In (2), the precondition failed, we just return `false`.
- In (3), only the cost value is returned, implying success (int or float ok).

### `Agent` and `Mapped`

Available planning actions must be listed by implementing `Agent` and/or `Mapped`.

Often implementing `Agent` suffices - easier to implement and (currently) faster.

```cs
Option[] Agent.Options() => new Option[]{ JumpForward, Shoot, ... };

public Cost JumpForward() => (Jump(1f, 1f), 2f);

public Cost Shoot() => ...;

```

NOTE: *With `Agent`, the solver outputs the action sequence as a list of strings. If you are using the integration, action names are automatically remapped to game actions (see 'integration' below)*

For more control, or if you need to parameterize actions dynamically, use `Mapped`; here is an example:

```cs
(Option, Action)[] Mapped.Options(){
    var n   = inventory.Length;
    var opt = new (Option, Action)[n];
    for(int i = 0; i < n; i++){
        var j = i;  // Don't capture the iterator!
        opt[i] = ( () => Use(j),
                   () => client.Use(inventory[j]) );
    }
    return opt;
}

public Cost Use(int itemIndex){
    if(!CanUse(itemIndex)) return false;
    /* Update model to reflect item use ... */
    return itemCost[itemIndex];  // assuming variable cost per item
}
```

In the above example:
- An inventory is considered
- An option is generated for each item in the inventory
- Options are mapped to game actions via `(Option, System.Action)` tuples

NOTE: *In the above we are careful* not *to use the iterator `i` inside the lambda, otherwise all invocations of the lambda function would end up using a value of `n-1`*.

`Mapped` options are flexible and type safe, giving you complete control over how a planning action maps to a game action; `Agent` is easier to implement.

### The Clonable interface

Implement `Allocate()` to create a model object instance. The purpose of this is to perform all memory allocations upfront, not determine state.

Implement `Clone(T storage)` to copy model state. This function **must** assign all fields (to avoid leaking dirty state in case of model reuse).

```cs
class MyModel : Clonable<MyModel>{

    T byRef;                 // Assume T : Clonable<T>; not required but handy
    int byValue;
    MyMonoBehaviour client;  // Assuming T : MonoBehaviour

    MyModel(){
        byRef = new T();  // Allocate everything
        // byValue = 5;   // Let's not do extra work
    }

    public MyModel Allocate() => new MyModel();

    public MyModel Clone(MyModel x){
        this.byRef.Clone(x.byRef);    // Don't shallow copy
        x.byValue = byValue;          // Set all fields
        x.client  = client;           // By ref, see below
    }

}
```

Above, we clone a model including the `byValue`, `byRef` and `client` fields.

- `byValue` is a value type so we don't need to initialize it during `Alloc`
- `byRef` must be 'deep cloned' to avoid sharing state between clones.
- `client` represents actual game data (a typical use of this is mapping options to actual game actions); this is shared data so we just forward a reference to the cloned state.

NOTE: *To keep your model separate from the game engine, for example in case you also run this in your backend), `client` should be accessed via an interface.*

Designed for instance reuse, this API enables optimizations, such as pooling.

NOTE: *The `Allocate` API is required because newing a `T : class, new()` object resolves to an `AlloceSlow` variant (the name says it all)*

### Test your model

Beyond GOAP cleanly separates the planning model from the executor (the object implementing actual game actions). This allows putting your model under test, making development faster and easier.

## Integration

With a working model handy, you want to plug this into your game/simulation.

The library provides a simple integration, mainly intended for (but not tied to) Unity3D.

The integration implements a two step *(planning -> action)* cycle:

1 - A plan is generated
2 - The *first* action in the plan is applied

(Rinse and repeat until the goal is attained)

We might plan once and step through all steps; however since world state changes dynamically, replanning often keeps our agents on track.

NOTE: *In the future the integration will give you more control over how often replanning is applied.*

To use the integration, subclass `GameAI`, as explained below.

### Subclass `GameAI`

[`GameAI`](../Runtime/GameAI.cs) specifies a handful of functions that you need to implement to get your game actors going:

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

Your implementation of `T Model()` should return an object representing the current state of the agent and their environment:

```cs
// Model definition
class MyModel{ float x, z; }

// Inside MyAI : GameAI<MyModel>
override public MyModel Model(){
    return new MyModel(transform.position.x, transform.position.z);
}
```

While `IsActing()` returns false, the planner will be running and evaluating the next action; how you implement this (whether event based, or testing the state of the game actor...) is entirely up to you; likewise the `Idle()` function.

```cs
override public bool IsActing() => SomeCondition() && SomeOther();

override public void Idle() => GetComponent<Animation>().Play("Idle");
```

### Provide counterparts to planning options

Since options aren't 'real' game actions, your `GameAI` implementation should supply (or usually, bridge) these.

- With `Agent`, all planning actions must have same-name, no-arg counterparts in `GameAI`.
- With `Mapped`, one approach consists in defining an interface, which specifies methods to be implemented both as options and as game actions. The [Baker](`../Tests/Models/Baker.cs`) example illustrates this approach.

## Run your AI

Once you have implemented your `GameAI` subclass, it can be added to any game object (In Unity, `GameAI` derives from `Monobehaviour`).

Additionally, tweaks are available...

- *verbose* - gives you basic information (in the console) about what actions are applied to the game AI

Then, under 'solver params':

- *Frame budget* - max number of solver iterations per game frame.
- *Max nodes* - max number of states that should exist within the planner at any given time.
- *Max iter* - the total, maximum number of iterations allowed to find a solution; after which the planner just bails out.
- *Tolerance* - represents how closely the heuristic should be followed. For example if you don't care about a $10 difference (if 'cost' represents money) or a 0.5 seconds delta (if 'time cost' is the heuristic), set this to $10 or 0.5 seconds.
Leaving this number to zero forces a full ordering, which significantly slows down the planner; but if you set this too high, you weaken the heuristic (which is also slower!) so there's no point in cranking it up.
- *Safe* - If your actions are cleanly implemented, a failing action won't mutate model state; then, uncheck this and get a small performance bonus. If unsure, leave unchecked.

## Good luck!

I (the author of this library) hope the information in this guide was helpful to you; if you'd like to report inaccuracies, bugs, or features, please open an issue!

If you need more help:
- Ask in the (Unity3D forum thread)[https://forum.unity.com/threads/a-goap-goal-oriented-action-planning-library-beyond-goap.763478/]
- For business inquiries, [contact the author](mailto:tea.desouza@gmail.com)
