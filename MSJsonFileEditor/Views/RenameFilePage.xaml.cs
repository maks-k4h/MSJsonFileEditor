using MSJsonFileEditor.Models;

namespace MSJsonFileEditor.Views;

public partial class RenameFilePage
{
    public RenameFilePage()
    {
        InitializeComponent();
        Content = GetPageContent(); // rendering
    }

    private View GetPageContent()
    {
        var fileNameEntry = new Entry
        {
            FontSize        = 18,
            Placeholder     = "New file name (without extension)",
            WidthRequest    = 300,
            HeightRequest   = 35,
        };
        var button = new Button
        {
            Text            = "Save",
            Margin          = new Thickness(5, 0, 0, 0),
            FontSize        = 18,
            WidthRequest    = 90,
            HeightRequest   = 35,
        };
        var content = new VerticalStackLayout
        {
            VerticalOptions     = LayoutOptions.Center,
            HorizontalOptions   = LayoutOptions.Center,
            Children =
            {
                new HorizontalStackLayout { fileNameEntry, button }
            }
        };
        
        // adding a handler
        button.Clicked += (_, _) => SaveName(fileNameEntry.Text ?? " ");

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