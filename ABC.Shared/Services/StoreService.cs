using System;
using Serilog;
using ABC.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace ABC.Shared.Services;
public partial class StoreService_SQL : ComponentBase
{
	#region FIELDS
	public String AbcDbConnection { get; set; } = String.Empty;


	#endregion

	#region SUPPLIER CRUD
	public async Task<List<Store>> GetStoreList(dynamic DBContext)
	{
		List<Store> StoreList = [];
		try
		{
			StoreList = await GetStoreListData(DBContext);
			return StoreList;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return StoreList;
		}
	}

	public async Task<Store> GetStoreInfo(dynamic DBContext, int Id)
	{
		Store StoreInfo = new();
		try
		{
			StoreInfo = await GetStoreData(DBContext, Id);
			return StoreInfo;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return StoreInfo;
		}
	}

	// ADD SUPPLIER
	public async Task<bool> AddStore(dynamic DBContext, Store supplier)
	{
		bool added = false;
		try
		{
			added = await AddStoreData(DBContext, supplier);
			return added;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return added;
		}
	}

	// UPDATE SUPPLIER
	public async Task<bool> UpdateStore(dynamic DBContext, Store supplier)
	{
		bool updated = false;
		try
		{
			updated = await UpdateStoreData(DBContext, supplier);
			return updated;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return updated;
		}
	}

	// REMOVE SUPPLIER
	public async Task<bool> RemoveStore(dynamic DBContext, Store supplier)
	{
		bool removed = false;
		try
		{
			removed = await RemoveStoreData(DBContext, supplier);
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

