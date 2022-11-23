using MSJsonFileEditor.Controllers;
using MSJsonFileEditor.Libs.FileExplorer;
using MSJsonFileEditor.Resources.Styles;

namespace MSJsonFileEditor;

public partial class MainPage : ContentPage
{
    private static int ViewerColumnsNumber = 5;
    
    private FileExplorerController _controller;
    
    private Label _statusLabel;
    private ScrollView _filesystemViewerWrapper;
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
                new ColumnDefinition{Width = new GridLength(400)},
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
            FontSize = 30,
        };
        var forwardButton = new Button
        {
            BackgroundColor = Colors.Transparent,
            TextColor = Colors.Black,
            FontSize = 30,
            Text = ">"
        };

        _starButton = new ImageButton()
        {
            HeightRequest = 20,
            WidthRequest = 20,
            Scale = .6,
            BackgroundColor = Colors.Transparent,
        };
        
        SetStar(false);
        _starButton.Clicked += (sender, args) => SetStar(DateTime.Now.Second % 2 == 0 ? true : false); 

        backwardButton.Clicked  += (sender, args) => _controller.ReturnBack();
        forwardButton.Clicked   += (sender, args) => _controller.ReturnForward(); 
        
        var stack = new HorizontalStackLayout
        {
            Padding = 5,
            Spacing = 5,
            Children = { backwardButton, forwardButton, _starButton }
        };

        return stack;
    }

    private View GetQuickAccessPanel()
    {
        var stack = new VerticalStackLayout()
        {
            Padding = 15,
            Spacing = 5,
            Children =
            {
                new Label { FontSize = 21, Text = "Quick access" },
                GetThisComputerAccessElement(),
                new Label { FontSize = 21, Text = "Favorites" },
                GetFavoritesAccessElement()
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
            grid.AddRowDefinition(new RowDefinition(160));
        }
        
        int p = 0;
        // creating files
        foreach (var v in _controller.CurrentFolder.Files)
        {
            var imagePath = "document.png";
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
    }

    private View GetFilesystemComponentView(FilesystemComponent component, string imagePath)
    {
        var image = new ImageButton
        {
            HorizontalOptions = LayoutOptions.Center,
            Source = imagePath,
            WidthRequest = 80,
            HeightRequest = 80,
        };

        if (component is Folder)
            image.Clicked += (sender, args) => _controller.GoTo((Folder)component); 

        var label = new Label
        {
            Text = component.Name, 
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
        if (b)
        {
            _starButton.Source = "star_filled.png";
        }
        else
        {
            _starButton.Source = "star_empty.png";
        }
    }
}