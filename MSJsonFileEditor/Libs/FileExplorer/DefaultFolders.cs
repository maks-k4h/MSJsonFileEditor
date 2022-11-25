namespace MSJsonFileEditor.Libs.FileExplorer;

public static class DefaultFolders
{
    public static readonly Folder Root       = new Folder("/", null, "Root");
    public static readonly Folder Documents  = new Folder(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
}