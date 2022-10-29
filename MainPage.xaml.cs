using System.Text.Json;
using static Restaurant_Randomizer.MainPage;

namespace Restaurant_Randomizer;

public partial class MainPage : ContentPage
{
    int radius = 10;
	Random rnd = new Random();
    Restaurant restaurant;
    

    public class Restaurant
    {
        public string? type { get; set; }
        public SubDetails[]? features { get; set; }

        public class SubDetails
        {
            public string? type { get; set; }
            public MainDetails? properties { get; set; }
        }

        public class MainDetails
        {
            public string? name { get; set; }
            public string? address_line2 { get; set; }
            public SourceData? datasource { get; set; }

        }

        public class SourceData
        {
            public RawData? raw { get; set; }
        }

        public class RawData
        {
            public string? cuisine { get; set; }
        }
    }

    public void OnCounterClicked(object sender, EventArgs e) 
    {
        var rand = new Random();

        if (restaurant == null) {
            UpdateLocation(sender, e);
        }
        var randomNumber = rand.Next(0, restaurant.features.Length);

        RestaurantLabel.Text = $"Try {restaurant.features[randomNumber].properties.name}";
        RestaurantAddress.Text = $"{restaurant.features[randomNumber].properties.address_line2}";
    }

    public void UpdateLocation(object sender, EventArgs e)
    {
        const string uri = "https://api.geoapify.com/v2/places?categories=catering.fast_food,catering.restaurant,catering.food_court&filter=circle:-111.7870222,43.8235163,10000&bias=proximity:-111.7870222,43.8235163&lang=en&limit=100&apiKey=5f0c40aba3f44b858a5bd67006b15a21";
        using var client = new HttpClient();
        using var getRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        using var jsonStream = client.Send(getRequestMessage).Content.ReadAsStream();
        using var reader = new StreamReader(jsonStream);
        var jsonItem = reader.ReadToEnd();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        restaurant = JsonSerializer.Deserialize<Restaurant>(jsonItem);
    }

    public void RadiusTo10(object sender, EventArgs e)
    {
        radius = 10;
        CurrentRadius.Text = "Current Radius: 10 Miles";
    }
    public void RadiusTo20(object sender, EventArgs e)
    {
        radius = 20;
        CurrentRadius.Text = "Current Radius: 20 Miles";
    }
    public void RadiusTo40(object sender, EventArgs e)
    {
        radius = 40;
        CurrentRadius.Text = "Current Radius: 40 Miles";
    }

    public MainPage()
	{
		InitializeComponent();
	}
}

