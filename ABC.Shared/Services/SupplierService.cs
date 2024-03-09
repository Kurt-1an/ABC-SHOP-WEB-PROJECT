using System;
using Serilog;
using ABC.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace ABC.Shared.Services;
public partial class SupplierService_SQL : ComponentBase
{
	#region FIELDS
	public String AbcDbConnection { get; set; } = String.Empty;


	#endregion

	#region SUPPLIER CRUD
	public async Task<List<Supplier>> GetSupplierList(dynamic DBContext)
	{
		List<Supplier> SupplierList = [];
		try
		{
			SupplierList = await GetSupplierListData(DBContext);
			return SupplierList;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return SupplierList;
		}
	}

	public async Task<Supplier> GetSupplierInfo(dynamic DBContext, int Id)
	{
		Supplier SupplierInfo = new();
		try
		{
			SupplierInfo = await GetSupplierData(DBContext, Id);
			return SupplierInfo;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return SupplierInfo;
		}
	}

	// ADD SUPPLIER
	public async Task<bool> AddSupplier(dynamic DBContext, Supplier supplier)
	{
		bool added = false;
		try
		{
			added = await AddSupplierData(DBContext, supplier);
			return added;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return added;
		}
	}

	// UPDATE SUPPLIER
	public async Task<bool> UpdateSupplier(dynamic DBContext, Supplier supplier)
	{
		bool updated = false;
		try
		{
			updated = await UpdateSupplierData(DBContext, supplier);
			return updated;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return updated;
		}
	}

	// REMOVE SUPPLIER
	public async Task<bool> RemoveSupplier(dynamic DBContext, Supplier supplier)
	{
		bool removed = false;
		try
		{
			removed = await RemoveSupplierData(DBContext, supplier);
			return removed;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return removed;
		}
	}
	#endregion
}

