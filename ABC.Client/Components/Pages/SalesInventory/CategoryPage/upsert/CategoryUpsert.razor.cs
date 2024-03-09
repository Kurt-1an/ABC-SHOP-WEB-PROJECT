using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace ABC.Client.Components.Pages.SalesInventory.CategoryPage.upsert;
public partial class CategoryUpsert
{
	#region DEPENDENCY INJECTIOn
	[Inject] CategoryService_SQL categoryService_SQL { get; set; }
	[Inject] ApplicationDbContext applicationDbContext { get; set; }
	[Inject] NavigationManager NavigationManager { get; set; }
	#endregion

	#region fields
	private List<Category> CategoryList { get; set; } = [];
	private Category selectedCategory { get; set; } = new();



	[SupplyParameterFromQuery(Name = "id")]
	public int CategoryId { get; set; }
	#endregion

	protected override async Task OnInitializedAsync()
	{
		categoryService_SQL.AbcDbConnection = AppSettingsHelper.AbcDbConnection;
		await LoadCategory();
	}

	private async Task LoadCategory()
	{
		selectedCategory = new();

		var _category =  categoryService_SQL.GetCategoryInfo(applicationDbContext, CategoryId);
		selectedCategory = await _category;

	}

	private async Task SaveCategory()
	{
		if (selectedCategory.Id == 0)
		{
			// If the Id is 0, it's a new product 
			bool added = await categoryService_SQL.AddCategory(applicationDbContext, selectedCategory);
            NavigationManager.NavigateTo("/Category", true);

        }
        else
		{
			// If the Id is not 0, it's an existing product to be updated
			bool updated = await categoryService_SQL.UpdateCategory(applicationDbContext, selectedCategory);

            if (updated)
            {
                NavigationManager.NavigateTo("/ProductList", true);
            }
            else
            {
                //toast
            }
            NavigationManager.NavigateTo("/Category", true);

        }
	}
}