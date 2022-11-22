using MSJsonFileEditor.Controllers;
using MSJsonFileEditor.Resources.Styles;

namespace MSJsonFileEditor;

public partial class MainPage : ContentPage
{
    private FileExplorerController _controller;

    private Label _statusLabel;
    
    public MainPage()
    {
        InitializeComponent();
        _controller = new FileExplorerController();

        Content = GetMainPage();
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
                new ColumnDefinition{Width = new GridLength(1, GridUnitType.Star)}
            }
        };
        
        grid.Add(GetSideBar());
        grid.Add(GetViewer(), 1);

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
            TextColor = AppColors.Primary,
            Text = "<",
            FontSize = 30,
        };
        var forwardButton = new Button
        {
            BackgroundColor = Colors.Transparent,
            TextColor = AppColors.Primary,
            FontSize = 30,
            Text = ">"
        };

        var starButton = new Button
        {
            BackgroundColor = Colors.Transparent,
            TextColor = AppColors.Primary,
            FontSize = 25,
            Text = "☆"
        };
        
        var stack = new HorizontalStackLayout
        {
            Padding = 5,
            Spacing = 5,
            Children = { backwardButton, forwardButton, starButton }
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
                new Label { FontSize = 24, Text = "This Computer" },
                GetThisComputerAccessElement(),
                new Label { FontSize = 24, Text = "Favorites" },
                GetFavoritesAccessElement()
            }
        };
        return stack;
    }

    private View GetViewer()
    {
        var grid = new Grid
        {
            BackgroundColor = Colors.White,
        };

        return grid;
    }

    private View GetThisComputerAccessElement()
    {
        var stack = new VerticalStackLayout { };

        foreach (var v in _controller.Default)
        {
            stack.Add(GetSidebarQuickAccessItem(v.Path));
        }
        
        return stack;
    }

    private View GetFavoritesAccessElement()
    {
        var stack = new VerticalStackLayout { };

        foreach (var v in _controller.Starred)
        {
            stack.Add(GetSidebarQuickAccessItem(v.Path));
        }
        
        return stack;
    }

    private View GetSidebarQuickAccessItem(string title)
    {
        var button = new Button
        {
            Text = title,
            TextColor = AppColors.Primary,
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Start,
            BackgroundColor = Colors.Transparent,
        };

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

    private void ShowStatus(string s = "")
    {
        _statusLabel.Text = s.Trim().Length > 0 ? s : " ";
    }
    
}