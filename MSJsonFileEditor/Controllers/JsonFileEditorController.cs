using System.Text.Json;
using MSJsonFileEditor.Models;

namespace MSJsonFileEditor.Controllers;

public class JsonFileEditorController
{
    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        WriteIndented = true,
        IncludeFields = true
    };
    
    private JsonFileEditorModel _model;

    public JsonFileEditorController()
    {
        _model = new JsonFileEditorModel();

        try
        {
            Load();
        }
        catch (Exception)
        {
            _model.Observations.Add(new MeteoriteObservation());
        }
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

    public async void Save()
    {
        var path = Path.Combine(JsonFileEditorModel.CurrentFolderPath, JsonFileEditorModel.FileName);

        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            await JsonSerializer.SerializeAsync(fs, _model.Observations, _jsonOptions);
        }
    }

    public void Load()
    {
        var path = Path.Combine(JsonFileEditorModel.CurrentFolderPath, JsonFileEditorModel.FileName);
        using (FileStream fs = new FileStream(path, FileMode.Open))
        {
            _model.Observations = JsonSerializer.Deserialize<List<MeteoriteObservation>>(fs, _jsonOptions);
        }
    }
}