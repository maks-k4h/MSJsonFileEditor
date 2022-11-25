namespace MSJsonFileEditor.Libs.Meteorites.MeteoriteSerialization;

public abstract class SerializationStrategy
{
    public abstract List<MeteoriteObservation> Deserialize(string path);
    public abstract Task Serialize(string path, List<MeteoriteObservation> observations);
}