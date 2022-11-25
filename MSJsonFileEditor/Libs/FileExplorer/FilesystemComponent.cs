namespace MSJsonFileEditor.Libs.FileExplorer;

public abstract class FilesystemComponent
{
    public readonly Folder Parent;
    
    public readonly string Path;
    public readonly string Name;

    protected FilesystemComponent(string path, string name = null, Folder parent = null)
    {
        Path = path;
        Name = name ?? System.IO.Path.GetFileName(path);
        Parent = parent;
    }

    public abstract bool IsLeaf();
}