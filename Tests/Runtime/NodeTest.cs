using System;
using NUnit.Framework;
using NullRef = System.NullReferenceException;

public class NodeTest : TestBase{

    const string ACTION_1 = "Test";
    const string ACTION_2 = "Test";

    [Test] public void Constructor(){
        var x = new Node<object>(ACTION_1, new object());
        o( x.prev, null );
        o( x.action, ACTION_1);
        o( x.state is object );
        o( x.value, 0);
    }

    [Test] public void ConstructorRequiresAction()
    => Assert.Throws<NullRef>( () => new Node<object>(null, null) );

    [Test] public void ConstructorRequiresState()
    => Assert.Throws<NullRef>( () => new Node<object>("X", null) );

    [Test] public void Value(){
        var x = new Node<object>(ACTION_1, new object());
        x.value = 1;
        o(x.value, 1);
    }

    [Test] public void Head(){
        var x = new Node<object>(ACTION_1, new object());
        o( x.Head(), ACTION_1);
    }

    [Test] public void Head1(){
        var x = new Node<object>(ACTION_1, new object());
        o( x.Head(), ACTION_1);
    }

    [Test] public void Head2(){
        var x = new Node<object>(Solver.INIT, new object());
        var y = new Node<object>(ACTION_1, new object(), x);
        o( y.Head(), ACTION_1);
    }

    [Test] public void Head3(){
        var x = new Node<object>(Solver.INIT, new object());
        var y = new Node<object>(ACTION_1, new object(), x);
        var z = new Node<object>(ACTION_2, new object(), y);
        o( z.Head(), ACTION_1);
    }

    [Test] public void String(){
        var x = new Node<object>(Solver.INIT, new object());
        o( x.ToString(), "[0 :: %init => System.Object]" );
    }

}
