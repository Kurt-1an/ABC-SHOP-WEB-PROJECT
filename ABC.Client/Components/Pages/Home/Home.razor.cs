using ABC.Client.Components.Pages.SalesInventory.ProductPage;
using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Components;
using MySqlX.XDevAPI.Common;

namespace ABC.Client.Components.Pages.Home;
public partial class Home
{
    [Inject] ApplicationDbContext applicationDbContext { get; set; }
    [Inject] ProductService_SQL productService_SQL { get; set; }
    [Inject] OrderHeaderService_SQL orderHeaderService_SQL { get; set; }

    #region fields
    private List<Product> ProductList { get; set; } = [];
    private List<OrderDetail> topSellingItems { get; set; } = [];

    #endregion

    #region
    protected override async Task OnInitializedAsync()
    {
        productService_SQL.AbcDbConnection = AppSettingsHelper.AbcDbConnection;
        ProductList = await productService_SQL.GetProductList(applicationDbContext);
        topSellingItems = await orderHeaderService_SQL.GetOrderDetailsList(applicationDbContext);
    }

    private async Task GetTopSellingItems()
    {
        var topResult = await orderHeaderService_SQL.GetOrderDetailsList(applicationDbContext);

        if (topResult is not null && topResult.Count > 0)
        {
            var topSellingItems = topResult
                .GroupBy(od => od.Product)
                .OrderByDescending(g => g.Sum(od => od.Count))
                .Take(3)
                .Select(g => g.Key)
                .ToList();
        }

        await InvokeAsync(StateHasChanged);
    }

    private async Task GetProductList()
    {
        var result = await productService_SQL.GetProductList(applicationDbContext);

        if (result is not null && result.Count > 0)
        {
            ProductList = result.OrderByDescending(x => x.DateAdded).ToList();
        }

        await InvokeAsync(StateHasChanged);
    }

    #endregion
    //[Inject] ApplicationDbContext applicationDbContext { get; set; }
    //[Inject] CategoryService_SQL categoryService_SQL { get; set; }

    //private List<Category> Categories { get; set; } = new List<Category>();
    //private Category categoryData { get; set; }

    //protected override async Task OnInitializedAsync()
    //{
    //    var _category = await categoryService_SQL.GetCategoryList(applicationDbContext);
    //    if (_category != null && _category.Count > 0)
    //    {
    //        Categories = _category;
    //    }
    //    await InvokeAsync(StateHasChanged);
    //}
}