using NUnit.Framework;

namespace Activ.GOAP{
public class Z_Eradicator : TestBase{

    [Test] public void Test(){
        var x = new Eradicator();
        var p = new Solver<Eradicator>();
        var s = p.Next(x, new Goal<Eradicator>(z => z.rats == 0));
        o( (string)s, "Eradicate");
    }

    [Test] public void TestLimit(){
        var x = new Eradicator();
        var p = new Solver<Eradicator>();
        p.maxIter = 10;
        var s = p.Next(x, new Goal<Eradicator>(z => z.rats == -1));
        o( s, null);
        // TODO - MaxIterExceeded was expected here
        o(p.status == PlanningState.Failed);
    }

}}
