using CommunityToolkit.Maui.Core;
using TeaStoreApp.Models;
using TeaStoreApp.Services;

namespace TeaStoreApp.Pages;

public partial class HomePage : ContentPage
{
    public HomePage()
	{
		InitializeComponent();

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (Application.Current.RequestedTheme == AppTheme.Light)
        {
            StatusBarStyle.StatusBarStyle = CommunityToolkit.Maui.Core.StatusBarStyle.DarkContent;
        }
        else if (Application.Current.RequestedTheme == AppTheme.Dark)
        {
            StatusBarStyle.StatusBarStyle = CommunityToolkit.Maui.Core.StatusBarStyle.LightContent;
        }
        LblUserName.Text = "Hi " + Preferences.Get("username", string.Empty);

        EmptyGrid.IsVisible = true;
        ContentGrid.IsVisible = false;
        GetCategories();

        GetTrendingProducts();

        GetBestSellingProducts();
        EmptyGrid.IsVisible = false;
        ContentGrid.IsVisible = true;
    }
    private async void GetBestSellingProducts()
    {
        var products = await ApiService.GetProducts("bestselling", string.Empty);
        CvBestSelling.ItemsSource = products;
    }

    public async  void GetCategories()
	{
		var categories = await ApiService.GetCategories();
		CvCategories.ItemsSource = categories;
	}

	public async void GetTrendingProducts()
	{
		var products=await ApiService.GetProducts("trending",string.Empty);
		CvTrending.ItemsSource = products;
	}

    private void CvCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
		var currentSelection = e.CurrentSelection.FirstOrDefault() as Category;
		if (currentSelection == null) return;
		Navigation.PushAsync(new ProductListPage(currentSelection.Id,currentSelection.Name));

		//clear the selelction after leaving
		((CollectionView)sender).SelectedItem = null;

    }

    private void CvBestSelling_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var currentSelection = e.CurrentSelection.FirstOrDefault() as Product;
        if (currentSelection == null) return;
        Navigation.PushAsync(new ProductDetailPage(currentSelection.Id, currentSelection.Name));
        ((CollectionView)sender).SelectedItem = null;
    }

    private void CvTrending_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var currentSelection = e.CurrentSelection.FirstOrDefault() as Product;
        if (currentSelection == null) return;
        Navigation.PushAsync(new ProductDetailPage(currentSelection.Id, currentSelection.Name));
        ((CollectionView)sender).SelectedItem = null;
    }
}