namespace MSJsonFileEditor.Libs.FileExplorer;

public static class DefaultFolders
{
    public static Folder Root       = new Folder("/", null, "Root");
    public static Folder Documents  = new Folder(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
}