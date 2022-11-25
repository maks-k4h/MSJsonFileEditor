namespace MSJsonFileEditor.Libs.FileExplorer;

public class Folder : FilesystemComponent
{
    public readonly List<FilesystemComponent> Children;

    public Folder(string path, Folder parent = null, string name = null) : base(path, name, parent)
    {
        Children = new List<FilesystemComponent>();
    }

    public override bool IsLeaf()
    {
        return false;
    }

    public void Open()
    {
        Children.Clear();
        try
        {
            // adding folders
            foreach (var s in Directory.GetDirectories(Path))
            {
                Children.Add(new Folder(s, this));
            }

            // adding files
            foreach (var s in Directory.GetFiles(Path))
            {
                Children.Add(new File(this, s));
            }
        }
        catch (Exception)
        {
            // ignored
        }
    }
}