// PROVISIONAL - interfaces here are used to enable a SolverInfo
// component.

namespace Activ.GOAP{

public interface SolverStats{

    PlanningState status { get; }
    int  fxMaxNodes      { get; }
    int  I               { get; }

}

public interface SolverOwner{ SolverStats stats{ get; } }

}
