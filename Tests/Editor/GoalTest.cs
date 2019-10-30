using NUnit.Framework;
using NullRef = System.NullReferenceException;

namespace Activ.GOAP{
public class GoalTest : TestBase{

    Condition<T> f = x => true;
    Heuristic<T> h = x => 1f;

    [Test] public void Goal_requires_goal_function()
    => Assert.Throws<NullRef>( () => new Goal<T>(null) );

    [Test] public void Goal_with_goal_function(){
        var g = new Goal<T>(f);
        o( g.match, f );
        o( g.h, null );
    }

    [Test] public void Goal_with_cond_and_heuristic(){
        var g = new Goal<T>(f, h);
        o( g.match, f );
        o( g.h, h );
    }

    [Test] public void Goal_with_cond_and_heuristic_from_tuple(){
        Goal<T> g = (f, h);
        o( g.match, f );
        o( g.h, h );
    }

    class T{}

}}
