using TeaStoreApp.Services;

namespace TeaStoreApp.Pages;

public partial class AddressPage : ContentPage
{
    private readonly IGeolocation _geolocation;
    private readonly IMap _map;
    private double _latitude;
    private double _longitude;
    public AddressPage(IGeolocation geolocation = null, IMap map = null)
    {
        InitializeComponent();
        _geolocation = geolocation ?? Geolocation.Default;
        _map = map ?? Map.Default;
    }

    private void BtnSave_Clicked(object sender, EventArgs e)
    {
        if (EntAddress.Text == null || EntName.Text == null || EntPhone == null)
        {
            DisplayAlert("Location", "The Fields are null... Please Fill them before Save!", "Okay");
            return;
        }
        Preferences.Set("address", EntAddress.Text + "," + EntPhone.Text + "," + EntName.Text);
        Navigation.PopAsync();

    }

    private void BtnCancel_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();

    }

    private async void GeoLocationBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            var location = await _geolocation.GetLastKnownLocationAsync();
            if (location == null)
            {
                location = await _geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(30)
                });
            }

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Location", $"Lat: {location.Latitude}, Long: {location.Longitude}", "OK");
            });
            EntAddress.Text = "Latitude: " + location.Latitude.ToString() + " Longitude: " + location.Longitude;
            _latitude = location.Latitude;
            _longitude = location.Longitude;
            
        }
        catch (Exception ex)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Error", ex.Message, "OK");
            });
        }
    }

    private void GeoLocationBtn_Clicked_1(object sender, EventArgs e)
    {

    }

    private async void MapBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            await _map.OpenAsync(_latitude, _longitude, new MapLaunchOptions
            {
                Name = "Your location",
                NavigationMode = NavigationMode.Driving
            });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Map",ex.Message, "OK");
        }
    }
}