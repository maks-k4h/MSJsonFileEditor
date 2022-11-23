using MSJsonFileEditor.Libs.FileExplorer;

namespace MSJsonFileEditor.Controllers;

public class FileExplorerController
{
    public event EventHandler DefaultUpdated;
    public event EventHandler StarredUpdated;
    public List<Folder> Default;
    public List<Folder> Starred;

    public event EventHandler CurrentFolderUpdated;
    public Folder CurrentFolder;
    public Folder ReturnedFromFolder = null;

    public FileExplorerController()
    {
        Default = new List<Folder>();
        Starred = new List<Folder>();
        
        // folder by default
        GoTo(DefaultFolders.Root);

        SetDefaultElements();
        SetStarredElements();
    }

    public void GoTo(Folder folder)
    {
        folder.Open();
        CurrentFolder = folder;
        ReturnedFromFolder = null;
        CurrentFolderUpdated?.Invoke(this, null!);
    }

    public void ReturnForward()
    {
        CurrentFolder = ReturnedFromFolder?? CurrentFolder;
        CurrentFolderUpdated?.Invoke(this, null!);
    }

    public void ReturnBack()
    {
        ReturnedFromFolder = CurrentFolder;
        if (CurrentFolder.Parent != null)
            CurrentFolder = CurrentFolder.Parent;
        CurrentFolderUpdated?.Invoke(this, null!);
    }

    private void AddDefaultElement(Folder component)
    {
        if (!Default.Contains(component))
        {
            Default.Add(component);
            DefaultUpdated?.Invoke(this, null!);
        }
    }

    private void AddStarredElement(Folder component)
    {
        if (!Starred.Contains(component))
        {
            Starred.Add(component);
            StarredUpdated?.Invoke(this, null!);
        }
    }

    private void SetDefaultElements()
    {
        AddDefaultElement(DefaultFolders.Root);
        AddDefaultElement(DefaultFolders.Documents);
    }

    private void SetStarredElements()
    {
        // TODO ...
        AddStarredElement(new Folder("/Users/makskonevych/Desktop/Books Library of Books/Machine Learning"));
        AddStarredElement(new Folder("/Users/makskonevych/Documents/C#/Labs", null, "C# labs"));
        AddStarredElement(new Folder("/Users/makskonevych/Desktop/Books Library of Books/University/Probability"));
        AddStarredElement(new Folder("/Users/makskonevych/Downloads"));
    }
}