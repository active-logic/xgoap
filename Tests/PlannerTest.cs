using NUnit.Framework;
using UnityEngine;

public class PlannerTest : TestBase{

    Planner x;

    [SetUp] public void Setup(){
        x = new Planner();
    }

    [Test] public void Clone(){
        var e = new Eradicator();
        var n = e.rats;
        var e1 = Planner.Clone(e);
        e.rats = -1;
        o( n != -1);
        o( e1.rats, n );
    }

}
