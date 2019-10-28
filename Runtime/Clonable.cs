namespace Activ.GOAP{
public interface Clonable<T>{

    T Allocate ();
    T Clone    (T storage);

}}
