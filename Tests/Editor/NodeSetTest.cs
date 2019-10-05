using System; using NUnit.Framework;

public class NodeSetTest : TestBase{

    NodeSet<Agent> x;
    Idler idler = new Idler();

    [SetUp] public void Setup()
    => x = new NodeSet<Agent>(idler, null);

    [Test] public void InitStateMustExist ()
    => Assert.Throws<NullReferenceException>(
                    () => new NodeSet<Agent>(null, null));

    [Test] public void TrueWithinCapacity()
    { if(x){ } else Assert.Fail(); }

    [Test] public void FalseOverCapacity()
    { o((bool)new NodeSet<Agent>(idler, null, capacity: 0), false); }

    [Test] public void FalseWhenEmpty()
    { x.Pop(); o( (bool)x, false); }

    [Test] public void InsertAndSkipExisting()
    { x.Insert(new Node<Agent>("x", idler)); o( x.count, 1); }

    [Test] public void InsertUnsorted(){
        x.sorted = false;
        x.Insert(new Node<Agent>("x", new Inc())); o( x.count, 2);
    }

    [Test] public void InsertAndSort()
    { x.Insert(new Node<Agent>("x", new Inc())); o( x.count, 2); }

    [Test] public void Pop(){
        var z = x.Pop(); o( x.count, 0 ); o( z.state is Idler );
        o( z.action, State.Init);
    }

}
