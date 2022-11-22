namespace MSJsonFileEditor.Libs.FileExplorer;

public class Folder : FilesystemComponent
{
    public Folder(string path)
    {
        Path = path;
    }
    
    public override void Open()
    {
        throw new NotImplementedException();
    }
}