using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.Entity.SqlServer;
using System.Text.RegularExpressions;

namespace WpfDataBase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        private bool IsItANewGood { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            UpdateCategoryList();
            UpdateProductList();
            FillDefaultValuesForProductManeger();

            UpdateGoodsList();
            FillDefaultValuesForInvenntoryManeger();


        }
        /*********  Product Mangment Tap   ***********/

        private void FillDefaultValuesForProductManeger()
        {
            CategorySearchTypeCB.Items.Add("Name");
            CategorySearchTypeCB.Items.Add("ID");
            CategorySearchTypeCB.SelectedIndex = 0;

            ProductSearchTypeCB.Items.Add("Name");
            ProductSearchTypeCB.Items.Add("ID");
            ProductSearchTypeCB.Items.Add("Serial Number");
            ProductSearchTypeCB.Items.Add("Category");
            ProductSearchTypeCB.Items.Add("Sale Price");
            ProductSearchTypeCB.Items.Add("Income Price");
            ProductSearchTypeCB.SelectedIndex = 0;



        }

        private void UpdateCategoryList()
        {
            using (SellEntities2 Context = new SellEntities2())
            {

                CategoriesDataGridVeiw.ItemsSource = Context.Categories.ToList();

                //Update Comobox
                CategoryCB.Items.Clear();
                CategoryCB_Sale.Items.Clear();
                CategoryCB_IM.Items.Clear();
                List<Category> categoriesName = Context.Categories.ToList();
                categoriesName.ForEach(c => CategoryCB.Items.Add(c.Name));
                categoriesName.ForEach(c => CategoryCB_Sale.Items.Add(c.Name));
                categoriesName.ForEach(c => CategoryCB_IM.Items.Add(c.Name));



            }
        }
        private void UpdateProductList()
        {
            using (SellEntities2 Context = new SellEntities2())
            {
                var query = from t in Context.Products
                            orderby t.Id
                            select new
                            {
                                t.Id,
                                ProductName = t.Name,
                                t.Quantity,
                                CategoryName = t.Category.Name
                            };

                ProductsDataGridVeiw.ItemsSource = query.ToList();
            }
        }

        private void ResetProductManeger()
        {
            CategoryNameTB.Text = "";
            CategorieSearchTB.Text = "Search...";
            UpdateCategoryList();
            ProductNameTB.Text = "";
            ProductSearchTB.Text = "Search...";
            UpdateProductList();


        }


        private void AddCategoryName_Click(object sender, RoutedEventArgs e)
        {
            using (SellEntities2 Context = new SellEntities2())
            {
                if (CategoryNameTB.Text == "" || CategoryNameTB.Text == " " || CategoryNameTB.Text == "  ")
                {
                    MessageBox.Show("Enter Category Name.");
                }
                else
                {
                    Context.Categories.Add(new Category { Name = CategoryNameTB.Text });
                    Context.SaveChanges();
                    UpdateCategoryList();
                }
            }
            ResetProductManeger();

        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            using (SellEntities2 Context = new SellEntities2())
            {
                //Category categoryName = 
                Product product = new Product
                {
                    Name = ProductNameTB.Text,
                    CategoryId = Context.Categories.FirstOrDefault(I => I.Name == CategoryCB.Text).Id
                };
                Context.Products.Add(product);
                Context.SaveChanges();
            }
            ResetProductManeger();


        }


        private void CategorieSearchTB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CategorieSearchTB.Text = "";
            UpdateCategoryList();
        }
        private void ProductSearchTB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ProductSearchTB.Text = "";
            UpdateProductList();
        }

        private void CategorySearchButton_Click(object sender, RoutedEventArgs e)
        {
            switch (CategorySearchTypeCB.Text)
            {
                case "Name":
                    using (SellEntities2 Context = new SellEntities2())
                    {
                        var query = from t in Context.Categories
                                    where t.Name.Contains(CategorieSearchTB.Text)
                                    orderby t.Id
                                    select new
                                    {
                                        t.Id,
                                        CategoryName = t.Name
                                    };

                        CategoriesDataGridVeiw.ItemsSource = query.ToList();
                    }
                    break;
                case "ID":
                    using (SellEntities2 Context = new SellEntities2())
                    {
                        var query = from t in Context.Categories
                                    where t.Id.ToString().Contains(CategorieSearchTB.Text)
                                    orderby t.Id
                                    select new
                                    {
                                        t.Id,
                                        CategoryName = t.Name
                                    };

                        CategoriesDataGridVeiw.ItemsSource = query.ToList();
                    }
                    break;
            }


            using (SellEntities2 Context = new SellEntities2())
            {
                var query = from t in Context.Categories
                            where t.Name.Contains(CategorieSearchTB.Text)
                            orderby t.Id
                            select new
                            {
                                t.Id,
                                CategoryName = t.Name
                            };

                CategoriesDataGridVeiw.ItemsSource = query.ToList();
            }
        }
        private void ProductSearchButton_Click(object sender, RoutedEventArgs e)
        {

            switch (ProductSearchTypeCB.Text)
            {
                case "Name":
                    using (SellEntities2 Context = new SellEntities2())
                    {
                        var query = from t in Context.Products
                                    where t.Name.Contains(ProductSearchTB.Text)
                                    orderby t.Id
                                    select new
                                    {
                                        t.Id,
                                        ProductName = t.Name,
                                        t.Quantity,
                                        CategoryName = t.Category.Name
                                    };

                        ProductsDataGridVeiw.ItemsSource = query.ToList();
                    }
                    break;
                case "ID":
                    using (SellEntities2 Context = new SellEntities2())
                    {
                        var query = from t in Context.Products
                                    where t.Id.ToString().Contains(ProductSearchTB.Text)
                                    orderby t.Id
                                    select new
                                    {
                                        t.Id,
                                        ProductName = t.Name,
                                        t.Quantity,
                                        CategoryName = t.Category.Name
                                    };

                        ProductsDataGridVeiw.ItemsSource = query.ToList();
                    }
                    break;

            }

        }
        private void CategoriesDataGridVeiw_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = CategoriesDataGridVeiw.SelectedItem as Category;
            if (selected != null)
                CategoryCB.Text = selected.Name;
            CategoriesDataGridVeiw.UnselectAll();
        }

        /*********  Sale Tap   ***********/

        private void QuantitySlider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            QuantityTextBox.Text = QuantitySlider.Value.ToString();

        }
        private void CategoryCB_Sale_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProductsCB_Sale.Items.Clear();
            using (SellEntities2 Context = new SellEntities2())
            {
                Category category = Context.Categories.FirstOrDefault(r => r.Name == CategoryCB_Sale.SelectedValue.ToString());
                List<Product> ProductsName = category.Products.ToList();
                ProductsName.ForEach(c => ProductsCB_Sale.Items.Add(c.Name));
            }
        }

        /*********  Inventory Maneger Tap   ***********/
        private void UpdateGoodsList()
        {
            //SearchInGoodsDataGridVeiwWithProductName(null);
        }
        private void FillDefaultValuesForInvenntoryManeger()
        {
            IsItANewGood = false;
            using(SellEntities2 Context = new SellEntities2())
                {
                List<Category> categories = Context.Categories.ToList();
                categories.ForEach(c => CategoryCB_IM.Items.Add(c.Name));

                List<Product> ProductsName = Context.Products.ToList();
                ProductsName.ForEach(c => ProductCB_IM.Items.Add(c.Name));
                
                }
        }
        private void ResetDefaultValues()
        {
            GoodsDataGridVeiw.ItemsSource = null;
            ProductCB_IM.Items.Clear();
            SerialTB_IM.Text = "";
            IncomePriceTB_IM.Text = "";
            SalePriceTB_IM.Text = "";
            QuantityTB_IM.Text = "0";
            IncomeQuantityTB_IM.Text = "";
            TotalQuantityTB_IM.Text = "";
            Serial2TB_IM.Text = "";
            CustomeCodeTB_IM.Text = "";
            DetailsTB_IM.Text = "";
            SerialSearchTB_IM.Text = "Search...";
        }
        
        private void SearchInGoodsDataGridVeiwWithSerialNumber(string SerialNumber)
        {
            if (SerialNumber == null)
            {
                using (SellEntities2 Context = new SellEntities2())
                {

                    GoodsDataGridVeiw.ItemsSource = Context.Goods.ToList();
                }
            }
            else
            {
                using (SellEntities2 Context = new SellEntities2())
                {
                    List<Good> goods = Context.Goods.ToList();
                    
                    GoodsDataGridVeiw.ItemsSource = Context.Goods.Where(r => r.SerialNumber.Contains(SerialNumber)).ToList();
                }
            }
        }
        private void SearchInGoodsDataGridVeiwWithProductName(string ProductName)
        {
            if (ProductName == null)
            {
                using (SellEntities2 Context = new SellEntities2())
                {
                    GoodsDataGridVeiw.ItemsSource = Context.Goods.ToList();
                 }
            }
            else
            {
                using (SellEntities2 Context = new SellEntities2())
                {
                    List<Good> goods = Context.Goods.Where(r => r.Product.Name.Contains(ProductName)).ToList();
                    GoodsDataGridVeiw.ItemsSource = goods;                 
                }
            }
        }

        private void CategoryCB_IM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResetDefaultValues();

            using (SellEntities2 Context = new SellEntities2())
            {
                Category category = Context.Categories.FirstOrDefault(r => r.Name == CategoryCB_IM.SelectedValue.ToString());
                List<Product> ProductsName = category.Products.ToList();
                ProductsName.ForEach(c => ProductCB_IM.Items.Add(c.Name));

            }
        }
        private void ProductCB_IM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductCB_IM.SelectedValue != null)
            {
                SearchInGoodsDataGridVeiwWithProductName(ProductCB_IM.SelectedValue.ToString());
            }
        }
        private void GoodsSearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchInGoodsDataGridVeiwWithSerialNumber(SerialSearchTB_IM.Text);
        }
        private void AddNewGood_Click(object sender, RoutedEventArgs e)
        {
            IsItANewGood = true;
            SerialTB_IM.Text = "";
            SerialTB_IM.IsReadOnly = false;

            IncomePriceTB_IM.Text = "";
            IncomePriceTB_IM.IsReadOnly = false;

            SalePriceTB_IM.Text = "";
            SalePriceTB_IM.IsReadOnly = false;

            Serial2TB_IM.Text = "";
            Serial2TB_IM.IsReadOnly = false;

            CustomeCodeTB_IM.Text = "";
            CustomeCodeTB_IM.IsReadOnly = false;

            DetailsTB_IM.Text = "";
            DetailsTB_IM.IsReadOnly = false;


            QuantityTB_IM.Text = "0";

            IncomeQuantityTB_IM.Text = "";
            TotalQuantityTB_IM.Text = "";

        }
        private void SerialSearchTB_IM_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SerialSearchTB_IM.Text = "";
        }
        private void GoodsDataGridVeiw_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = GoodsDataGridVeiw.SelectedItem as Good;
            if (selected != null)
            {
                SerialTB_IM.Text = selected.SerialNumber;
                IncomePriceTB_IM.Text = selected.IncimePrice;
                SalePriceTB_IM.Text = selected.SalePrice;
                QuantityTB_IM.Text = selected.Quantity;
                Serial2TB_IM.Text = selected.SerialNumber2;
                CustomeCodeTB_IM.Text = selected.CustomCode;
                DetailsTB_IM.Text = selected.Details;
                IncomeQuantityTB_IM.Text = "";
                TotalQuantityTB_IM.Text = "";
            }
            CategoriesDataGridVeiw.UnselectAll();
        }
        private void IncomeQuantityTB_IM_TextChanged(object sender, TextChangedEventArgs e)
        {
            IncomeQuantityTB_IM.Text = Regex.Replace(IncomeQuantityTB_IM.Text, "[^0-9]+", "");
            if (IncomeQuantityTB_IM.Text.Length != 0 && QuantityTB_IM.Text.Length != 0)
            {
                TotalQuantityTB_IM.Text = (int.Parse(QuantityTB_IM.Text) + int.Parse(IncomeQuantityTB_IM.Text)).ToString();
            }
            else
                TotalQuantityTB_IM.Text = "";
        }

        

        private void ConfermButton_IM_Click(object sender, RoutedEventArgs e)
        {
            if (IsItANewGood)
            {
                using (SellEntities2 Context = new SellEntities2())
                {
                    Good good = new Good
                    {
                        ProductId = Context.Products.FirstOrDefault(r => r.Name == ProductCB_IM.SelectedValue.ToString()).Id,
                        SerialNumber = SerialTB_IM.Text,
                        IncimePrice = IncomePriceTB_IM.Text,
                        SalePrice = SalePriceTB_IM.Text,
                        SerialNumber2 = Serial2TB_IM.Text,
                        CustomCode = CustomeCodeTB_IM.Text,
                        Details = DetailsTB_IM.Text,
                        Quantity = IncomeQuantityTB_IM.Text

                    };
                    Context.Goods.Add(good);
                    Context.SaveChanges();
                }
                SearchInGoodsDataGridVeiwWithProductName(ProductCB_IM.SelectedValue.ToString());

                IsItANewGood = false;
                SerialTB_IM.Text = "";
                SerialTB_IM.IsReadOnly = true;

                IncomePriceTB_IM.Text = "";
                IncomePriceTB_IM.IsReadOnly = true;

                SalePriceTB_IM.Text = "";
                SalePriceTB_IM.IsReadOnly = true;

                Serial2TB_IM.Text = "";
                Serial2TB_IM.IsReadOnly = true;

                CustomeCodeTB_IM.Text = "";
                CustomeCodeTB_IM.IsReadOnly = true;

                DetailsTB_IM.Text = "";
                DetailsTB_IM.IsReadOnly = true;


                QuantityTB_IM.Text = "0";

                IncomeQuantityTB_IM.Text = "";
                TotalQuantityTB_IM.Text = "";
            }
            else
            {
                if (int.Parse(IncomeQuantityTB_IM.Text) > 0)
                {
                    using (SellEntities2 Context = new SellEntities2())
                    {
                        Good good = Context.Goods.FirstOrDefault(r => r.SerialNumber == SerialTB_IM.Text);
                        good.Quantity = int.Parse(TotalQuantityTB_IM.Text).ToString();
                        Context.SaveChanges();
                    }
                    SearchInGoodsDataGridVeiwWithProductName(ProductCB_IM.SelectedValue.ToString());
                }
                else { MessageBox.Show("You Didn't increase the number of Quantities"); }
            }
        }
        private void CancelButton_IM_Click(object sender, RoutedEventArgs e)
        {
            IsItANewGood = false;
            SerialTB_IM.Text = "";
            SerialTB_IM.IsReadOnly = true;

            IncomePriceTB_IM.Text = "";
            IncomePriceTB_IM.IsReadOnly = true;

            SalePriceTB_IM.Text = "";
            SalePriceTB_IM.IsReadOnly = true;

            Serial2TB_IM.Text = "";
            Serial2TB_IM.IsReadOnly = true;

            CustomeCodeTB_IM.Text = "";
            CustomeCodeTB_IM.IsReadOnly = true;

            DetailsTB_IM.Text = "";
            DetailsTB_IM.IsReadOnly = true;


            QuantityTB_IM.Text = "0";

            IncomeQuantityTB_IM.Text = "";
            TotalQuantityTB_IM.Text = "";
        }
        // Tap Buttons Click
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);
            GridCursor.Margin = new Thickness(10 + (160 * index), 0, 0, 0);

            switch (index)
            {
                case 0:
                    ProductMangerGrid.Visibility = Visibility.Collapsed;
                    SaleGrid.Visibility = Visibility.Collapsed;
                    InventoryMangerGrid.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    ProductMangerGrid.Visibility = Visibility.Collapsed;
                    SaleGrid.Visibility = Visibility.Visible;
                    InventoryMangerGrid.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    ProductMangerGrid.Visibility = Visibility.Collapsed;
                    SaleGrid.Visibility = Visibility.Collapsed;
                    InventoryMangerGrid.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    ProductMangerGrid.Visibility = Visibility.Visible;
                    SaleGrid.Visibility = Visibility.Collapsed;
                    InventoryMangerGrid.Visibility = Visibility.Collapsed;
                    break;
                case 4:
                    ProductMangerGrid.Visibility = Visibility.Collapsed;
                    SaleGrid.Visibility = Visibility.Collapsed;
                    InventoryMangerGrid.Visibility = Visibility.Visible;
                    break;
                case 5:
                    ProductMangerGrid.Visibility = Visibility.Collapsed;
                    SaleGrid.Visibility = Visibility.Collapsed;
                    InventoryMangerGrid.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        
    }

}
