using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Models.ViewModels;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace ABC.Client.Components.Pages.SalesInventory.CategoryPage;
public partial class CategoryIndex
{

	#region DEPENDENCY INJECTIOn
	[Inject] CategoryService_SQL categoryService_SQL { get; set; }
	[Inject] ApplicationDbContext applicationDbContext { get; set; }
	[Inject] NavigationManager NavigationManager { get; set; }
	#endregion

	#region fields
	private List<Category> Category { get; set; } = [];
	private Category selectedCategory { get; set; } = new();
	#endregion

	protected override async Task OnInitializedAsync()
	{
		categoryService_SQL.AbcDbConnection = AppSettingsHelper.AbcDbConnection;
		await LoadCategory();
	}

	private async Task LoadCategory()
	{
		Category = await categoryService_SQL.GetCategoryList(applicationDbContext);
	}

	private async Task PopulateCategoryDetails(int categoryId)
	{
		selectedCategory = new();

		// Find the Item
		var result = await categoryService_SQL.GetCategoryInfo(applicationDbContext, categoryId);
		if (result is not null)
		{
			selectedCategory = result;
		}
		await InvokeAsync(StateHasChanged);

	}


	private async Task Remove()
	{
		// Change the product's status 
		selectedCategory.status = false;

		// Call service to remove product 
		bool removed = await categoryService_SQL.RemoveCategory(applicationDbContext, selectedCategory);

		if (removed)
		{
			//refresh the list
			await LoadCategory();
		}
	}

}