using System; using NUnit.Framework;
using static Activ.GOAP.Solver<Activ.GOAP.Idler>;
using static Activ.GOAP.Strings;

namespace Activ.GOAP{
public class NodeSetTest : TestBase{

    NodeSet<Idler> x;
    Idler idler = new Idler();

    [SetUp] public void Setup()
    => x = new NodeSet<Idler>().Init(idler, null);

    [Test] public void NeedsClearBeforeInit()
    => Assert.Throws<Exception>( () => x.Init(idler, null) );

    [Test] public void InitStateMustExist ()
    => Assert.Throws<NullReferenceException>(
                    () => new NodeSet<Idler>().Init(null, null));

    [Test] public void TrueWithinCapacity()
    { if(x){ } else Assert.Fail(); }

    [Test] public void FalseOverCapacity(){
        o((bool)new NodeSet<Idler>().Init(idler, null, capacity: 0),
          false);
      }

    [Test] public void FalseWhenEmpty()
    { x.Pop(); o( (bool)x, false); }

    [Test] public void InsertAndSkipExisting()
    { x.Insert(new Node<Idler>("x", idler)); o( x.count, 1); }

    [Test] public void InsertUnsorted(){
        var z = newWith_T;
        z.sorted = false;
        z.Insert(newNode_T); o( z.count, 2);
    }

    [Test] public void InsertAndSort(){
        var z = newWith_T;
        z.Insert(newNode_T); o( z.count, 2); }

    [Test] public void Pop(){
        var z = x.Pop(); o( x.count, 0 ); o( z.state is Idler );
        o( z.action, INITIAL_STATE);
    }

    [Test] public void Clear(){
        var z = newWith_T;
        o( z.count, 1 ); o( z.visited, 1 );
        x.Clear();
        o( x.count, 0 ); o( x.visited, 0 );
    }

    NodeSet<T> newWith_T => new NodeSet<T>().Init(new T(), null);
    Node<T>    newNode_T => new Node<T>("x", new T());

    class T{
        int x = 0; public T() {} public T(int x){ this.x = x; }
    }

}}
