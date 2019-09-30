# GOAP planners

This package contains several GOAP (goal oriented action planning) solutions. Compared to similar packages available on Github, the differences are as follows:

- The emphasis is on less verbose, easy to implement models.
- Several algorithms are supported, and you can easily switch between methods. For example, the original GOAP requires a cost function. If you have don't have a cost function (or not yet) you can easily switch to the BrF (bread-first) planner.

# Getting started

[Explain how to setup a simple model]

# Unity integration

[Explain unity integrated example]

# Available methods

## Breadth First planner

[BrPlanner](Documentation/Breadth-First.md) implements planning using a breadth first search. In this case, cost functions are not supported so the underlying assumption is that every action has the same cost.

## Best First planner

[BfPlanner](Documentation/Best-First.md) implements cost minimization. This is similar to some GOAP implementations, for example []

## A* planner

[BfPlanner](Documentation/Best-First.md) implements A* cost minimization, with a goal distance estimate.
