using System;

namespace Activ.GOAP{

public delegate Cost Option();

public interface Agent{
    Option[] Options();
}

public interface Mapped{
    (Option option, Action action)[] Options();
}

public interface Clonable<T>{
    T Allocate ();
    T Clone    (T storage);
}

}
