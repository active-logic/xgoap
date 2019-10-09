namespace Activ.GOAP{
public class BakerAI : GameAI<Baker>, Baker.AI{

    int temperature = 0;
    public float bake;

    override public Goal<Baker> Goal()
    => new Goal<Baker>( x => x.state == Baker.Cooking.Cooked );

    override public Baker Model()
    => new Baker(this){ temperature=temperature, bake=bake };

    public void SetTemperature(int degrees)
    => temperature = degrees;

    public void Bake()
    => bake += temperature/2;

    public void ApplyEffect(object arg) => Effect(arg);

}}
