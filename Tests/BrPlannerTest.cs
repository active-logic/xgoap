using NUnit.Framework;
using UnityEngine;

public class BrPlannerTest : TestBase{

    BrPlanner x;

    [SetUp] public void Setup(){
        x = new BrPlanner();
    }

    [Test] public void Clone(){
        var e = new Eradicator();
        var n = e.rats;
        var e1 = BrPlanner.Clone(e);
        e.rats = -1;
        o( n != -1);
        o( e1.rats, n );
    }

}
