using NUnit.Framework;
using System;

namespace Activ.GOAP{
public class GameAITest : TestBase{

    BakerAI x;

    [SetUp] public void Setup(){
        #if UNITY_2018_1_OR_NEWER
        x = new UnityEngine.GameObject().AddComponent<BakerAI>();
        #else
        x = new BakerAI();
        #endif
    }

    [Test] public void Verbose(){
        o( x.verbose, false);
        o( x.verbose = true, true);
    }

    [Test] public void Busy() => o( x.busy, false);

    [Test] public void Update(){
        x.frameBudget = 32;
        o( x.Model(), x.Model()); // temp
        o( x.Model().state == Baker.Cooking.Raw );
        x.Update();
        o( x.solver.state, PlanningState.Done);
        x.Update();
        o( x.solver.state, PlanningState.Done);
        o( x.Model().ToString(), "Baker[ Cooked at 165℃ ]");
    }

    [Test] public void Update_pie_is_already_burned(){
        x.verbose = true;
        x.frameBudget = 32;
        x.bake = 150;
        x.Update();
        o( x.solver.state, PlanningState.Failed);
    }

    [Test] public void Update_and_keep_running(){
        x.verbose = true;
        x.Update();
        o( x.solver.state, PlanningState.Running);
        x.Update();
        x.frameBudget = 32;
        x.Update();
        o( x.solver.state, PlanningState.Done);
    }

    // Transitional --------------------------------------------

    [Test] public void ClientIntegrity() => x.Bake();

    [Test] public void ModelIntegrity(){
        var a = new Baker(){ temperature=0, bake = 0 };
        var b = new Baker(){ temperature=1, bake = 0 };
        var c = new Baker(){ temperature=0, bake = 1 };
        o( !a.Equals(b) );
        o( !a.Equals(c) );
    }

}}
