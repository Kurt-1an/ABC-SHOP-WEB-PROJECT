using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Components;



namespace ABC.Client.Components.Pages.SalesInventory.ProductPage;

public partial class ProductList
{
    #region DEPENDENCY INJECTIOn
    [Inject] ProductService_SQL productService_SQL { get; set; }
    [Inject] ApplicationDbContext applicationDbContext { get; set; }

    #endregion

    #region fields
    private List<Product> Products { get; set; } = [];
    private Product selectedProduct { get; set; } = new();


    #endregion


    protected override async Task OnInitializedAsync()
    {
        productService_SQL.AbcDbConnection = AppSettingsHelper.AbcDbConnection;
        await LoadProducts();
    }

    private async Task LoadProducts()
    {
        Products = await productService_SQL.GetProductList(applicationDbContext);
    }

	private async Task PopulateProductDetails(int productId)
    {
        selectedProduct = new();

        // Find the Item
        var result = await productService_SQL.GetProductInfo(applicationDbContext, productId);


        if (result is not null)
        {
            selectedProduct = result;
        }
        await InvokeAsync(StateHasChanged);

    }


    private async Task Remove()
    {
		// Change the product's status 
		selectedProduct.status = false;

		// Call service to remove product 
		bool removed = await productService_SQL.RemoveProduct(applicationDbContext, selectedProduct);

		if (removed)
		{
            //refresh the list
			await LoadProducts(); 
		}
	}

}

