namespace MSJsonFileEditor.Libs.FileExplorer;

public abstract class FilesystemComponent
{
    public string Path;
    public string Name;

    protected FilesystemComponent(string path, string name = null)
    {
        Path = path;
        Name = name ?? System.IO.Path.GetFileName(path);
    }
    public abstract void Open();

}