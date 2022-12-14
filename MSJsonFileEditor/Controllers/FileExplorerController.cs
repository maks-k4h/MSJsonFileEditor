using MSJsonFileEditor.Libs.FileExplorer;

namespace MSJsonFileEditor.Controllers;

public class FileExplorerController
{
    public readonly List<Folder> Default;
    public readonly List<Folder> Starred;
    public event EventHandler DefaultUpdated;
    public event EventHandler StarredUpdated;

    public Folder CurrentFolder;
    
    private Folder _returnedFromFolder;
    public event EventHandler CurrentFolderUpdated;

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
        _returnedFromFolder = null;
        CurrentFolderUpdated?.Invoke(this, null!);
    }

    public void ReturnForward()
    {
        CurrentFolder = _returnedFromFolder?? CurrentFolder;
        CurrentFolderUpdated?.Invoke(this, null!);
    }

    public void ReturnBack()
    {
        _returnedFromFolder = CurrentFolder;
        if (CurrentFolder.Parent != null)
            CurrentFolder = CurrentFolder.Parent;
        CurrentFolderUpdated?.Invoke(this, null!);
    }

    public void Update()
    {
        CurrentFolder.Open();
        CurrentFolderUpdated?.Invoke(this, null!);
    }

    public bool IsStarred()
    {
        return CurrentFolder != null && Starred.Contains(CurrentFolder);
    }

    public void SetStar(bool b)
    {
        if (b)
        {
            if (!Starred.Contains(CurrentFolder))
                Starred.Add(CurrentFolder);
        }
        else
        {
            if (Starred.Contains(CurrentFolder))
                Starred.Remove(CurrentFolder);
        }
        FolderListSaver.TryToSave(Starred);
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
        var paths = FolderListSaver.Load();
        foreach (var path in paths)
        {
            AddStarredElement(path);
        }
    }
}