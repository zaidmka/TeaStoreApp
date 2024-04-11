using TeaStoreApp.Services;

namespace TeaStoreApp.Pages;

public partial class SignupPage : ContentPage
{
	public SignupPage()
	{
		InitializeComponent();
	}

    private async void BtnSignup_Clicked(object sender, EventArgs e)
    {
		var response = await ApiService.RegisterUser(EntName.Text,EntEmail.Text,EntPhone.Text,EntPassword.Text);
		if (response)
		{
			await DisplayAlert("Registration", "Your Account has been created", "Okay");
			await Navigation.PushAsync(new LoginPage());
		}
		else {
            await DisplayAlert("Registration", "Opps... Something got wrong", "Cancel");

        }
    }

    private async void TapLogin_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());

    }
}