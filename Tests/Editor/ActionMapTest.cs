using NUnit.Framework;
using System;

namespace Activ.GOAP{
public class ActionMapTest : TestBase{

    ActionMap x;

    [SetUp] public void Setup() => x = new ActionMap();

    [Test] public void Effect_with_invalid_type()
    => Assert.Throws<ArgumentException>(
        () => ((ActionHandler)x).Effect(
                                          0, (GameAI<Agent>)null) );

    [Test] public void Print_verbose(){
        x.verbose = true;
        x.Print("");
    }

    [Test] public void Print_non_verbose(){
        x.verbose = false;
        x.Print("");
    }

}}
