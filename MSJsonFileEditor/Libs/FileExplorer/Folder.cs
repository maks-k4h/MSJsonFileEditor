namespace MSJsonFileEditor.Libs.FileExplorer;

public class Folder : FilesystemComponent
{
    public List<Folder> Subfolders;
    public List<File>   Files;
    public Folder       Parent;

    public Folder(string path, Folder parent = null, string name = null) : base(path, name)
    {
        Parent = parent;
        Subfolders = new List<Folder>();
        Files = new List<File>();
    }
    
    public override void Open()
    {
        try
        {
            // adding folders
            Subfolders.Clear();
            foreach (var s in Directory.GetDirectories(Path))
            {
                Subfolders.Add(new Folder(s, this));
            }

            // adding files
            Files.Clear();
            foreach (var s in Directory.GetFiles(Path))
            {
                Files.Add(new File(s));
            }
        }
        catch (Exception e)
        {
            // ignored
        }
    }
}