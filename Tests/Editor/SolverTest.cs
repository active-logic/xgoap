using NUnit.Framework;
using NullRef = System.NullReferenceException;
using Ex      = System.Exception;
using InvOp   = System.InvalidOperationException;
using static Activ.GOAP.Solver<Activ.GOAP.Agent>;
using static Activ.GOAP.Strings;

namespace Activ.GOAP{
public class SolverTest : TestBase{

    Solver<object> x;
    Goal<object> unreachable = new Goal<object>(x => false),
                 stasis      = new Goal<object>(x => true),
                 hStasis     = new Goal<object>(x => true, x => 0f);

    [SetUp] public void Setup(){
        x = new Solver<object>();
        x.maxIter = x.maxNodes = 100;
    }

    // Defaults ----------------------------------------------------

    // Important because tolerance > 0 may affect search result
    [Test] public void ZeroToleranceDefault() => x.tolerance = 0;

    // Unsafe mode should be enabled by user once they figure out
    // what it means
    [Test] public void SafeDefault() => x.safe = true;

    // Default to A*, not breadth-first
    [Test] public void BRFSDefault() => x.brfs = false;

    // isRunning ---------------------------------------------------

    [Test] public void IsRunning_false_on_construct()
    => o( x.isRunning, false );

    [Test] public void IsRunning_false_after_solving(){
        var g = new Goal<object>(x => true);
        var z = x.Next(new Idler(),
                       in g);
        o( x.isRunning, false);
    }

    [Test] public void IsRunning_true_with_zero_frames_budget(){
        var z = x.Next(new Idler(), unreachable, iter: 0);
        o( x.isRunning, true);
    }

    [Test] public void IsRunning_true_with_remaining_frames(){
        var z = x.Next(new Inc(), unreachable, iter: 1);
        o( x.isRunning, true);
    }

    // Next --------------------------------------------------------

    [Test] public void Next_no_agent_throws()
    => Assert.Throws<NullRef>(
        () => x.Next((Agent)null, unreachable) );

    [Test] public void Next_start_at_goal()
    => o ( (string)x.Next(new Idler(), stasis), INITIAL_STATE );

    [Test] public void Next_start_at_goal_with_heuristic()
    => o ( (string)x.Next( new Idler(),
                   hStasis), INITIAL_STATE );

    [Test] public void Next_OTPoneyPassThrough()
    => o( (string)x.Next(new OTPoney(), stasis), INITIAL_STATE );

    [Test] public void Next_HeuristicOTPoneyPassThrough()
    => o( (string)x.Next(new OTPoney(), hStasis), INITIAL_STATE );

    [Test] public void Next_use_heuristic(){
        bool h = false;
        x.Next(new Inc(), new Goal<object>(
            x => false, x => { h = true; return 0f; }));
        o( h, true );
    }

    [Test] public void IgnoreHeuristic(){
        bool h = false; x.brfs = true;
        x.Next(new Inc(), new Goal<object>(
            x => false, x => { h = true; return 0f; }));
        o( h, false );
    }

    // Iterate -----------------------------------------------------

    [Test] public void Iterate_no_init_state()
    => Assert.Throws<InvOp>( () => x.Iterate() );

    [Test] public void Iterate_MaxIterExceeded(){
        x.maxIter = 2;
        x.Next(new Inc(), unreachable, 10);
        o(x.status == PlanningState.MaxIterExceeded);
        var z = x.Iterate();
        o(z, null);
    }

    [Test] public void Iterate_no_solution(){
        x.Next(new SixShot(), unreachable, 4);
        o(x.status != PlanningState.MaxIterExceeded);
        var z = x.Iterate(4);
        o(z, null);
        o(x.status, PlanningState.Failed);
    }

    [Test] public void Iterate_stalling(){
        x.maxIter = 8;
        x.Next(new Inc(), unreachable, 5);
        o(x.status != PlanningState.MaxIterExceeded);
        var z = x.Iterate(5);
        o(x.status == PlanningState.MaxIterExceeded);
    }

    // Expansions --------------------------------------------------

    [Test] public void ExpandActions_zero_cost_throws()
    => Assert.Throws<Ex>(
            () => x.Next(new CouchSurfer(), unreachable) );

    [Test] public void ExpandMethods_zero_cost_throws()
    => Assert.Throws<Ex>(
            () => x.Next(new Freeloader(), unreachable) );

    [Test] public void ExpandMethods_zero_cost_brfs(){
        x.brfs = true;
        var s = x.Next(new Freeloader(), unreachable);
    }

}}
