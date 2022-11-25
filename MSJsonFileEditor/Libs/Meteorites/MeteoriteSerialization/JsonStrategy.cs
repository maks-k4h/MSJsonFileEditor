using System.Text.Json;

namespace MSJsonFileEditor.Libs.Meteorites.MeteoriteSerialization;

public class JsonStrategy : SerializationStrategy
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        IncludeFields = true,
    };
    
    public override List<MeteoriteObservation> Deserialize(string path)
    {
        using FileStream fs = new FileStream(path, FileMode.Open);
        return JsonSerializer.Deserialize<List<MeteoriteObservation>>(fs, _jsonOptions);
    }

    public override async Task Serialize(string path, List<MeteoriteObservation> observations)
    {
        await using var fs = new FileStream(path, FileMode.Create);
        await JsonSerializer.SerializeAsync(fs, observations, _jsonOptions);
    }
}