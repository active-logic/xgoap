namespace Activ.GOAP{
[System.Serializable] public class SolverParams{

    public int   frameBudget = 25;
    public int   maxNodes    = 1000;
    public int   maxIter     = 1000;
    public float tolerance   = 0;
    public bool  safe        = true;

    public void Reset<T>(Solver<T> solver) where T : class{
        solver.maxNodes  = maxNodes;
        solver.maxIter   = maxIter;
        solver.tolerance = tolerance;
        solver.safe      = safe;
        solver.Reset();
    }

}}
