using NUnit.Framework;

namespace Activ.GOAP{
public class Z_Baker : TestBase{

    [Test] public void Solvability(){
        var x = new Baker();
        x.SetTemperature(200);
        x.Bake();
        o( Goal(x), true);
    }

    [Test] public void OptionIntegrity(){
        var x = new Baker();
        var M = ((Mapped)x).Options();
        int t = 0;
        foreach(var m in M){
            m.option();
            o(x.temperature, t);
            t += Baker.Step;
        }
    }

    [Test] public void SolvabilityUsingOptions(){
        var x = new Baker();
        o( x.temperature, 0 );
        var act = ((Mapped)x).Options()[3];
        o( x.temperature, 0 );
        act.option();
        o( x.temperature, 165 );

        ((Agent)x).Options()[0]();
        o( x.bake, 82 );
        o( Goal(x), true);
    }

    // TODO - this does not fully verify the output
    [Test] public void Test(){
        var  x = new Baker();
        var  p = new Solver<Baker>();
        Goal<Baker> g = new Goal<Baker>(this.Goal);
        var s = (System.Delegate)p.Next(x, in g);
        o( s is System.Delegate );
    }

    bool Goal(Baker x) => x.state == Baker.Cooking.Cooked;

}}
