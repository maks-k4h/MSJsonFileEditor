using System.Globalization;
using MSJsonFileEditor.Controllers;
using MSJsonFileEditor.Libs.Meteorites;
using MSJsonFileEditor.Models;
using MSJsonFileEditor.Resources.Styles;

namespace MSJsonFileEditor.Views;

public partial class JsonFileEditorPage
{
    // widths of columns
    private const int RemoveRowButtonWidth          = 50;
    private const int LabelFieldWidth               = 200;
    private const int ClassificationFieldWidth      = 150;
    private const int LongitudeFieldWidth           = 150;
    private const int LatitudeFieldWidth            = 150;
    private const int DiameterFieldWidth            = 150;
    private const int WeightFieldWidth              = 150;
    private const int ChondruleFractionFieldWidth   = 150;
    
    private const int RowWidth = RemoveRowButtonWidth 
                                 + LabelFieldWidth 
                                 + ClassificationFieldWidth
                                 + LongitudeFieldWidth 
                                 + LatitudeFieldWidth 
                                 + DiameterFieldWidth 
                                 + WeightFieldWidth
                                 + ChondruleFractionFieldWidth;
    

    private Label       _statusLabel;
    private ScrollView  _editorViewStackWrapper;
    private List<IView> _editorElements;

    private readonly JsonFileEditorController _controller;

    public JsonFileEditorPage()
    {
        InitializeComponent();

        _editorElements = new List<IView>(); // todo: can be used for further optimization
        _controller = new JsonFileEditorController();
        Content = GetPage(); // rendering page
        
        try
        {
            _controller.LoadObservations();
            ShowOkStatus();
        }
        catch (Exception)
        {
            ShowErrorStatus("The file has been damaged.");
        }
        
        // rendering data view
        UpdateEditor();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Title = JsonFileEditorModel.FileName;
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
        // Todo : update procedure can be significantly optimized by wise usage of this list...
        _editorElements = new List<IView>();
        
        var editorView = GetEditor();

        if (_controller != null)
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
            Text                = "+",
            FontSize            = 25,
            TextColor           = Colors.Green,
            BorderColor         = Colors.Transparent,
            WidthRequest        = RemoveRowButtonWidth,
            VerticalOptions     = LayoutOptions.Center,
            BackgroundColor     = Colors.Transparent,
            HorizontalOptions   = LayoutOptions.Center,
        };
        
        button.Clicked += (_, _) =>
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
            Text                    = text,
            Padding                 = 3,
            FontSize                = 17,
            WidthRequest            = width,
            VerticalTextAlignment   = TextAlignment.Center,
        };
        
        return label;
    }

    private View GetEditorElement(MeteoriteObservation observation)
    {
        // remove row button
        var removeButton = new Button
        {
            WidthRequest        = RemoveRowButtonWidth,
            HorizontalOptions   = LayoutOptions.Center,
            VerticalOptions     = LayoutOptions.Center,
            Text                = "—",
            FontSize            = 25,
            TextColor           = Colors.Red,
            BorderColor         = Colors.Transparent,
            BackgroundColor     = Colors.Transparent,
        };
        
        // label
        var labelEntry = new Entry
        {
            Text            = observation.Label,
            WidthRequest    = LabelFieldWidth,
        };

        // classification
        var classEntry = new Entry
        {
            Text            = observation.Classification,
            WidthRequest    = ClassificationFieldWidth,
        };

        // longitude
        var longitudeEntry = new Entry
        {
            Text            = observation.Longitude.ToString(CultureInfo.InvariantCulture),
            WidthRequest    = LongitudeFieldWidth,
        };

        // latitude
        var latitudeEntry = new Entry
        {
            Text            = observation.Latitude.ToString(CultureInfo.InvariantCulture),
            WidthRequest    = LatitudeFieldWidth,
        };

        // diameter
        var diameterEntry = new Entry
        {
            Text            = observation.Diameter.ToString(CultureInfo.InvariantCulture),
            WidthRequest    = DiameterFieldWidth,
        };

        // weight
        var weightEntry = new Entry
        {
            Text            = observation.Weight.ToString(CultureInfo.InvariantCulture),
            WidthRequest    = WeightFieldWidth,
        };

        // chondrule fraction
        var chondruleFractionEntry = new Entry
        {
            Text            = observation.ChondruleFraction.ToString(CultureInfo.InvariantCulture),
            WidthRequest    = ChondruleFractionFieldWidth,
        };
        
        // setting logic
        SetRowHandlers(observation, 
            removeButton, 
            labelEntry, 
            classEntry, 
            longitudeEntry, 
            latitudeEntry, 
            diameterEntry, 
            weightEntry, 
            chondruleFractionEntry);

        // wrapper
        var horizontalStack = new HorizontalStackLayout
        {
            removeButton,
            labelEntry,
            classEntry,
            longitudeEntry,
            latitudeEntry,
            diameterEntry,
            weightEntry,
            chondruleFractionEntry,
        };

        return horizontalStack;
    }

    private void SetRowHandlers(MeteoriteObservation observation, Button remove, Entry label, Entry classification, 
        Entry longitude, Entry latitude, Entry diameter, Entry weight, Entry chondrule)
    {
        // remove button
        remove.Clicked += (_, _) =>
        {
            _controller.RemoveObservation(observation);
            UpdateEditor();
            ShowOkStatus();
        };

        // label entry
        label.Completed += (_, _) =>
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
        classification.Completed += (_, _) =>
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
        longitude.Completed += (_, _) =>
        {
            try
            {
                observation.Longitude = double.Parse(longitude.Text ?? "");
                ShowOkStatus();
            }
            catch (Exception e)
            {
                ShowErrorStatus(e.Message);
                longitude.Text = observation.Longitude.ToString(CultureInfo.InvariantCulture);
            }
        };
        
        // latitude entry
        latitude.Completed += (_, _) =>
        {
            try
            {
                observation.Latitude = double.Parse(latitude.Text ?? "");
                ShowOkStatus();
            }
            catch (Exception e)
            {
                ShowErrorStatus(e.Message);
                latitude.Text = observation.Latitude.ToString(CultureInfo.InvariantCulture);
            }
        };
        
        // diameter entry
        diameter.Completed += (_, _) =>
        {
            try
            {
                observation.Diameter = double.Parse(diameter.Text ?? "");
                ShowOkStatus();
            }
            catch (Exception e)
            {
                ShowErrorStatus(e.Message);
                diameter.Text = observation.Diameter.ToString(CultureInfo.InvariantCulture);
            }
        };
        
        // weight entry
        weight.Completed += (_, _) =>
        {
            try
            {
                observation.Weight = double.Parse(weight.Text ?? "");
                ShowOkStatus();
            }
            catch (Exception e)
            {
                ShowErrorStatus(e.Message);
                weight.Text = observation.Weight.ToString(CultureInfo.InvariantCulture);
            }
        };
        
        // chondrule fraction
        chondrule.Completed += (_, _) =>
        {
            try
            {
                observation.ChondruleFraction = double.Parse(chondrule.Text ?? "");
                ShowOkStatus();
            }
            catch (Exception e)
            {
                ShowErrorStatus(e.Message);
                chondrule.Text = observation.ChondruleFraction.ToString(CultureInfo.InvariantCulture);
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
        Shell.Current.GoToAsync(nameof(HelpPage));
    }
    
    private void SaveClicked(object sender, EventArgs e)
    {
        _controller.SaveObservations();
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