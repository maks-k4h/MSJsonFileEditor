using MSJsonFileEditor.Libs.Meteorites;

namespace MSJsonFileEditor.Models;

public class JsonFileEditorModel
{
    // buffer for convenient path exchanging

    public static string CurrentFolderPath = "";
    public static string FileName = "";

    public List<MeteoriteObservation> Observations;

    public JsonFileEditorModel()
    {
        Observations = new List<MeteoriteObservation>();
    }
}