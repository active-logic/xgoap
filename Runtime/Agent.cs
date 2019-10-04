using System;

public interface Agent{

    Func<bool>[] actions{ get; }

    float cost { get; }

}

public interface Parametric{

    Action[] methods{ get; }

}
