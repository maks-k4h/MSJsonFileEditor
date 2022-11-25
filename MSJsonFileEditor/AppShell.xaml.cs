using MSJsonFileEditor.Views;

namespace MSJsonFileEditor;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        Routing.RegisterRoute(nameof(JsonFileEditorPage), typeof(JsonFileEditorPage));
        Routing.RegisterRoute(nameof(RenameFilePage), typeof(RenameFilePage));
        Routing.RegisterRoute(nameof(HelpPage), typeof(HelpPage));
    }
}