using MSJsonFileEditor.Controllers;
using MSJsonFileEditor.Libs.FileExplorer;
using MSJsonFileEditor.Models;
using MSJsonFileEditor.Resources.Styles;
using MSJsonFileEditor.Views;

namespace MSJsonFileEditor;

public partial class RenameFilePage : ContentPage
{
    public RenameFilePage()
    {
        InitializeComponent();
        Content = GetPageContent();
    }

    private View GetPageContent()
    {
        var fileNameEntry = new Entry
        {
            FontSize = 18,
            Placeholder = "New file name (without extension)",
            HeightRequest = 35,
            WidthRequest = 300,
        };
        var button = new Button
        {
            FontSize = 18,
            Text = "Save",
            HeightRequest = 35,
            WidthRequest = 90,
            Margin = new Thickness(5, 0, 0, 0)
        };
        var content = new VerticalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions =LayoutOptions.Center,
            Children =
                { new HorizontalStackLayout { fileNameEntry, button } }
        };
        
        // adding a handler
        button.Clicked += (sender, args) => SaveName(fileNameEntry.Text ?? " ");

        return content;
    }

    private void GetHelpClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(HelpPage));
    }

    private void SaveName(string name)
    {
        JsonFileEditorModel.FileName = name + ".json";
        Shell.Current.GoToAsync("..");
    }
}