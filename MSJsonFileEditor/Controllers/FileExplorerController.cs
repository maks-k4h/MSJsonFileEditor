using MSJsonFileEditor.Libs.FileExplorer;

namespace MSJsonFileEditor.Controllers;

public class FileExplorerController
{
    public event EventHandler DefaultUpdated;
    public event EventHandler StarredUpdated;
    public List<FilesystemComponent> Default;
    public List<FilesystemComponent> Starred;

    public FileExplorerController()
    {
        Default = new List<FilesystemComponent>();
        Starred = new List<FilesystemComponent>();

        SetDefaultElements();
        SetStarredElements();
    }

    private void AddDefaultElement(FilesystemComponent component)
    {
        if (!Default.Contains(component))
        {
            Default.Add(component);
            DefaultUpdated?.Invoke(this, null!);
        }
    }

    private void AddStarredElement(FilesystemComponent component)
    {
        if (!Starred.Contains(component))
        {
            Starred.Add(component);
            StarredUpdated?.Invoke(this, null!);
        }
    }

    private void SetDefaultElements()
    {
        AddDefaultElement(new Folder("Desktop"));
        AddDefaultElement(new Folder("Documents"));
        AddDefaultElement(new Folder("Pictures"));
    }

    private void SetStarredElements()
    {
        AddStarredElement(new Folder("Machine Learning"));
        AddStarredElement(new Folder("Books Library of Books"));
        AddStarredElement(new Folder("Probability"));
        AddStarredElement(new Folder("Bandera Recipes"));
    }
}