using Activ.GOAP;

namespace Activ.GOAP.Test{
public class AI : GameAI<Model>{

    public Goal<Model>[] goals;

    override public Goal<Model>[] Goals() => goals;

    override public Model Model() => new Model();

}}
