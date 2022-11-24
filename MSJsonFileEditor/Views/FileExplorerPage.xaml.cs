using MSJsonFileEditor.Controllers;
using MSJsonFileEditor.Libs.FileExplorer;
using MSJsonFileEditor.Models;
using MSJsonFileEditor.Resources.Styles;
using MSJsonFileEditor.Views;
using File = System.IO.File;

namespace MSJsonFileEditor;

public partial class MainPage : ContentPage
{
    private static int ViewerColumnsNumber  = 5;
    private static int MaxFileNameLength    = 40;
    
    private FileExplorerController _controller;
    
    private Label _statusLabel;
    private ScrollView _filesystemViewerWrapper;
    private ScrollView _starredFoldersWrapper;
    private ImageButton _starButton;

    public MainPage()
    {
        InitializeComponent();
        _controller = new FileExplorerController();
        Content = GetMainPage();
        UpdateViewer();

        _controller.CurrentFolderUpdated += (sender, args) => UpdateViewer();
        ShowStatus();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
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

        var sideBar = GetSideBar();
        _filesystemViewerWrapper = new ScrollView { };
        
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
            BackgroundColor = Colors.Transparent,
            TextColor = Colors.Black,
            Text = "<",
            FontSize = 20,
        };
        var forwardButton = new Button
        {
            BackgroundColor = Colors.Transparent,
            TextColor = Colors.Black,
            FontSize = 20,
            Text = ">"
        };

        _starButton = new ImageButton()
        {
            HeightRequest = 15,
            WidthRequest = 15,
            Scale = .4,
            BackgroundColor = Colors.Transparent,
        };

        var plusButton = new ImageButton
        {
            HeightRequest = 15,
            WidthRequest = 15,
            Scale = .415,
            Margin = new Thickness(0,2,0,0),
            Source = "plus.png",
            BackgroundColor = Colors.Transparent,
        };
        
        _starButton.Clicked += (sender, args) => SetStar(!_controller.IsStarred()); ;
        plusButton.Clicked += CreateClicked;
        
        backwardButton.Clicked  += (sender, args) => _controller.ReturnBack();
        forwardButton.Clicked   += (sender, args) => _controller.ReturnForward(); 
        
        var stack = new HorizontalStackLayout
        {
            Padding = 5,
            Spacing = 5,
            Children = { backwardButton, forwardButton, plusButton, _starButton }
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
            Children =
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
        var stack = new VerticalStackLayout { };

        foreach (var v in _controller.Default)
        {
            stack.Add(GetSidebarQuickAccessItem(v.Name, (sender, args) => _controller.GoTo(v)));
        }
        
        return stack;
    }

    private View GetFavoritesAccessElement()
    {
        var stack = new VerticalStackLayout { };

        foreach (var v in _controller.Starred)
        {
            stack.Add(GetSidebarQuickAccessItem(v.Name, (sender, args) => _controller.GoTo(v)));
        }
        
        return stack;
    }

    private View GetSidebarQuickAccessItem(string title, EventHandler handler)
    {
        var button = new Button
        {
            Text = title,
            TextColor = AppColors.Primary,
            FontSize = 18,
            HorizontalOptions = LayoutOptions.Start,
            BackgroundColor = Colors.Transparent,
        };
        
        button.Clicked += handler;

        return button;
    }

    private View GetStatusLabel()
    {
        _statusLabel = new Label { 
            Text = " ",
            TextColor = Colors.Red 
        };
        return _statusLabel;
    }

    private void UpdateViewer()
    {
        var grid = new Grid
        {
            ColumnSpacing = 20,
            RowSpacing = 20,
            Padding = 15,
        };

        for (int i = 0; i < ViewerColumnsNumber; ++i)
        {
            grid.AddColumnDefinition(new ColumnDefinition(110));
        }
        
        var rows = 1 + (_controller.CurrentFolder.Files.Count + _controller.CurrentFolder.Subfolders.Count)
                   / ViewerColumnsNumber;
        for (int i = 0; i < rows; ++i)
        {
            grid.AddRowDefinition(new RowDefinition(150));
        }
        
        int p = 0;
        // creating files
        foreach (var v in _controller.CurrentFolder.Files)
        {
            var imagePath = "document2.png";    // default document icon
            
            if (v.Path.EndsWith(".json"))
                imagePath = "json.png";
            
            grid.Add(GetFilesystemComponentView(v, imagePath), p % ViewerColumnsNumber, p / ViewerColumnsNumber);
            ++p;
        }
        
        // creating folder
        foreach (var v in _controller.CurrentFolder.Subfolders)
        {
            grid.Add(GetFilesystemComponentView(v, "folder.png"), p % ViewerColumnsNumber, p / ViewerColumnsNumber);
            ++p;
        }

        _filesystemViewerWrapper.Content = grid;

        Title = _controller.CurrentFolder.Path;
        
        SetStarIcon(_controller.IsStarred());
    }

    private View GetFilesystemComponentView(FilesystemComponent component, string imagePath)
    {
        var image = new ImageButton
        {
            HorizontalOptions = LayoutOptions.Center,
            Source = imagePath,
            WidthRequest = 70,
            HeightRequest = 70,
        };

        if (component is Folder)
            image.Clicked += (sender, args) => _controller.GoTo((Folder)component);
        if (component.Path.EndsWith(".json"))
            image.Clicked += (sender, args) =>
            {
                JsonFileEditorModel.CurrentFolderPath = _controller.CurrentFolder.Path;
                JsonFileEditorModel.FileName = component.Name;
                Shell.Current.GoToAsync(nameof(JsonFileEditorPage));
            }; 

        // cropping name if needed
        var name = component.Name.Length > MaxFileNameLength 
            ? component.Name[..MaxFileNameLength] + "..." 
            : component.Name;
        
        var label = new Label
        {
            Text = name, 
            FontSize = 16,
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
        JsonFileEditorModel.CurrentFolderPath = _controller.CurrentFolder.Path;
        JsonFileEditorModel.FileName = _controller.CurrentFolder.Path;
        Shell.Current.GoToAsync(nameof(JsonFileEditorPage));
    }

    private void GetHelpClicked(object sender, EventArgs e)
    {
        
    }
}