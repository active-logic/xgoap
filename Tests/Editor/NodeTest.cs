using System;
using NUnit.Framework;
using NullRef = System.NullReferenceException;
using static Activ.GOAP.Solver<Activ.GOAP.Agent>;

namespace Activ.GOAP{
public class NodeTest : TestBase{

    const string ACTION_1 = "Test";
    const string ACTION_2 = "Test";
    //const string ACTION_2 = "Test";

    Node<object> x;

    [SetUp] public void Setup()
    => x = new Node<object>(ACTION_1, new object());

    [Test] public void Prev() => o(x.prev, null);

    [Test] public void Action() => o(x.action, ACTION_1);

    [Test] public void State() => o(x.state is object);

    [Test] public void Value() => o(x.value = 1, 1);

    [Test] public void Constructor(){
        o( x.prev, null );
        o( x.action, ACTION_1);
        o( x.state is object );
        o( x.value, 0);
    }

    [Test] public void ConstructorWithPredecessor(){
        var a = new Node<object>(ACTION_1, new object(), cost: 1);
        var x = new Node<object>(ACTION_1, new object(), a, 1);
        o( x.prev, a );
        o( x.action, ACTION_1);
        o( x.state is object );
        o( x.value, 0);
        o( x.cost, 2);
    }

    [Test] public void ConstructorRequiresAction()
    => Assert.Throws<NullRef>( () => new Node<object>(null, null) );

    [Test] public void ConstructorRequiresState()
    => Assert.Throws<NullRef>( () => new Node<object>("X", null) );

    [Test] public void Head1() => o( x.Head(), ACTION_1);

    [Test] public void Path1(){
        var path = x.Path();
        o( path[0].ToString(), "[0.0 :: Test => object]");
    }

    [Test] public void Head2(){
        var x = new Node<object>(INIT, new object());
        var y = new Node<object>(ACTION_1, new object(), x);
        o( y.Head(), ACTION_1);
    }

    [Test] public void Path2(){
        var x = new Node<object>(INIT, new object());
        var y = new Node<object>(ACTION_1, new object(), x);
        var path = y.Path();
        o( path[0].ToString(), "[0.0 :: %init => object]");
        o( path[1].ToString(), "[0.0 :: Test => object]");
    }

    [Test] public void Head3(){
        var x = new Node<object>(INIT, new object());
        var y = new Node<object>(ACTION_1, new object(), x);
        var z = new Node<object>(ACTION_2, new object(), y);
        o( z.Head(), ACTION_1);
    }

    [Test] public void Path3(){
        var x = new Node<object>(INIT, new object());
        var y = new Node<object>(ACTION_1, new object(), x);
        var z = new Node<object>(ACTION_2, new object(), y);
        var path = z.Path();
        o( path[0].ToString(), "[0.0 :: %init => object]");
        o( path[1].ToString(), "[0.0 :: Test => object]");
        o( path[2].ToString(), "[0.0 :: Test => object]");
    }

    [Test] public void PathToString(){
        var x = new Node<object>(INIT, new object());
        var y = new Node<object>(ACTION_1, new object(), x);
        var path = y.PathToString();
        o(path, "%init\nTest\n");
    }

    [Test] public void String(){
        var x = new Node<object>(INIT, new object());
        o( x.ToString(), "[0.0 :: %init => object]" );
    }

}}
