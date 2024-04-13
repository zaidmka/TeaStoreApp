
using System.Collections.ObjectModel;
using TeaStoreApp.Models;
using TeaStoreApp.Services;

namespace TeaStoreApp.Pages;

public partial class CartPage : ContentPage
{
    private ObservableCollection<ShoppingCartItem> _shoppingCartItems = new ObservableCollection<ShoppingCartItem>();
    public CartPage()
    {
        InitializeComponent();
    }
    protected override void OnAppearing()
    {
        EmptyGrid.IsVisible = true;
        ContentGrid.IsVisible = false;
        base.OnAppearing();
        GetShoppingCartItems();
        bool address = Preferences.ContainsKey("address");
        if (address)
        {
            LblAddress.Text = Preferences.Get("address", string.Empty);
        }
        else
        {
            LblAddress.Text = "Please Provide an address!";
        }
    }

    private async void GetShoppingCartItems()
    {
        _shoppingCartItems.Clear();
        var shoppingCartItems = await ApiService.GetShoppingCartItems(Preferences.Get("userid", 0));
        if (shoppingCartItems.Count>0)
        {
            foreach (var item in shoppingCartItems)
            {
                _shoppingCartItems.Add(item);
            }
            EmptyGrid.IsVisible = false;
            ContentGrid.IsVisible = true;
            PlaceOrderbutton.IsVisible = true;
        }
        else
        {
            _shoppingCartItems.Clear();
            EmptyGrid.IsVisible = false;
            ContentGrid.IsVisible = true;
        }

        CvCart.ItemsSource = _shoppingCartItems;
        UpdateTotalPrice();
    }

    private void UpdateTotalPrice()
    {
        var totalPrice = _shoppingCartItems.Sum(item => item.Price * item.Qty);
        LblTotalPrice.Text = totalPrice.ToString();
    }


    private async void UpdateCartQuantity(int productId, string action)
    {
        var response = await ApiService.UpdateCartQuantity(productId, action);
        if (response) return;
        else
        {
            await DisplayAlert("Opps", "Something went wrong", "Cancel");
        }
    }
    private void BtnDecrease_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is ShoppingCartItem shoppingCartItem)
        {
            if (shoppingCartItem.Qty == 1) return;
            else if (shoppingCartItem.Qty > 1)
            {
                shoppingCartItem.Qty--;
                UpdateTotalPrice();
                UpdateCartQuantity(shoppingCartItem.ProductId, "decrease");

            }
        }
    }

    private void BtnIncrease_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is ShoppingCartItem shoppingCartItem)
        {
            shoppingCartItem.Qty++;
            UpdateTotalPrice();
            UpdateCartQuantity(shoppingCartItem.ProductId, "increase");
        }
    }

    private void BtnDelete_Clicked(object sender, EventArgs e)
    {
        if (sender is ImageButton button && button.BindingContext is ShoppingCartItem shoppingCartItem)
        {
            _shoppingCartItems.Remove(shoppingCartItem);
            UpdateTotalPrice();
            UpdateCartQuantity(shoppingCartItem.ProductId, "delete");
            if(_shoppingCartItems.Count == 0) PlaceOrderbutton.IsVisible = false;
        }
    }

    private void BtnEditAddress_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AddressPage());
    }

    private async void TapPlaceOrder_Tapped(object sender, TappedEventArgs e)
    {
        var order = new Order()
        {
            Address = LblAddress.Text,
            UserId = Preferences.Get("userid", 0),
            OrderTotal = Convert.ToInt32(LblTotalPrice.Text)
        };
        var response = await ApiService.PlaceOrder(order);
        if (response)
        {
            await DisplayAlert("Orders", "Your order has been placed", "Alright");
            _shoppingCartItems.Clear();
        }
        else
        {
            await DisplayAlert("Orders", "Opps.... something went wrong!", "Cancel");

        }
    }
}