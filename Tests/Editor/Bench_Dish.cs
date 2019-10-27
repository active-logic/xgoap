using NUnit.Framework;

namespace Activ.GOAP{
public class Bench_Dish{

    T proto = new T();
    PolyDish<T> x;

    [SetUp] public void Setup() => x = new PolyDish<T>();

    [Test] public void Perf_XDish()
    => RunTest(new PolyDish<T>());

    [Test] public void Perf_DirtyDish()
    => RunTest(new DirtyDish<T>());

    void RunTest(Dish<T> x){
        x.Init(proto);
        for(int i = 0; i < 1e6; i++){
            x.Avail();
            x.Invalidate();
            x.Avail();
            x.Consume();
            x.Avail();
        }
    }

    class T : Clonable<T>{
        public T Allocate() => new T();
        public T Clone(T t) => t;
    }

}}
