using System;

namespace Activ.GOAP{
public interface Agent{

    Func<Cost>[] actions{ get; }

}}
