

using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Models.ViewModels;
using ABC.Shared.Services;
using ABC.Shared.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace ABC.Client.Components.Pages.SalesInventory.OrderPage;
	public partial class Order
	{
	#region DEPENDENCY INJECTIOn
	[Inject] ProductService_SQL productService_SQL { get; set; }
	[Inject] ApplicationDbContext applicationDbContext { get; set; }
    [Inject] ApplicationUserService_SQL applicationUserService_SQL { get; set; }

    [Inject] OrderHeaderService_SQL orderHeaderService_SQL { get; set; }
    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    [Inject] IHttpContextAccessor HttpContextAccessor { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }
    #endregion

    #region fields
    public HttpContext? HttpContext { get; set; }
    private string userId;

    private List<OrderHeader> OrderHeader { get; set; } = [];
    private OrderHeader orderHeader { get; set; } = new();
    public ApplicationUser User { get; set; }

    #endregion

    protected override async Task OnInitializedAsync()
	{
		orderHeaderService_SQL.AbcDbConnection = AppSettingsHelper.AbcDbConnection;
		await LoadProducts();
	}

	private async Task LoadProducts()
	{

        OrderHeader = await orderHeaderService_SQL.GetOrdersList(applicationDbContext);


    }

}

