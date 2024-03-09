using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABC.Shared.Models;
using ABC.Shared.Services;
using Serilog;

using Microsoft.AspNetCore.Components;
using ABC.Client.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;

namespace ABC.Client.Components.Pages.POS;
public partial class POS
{
    #region DEPENDENCY INJECTIOn
    [Inject] POSService_SQL pOSService_SQL { get; set; }
    [Inject] ProductService_SQL ProductService_SQL { get; set; }
    [Inject] ApplicationDbContext applicationDbContext { get; set; }
    [Inject] UserManager<ApplicationUser> UserManager { get; set; }
    private ApplicationUser applicatioUser { get; set; } = new();
    
    #endregion

    #region FIELDS
    private CancellationTokenSource _tokenSource = null;
    private List<CustomerBasicInfo> CustomerList { get; set; } = [];
    private List<Product> ProductList { get; set; } = [];
    private List<OrderDetail> ShoppingCart { get; set; } = [];
    private Product ProductInModal { get; set; } = new();
    private OrderHeader OrderSummary { get; set; } = new();
    private CustomerData Customer { get; set; } = new();
    private decimal TotalFees { get; set; } = 0;
    private String ProductSearchInput { get; set; } = String.Empty;
    private Discount discount { get; set; } = new();
    private DiscountModel DiscountTypes { get; set; } = new();
    private int ItemPostRemovalId { get; set; } = 0;
    private bool ShowDropdown { get; set; } = false;
    private bool ShowProductDropdown { get; set; } = false;
    #endregion
    protected override async Task OnInitializedAsync()
    {
        pOSService_SQL.AbcDbConnection = AppSettingsHelper.AbcDbConnection;
    }

    private async Task ProcessPurchase()
    {
    }
    private async Task ShowDropdownHandler(bool show)
    {
        await Task.Delay(1000);
        ShowDropdown = show;
        StateHasChanged();
    }

    private async Task ShowProductDropdownHandler(bool show){
        await Task.Delay(1000);
        ShowProductDropdown = show;
        StateHasChanged();
    }

    private async Task GetCustomerList(ChangeEventArgs e)
    {
        Customer.ContactNumber = e?.Value?.ToString();
        CustomerList.Clear();

        var result = await pOSService_SQL.GetCustomerList(applicationDbContext);
        if (result is not null && result.Count > 0 && !String.IsNullOrEmpty(Customer.ContactNumber))
        {
            foreach (var item in result)
            {
                if (item.ContactNumber.ToString().StartsWith(Customer.ContactNumber, StringComparison.CurrentCultureIgnoreCase))
                {
                    CustomerBasicInfo customerBasicInfo = new()
                    {
                        Id = item.Id.ToString(),
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        ContactNumber = Convert.ToInt64(item.ContactNumber)
                    };
                    CustomerList.Add(customerBasicInfo);
                }
            }
        }
        await InvokeAsync(StateHasChanged);
    }

    private async Task GetProductList(ChangeEventArgs e)
    {
        ProductSearchInput = e?.Value?.ToString();
        ProductList.Clear();

        var result = await pOSService_SQL.GetProductList(applicationDbContext);
        if (result is not null && result.Count > 0 && !String.IsNullOrEmpty(ProductSearchInput))
        {
            ProductList = result.Where(x => x.productName.ToString().StartsWith(ProductSearchInput, StringComparison.CurrentCultureIgnoreCase)).ToList();
        }
        await InvokeAsync(StateHasChanged);
    }

    private async Task PopulateFormData(string id)
    {
        var result = await pOSService_SQL.GetCustomerInfo(applicationDbContext, id);
        Customer = new()
        {
            Id = result.Id,
            FirstName = result.FirstName,
            LastName = result.LastName,
            EmailAddress = result.EmailAddress,
            ContactNumber = result.ContactNumber.ToString(),
            Type = result.Type,
            ApSuUn = result.ApSuUn,
            StreetorSubd = result.StreetorSubd,
            Barangay = result.Barangay,
            City = result.City,
            Province = result.Province,
            ZipCode = result.ZipCode,
        };
    }

    private async Task PopulateProductDetails(int productId){
        ProductInModal = new();
        // Find the Item
        var result = await pOSService_SQL.GetProductInfo(applicationDbContext, productId);
        if (result is not null)
        {
            ProductInModal = result;
        }
    }

    private async Task AddToCart(int productId, int quantity){
        // Find the Item
        if(quantity == 0){
            return;
        };
        var result = await pOSService_SQL.GetProductInfo(applicationDbContext, productId);
        if (result is not null)
        {
            OrderDetail NewItem = new(){
                ProductId = result.Id,
                Count = quantity,
                Price = result.RetailPrice,
                TotalPrice = quantity * result.RetailPrice
            };

            var item = ShoppingCart.FirstOrDefault(x => x.ProductId == NewItem.ProductId);
            if(item is not null && item.Count > 0){
                var removed = ShoppingCart.Remove(item);
            }
            ShoppingCart.Add(NewItem);
        }
        ProductSearchInput = "";
        await UpdateOrderSummary();
        await InvokeAsync(StateHasChanged);
    }

    private async Task ItemPostRemoval(int productId){
        ItemPostRemovalId = productId;
    }

    private async Task RemoveFromCart(){
        var item = ShoppingCart.FirstOrDefault(x => x.ProductId == ItemPostRemovalId);
        if(item is not null && item.Count > 0){
            var removed = ShoppingCart.Remove(item);
        }
        await InvokeAsync(StateHasChanged);
    }
    private async Task AddCustomer()
    {
        try
        {
            Customer customer = new()
            {
                FirstName = "",
                LastName = "Esan",
                EmailAddress = "admintest@test.com",
                ContactNumber = Convert.ToInt64("09995514412"),
                ApSuUn = "IDK",
                Type = "User",
                StreetorSubd = "Tokyo St.",
                Barangay = "Tokyo",
                City = "Tokyo",
                Province = "Tokyo",
                ZipCode = 1114
            };

            var result = await pOSService_SQL.AddCustomer(applicationDbContext, customer);
            
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
        }
    }

    private async Task UpdateTotalFees(){
        TotalFees = OrderSummary.ServiceFee + OrderSummary.DeliveryFee;
        await InvokeAsync(StateHasChanged);
    }

    private async Task UpdatePostTotalDiscount(string discountType = "percent"){
        decimal orderTotal = 0;
        foreach(var product in ShoppingCart){
            orderTotal += Convert.ToDecimal(product.TotalPrice);
        }

        if(String.IsNullOrEmpty(discountType)){
            discountType = "percent";
        }
        discount.DiscountType = discountType;
        if(discountType == "percent"){
            discount.TotalDiscount = Convert.ToDouble(orderTotal) * (discount.DiscountAmount / 100);
        }else{
            discount.TotalDiscount = discount.DiscountAmount;
        }
        discount.DiscountedPrice = Convert.ToDouble(orderTotal) - discount.TotalDiscount;
        await InvokeAsync(StateHasChanged);
    }

    private async Task UpdateTotalDiscount(){
        OrderSummary.Discount = Convert.ToDecimal(discount.TotalDiscount); 
        await UpdateOrderSummary();
        await InvokeAsync(StateHasChanged);
    }

    private async Task UpdateOrderSummary(){
        decimal orderTotal = 0;
        foreach(var product in ShoppingCart){
            orderTotal += Convert.ToDecimal(product.TotalPrice);
        }
        orderTotal += (OrderSummary.ServiceFee + OrderSummary.DeliveryFee) - OrderSummary.Discount;
        OrderSummary.OrderTotal = Convert.ToDouble(orderTotal);
    }
}