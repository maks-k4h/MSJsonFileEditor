namespace MSJsonFileEditor.Libs.Meteorites.MeteoriteSerialization;

public class SerializationContext
{
    private SerializationStrategy _strategy;
    
    public enum Strategy
    {
        Json,
    }

    public void SetStrategy(Strategy strategy)
    {
        _strategy = strategy switch
        {
            Strategy.Json => new JsonStrategy(),
            _ => throw new NotImplementedException()
        };
    }

    public void Serialize(string path, List<MeteoriteObservation> observations)
    {
        _strategy.Serialize(path, observations);
    }

    public List<MeteoriteObservation> Deserialize(string path)
    {
        return _strategy.Deserialize(path);
    }
}