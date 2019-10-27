using Ex = System.Exception;
using System;

// Containers for reusable planning states
namespace Activ.GOAP{
internal abstract class Dish<T> where T : class{

    protected T proto, state;

    public abstract void Init(T prototype);
    public abstract T    Avail();

    public virtual void Invalidate (){}
    public void Consume    () => state = null;

}

// Dirty dish assumes failing actions do not mutate model state
// Faster because no 'cleaning the dish' after a failed action.
internal class DirtyDish<T> : Dish<T> where T : class{

    bool dirty; Clonable<T> clonable;

    override public void Init(T prototype){
        clonable = (Clonable<T>) prototype;
        proto = prototype; dirty = true;
    }

    override public T Avail()
    { if(dirty) state = Clone(); dirty = false; return state; }

    override public void Invalidate() => dirty = true;

    T Clone() => clonable.Clone(state ?? clonable.Allocate());

}

// Cleans after every action; supports serial clones
internal class PolyDish<T> : Dish<T> where T : class{

    override public void Init(T prototype) { proto = prototype; }

    override public T Avail() { state = Clone(); return state; }

    T Clone() => (proto is Clonable<T> src)
        ? src.Clone(state ?? src.Allocate())
        : CloneUtil.DeepClone(proto);

}}
