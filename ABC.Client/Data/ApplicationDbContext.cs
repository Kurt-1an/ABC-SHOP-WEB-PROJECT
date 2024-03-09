using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ABC.Shared.Models;
using Microsoft.AspNetCore.Identity;
using ABC.Shared.Utility;

namespace ABC.Client.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
	//Create Database
	public DbSet<Category> Categories { get; set; }
	public DbSet<Product> Products { get; set; }
	public DbSet<Supplier> Suppliers { get; set; }
	public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
	public DbSet<Customer> Customers { get; set; }
	public DbSet<ShoppingCart> ShoppingCarts { get; set; }
	public DbSet<ApplicationUser> ApplicationUsers { get; set; }
	public DbSet<OrderHeader> OrderHeaders { get; set; }
	public DbSet<OrderDetail> OrderDetails { get; set; }

	public DbSet<AuditLog> AuditLogs { get; set; }
	public DbSet<Content> Contents { get; set; }

	public DbSet<Store> Stores { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{

		base.OnModelCreating(modelBuilder);


		// ASP user IDs
		string adminId = Guid.NewGuid().ToString();
		string empId = Guid.NewGuid().ToString();
		string custId = Guid.NewGuid().ToString();

		// ASP role IDs
		string adminRoleId = Guid.NewGuid().ToString();
		string empRoleId = Guid.NewGuid().ToString();
		string custRoleId = Guid.NewGuid().ToString();

		// Seed roles 
		modelBuilder.Entity<IdentityRole>().HasData(
		  new IdentityRole
		  {
			  Id = adminRoleId,
			  Name = "Admin",
			  NormalizedName = "ADMIN"
		  },
		  new IdentityRole
		  {
			  Id = empRoleId,
			  Name = "Employee",
			  NormalizedName = "EMPLOYEE"
		  },
		  new IdentityRole
		  {
			  Id = custRoleId,
			  Name = "Customer",
			  NormalizedName = "CUSTOMER"
		  }
		);

		//a hasher to hash the password before seeding the user to the db
		var passwordHasher = new PasswordHasher<IdentityUser>();

		// Create admin user
		var adminUser = new ApplicationUser
		{
			Id = adminId,
			UserName = "admin@abc.com",
			FirstName = "Ej Admin",
			LastName = "Esan",
			NormalizedUserName = "ADMIN@ABC.COM",
			Email = "admin@abc.com",
			EmailConfirmed = true,
			PasswordHash = passwordHasher.HashPassword(null, "Admin123!")
		};

		// Create employee user
		var empUser = new ApplicationUser
		{
			Id = empId,
			UserName = "emp@abc.com",
			FirstName = "Ej Employee",
			LastName = "Esan",
			NormalizedUserName = "EMP@ABC.COM",
			Email = "emp@abc.com",
			EmailConfirmed = true,
			PasswordHash = passwordHasher.HashPassword(null, "Emp123!")
		};

		// Create customer user
		var custUser = new ApplicationUser
		{
			Id = custId,
			UserName = "cust@abc.com",
			FirstName = "Ej Customer",
			LastName = "Esan",
			NormalizedUserName = "CUST@ABC.COM",
			Email = "cust@abc.com",
			EmailConfirmed = true,
			PasswordHash = passwordHasher.HashPassword(null, "Cust123!")
		};

		// Seed users
		modelBuilder.Entity<ApplicationUser>().HasData(
		  adminUser, empUser, custUser
		);

		// Seed user-role relations
		modelBuilder.Entity<IdentityUserRole<string>>().HasData(
		  new IdentityUserRole<string> { UserId = adminId, RoleId = adminRoleId },
		  new IdentityUserRole<string> { UserId = empId, RoleId = empRoleId },
		  new IdentityUserRole<string> { UserId = custId, RoleId = custRoleId }
		);


		////Pushed Data into Category Database
		modelBuilder.Entity<Category>().HasData(
			new Category { Id = 1, Name = "CCTV"},
			new Category { Id = 2, Name = "Printers"},
			new Category { Id = 3, Name = "Computer Accesories"},
			new Category { Id = 4, Name = "Cables & Tools"}
			);

		//Pushed Data into PurchaseOrder Database
		modelBuilder.Entity<PurchaseOrder>().HasData(
			new PurchaseOrder { Id = 1, SupplierName = "Kurt", LocationDelivery = "Pasig", PaymentTerm = "Cash", ExpectedDeliveryDate = new DateTime(2023, 10, 21), EmployeeName = "Neil", ContactNumber = 09568271611, ShipmentPreference = "Cash On Delivery", AdditionalNote = "additional note goes in here" }
			);


		// Create GUID for customer
		Guid customerGuid = Guid.NewGuid();

		// Create Customer instance
		Customer customer = new Customer
		{
			Id = customerGuid,
			FirstName = "Kurt",
			LastName = "Betonio",
			EmailAddress = "neiljejomar@gmail.com",
			ContactNumber = 09568271611,
			Type = "Walk in",
			ApSuUn = "Unit 1234",
			StreetorSubd = "Jasmine St.",
			Barangay = "Batman",
			City = "Antipolo",
			Province = "Rizal",
			ZipCode = 1870
		};

		// Add to modelBuilder
		modelBuilder.Entity<Customer>().HasData(customer);

		//Pushed Data into Product Database
		modelBuilder.Entity<Product>().HasData(
			new Product
			{
				Id = 1,
				Barcode = 0832175698,
				SKU = "printer-AllInOne-XYZ123",
				productName = "XYZ123 All-in-One Printer",
				CategoryId = 1,
				Brand = "HP",
				StoreId = 1,
				Description = "Versatile all-in-one printer for printing, copying, and scanning",
				CostPrice = 800,
				RetailPrice = 1299,
				StockQuantity = 20,
				MinimumStockQuantity = 5,
				Type = "Extended Warranty",
				Duration = "12 months from date of purchase",
				Provider = "Third-Party Warranty Company",
				addNotes = "Additional Notes is here color touchscreen interface ",
				SupplierId = 2,
				ImageUrl = ""

			},
			new Product
			{
				Id = 2,
				Barcode = 0954532414,
				SKU = "cctv-SmartCam-360",
				productName = "SmartCam 360 Security Camera",
				CategoryId = 2,
				Brand = "Samsung",
				StoreId = 1,
				Description = "Panoramic view with motion detection",
				CostPrice = 1200,
				RetailPrice = 1999,
				StockQuantity = 15,
				MinimumStockQuantity = 4,
				Type = "Manufacturers Warranty",
				Duration = "7 days from date of purchase",
				Provider = "Manufacturer",
				SupplierId = 1,
				ImageUrl = ""
			},
			new Product
			{
				Id = 3,
				Barcode = 123456789013,
				SKU = "laptop-ultrabook-ABC789",
				productName = "ABC789 13-inch Laptop",
				CategoryId = 4,
				Brand = "Dell",
				StoreId = 1,

				Description = "Lightweight 13-inch laptop with SSD and 8GB RAM",
				CostPrice = 600,
				RetailPrice = 899,
				StockQuantity = 8,
				MinimumStockQuantity = 3,
				Type = "Extended Warranty",
				Duration = "12 months from date of purchase",

				Provider = "Third-Party Warranty Company",
				addNotes = "Backlit keyboard, Windows 10",
				SupplierId = 2,
				ImageUrl = ""
			},
			new Product
			{
				Id = 4,
				Barcode = 123456789014,

				SKU = "phone-smartphone-XYZ101",
				productName = "XYZ101 Smartphone",
				CategoryId = 4,
				Brand = "Apple",

				StoreId = 1,
				Description = "5.8-inch OLED smartphone with dual camera",
				CostPrice = 500,
				RetailPrice = 999,
				StockQuantity = 12,
				MinimumStockQuantity = 5,
				Type = "Extended Warranty",
				Duration = "24 months from date of purchase",
				Provider = "Manufacturer Warranty",
				addNotes = "Facial recognition, water resistant",

				SupplierId = 1,
				ImageUrl = ""
			},
			new Product
			{
				Id = 5,
				Barcode = 123456789015,
				SKU = "headphones-wireless-XYZ222",

				productName = "XYZ222 Wireless Headphones",
				CategoryId = 4,
				Brand = "Bose",
				StoreId = 1,
				Description = "Noise cancelling wireless over-ear headphones",
				CostPrice = 150,

				RetailPrice = 249,
				StockQuantity = 20,
				MinimumStockQuantity = 5,
				Type = "Extended Warranty",
				Duration = "12 months from date of purchase",
				Provider = "Third-Party Warranty Company",
				addNotes = "Bluetooth, 30+ hour battery life",
				SupplierId = 2,
				ImageUrl = ""
			});

		//Pushed Data into Supplier Database
		modelBuilder.Entity<Supplier>().HasData(
			new Supplier
			{
				Id = 1,
				supplierCompanyName = "Addvert",
				supplierContactNumber = 09651232235,
				supplierEmail = "addvert214@gmail.com",
				supplierStatus = "Active",
				supplierDescription = "N/A",
				supplierLotBlk = "c4 l5",
				supplierStreetSubdv = "E. Corazon",
				supplierBarangay = "Maybancal",
				supplierCity = "Tanay",
				supplierProvince = "Rizal",
				supplierZipCode = 1870,
				supplierNote = "My supplier"
			},

			new Supplier
			{
				Id = 2,
				supplierCompanyName = "Addvert",
				supplierContactNumber = 09651232235,
				supplierEmail = "addvert214@gmail.com",
				supplierStatus = "Active",
				supplierDescription = "N/A",
				supplierLotBlk = "c4 l5",
				supplierStreetSubdv = "E. Corazon",
				supplierBarangay = "Maybancal",
				supplierCity = "Tanay",
				supplierProvince = "Rizal",
				supplierZipCode = 1870,
				supplierNote = "My supplier"
			});

		//Pushed Data into Store Database
		modelBuilder.Entity<Store>().HasData(
			new Store
			{
				Id = 1,
				storeName = "Abiz Head",
				storeContactNumber = 09651232235,
				storeEmail = "abiz214@gmail.com",
				storeStatus = "Active",
				storeLotBlk = "c4 l5",
				storeStreetSubdv = "E. Corazon",
				storeBarangay = "Maybancal",
				storeCity = "Tanay",
				storeProvince = "Rizal",
				storeZipCode = 1870
			});

		modelBuilder.Entity<OrderHeader>().HasData(
		  new OrderHeader
		  {
			  Id = 1,
			  ApplicationUserId = custUser.Id,
			  OrderDate = DateTime.Parse("2023-12-23 19:03:02.3832563"),
			  ShippingDate = DateTime.Parse("2023-12-23 19:04:08.7255290"),
			  OrderTotal = 10366,
			  OrderStatus = "Completed",
			  PaymentStatus = "Paid",
			  TrackingNumber = "123123123",
			  Carrier = "Neil",
			  Discount = 50,
			  ServiceFee = 50,
			  DeliveryFee = 250,
			  PaymentMode = "Cash",
			  OfficialReceipt = "ABC0001",
			  CustomerId = customerGuid

		  });

		modelBuilder.Entity<OrderDetail>().HasData(
		  new OrderDetail
		  {
			  Id = 1,
			  OrderHeaderId = 1,
			  ProductId = 1,
			  Charge = null,
			  Discount = null,
			  Count = 1,
			  Price = 899
		  });

		//modelBuilder.Entity<ShoppingCart>().HasData(
		//       new ShoppingCart
		//       {
		//        Id = 1,
		//        ProductName = "XYZ222 Wireless Headphones",
		//        Quantity = 1,
		//        Price = 249,
		//              ApplicationUserId = custUser.Id
		//       });

	}
}
