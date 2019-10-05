# Baker Example - Unity

In this example we're going to integrate the baker model into an actual AI which can perform the task.

Let's create a class named `BakerAI`, derived from `GameAI`.

```cs
public class BakerAI : GameAI<Baker>(){ }
```

We need to supply the `Goal<T> Goal()` and `T Model()` methods.

Since we don't have a heuristic for the goal, we'll just supply the goal condition:

```cs
override protected Goal<T> Goal()
=> new Goal<Baker>( x => x.state == Baker.Cooking.Cooked );
```

Now let's supply the model:

```cs
override protected Baker Model()
=> new Baker{ temperature: temperature, bake: bake };
```

The model must be reconstructed before planning. This is because, in the game environment, the effects of an action are not guarranted. Initially though, the temperature (which really is a property of an oven object) and bake amount (which really is a property of the pie)
are just designated as properties of the game AI.

Next we need to implement the SetTemperature and Bake methods. For now these are almost the same as the model's except they don't return a boolean value:

```cs
public void SetTemperature(int degrees) => temperature = degrees;

public void Bake()
=> bake += temperature/2;
```

These two methods are not bound by an interface so we need to be especially careful to use the names defined by the model.
