namespace Activ.GOAP{
public interface ActionHandler{

    void Effect<T>(object action, GameAI<T> client);

}}
