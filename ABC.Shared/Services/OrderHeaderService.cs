using System;
using Serilog;
using ABC.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace ABC.Shared.Services;
public partial class OrderHeaderService_SQL : ComponentBase
{
	#region DICTIONARIES 
	#endregion

	#region FIELDS
	public String AbcDbConnection { get; set; } = String.Empty;


    #endregion

    #region ORDERHEADER CRUD

    public async Task<List<OrderHeader>> GetOrdersList(dynamic DBContext)
    {
        List<OrderHeader> OrdersList = new List<OrderHeader>();
        try
        {
            OrdersList = await GetOrdersListData(DBContext);

            // Load ApplicationUser data for each OrderHeader
            foreach (var order in OrdersList)
            {
                order.ApplicationUser = await DBContext.ApplicationUsers.FindAsync(order.ApplicationUserId);
            }

            return OrdersList;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return OrdersList;
        }
    }



	//Get Single OrderHeader from the list of OrderHeaders
	public async Task<OrderHeader> GetOrderHeader(dynamic DBContext, int orderId)
	{
		OrderHeader OrderHeader = new();
		try
		{
			OrderHeader = await GetOrderHeaderData(DBContext, orderId);

			// Load ApplicationUser data for the OrderHeader
			OrderHeader.ApplicationUser = await DBContext.ApplicationUsers.FindAsync(OrderHeader.ApplicationUserId);

			return OrderHeader;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return OrderHeader;
		}
	}

	//Collection of OrderDetails based on OrderHeader id
	public async Task<List<OrderDetail>> GetOrderDetailsList(dynamic DBContext)
	{
		List<OrderDetail> OrderDetailsList = [];
		try
		{
			OrderDetailsList = await GetOrdersListData(DBContext);
			return OrderDetailsList;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return OrderDetailsList;
		}
	}


	//GET SINGLE Order detail based on product id from the list of OrderDetails
	public async Task<OrderDetail> GetOrderDetail(dynamic DBContext, int productId)
	{
		OrderDetail _OrderDetail = new();
		try
		{
			_OrderDetail = await GetOrderHeaderData(DBContext, productId);
			return _OrderDetail;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return _OrderDetail;
		}
	}

	//ADD OrderHeader
	public async Task<bool> AddOrderHeader(dynamic DBContext, OrderHeader order)
	{
		bool added = false;
		try
		{
			added = await AddOrderHeaderData(DBContext, order);
			return added;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return added;
		}
	}



	//ADD OrderDetail
	public async Task<bool> AddOrderDetail(dynamic DBContext, List<OrderDetail> order)
	{
		bool added = false;
		try
		{
			added = await AddOrderDetailData(DBContext, order);
			return added;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return added;
		}
	}

	// UPDATE Order
	public async Task<bool> UpdateOrderHeaderStatus(dynamic DBContext, OrderHeader order)
	{
		bool updated = false;
		try
		{
			updated = await UpdateOrderHeaderStatusData(DBContext, order);
			return updated;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return updated;
		}
	}

	#endregion
}