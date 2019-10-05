using System;
using NUnit.Framework;
using NullRef = System.NullReferenceException;

namespace Activ.GOAP{
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

    [Test] public void Head1(){
        var x = new Node<object>(ACTION_1, new object());
        o( x.Head(), ACTION_1);
    }

    [Test] public void Path1(){
        var x = new Node<object>(ACTION_1, new object());
        var path = x.Path();
        o( path[0].ToString(), "[0 :: Test => object]");
    }

    [Test] public void Head2(){
        var x = new Node<object>(State.Init, new object());
        var y = new Node<object>(ACTION_1, new object(), x);
        o( y.Head(), ACTION_1);
    }

    [Test] public void Path2(){
        var x = new Node<object>(State.Init, new object());
        var y = new Node<object>(ACTION_1, new object(), x);
        var path = y.Path();
        o( path[0].ToString(), "[0 :: %init => object]");
        o( path[1].ToString(), "[0 :: Test => object]");
    }

    [Test] public void Head3(){
        var x = new Node<object>(State.Init, new object());
        var y = new Node<object>(ACTION_1, new object(), x);
        var z = new Node<object>(ACTION_2, new object(), y);
        o( z.Head(), ACTION_1);
    }

    [Test] public void Path3(){
        var x = new Node<object>(State.Init, new object());
        var y = new Node<object>(ACTION_1, new object(), x);
        var z = new Node<object>(ACTION_2, new object(), y);
        var path = z.Path();
        o( path[0].ToString(), "[0 :: %init => object]");
        o( path[1].ToString(), "[0 :: Test => object]");
        o( path[2].ToString(), "[0 :: Test => object]");
    }

    [Test] public void String(){
        var x = new Node<object>(State.Init, new object());
        o( x.ToString(), "[0 :: %init => object]" );
    }

}}
