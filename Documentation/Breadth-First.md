# Breadth first planner

There's a few kinks with this first version of the breadth first planner (BFP). Let's review.

*Non-termination* - If there is no solution, the planner will go on until we run out of memory. We'd like to fail faster and cleaner. For many games probably the most precious resource is time. I wonder if we can accurately limit processing based on that.
Limiting the number of nodes is a fair approximation.

*Cloning looks expensive* - Many cloning implementations are faster; others optimise memory usage. However, these constrain the structure of our world model so, probably a good starting point

*Expanding feels clunky* - The symptom of that is we can't use a `foreach()` because this would not sequence modified copies of the original model; instead, it would iteratively alter the model; still, I think the API is correct, in that available actions may reflect prior steps.

*Reentry is allowed* - An intuitive optimization that can be made is not re-entering previously visited states. This requires comparing a new state with previously generated (including 'closed') states which, in general, is still cost effective.

*Memory management is suboptimal* - Between using a *List* and how we clone the model, it's pretty obvious that this can improved dramatically. However, maybe not a problem for all potential applications.

*No handling of action parameters* - Normally agent actions aren't no-args; here, no obvious way to support this.

*No cost function* - This limitation is inherent to using breadth first search. Using a cost function does not make the search faster; it makes the result 'cheaper' (better) when the perceived cost of each available action varies.

*Does not return the path* - The solution only returns the next action. This is both a design choice and a simplification. In a dynamic environment, reformulating the plan after each step is more effective; only the next step matters.
