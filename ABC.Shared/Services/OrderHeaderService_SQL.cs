using Serilog;
using Dapper;
using MySql.Data.MySqlClient;
using ABC.Shared.Models;
using System.Net.Sockets;
using Mysqlx.Crud;

namespace ABC.Shared.Services;

public partial class OrderHeaderService_SQL
{

	#region ORDERHEADER CRUD

	//Get List of OrderHeaders
	private async Task<List<OrderHeader>> GetOrdersListData(dynamic DBContext)
	{
		List<OrderHeader> _order = [];
		try
		{
			var context = DBContext;
			var orderList = context.OrderHeaders;
			foreach (var item in orderList)
			{
				_order.Add(item);
			}
			return _order;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return _order;
		}
	}

	//Get Single OrderHeader from the list of OrderHeaders
	private async Task<OrderHeader> GetOrderHeaderData(dynamic DBContext, int orderId)
	{
		OrderHeader _orderHeader = new();
		try
		{
			var context = DBContext;
			var result = context.OrderHeaders.Find(orderId);
			if (result is not null)
			{
				_orderHeader = result;
			}
			return _orderHeader;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return _orderHeader;
		}
	}

	//Collection of OrderDetails based on OrderHeader id
	private async Task<List<OrderDetail>> GetOrderDetailsListData(dynamic DBContext, int id)
	{
		List<OrderDetail> _orderDetails = new List<OrderDetail>();
		try
		{
			var context = DBContext;
			var result = context.OrderDetail.ToList();
			foreach (var orderDetail in result)
			{
				if (orderDetail.OrderHeaderId == id)
				{
					_orderDetails.Add(orderDetail);
				}
			}

			return _orderDetails;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return _orderDetails;
		}
	}

	//GET SINGLE Order detail based on product id from the list of OrderDetails
	private async Task<OrderDetail> GetOrderDetailData(dynamic DBContext, int productId)
	{
		OrderDetail _orderDetail = new();
		try
		{
			var context = DBContext;
			var result = context.Products.Find(productId);
			if (result is not null)
			{
				_orderDetail = result;
			}
			return _orderDetail;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return _orderDetail;
		}
	}

	//ADD OrderHeader
	private async Task<bool> AddOrderHeaderData(dynamic DBContext, OrderHeader order)
	{
		try
		{
			var context = DBContext;
			context.OrderHeaders.Add(order);
			var result = context.SaveChanges();
			return result > 0 ? true : false;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return false;
		}
	}

    //ADD OrderDetails
    private async Task<bool> AddOrderDetailData(dynamic DBContext, List<OrderDetail> orderDetails)
    {
        try
        {
            var context = DBContext;

			foreach (var item in orderDetails)
			{
				context.OrderDetails.Add(item);
			}

            var result = context.SaveChanges();
            return result > 0 ? true : false;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return false;
        }
    }

    //* UPDATE Order Header status ON DB
    private async Task<bool> UpdateOrderHeaderStatusData(dynamic DBContext, OrderHeader order)
	{
		try
		{
			var context = DBContext;
			context.OrderHeader.Update(order);
			var result = context.SaveChanges();
			return result > 0 ? true : false;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return false;
		}
	}
	#endregion

}