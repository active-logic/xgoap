using System;

namespace Activ.GOAP{

public interface Agent{
    Func<Cost>[] Actions();
}

public interface Parametric{
    Complex[] Functions();
}

public interface Clonable<T>{
    T Allocate ();
    T Clone    (T storage);
}

}
