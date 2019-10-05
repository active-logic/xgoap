using System;

namespace Activ.GOAP{
public interface Agent{

    Func<bool>[] actions{ get; }

    float cost { get; }

}}
