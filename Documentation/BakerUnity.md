# Baker Example - Unity

We're going to integrate the [baker model](../Tests/Models/Baker.cs) into an actual game AI which can perform the task.

Let's create a class named `BakerAI`, derived from `GameAI` (for Unity) and `Baker.AI` (client interface defined in *Baker.cs*)

```cs
public class BakerAI : GameAI<Baker>, Baker.AI{ }
```

We supply the `Goal<T> Goal()` and `T Model()` methods.

Since we don't have a heuristic for the goal, we'll just supply the goal condition:

```cs
override public Goal<Baker> Goal()
=> new Goal<Baker>( x => x.state == Baker.Cooking.Cooked );
```

Now let's provide the model:

```cs
override public Baker Model()
=> new Baker(this){ temperature = temperature, bake = bake };
```

Reconstructing the model before planning is advisable; in a dynamic game environment, the effects of an action are not guarranted.

In this example, the temperature (which really should be a property of an oven object) and bake amount (which would be a property of the pie) are stored by the game AI.

Last, we implement `Baker.AI`.

```cs
public void SetTemperature(int degrees) => temperature = degrees;

public void Bake() => bake += temperature/2;
```

All in! `BakerAI` may then be added to a Unity game object. Upon starting, the planner will automatically drive behavior.
