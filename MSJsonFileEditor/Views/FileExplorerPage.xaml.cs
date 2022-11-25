using MSJsonFileEditor.Controllers;
using MSJsonFileEditor.Libs.FileExplorer;
using MSJsonFileEditor.Models;
using MSJsonFileEditor.Resources.Styles;

namespace MSJsonFileEditor.Views;

public partial class MainPage
{
    private const int MaxFileNameLength     = 40;
    private const int ViewerColumnsNumber   = 5;

    private readonly FileExplorerController _controller;
    
    private Label       _statusLabel;
    private ScrollView  _filesystemViewerWrapper;
    private ScrollView  _starredFoldersWrapper;
    private ImageButton _starButton;

    public MainPage()
    {
        InitializeComponent();
        
        // rendering page
        _controller = new FileExplorerController();
        Content = GetMainPage();
        UpdateViewer();
        
        _controller.CurrentFolderUpdated += (_, _) => UpdateViewer();
        ShowStatus();   // ok status (empty message)
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        // updating controller -> rendering updates
        _controller.Update(); 
    }

    private View GetMainPage()
    {
        // the grid is a backbone
        var grid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition{Height = new GridLength(1, GridUnitType.Star)}
            },
            ColumnDefinitions =
            {
                new ColumnDefinition{Width = new GridLength(230)},
                new ColumnDefinition{Width = new GridLength(1, GridUnitType.Star)},
                new ColumnDefinition{Width = new GridLength(0)},
            }
        };
        
        // side bar
        var sideBar = GetSideBar();
        // viewer
        _filesystemViewerWrapper = new ScrollView();
        
        grid.Add(sideBar);
        grid.Add(_filesystemViewerWrapper, 1);

        return grid;
    }

    private View GetSideBar()
    {
        var grid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = new GridLength(50) },
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(50) }
            },
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
            },
            BackgroundColor = AppColors.Gray,
        };
        
        grid.Add(GetControlBar());
        grid.Add(GetQuickAccessPanel(), 0, 1);
        grid.Add(GetStatusLabel(), 0, 2);

        return grid;
    }
    

    private View GetControlBar()
    {
        var backwardButton = new Button
        {
            Text            = "<",
            FontSize        = 20,
            TextColor       = Colors.Black,
            BackgroundColor = Colors.Transparent,
        };
        var forwardButton = new Button
        {
            Text            = ">",
            FontSize        = 20,
            TextColor       = Colors.Black,
            BackgroundColor = Colors.Transparent,
        };

        _starButton = new ImageButton()
        {
            Scale           = .4,
            WidthRequest    = 15,
            HeightRequest   = 15,
            BackgroundColor = Colors.Transparent,
        };

        var plusButton = new ImageButton
        {
            Scale           = .415,
            Margin          = new Thickness(0,2,0,0),
            Source          = "plus.png",
            WidthRequest    = 15,
            HeightRequest   = 15,
            BackgroundColor = Colors.Transparent,
        };

        plusButton.Clicked      += CreateClicked;
        _starButton.Clicked     += StarClicked;
        forwardButton.Clicked   += ReturnForwardClicked; 
        backwardButton.Clicked  += ReturnBackClicked;
        
        var stack = new HorizontalStackLayout
        {
            Padding         = 5,
            Spacing         = 5,
            Children        = { backwardButton, forwardButton, plusButton, _starButton }
        };

        return stack;
    }

    private View GetQuickAccessPanel()
    {
        _starredFoldersWrapper = new ScrollView { Content = GetFavoritesAccessElement() };
        
        var stack = new VerticalStackLayout()
        {
            Padding = 15,
            Spacing = 5,
            Children=
            {
                new Label { FontSize = 19, Text = "Quick access" },
                GetThisComputerAccessElement(),
                new Label { FontSize = 19, Text = "Favorites" },
                _starredFoldersWrapper
            }
        };
        return stack;
    }

    private View GetThisComputerAccessElement()
    {
        var stack = new VerticalStackLayout();

        foreach (var v in _controller.Default)
            stack.Add(GetSidebarQuickAccessItem(v.Name, (_, _) => _controller.GoTo(v)));

        return stack;
    }

    private View GetFavoritesAccessElement()
    {
        var stack = new VerticalStackLayout();

        foreach (var v in _controller.Starred)
            stack.Add(GetSidebarQuickAccessItem(v.Name, (_, _) => _controller.GoTo(v)));

        return stack;
    }

    private static View GetSidebarQuickAccessItem(string title, EventHandler handler)
    {
        var button = new Button
        {
            Text                = title,
            FontSize            = 18,
            TextColor           = AppColors.Primary,
            BackgroundColor     = Colors.Transparent,
            HorizontalOptions   = LayoutOptions.Start,
        };
        
        button.Clicked += handler;

        return button;
    }

    private View GetStatusLabel()
    {
        _statusLabel = new Label { 
            Text                = " ",
            TextColor           = Colors.Red,
            VerticalOptions     = LayoutOptions.Center,
            HorizontalOptions   = LayoutOptions.Center,
        };
        return _statusLabel;
    }

    private void UpdateViewer()
    {
        var grid = new Grid
        {
            Padding         = 15,
            RowSpacing      = 20,
            ColumnSpacing   = 20,
        };
        
        // calculating the needed number of rows
        var rows = 1 + _controller.CurrentFolder.Children.Count / ViewerColumnsNumber;
        
        for (int i = 0; i < rows; ++i) 
            grid.AddRowDefinition(new RowDefinition(150));
        
        for (int i = 0; i < ViewerColumnsNumber; ++i)
            grid.AddColumnDefinition(new ColumnDefinition(110));

        int p = 0;
        foreach (var v in _controller.CurrentFolder.Children)
        {
            if (v.IsLeaf())
            {
                grid.Add(GetFilesystemComponentView(v, GetImagePathFromFileName(v.Name)), 
                    p % ViewerColumnsNumber, 
                    p / ViewerColumnsNumber);
            }
            else
            {
                grid.Add(GetFilesystemComponentView(v, GetFolderImagePath()), 
                    p % ViewerColumnsNumber, 
                    p / ViewerColumnsNumber);
            }
            ++p;
        }

        // rendering
        _filesystemViewerWrapper.Content = grid;
        Title = _controller.CurrentFolder.Path;
        SetStarIcon(_controller.IsStarred());
        ShowStatus();
    }

    private static string GetFolderImagePath()
    {
        return "folder.png";
    }

    private static string GetImagePathFromFileName(string filename)
    {
        if (filename.EndsWith(".json"))
            return "json_file.png";
        if (filename.EndsWith(".pdf"))
            return "pdf_file.png";
        if (filename.EndsWith(".jpg") || filename.EndsWith(".jpeg") || filename.EndsWith(".png"))
            return "image_file.png";
        
        return "default_file.png";
    }

    private View GetFilesystemComponentView(FilesystemComponent component, string imagePath)
    {
        var image = new ImageButton
        {
            Source              = imagePath,
            WidthRequest        = 70,
            HeightRequest       = 70,
            HorizontalOptions   = LayoutOptions.Center,
        };

        SetFilesystemComponentClickedHandler(image, component);

        // cropping name if needed
        var name = component.Name.Length > MaxFileNameLength 
            ? component.Name[..MaxFileNameLength] + "..." 
            : component.Name;
        
        var label = new Label
        {
            Text                    = name, 
            FontSize                = 16,
            HorizontalTextAlignment = TextAlignment.Center
        };
        
        var stack = new VerticalStackLayout
        {
            Spacing = 3,
            Children =
            {
                image,
                label
            }
        };
        return stack;
    }

    private void SetFilesystemComponentClickedHandler(ImageButton image, FilesystemComponent component)
    {
        if (component is Folder)
            image.Clicked += (_, _) => _controller.GoTo((Folder)component);
        else if (component.Path.EndsWith(".json"))
            image.Clicked += (_, _) =>
            {
                JsonFileEditorModel.CurrentFolderPath = _controller.CurrentFolder.Path;
                JsonFileEditorModel.FileName = component.Name;
                Shell.Current.GoToAsync(nameof(JsonFileEditorPage));
            };
        else
            image.Clicked += (_, _) => ShowStatus("Only JSON files can be opened!");
    }

    private void ShowStatus(string s = "")
    {
        _statusLabel.Text = s.Trim().Length > 0 ? s : " ";
    }

    private void SetStar(bool b)
    {
        _controller.SetStar(!_controller.IsStarred());
        _starredFoldersWrapper.Content = GetFavoritesAccessElement();
        
        SetStarIcon(b);
    }

    private void SetStarIcon(bool b)
    {
        _starButton.Source = b ? "star_filled.png" : "star_empty.png";
    }

    private void CreateClicked(object sender, EventArgs e)
    {
        JsonFileEditorModel.FileName            = "";
        JsonFileEditorModel.CurrentFolderPath   = _controller.CurrentFolder.Path;
        
        Shell.Current.GoToAsync(nameof(JsonFileEditorPage));
    }

    private void ReturnBackClicked(object sender, EventArgs e)
    {
        _controller.ReturnBack();
    }

    private void ReturnForwardClicked(object sender, EventArgs e)
    {
        _controller.ReturnForward();
    }

    private void StarClicked(object sender, EventArgs e)
    {
        SetStar(!_controller.IsStarred());
    }

    private void GetHelpClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(HelpPage));
    }
}