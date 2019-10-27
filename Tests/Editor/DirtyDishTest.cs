using NUnit.Framework;
using Activ.GOAP;

namespace Activ.GOAP{
public class DirtyDishTest{

    T proto = new T();
    DirtyDish<T> x;

    [SetUp] public void Setup()     => x = new DirtyDish<T>();
    [Test] public void Init()       => x.Init(proto);
    [Test] public void Avail()      => x.Avail();
    [Test] public void Invalidate() => x.Invalidate();
    [Test] public void Consume()    => x.Consume();

    class T : Clonable<T>{
        public T Allocate() => new T();
        public T Clone(T t) => t;
    }

}}
