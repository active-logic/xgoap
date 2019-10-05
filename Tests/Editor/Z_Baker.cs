using NUnit.Framework;
using UnityEngine;

namespace Activ.GOAP{
public class Z_Baker : TestBase{

    [Test] public void Solvability(){
        var x = new Baker();
        x.SetTemperature(200);
        x.Bake();
        o( Goal(x), true);
    }

    [Test] public void ActionIntegrity(){
        var x = new Baker();
        var M = ((Parametric)x).methods;
        int t = 0;
        foreach(var m in M){
            m.action();
            o(x.temperature, t);
            t += Baker.Step;
        }
    }

    [Test] public void SolvabilityUsingActions(){
        var x = new Baker();
        o( x.temperature, 0 );
        var act = ((Parametric)x).methods[3];
        o( x.temperature, 0 );
        act.action();
        o( x.temperature, 165 );

        ((Agent)x).actions[0]();
        o( x.bake, 82 );
        o( Goal(x), true);
    }

    // TODO - this does not fully verify the output
    [Test] public void Test(){
        var x = new Baker();
        var p = new Solver<Baker>();
        var s = p.Next(x, z => z.state == Baker.Cooking.Cooked);
        o( s is System.Delegate );
    }

    bool Goal(Baker x) => x.state == Baker.Cooking.Cooked;

}}
