namespace MSJsonFileEditor.Libs.FileExplorer;

public class File : FilesystemComponent
{
    public File(Folder parent, string path, string name = null) : base(path, name, parent)
    {
    }

    public override bool IsLeaf()
    {
        return true;
    }
}