using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ABC.Shared.Models
{
	public class Product
	{
		[Key]
		public int Id { get; set; }
		public DateTime DateAdded { get; set; }
		public DateTime EndSaleDate { get; set; }

		//Product Image
		[DisplayName("For Inventory")]
		[ValidateNever]
		public string ImageUrl { get; set; }
		public long Barcode { get; set; }
		public string SKU { get; set; }
		[Required]
		[DisplayName("Product Name")]
		public string productName { get; set; }
		[DisplayName("Category")]
		public int CategoryId { get; set; }
		[ForeignKey("CategoryId")]
		[ValidateNever]
		public Category Category { get; set; }
		public string Brand { get; set; }
		//foreign key store
		[DisplayName("Store")]
		public int StoreId { get; set; }
		[ForeignKey("StoreId")]
		[ValidateNever]
		public Store Store { get; set; }

        [Required(ErrorMessage = "Please enter the Description.")]
        public string? Description { get; set; }
		//Pricing
		[DisplayName("Cost Price")]
		public float CostPrice { get; set; }
		[Required]
		[DisplayName("Retail Price")]
		public float RetailPrice { get; set; }
		//Inventory
		[Required]
		[DisplayName("Stock Quantity")]
		public int StockQuantity { get; set; }
		[Required]
		[DisplayName("Min. Stock Quantity")]
		public int MinimumStockQuantity { get; set; }
		//Warranty
		[Required]
		public string Type { get; set; }
		[Required]
		public string Duration { get; set; }
		[Required]
		public string Provider { get; set; }
		//Additional Notes
		[DisplayName("Additional Notes")]
		public string? addNotes { get; set; }
		//Foreign Key Relation of Supplier
		[DisplayName("Supplier")]
		public int SupplierId { get; set; }
		[ForeignKey("SupplierId")]
		[ValidateNever]
		public Supplier Supplier { get; set; }
		public bool? status { get; set; }


		[NotMapped]
		public int Quantity { get; set; }
	}
}

