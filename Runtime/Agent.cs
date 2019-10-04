using System;

public interface Agent{

    Func<bool>[] actions{ get; }

    float cost { get; }

}

public interface Parametric<T>{

    Action<T>[] methods{ get; }

}
