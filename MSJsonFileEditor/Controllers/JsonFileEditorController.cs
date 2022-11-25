using MSJsonFileEditor.Libs.Meteorites;
using MSJsonFileEditor.Libs.Meteorites.MeteoriteSerialization;
using MSJsonFileEditor.Models;

namespace MSJsonFileEditor.Controllers;

public class JsonFileEditorController
{
    private readonly SerializationContext _serializationContext;
    private readonly JsonFileEditorModel _model;

    public JsonFileEditorController()
    {
        _serializationContext = new SerializationContext();
        _serializationContext.SetStrategy(SerializationContext.Strategy.Json);
        _model = new JsonFileEditorModel();
    }

    public List<MeteoriteObservation> GetObservations()
    {
        return _model.Observations;
    }

    public void AddObservation()
    {
        _model.Observations.Insert(0, new MeteoriteObservation());
    }

    public bool RemoveObservation(MeteoriteObservation observation)
    {
        return _model.Observations.Remove(observation);
    }

    public void SaveObservations()
    {
        var path = Path.Combine(JsonFileEditorModel.CurrentFolderPath, JsonFileEditorModel.FileName);
        
        _serializationContext.Serialize(path, GetObservations());
    }

    public void LoadObservations()
    {
        // nothing to load
        if (JsonFileEditorModel.FileName.Trim().Length == 0)
            return;

        // loading form file
        var path = Path.Combine(JsonFileEditorModel.CurrentFolderPath, JsonFileEditorModel.FileName);
        _model.Observations = _serializationContext.Deserialize(path);
    }
}