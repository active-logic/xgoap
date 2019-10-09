using NUnit.Framework;
using NullRef = System.NullReferenceException;
using static Activ.GOAP.Solver<Activ.GOAP.Agent>;

namespace Activ.GOAP{
public class SolverTest : TestBase{

    Solver<Agent> x;

    [SetUp] public void Setup(){
        x = new Solver<Agent>();
        x.maxIter = x.maxNodes = 100;
    }

    [Test] public void AgentMissing()
    => Assert.Throws<NullRef>( () => x.Next((Agent)null,
                                            x => false) );

    [Test] public void GoalMissing()
    => Assert.Throws<NullRef>( () => x.Next( new Idler(), null) );

    [Test] public void IdlePassThrough()
    => o ( x.Next(new Idler(), goal: x => true), INIT );

    [Test] public void HeuristicIdlePassThrough()
    => o ( x.Next(new Idler(), goal: x => true, h: x => 0f),
           INIT );

    // TODO doesn't look right. First off, returned func should be
    // OneTrick, secondly if goal is always fulfilled no action is
    // needed
    [Test] public void OTPoneyPassThrough()
    => o( x.Next(new OTPoney(), goal: x => true), INIT );

    // TODO see above
    [Test] public void HeuristicOTPoneyPassThrough()
    => o( x.Next(new OTPoney(), goal: x => true, h: x => 0f),
          INIT );

    [Test] public void UseHeuristic(){
        bool h = false;
        x.Next(new Inc(), x => false,
               h: x => { h = true; return 0f; });
        o( h, true );
    }

    [Test] public void IgnoreHeuristic(){
        bool h = false; x.brfs = true;
        x.Next(new Inc(), x => false,
               h: x => { h = true; return 0f; });
        o( h, false );
    }

    [Test] public void RemoveRedundantStates(){
        x.Next(new TikTok(), x => false);
    }

}}
