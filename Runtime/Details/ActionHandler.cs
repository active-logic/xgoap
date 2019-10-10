namespace Activ.GOAP{
public interface ActionHandler<X>{

    void Effect<T>(X action, GameAI<T> client) where T : Agent;

}}
