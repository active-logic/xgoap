using System;
using NUnit.Framework;
using NullRef = System.NullReferenceException;
//using static Activ.GOAP.Solver<Activ.GOAP.Agent>;

namespace Activ.GOAP{
public class GoalTest : TestBase{

    [Test] public void Goal_requires_goal_function()
    => Assert.Throws<NullRef>( () => new Goal<Agent>(null) );

    [Test] public void Goal_with_goal_function(){
        Func<Agent, bool> f = x => true;
        var g = new Goal<Agent>(f);
        o( g.match, f );
        o( g.h, null );
    }

    [Test] public void Goal_with_goal_function_and_heuristic(){
        Func<Agent, bool>  f = x => true;
        Func<Agent, float> h = x => 1f;
        var g = new Goal<Agent>(f, h);
        o( g.match, f );
        o( g.h, h );
    }

}}
