using System;
using Serilog;
using ABC.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace ABC.Shared.Services;
public partial class ProductService_SQL : ComponentBase
{
    #region DICTIONARIES 
    #endregion

    #region FIELDS
    public String AbcDbConnection { get; set; } = String.Empty;


    #endregion

    #region PRODUCTS CRUD
    public async Task<Product> GetProductInfo(dynamic DBContext, int Id)
    {
        Product ProductInfo = new();
        try
        {
            ProductInfo = await GetProductData(DBContext, Id);
            return ProductInfo;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return ProductInfo;
        }
    }

    public async Task<List<Product>> GetProductList(dynamic DBContext)
    {
        List<Product> ProductList = [];
        try
        {
            ProductList = await GetProductsListData(DBContext);
            return ProductList;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return ProductList;
        }
    }

    // ADD PRODUCT
    public async Task<bool> AddProduct(dynamic DBContext, Product product)
    {
        bool added = false;
        try
        {
            added = await AddProductData(DBContext, product);
            return added;
        }
        catch(Exception ex)
        {
            Log.Error(ex.ToString());
            return added;
        }
    }

    // UPDATE PRODUCT
    public async Task<bool> UpdateProduct(dynamic DBContext, Product product)
    {
        bool updated = false;
        try
        {
            updated = await UpdateProductData(DBContext, product);
            return updated;
        }
        catch(Exception ex)
        {
            Log.Error(ex.ToString());
            return updated;
        }
    }

    // REMOVE PRODUCT
    public async Task<bool> RemoveProduct(dynamic DBContext, Product product)
    {
        bool removed = false;
        try
        {
            removed = await RemoveProductData(DBContext, product);
            return removed;
        }
        catch(Exception ex)
        {
            Log.Error(ex.ToString());
            return removed;
        }
    }

    #endregion

    #region CATEGORIES CRUD
    public async Task<List<Category>> GetCategoryList(dynamic DBContext)
    {
        List<Category> CategoryList = [];
        try
        {
            CategoryList = await GetCategoriesListData(DBContext);
            return CategoryList;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return CategoryList;
        }
    }

    public async Task<Category> GetCategoryInfo(dynamic DBContext, int Id)
    {
        Category CategoryInfo = new();
        try
        {
            CategoryInfo = await GetCategoryData(DBContext, Id);
            return CategoryInfo;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return CategoryInfo;
        }
    }
    #endregion
}