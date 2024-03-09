using Serilog;
using Dapper;
using MySql.Data.MySqlClient;
using ABC.Shared.Models;
using System.Net.Sockets;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Forms;

namespace ABC.Shared.Services;

public partial class POSService_SQL
{ 
    
    #region BATCH FETCH OF MARKET DATA: SECURITY INFORMATION AND SECURITY QUOTATION
    
    private async Task<List<Customer>> GetCustomerListData(dynamic DBContext)
    {
        List<Customer> _customer = [];
        try
        {
            var context = DBContext;
            var customerList = context.Customers;
            foreach(var item in customerList){
                _customer.Add(item);
            }
            return _customer;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return _customer;          
        }
    }

    private async Task<Customer> GetCustomerData(dynamic DBContext, string id)
    {
        Customer _customer = new();
        try
        {
            var context = DBContext;
            var result = context.Customers.Find(Guid.Parse(id));
            if(result is not null){
                _customer = result;
            }
            return _customer;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return _customer;          
        }
    }

    private async Task<List<Product>> GetProductsListData(dynamic DBContext)
    {
        List<Product> _product = [];
        try
        {
            var context = DBContext;
            var productsList = context.Products;
            foreach(var item in productsList){
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

    private async Task<Product> GetProductData(dynamic DBContext, int id)
    {
        Product _product = new();
        try
        {
            var context = DBContext;
            var result = context.Products.Find(id);
            if(result is not null){
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

    private async Task<bool> AddCustomerData(dynamic DBContext, Customer customer)
    {
        try
        {
            var context = DBContext;
            context.Customers.Add(customer);
            var result = context.SaveChanges();
            return result > 0 ? true: false;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return false;          
        }
    }


    
    #endregion
}