using Activ.GOAP;

namespace Activ.GOAP.Test{
public class BakerAI : GameAI<Baker>, Baker.AI{

    int temperature = 0;
    public float bake;

    override public Goal<Baker>[] Goals()
    => new Goal<Baker>[]
    { (x => x.state == Baker.Cooking.Cooked, null) };

    override public Baker Model()
    => new Baker(this){ temperature = temperature, bake = bake };

    public void SetTemperature(int degrees)
    => temperature = degrees;

    public void Bake()
    => bake += temperature/2;

}}
