using System.Globalization;
using MSJsonFileEditor.Controllers;
using MSJsonFileEditor.Models;
using MSJsonFileEditor.Resources.Styles;

namespace MSJsonFileEditor.Views;

public partial class JsonFileEditorPage : ContentPage
{
    private const int RemoveRowButtonWidth          = 50;
    private const int LabelFieldWidth               = 200;
    private const int ClassificationFieldWidth      = 150;
    private const int LongitudeFieldWidth           = 150;
    private const int LatitudeFieldWidth            = 150;
    private const int DiameterFieldWidth            = 150;
    private const int WeightFieldWidth              = 150;
    private const int ChondruleFractionFieldWidth   = 150;

    private const int RowWidth = RemoveRowButtonWidth + LabelFieldWidth + ClassificationFieldWidth
                                 + LongitudeFieldWidth + LatitudeFieldWidth + DiameterFieldWidth + WeightFieldWidth
                                 + ChondruleFractionFieldWidth;
    

    private Label _statusLabel;
    private ScrollView _editorViewStackWrapper;
    private List<IView> _editorElements;

    private JsonFileEditorController _controller;

    public JsonFileEditorPage()
    {
        InitializeComponent();

        _editorElements = new List<IView>();
        _controller = new JsonFileEditorController();
       Content =  GetPage();
    }

    protected override void OnAppearing()
    {
        try
        {
            base.OnAppearing();
            ShowOkStatus();
            UpdateEditor();
        }
        catch (Exception e)
        {
            ShowErrorStatus(e.Message);
        }
    }

