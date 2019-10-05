using System;

public interface Agent{

    Func<bool>[] actions{ get; }

    float cost { get; }

}
