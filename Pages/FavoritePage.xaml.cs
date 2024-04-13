
using TeaStoreApp.Models;
using TeaStoreApp.Services;

namespace TeaStoreApp.Pages;

public partial class FavoritePage : ContentPage
{
    private BookmarkItemService bookmarkItemService;
	public FavoritePage()
	{
		InitializeComponent();
        bookmarkItemService = new BookmarkItemService();
	}

    private void CvProducts_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
    {
        var currentSelection = e.CurrentSelection.FirstOrDefault() as BookmarkProduct;
        if (currentSelection == null) return;
        Navigation.PushAsync(new ProductDetailPage(currentSelection.ProductId,currentSelection.Name));
        ((CollectionView)sender).SelectedItem = null;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        GetFavoriteProducts();
    }

    private void GetFavoriteProducts()
    {
        var favoriateProducts = bookmarkItemService.ReadAll();
        CvProducts.ItemsSource = favoriateProducts;
    }
}