namespace MSJsonFileEditor.Libs.FileExplorer;

public class File : FilesystemComponent
{
    public File(string path, string name = null) : base(path, name)
    {
    }

    public override void Open()
    {
        throw new NotImplementedException();
    }
}