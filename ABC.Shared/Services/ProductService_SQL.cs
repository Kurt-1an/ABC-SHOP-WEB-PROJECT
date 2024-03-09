using Serilog;
using Dapper;
using MySql.Data.MySqlClient;
using ABC.Shared.Models;
using System.Net.Sockets;

namespace ABC.Shared.Services;

public partial class ProductService_SQL
{
    #region PRODUCTS CRUD
    //* GET ALL PRODUCTS
    private async Task<List<Product>> GetProductsListData(dynamic DBContext)
    {
        List<Product> _product = [];
        try
        {
            var context = DBContext;
            var productList = context.Products;
            foreach (var item in productList)
            {
                _product.Add(item);
            }
            return _product;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return _product;
        }
    }

    //* GETS SINGLE PRODUCT BASE ON PRODUCT ID
    private async Task<Product> GetProductData(dynamic DBContext, int id)
    {
        Product _product = new();
        try
        {
            var context = DBContext;
            var result = context.Products.Find(id);
            if (result is not null)
            {
                _product = result;
            }
            return _product;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return _product;
        }
    }

    //* ADDS PRODUCT TO DB
    private async Task<bool> AddProductData(dynamic DBContext, Product product)
    {
        try
        {
            var context = DBContext;
            context.Products.Add(product);
            var result = context.SaveChanges();
            return result > 0 ? true : false;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return false;
        }
    }

    //* UPDATE PRODUCT ON DB
    private async Task<bool> UpdateProductData(dynamic DBContext, Product product)
    {
        try
        {
            var context = DBContext;
            context.Products.Update(product);
            var result = context.SaveChanges();
            return result > 0 ? true : false;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return false;
        }
    }

    //* REMOVE/ARCHIVE PRODUCT FROM DB
    private async Task<bool> RemoveProductData(dynamic DBContext, Product product)
    {
        try
        {
            var context = DBContext;
            context.Products.Update(product);
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

    #region CATEGORIES CRUD
    //* GETS ALL CATEGORIES
    public async Task<List<Category>> GetCategoriesListData(dynamic DBContext)
    {
        List<Category> _category = [];
        try
        {
            var context = DBContext;
            var categoriesList = context.Categories;
            foreach (var item in categoriesList)
            {
                _category.Add(item);
            }
            return _category;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return _category;
        }
    }

    //* GETS SINGLE CATEGORY BASE ON ID 
    private async Task<Category> GetCategoryData(dynamic DBContext, int id)
    {
        Category _category = new();
        try
        {
            var context = DBContext;
            var result = context.Categories.Find(id);
            if (result is not null)
            {
                _category = result;
            }
            return _category;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return _category;
        }
    }
    #endregion
}