using System.Text.Json.Serialization;

namespace MSJsonFileEditor.Models;

public class MeteoriteObservation
{
    public const double MaxLatitude     = 90;
    public const double MinLatitude     = -90;
    public const double MaxLongitude    = 180;
    public const double MinLongitude    = -180;
    
    [JsonPropertyName("label")]
    public string Label;
    [JsonPropertyName("class")]
    public string Classification;
    
    [JsonPropertyName("latitude")]
    public double Latitude
    {
        get => _latitude;
        set
        {
            if (value < MinLatitude || value > MaxLatitude)
                throw new ArgumentException("Latitude out of range");
            _latitude = value;
        }
    }
    [JsonPropertyName("longitude")]
    public double Longitude
    {
        get => _longitude;
        set
        {
            if (value < MinLongitude || value > MaxLongitude)
                throw new ArgumentException("Longitude out of range");
            _longitude = value;
        }
    }
    [JsonPropertyName("diameter")]
    public double Diameter
    {
        get => _diameter;
        set
        {
            if (value < 0)
                throw new ArgumentException("Diameter cannot be negative.");
            _diameter = value;
        }
    }
    [JsonPropertyName("weight")]
    public double Weight
    {
        get => _weight;
        set
        {
            if (value < 0)
                throw new ArgumentException("Weight cannot be negative.");
            _weight = value;
        }
    }
    [JsonPropertyName("chondrule_fraction")]
    public double ChondruleFraction
    {
        get => _chondruleFraction;
        set
        {
            if (value < 0 || value > 1)
                throw new ArgumentException("Chondrule fraction must be in [0;1].");
            _chondruleFraction = value;
        }
    }

    private double _latitude;
    private double _longitude;
    private double _diameter;
    private double _weight;
    private double _chondruleFraction;

    public MeteoriteObservation()
    {
        ChondruleFraction = 0;
        Classification = "";
        Longitude = 0;
        Latitude = 0;
        Diameter = 0;
        Weight = 0;
        Label = "";
    }

    public MeteoriteObservation(string label, string classification, double longitude, double latitude,
        double diameter, double weight, double chondruleFraction)
    {
        ChondruleFraction = chondruleFraction;
        Classification = classification;
        Longitude = longitude;
        Latitude = latitude;
        Diameter = diameter;
        Weight = weight;
        Label = label;
    }
}