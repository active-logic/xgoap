using NUnit.Framework;
using Activ.GOAP;
using ArgNull = System.ArgumentNullException;

namespace Activ.GOAP{
public class PolyDishTest{

    T proto = new T();
    PolyDish<T> x;

    [SetUp] public void Setup() => x = new PolyDish<T>();

    [Test] public void Init() => x.Init(proto);

    [Test] public void Avail() { x.Init(proto); x.Avail(); }

    [Test] public void AvailThrows()
    { Assert.Throws<ArgNull>( () => x.Avail() ); }

    [Test] public void Invalidate() => x.Invalidate();
    [Test] public void Consume() => x.Consume();

    class T : Clonable<T>{
        public T Allocate() => new T();
        public T Clone(T t) => t;
    }

}}
