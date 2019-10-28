using NUnit.Framework;
using Activ.GOAP;
using ArgNull = System.ArgumentNullException;

namespace Activ.GOAP{
public class DishTest : TestBase{

    T clonable = new T();

    [Test] public void CreateSafeDishWithClonable()
    => o( Dish<T>.Create(clonable, safe: true) is PolyDish<T> );

    [Test] public void CreateUnsafeDishWithClonable()
    => o( Dish<T>.Create(clonable, safe: false) is DirtyDish<T> );

    [Test] public void CreateWithSerializable(
                                   [Values(true, false)] bool safe){
        o( Dish<U>.Create(new U(), safe) is PolyDish<U> );
    }

    class T : Clonable<T>{
        public T Allocate() => new T();
        public T Clone(T t) => t;
    }

    [System.Serializable] class U{}

}}
