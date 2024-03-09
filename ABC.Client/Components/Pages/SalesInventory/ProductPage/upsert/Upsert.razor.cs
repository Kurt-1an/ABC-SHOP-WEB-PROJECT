using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Models.ViewModels;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;


namespace ABC.Client.Components.Pages.SalesInventory.ProductPage.upsert;
    public partial class Upsert
    {

    /*rivate readonly IWebHostEnvironment _webHostEnvironment;*/

    #region DEPENDENCY INJECTIOn
    [Inject] ProductService_SQL productService_SQL { get; set; }
    [Inject] CategoryService_SQL categoryService_SQL { get; set; }
    [Inject] SupplierService_SQL supplierService_SQL { get; set; }
    [Inject] StoreService_SQL storeService_SQL { get; set; }

    [Inject] IWebHostEnvironment _webHostEnvironment { get; set; }
    [Inject] ApplicationDbContext applicationDbContext { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }
    #endregion

    #region fields
    private List<Product> Products { get; set; } = [];
    private List<Category> CategoryList { get; set; } = [];
    private List<Supplier> SupplierList { get; set; } = [];
    private List<Store> StoreList { get; set; } = [];
    private Product SelectedProduct { get; set; } = new();

    private IBrowserFile selectedFile;
    private string previewImageData = null;


    [SupplyParameterFromQuery(Name = "id")]
	public int ProductId { get; set; }
    #endregion


    protected override async Task OnInitializedAsync()
    {
        productService_SQL.AbcDbConnection = AppSettingsHelper.AbcDbConnection;
        await LoadProducts();
    }

    private async Task LoadProducts()
    {
        // Load product information, category list, store list, and supplier list concurrently
        var productTask = productService_SQL.GetProductInfo(applicationDbContext, ProductId);
        var categoryTask = categoryService_SQL.GetCategoryListData(applicationDbContext);
        var storeTask = storeService_SQL.GetStoreListData(applicationDbContext);
        var supplierTask = supplierService_SQL.GetSupplierList(applicationDbContext);

        // Await all tasks simultaneously
        SelectedProduct = await productTask;
        CategoryList = await categoryTask;
        StoreList = await storeTask;
        SupplierList = await supplierTask;

    }

    private async Task SaveProduct()
    {
        if (SelectedProduct.Id == 0)
        {
            // If the Id is 0, it's a new product
            if (selectedFile != null)
            {
                // Handle file upload
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(selectedFile.Name);
                string productPath = Path.Combine(wwwRootPath, @"image\product");

                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    await selectedFile.OpenReadStream().CopyToAsync(fileStream);
                }

                SelectedProduct.ImageUrl = $@"\image\product\{fileName}";
            }

            bool added = await productService_SQL.AddProduct(applicationDbContext, SelectedProduct);
            NavigationManager.NavigateTo("/ProductList", true);
        }
        else
        {
            // If the Id is not 0, it's an existing product to be updated
            if (selectedFile != null)
            {
                // Handle file upload and deletion of old image
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(selectedFile.Name);
                string productPath = Path.Combine(wwwRootPath, @"image\product");

                if (!string.IsNullOrEmpty(SelectedProduct.ImageUrl))
                {
                    var oldImagePath = Path.Combine(wwwRootPath, SelectedProduct.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    await selectedFile.OpenReadStream().CopyToAsync(fileStream);
                }

                SelectedProduct.ImageUrl = $@"\image\product\{fileName}";
            }

            bool updated = await productService_SQL.UpdateProduct(applicationDbContext, SelectedProduct);
            if (updated)
            {
                NavigationManager.NavigateTo("/ProductList", true);
            }
            else
            {
                // Toast
            }
        }
    }


    private async Task HandleFileSelected(InputFileChangeEventArgs e)
    {
        selectedFile = e.File;

        // Read the file and convert it to a base64-encoded string
        var memoryStream = new MemoryStream();
        await e.File.OpenReadStream().CopyToAsync(memoryStream);
        var fileBytes = memoryStream.ToArray();
        previewImageData = $"data:{e.File.ContentType};base64,{Convert.ToBase64String(fileBytes)}";

        StateHasChanged();
    }

}

