using ABC.Client.Components.Pages.SalesInventory.ProductPage;
using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Models.ViewModels;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Serilog;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Security.Claims;
using System;
using ABC.Shared.Utility;
using Microsoft.EntityFrameworkCore;

namespace ABC.Client.Components.Pages.ShopWeb.Cart.OrderCheckout;
public partial class Summary
{
	#region Injections
	[Inject] IHttpContextAccessor httpContextAccessor { get; set; }
	[Inject] ApplicationDbContext applicationDbContext { get; set; }
	[Inject] ShoppingCartService_SQL shoppingCartService_SQL { get; set; } 
	[Inject] ProductService_SQL productService_SQL { get; set; }
    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject] ApplicationUserService_SQL applicationUserService_SQL { get; set; }
	[Inject] OrderHeaderService_SQL orderHeaderService_SQL { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }

    #endregion

    #region Fields
    [CascadingParameter]
	public HttpContext? HttpContext { get; set; }
	private List<ShoppingCart> shoppingCartList { get; set; } = [];
    public ShoppingCart cart { get; set; }
    public ShoppingCartVM shoppingCart { get; set; }
	public ApplicationUser UserInfo { get; set; }
    public Product product { get; set; }


    [SupplyParameterFromForm]
	public ShoppingCartVM checkoutFormModel { get; set; }

    private string userId;
    private Toast toastRef;
    #endregion

    protected override async Task OnInitializedAsync()
	{
		checkoutFormModel ??= new();

        // Get current authenticated user
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity.IsAuthenticated)
        {
            var claimsIdentity = user.Identity as ClaimsIdentity;
            userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

		// Get user by id
		UserInfo = await applicationUserService_SQL.GetApplicationUserInfo(applicationDbContext, userId);
		shoppingCartList = await shoppingCartService_SQL.GetShoppingCartList(applicationDbContext, userId);


	}

    private async Task PlaceOrderHandler()
    {
        ShoppingCartVM summary = new()
        {
            OrderHeader = new()
            {
                OrderDate = DateTime.Now,
                ShippingDate = DateTime.Now.AddDays(3),
                OrderTotal = 0,
                OrderStatus = SD.StatusPending,
                PaymentStatus = SD.PaymentStatusPending,
                TrackingNumber = "1234567890",
                Carrier = "Sample Employee",
                Discount = 0,
                ServiceFee = 0,
                PaymentMode = "Cash On Delivery",
                OfficialReceipt = "sample Receipt No",
                ApplicationUserId = userId
            }
        };

        // Calculate OrderTotal
        foreach (var cart in shoppingCartList)
        {
            cart.Price = GetPrice(cart);
            summary.OrderHeader.OrderTotal += (cart.Price * cart.Quantity);
        }

        bool addedOrderHeader = await orderHeaderService_SQL.AddOrderHeader(applicationDbContext, summary.OrderHeader);

        summary.OrderDetailsList = new();
        foreach (var item in shoppingCartList)
        {
            OrderDetail _orderDetail = new()
            {
                ProductId = item.ProductId,
                OrderHeaderId = summary.OrderHeader.Id,
                Price = item.Price,
                Count = item.Quantity
            };


            product = await productService_SQL.GetProductInfo(applicationDbContext, item.ProductId);
            product.StockQuantity -= item.Quantity;
            await productService_SQL.UpdateProduct(applicationDbContext, product);

            summary.OrderDetailsList.Add(_orderDetail);
        }

        bool addedOrderDetail = await orderHeaderService_SQL.AddOrderDetail(applicationDbContext, summary.OrderDetailsList);

        foreach (var cartItem in shoppingCartList)
        {
		    bool removed = await shoppingCartService_SQL.RemoveShoppingCart(applicationDbContext, cartItem);
        }



        NavigationManager.NavigateTo("/OrderConfirmation", true);
    }

    private double GetPrice(ShoppingCart shoppingCart)
    {
        return shoppingCart.Product.RetailPrice;
    }
}
