
using TeaStoreApp.Models;
using TeaStoreApp.Services;

namespace TeaStoreApp.Pages;

public partial class ProductDetailPage : ContentPage
{
	private int _productId;
	private string imageUrl;
	private BookmarkItemService bookmarkItemService = new BookmarkItemService(); 
	public ProductDetailPage(int productId,string productName)
	{
		InitializeComponent();
		GetProductDetail(productId);
		_productId = productId;
		this.Title = productName;

    }

    private async void GetProductDetail(int productId)
    {
		var product = await ApiService.GetProductDetail(productId);
		LblProductName.Text = product.Name;
		LblProductDescription.Text = product.Detail;
		LblProductPrice.Text=product.Price.ToString();
		ImgProduct.Source = product.FullImageUrl;
		LblTotalPrice.Text = product.Price.ToString();
		imageUrl = product.FullImageUrl;


    }

    private void BtnRemove_Clicked(object sender, EventArgs e)
    {
		var i = Convert.ToInt32(LblQty.Text);
		i--;
		if (i < 1) return;
		LblQty.Text = i.ToString();
		var totalPrice = i*Convert.ToInt32(LblProductPrice.Text);
		LblTotalPrice.Text=totalPrice.ToString();
		
    }

    private void BtnAdd_Clicked(object sender, EventArgs e)
    {
        var i = Convert.ToInt32(LblQty.Text);
        i++;
        LblQty.Text = i.ToString();
        var totalPrice = i * Convert.ToInt32(LblProductPrice.Text);
        LblTotalPrice.Text = totalPrice.ToString();
    }

    private async void BtnAddToCart_Clicked(object sender, EventArgs e)
    {
		var shppoingCart = new ShoppingCart()
		{
			Qty = Convert.ToInt32(LblQty.Text),
			Price = Convert.ToInt32(LblProductPrice.Text),
			TotalAmount = Convert.ToInt32(LblProductPrice.Text),
			ProductId = _productId,
			CustomerId = Preferences.Get("userid",0)
		};
		var response = await ApiService.AddItemsInCart(shppoingCart);
		if (response)
		{
			await DisplayAlert("Cart", "Your item has been added to cart", "Alright");
		}
		else
		{
            await DisplayAlert("Cart", "Opps... something went wrong", "Cancel");

        }
    }

    private void ImgBtnFavorite_Clicked(object sender, EventArgs e)
    {
		var existingBookmark = bookmarkItemService.Read(_productId);
		if (existingBookmark != null)
		{
			bookmarkItemService.Delete(existingBookmark);
		}
		else
		{
			var bookmarkProduct = new BookmarkProduct()
			{
				ProductId = _productId,
				IsBookmarked=true,
				Detail = LblProductDescription.Text,
				Name = LblProductName.Text,
				Price=Convert.ToInt32(LblProductPrice.Text),
				ImageUrl=imageUrl
			};
			bookmarkItemService.Create(bookmarkProduct);
		}
		UpdateFavoriteButton();
    }
	private void UpdateFavoriteButton()
	{
        var existingBookmark = bookmarkItemService.Read(_productId);
        if (existingBookmark != null)
        {
			FavButton.ImageSource = "heartfill";
        }
        else
        {
            FavButton.ImageSource = "heart";

        }
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateFavoriteButton();

    }

    private void FavButton_Clicked(object sender, EventArgs e)
    {
        var existingBookmark = bookmarkItemService.Read(_productId);
        if (existingBookmark != null)
        {
            bookmarkItemService.Delete(existingBookmark);
        }
        else
        {
            var bookmarkProduct = new BookmarkProduct()
            {
                ProductId = _productId,
                IsBookmarked = true,
                Detail = LblProductDescription.Text,
                Name = LblProductName.Text,
                Price = Convert.ToInt32(LblProductPrice.Text),
                ImageUrl = imageUrl
            };
            bookmarkItemService.Create(bookmarkProduct);
        }
        UpdateFavoriteButton();
    }

   
}