    private View GetPage()
    {
        var grid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition(new GridLength(1, GridUnitType.Star)),
                new RowDefinition(new GridLength(35)),
            },
            ColumnDefinitions =
            {
                new ColumnDefinition(new GridLength(1, GridUnitType.Star))
            }
        };

        _statusLabel = GetStatusLabel();

        var editorView = GetEditor();
        _editorViewStackWrapper = new ScrollView { Content = editorView };
        
        grid.Add(_editorViewStackWrapper);
        grid.Add(_statusLabel,0,1);

        return grid;
    }

    private VerticalStackLayout GetEditor()
    {
        var stack = new VerticalStackLayout
        {
            WidthRequest = RowWidth,
        };
        return stack;
    }

    private void UpdateEditor()
    {
        _editorElements = new List<IView>();
        var editorView = GetEditor();

        foreach (var observation in _controller.GetObservations())
        {
            _editorElements.Add(GetEditorElement(observation));
        }
        
        editorView.Add(GetColumnHeadersElement());

        foreach (var element in _editorElements)
        {
            editorView.Add(element);
        }

        _editorViewStackWrapper.Content = editorView;
    }

    private IView GetColumnHeadersElement()
    {
        // add row button
        var button = new Button
        {
            Text = "+",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            WidthRequest = RemoveRowButtonWidth,
            FontSize = 25,
            TextColor = Colors.Green,
            BorderColor = Colors.Transparent,
            BackgroundColor = Colors.Transparent,
        };
        button.Clicked += (sender, _) =>
        {
            _controller.AddObservation();
            UpdateEditor();
            ShowOkStatus();
        };
        
        // label
        var label = GetColumnTitleElement("Label", LabelFieldWidth);
        
        // classification
        var classification = GetColumnTitleElement("Classification", ClassificationFieldWidth);
        
        // longitude
        var longitude = GetColumnTitleElement("Longitude", LongitudeFieldWidth);
        
        // latitude
        var latitude = GetColumnTitleElement("Latitude", LatitudeFieldWidth);
        
        // diameter
        var diameter = GetColumnTitleElement("Diameter", DiameterFieldWidth);
        
        // weight
        var weight = GetColumnTitleElement("Weight", WeightFieldWidth);
        
        // chondrule fraction
        var chondrule = GetColumnTitleElement("Chond. fract.", ChondruleFractionFieldWidth);
        
        var stack = new HorizontalStackLayout { WidthRequest = RowWidth, };
        stack.Add(button);
        stack.Add(label);
        stack.Add(classification);
        stack.Add(longitude);
        stack.Add(latitude);
        stack.Add(diameter);
        stack.Add(weight);
        stack.Add(chondrule);

        return stack;
    }

    private View GetColumnTitleElement(string text, int width)
    {
        var label = new Label
        {
            VerticalTextAlignment = TextAlignment.Center,
            FontSize = 17,
            Padding = 3,
            WidthRequest = width,
            Text = text,
        };
        return label;
    }

    private View GetEditorElement(MeteoriteObservation observation)
    {
        // remove row button
        var removeButton = new Button
        {
            WidthRequest = RemoveRowButtonWidth,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Text = "—",
            FontSize = 25,
            TextColor = Colors.Red,
            BorderColor = Colors.Transparent,
            BackgroundColor = Colors.Transparent,
        };
        
        // label
        var labelEntry = new Entry
        {
            WidthRequest = LabelFieldWidth,
            Text = observation.Label,
        };

        // classification
        var classEntry = new Entry
        {
            WidthRequest = ClassificationFieldWidth,
            Text = observation.Classification,
        };

        // longitude
        var longitudeEntry = new Entry
        {
            WidthRequest = LongitudeFieldWidth,
            Text = observation.Longitude.ToString(CultureInfo.InvariantCulture),
        };

        // latitude
        var latitudeEntry = new Entry
        {
            WidthRequest = LatitudeFieldWidth,
            Text = observation.Latitude.ToString(CultureInfo.InvariantCulture),
        };

        // diameter
        var diameterEntry = new Entry
        {
            WidthRequest = DiameterFieldWidth,
            Text = observation.Diameter.ToString(CultureInfo.InvariantCulture),
        };

        // weight
        var weigthEntry = new Entry
        {
            WidthRequest = WeightFieldWidth,
            Text = observation.Weight.ToString(CultureInfo.InvariantCulture),
        };

        // chondrule fraction
        var chondruleFractionEntry = new Entry
        {
            WidthRequest = ChondruleFractionFieldWidth,
            Text = observation.ChondruleFraction.ToString(CultureInfo.InvariantCulture),
        };
        
        // setting logic
        SetRowHandlers(observation, removeButton, labelEntry, classEntry, longitudeEntry, latitudeEntry, 
            diameterEntry, weigthEntry, chondruleFractionEntry);

        // wrapper
        var horizontalStack = new HorizontalStackLayout
        {
            removeButton,
            labelEntry,
            classEntry,
            longitudeEntry,
            latitudeEntry,
            diameterEntry,
            weigthEntry,
            chondruleFractionEntry,
        };

        return horizontalStack;
    }

    private void SetRowHandlers(MeteoriteObservation observation, Button remove, Entry label, Entry classification, 
        Entry longitude, Entry latitude, Entry diameter, Entry weight, Entry chondrule)
    {
        // remove button
        remove.Clicked += (sender, args) =>
        {
            _controller.RemoveObservation(observation);
            UpdateEditor();
            ShowOkStatus();
        };

        // label entry
        label.Completed += (sender, args) =>
        {
            try
            {
                observation.Label = label.Text ?? "";
                ShowOkStatus();
            }
            catch (Exception e)
            {
                ShowErrorStatus(e.Message);
                label.Text = observation.Label;
            }
        };
        
        // classification entry
        classification.Completed += (sender, args) =>
        {
            try
            {
                observation.Classification = classification.Text ?? "";
                ShowOkStatus();
            }
            catch (Exception e)
            {
                ShowErrorStatus(e.Message);
                classification.Text = observation.Classification;
            }
        };
        
        // longitude entry
        longitude.Completed += (sender, args) =>
        {
            try
            {
                observation.Longitude = double.Parse(longitude.Text ?? "");
                ShowOkStatus();
            }
            catch (Exception e)
            {
                ShowErrorStatus(e.Message);
                longitude.Text = observation.Longitude.ToString();
            }
        };
        
        // latitude entry
        latitude.Completed += (sender, args) =>
        {
            try
            {
                observation.Latitude = double.Parse(latitude.Text ?? "");
                ShowOkStatus();
            }
            catch (Exception e)
            {
                ShowErrorStatus(e.Message);
                latitude.Text = observation.Latitude.ToString();
            }
        };
        
        // diameter entry
        diameter.Completed += (sender, args) =>
        {
            try
            {
                observation.Diameter = double.Parse(diameter.Text ?? "");
                ShowOkStatus();
            }
            catch (Exception e)
            {
                ShowErrorStatus(e.Message);
                diameter.Text = observation.Diameter.ToString();
            }
        };
        
        // weight entry
        weight.Completed += (sender, args) =>
        {
            try
            {
                observation.Weight = double.Parse(weight.Text ?? "");
                ShowOkStatus();
            }
            catch (Exception e)
            {
                ShowErrorStatus(e.Message);
                weight.Text = observation.Weight.ToString();
            }
        };
        
        // chondrule fraction
        chondrule.Completed += (sender, args) =>
        {
            try
            {
                observation.ChondruleFraction = double.Parse(chondrule.Text ?? "");
                ShowOkStatus();
            }
            catch (Exception e)
            {
                ShowErrorStatus(e.Message);
                chondrule.Text = observation.ChondruleFraction.ToString();
            }
        };
    }

    private Label GetStatusLabel()
    {
        var label = new Label
        {
            Padding = new Thickness(10, 0),
            FontSize = 18,
            TextColor = Colors.White,
            HorizontalTextAlignment = TextAlignment.End,
            VerticalTextAlignment = TextAlignment.Center,
        };
        return label;
    }
    
    private void GetHelpClicked(object sender, EventArgs e)
    {
        
    }
    
    private void SaveClicked(object sender, EventArgs e)
    {
        _controller.Save();
        ShowOkStatus("Saved!", "💾");
    }
    
    private void RenameClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(RenameFilePage));
    }

    private void ShowOkStatus(string message = "So far, so good!", string sticker = "👌")
    {
        _statusLabel.BackgroundColor = AppColors.Secondary;
        _statusLabel.Text = message + " " + sticker;
    }
    
    private void ShowErrorStatus(string message = "Oops, something went wrong!", string sticker = "🙀")
    {
        _statusLabel.BackgroundColor = Colors.Red;
        _statusLabel.Text = message + " " + sticker;
    }
}