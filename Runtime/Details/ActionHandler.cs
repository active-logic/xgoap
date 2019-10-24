namespace Activ.GOAP{
public interface ActionHandler<X, T> where T : Agent, new(){

    void Effect(X action, GameAI<T> client);

}}
