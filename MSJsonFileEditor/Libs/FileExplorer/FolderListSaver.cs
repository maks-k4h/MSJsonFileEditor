namespace MSJsonFileEditor.Libs.FileExplorer;

public class FolderListSaver
{
    private static readonly string Path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "MSJsonFileEditorFavorites.txt");

    public static List<Folder> Load()
    {
        var res = new List<Folder>();
        try
        {
            foreach (var path in System.IO.File.ReadLines(Path))
            {
                res.Add(new Folder(path));
            }
        }
        catch (Exception)
        {
            res.Clear();
        }

        return res;
    }

    public static void TryToSave(IEnumerable<Folder> folders)
    {
        var contents = "";
        try
        {
            foreach (var folder in folders)
            {
                contents += folder.Path + "\n";
            }
            System.IO.File.WriteAllText(Path, contents);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}