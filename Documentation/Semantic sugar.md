# Semantic sugar in API design

## Problem being solved

In XGOAP, there was both 'planning actions' and 'planning functions'. The intention simply is to support both no-arg and parametric planning moves.

In practice, however, the solver does not deal with parametric functions, it only deals with parameterized invocations. In other words, choosing argument sets is still the client's responsibility.

As a result, how 'planning functions' are implemented is via explicit mappings between the planning action and the actual move performed by the agent, which created a need for naming this mapping.
