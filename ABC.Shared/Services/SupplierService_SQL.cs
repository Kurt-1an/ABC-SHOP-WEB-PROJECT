using Serilog;
using Dapper;
using MySql.Data.MySqlClient;
using ABC.Shared.Models;
using System.Net.Sockets;

namespace ABC.Shared.Services;
public partial class SupplierService_SQL
{
	#region SUPPLIER CRUD
	//* GETS ALL SUPPLIERS
	public async Task<List<Supplier>> GetSupplierListData(dynamic DBContext)
	{
		List<Supplier> _supplier = [];
		try
		{
			var context = DBContext;
			var suppliersList = context.Suppliers;
			foreach (var item in suppliersList)
			{
				_supplier.Add(item);
			}
			return _supplier;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return _supplier;
		}
	}

	//* GETS SINGLE CATEGORY BASE ON ID 
	private async Task<Supplier> GetSupplierData(dynamic DBContext, int id)
	{
		Supplier _supplier = new();
		try
		{
			var context = DBContext;
			var result = context.Suppliers.Find(id);
			if (result is not null)
			{
				_supplier = result;
			}
			return _supplier;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return _supplier;
		}
	}

	//* ADDS SUPPLIER TO DB
	private async Task<bool> AddSupplierData(dynamic DBContext, Supplier supplier)
	{
		try
		{
			var context = DBContext;
			context.Suppliers.Add(supplier);
			var result = context.SaveChanges();
			return result > 0 ? true : false;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return false;
		}
	}

	//* UPDATE SUPPLIER ON DB
	private async Task<bool> UpdateSupplierData(dynamic DBContext, Supplier supplier)
	{
		try
		{
			var context = DBContext;
			context.Suppliers.Update(supplier);
			var result = context.SaveChanges();
			return result > 0 ? true : false;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return false;
		}
	}

	//* REMOVE/ARCHIVE SUPPLIER FROM DB
	private async Task<bool> RemoveSupplierData(dynamic DBContext, Supplier supplier)
	{
		try
		{
			var context = DBContext;
			context.Suppliers.Update(supplier);
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

