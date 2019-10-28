using System; using NUnit.Framework;
using static Activ.GOAP.Solver<Activ.GOAP.Idler>;
using static Activ.GOAP.Strings;

namespace Activ.GOAP{
public class NodeSetTest : TestBase{

    NodeSet<Idler> x;
    Idler idler = new Idler();

    [SetUp] public void Setup()
    => x = new NodeSet<Idler>(idler, null);

    [Test] public void InitStateMustExist ()
    => Assert.Throws<NullReferenceException>(
                    () => new NodeSet<Idler>(null, null));

    [Test] public void TrueWithinCapacity()
    { if(x){ } else Assert.Fail(); }

    [Test] public void FalseOverCapacity()
    { o((bool)new NodeSet<Idler>(idler, null, capacity: 0), false); }

    [Test] public void FalseWhenEmpty()
    { x.Pop(); o( (bool)x, false); }

    [Test] public void InsertAndSkipExisting()
    { x.Insert(new Node<Idler>("x", idler)); o( x.count, 1); }

    [Test] public void InsertUnsorted(){
        var z = new NodeSet<T>(new T(), null);
        z.sorted = false;
        z.Insert(new Node<T>("x", new T())); o( z.count, 2);
    }

    [Test] public void InsertAndSort(){
        var z = new NodeSet<T>(new T(), null);
        z.Insert(new Node<T>("x", new T())); o( z.count, 2); }

    [Test] public void Pop(){
        var z = x.Pop(); o( x.count, 0 ); o( z.state is Idler );
        o( z.action, INITIAL_STATE);
    }

    class T{
        int x = 0; public T() {} public T(int x){ this.x = x; }
    }

}}
