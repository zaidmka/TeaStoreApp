using TeaStoreApp.Models;
using TeaStoreApp.Services;
namespace TeaStoreApp.Pages;

public partial class ProductListPage : ContentPage
{
	public ProductListPage(int categoryId,string categoryName)
	{
		InitializeComponent();
        this.Title = categoryName; // Set the page title to the passed category name

        GetProducts(categoryId);
	}

    private async void GetProducts(int categoryId)
    {
		var products = await ApiService.GetProducts("category",categoryId.ToString());
		CvProducts.ItemsSource = products;
    }

    private void CvProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var currentSelection = e.CurrentSelection.FirstOrDefault() as Product;
        if (currentSelection == null) return;
        Navigation.PushAsync(new ProductDetailPage(currentSelection.Id, currentSelection.Name));
        ((CollectionView)sender).SelectedItem = null;
    }
}