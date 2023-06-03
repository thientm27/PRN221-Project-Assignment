using System;
using System.Collections.Generic;
using System.Windows;
using BussinessObject.Models;
using Repositories;
using Repositories.Implementation;

namespace TranMinhThienWPF
{
    public partial class FlowerEditor : Window
    {
        private readonly Action _onFinish;
        private readonly FlowerBouquet _updateFlower;
        private readonly bool _isUpdate;
        private readonly IFlowerBouquetRepository _flowerBouquetRepository = new FlowerBouquetRepository();
        private readonly ICategoryRepository _categoryRepository = new CategoryRepository();
        private readonly ISupplierRepository _supplierRepository = new SupplierRepository();

        private List<Category> _categories = new();
        private List<Supplier> _suppliers = new();

        public FlowerEditor()
        {
            InitializeComponent();
        }

        public FlowerEditor(FlowerBouquet? updateFlower, Action onFinishUpdate)
        {
            InitializeComponent();
            _onFinish = onFinishUpdate;

            if (updateFlower != null)
            {
                _updateFlower = updateFlower;
                ShowOldInformation();
                _isUpdate = true;
            }
            else
            {
                _isUpdate = false;
            }
        }


        private void ShowOldInformation()
        {
        }

        private void InitSupplierComboBox()
        {
            _suppliers = _supplierRepository.GetAllSupplier();
            List<string> displayString = new List<string>();
            displayString.Add("None");
            foreach (var supplier in _suppliers)
            {
                displayString.Add(supplier.SupplierName);
            }

            Supplier.ItemsSource = displayString;
        }

        private void InitCategoryComboBox()
        {
            _categories = _categoryRepository.GetAllCategory();
            List<string> displayString = new List<string>();
            foreach (var category in _categories)
            {
                displayString.Add(category.CategoryName);
            }

            Category.ItemsSource = displayString;
        }

        private int GetSupplierId()
        {
            if (Supplier.SelectedIndex == -1 || Supplier.SelectedIndex == 0)
            {
                return -1;
            }

            return _suppliers[Supplier.SelectedIndex - 1].SupplierId;
        }

        private int GetCategoryId()
        {
            if (Category.SelectedIndex == -1)
            {
                return -1;
            }

            return _categories[Supplier.SelectedIndex].CategoryId;
        }

        private void UpdateFlower()
        {
            try
            {
                var message = _flowerBouquetRepository.UpdateFlower(_updateFlower, GetCategoryId(), FlowerName.Text,
                    FlowerDes.Text, FlowerPrice.Text, FlowerUnitsInStock.Text, GetSupplierId());
                if (!string.IsNullOrEmpty(message)) // ERROR
                {
                    MessageBox.Show(message, "ERROR");
                    return;
                }

                MessageBox.Show("Update successfully");
                _onFinish?.Invoke();
                Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "ERROR");
            }
        }

        private void CreateFlower()
        {
            try
            {
                var message = _flowerBouquetRepository.CreateFlower(GetCategoryId(), FlowerName.Text,
                    FlowerDes.Text, FlowerPrice.Text, FlowerUnitsInStock.Text, GetSupplierId());
                if (!string.IsNullOrEmpty(message)) // ERROR
                {
                    MessageBox.Show(message, "ERROR");
                    return;
                }

                MessageBox.Show("Create successfully");
                _onFinish?.Invoke();
                Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "ERROR");
            }
        }

        #region Event

        private void Awake(object sender, RoutedEventArgs e)
        {
            InitCategoryComboBox();
            InitSupplierComboBox();
            if (_isUpdate)
            {
                FlowerName.Text = _updateFlower.FlowerBouquetName;
                FlowerDes.Text = _updateFlower.Description;
                FlowerPrice.Text = _updateFlower.UnitPrice.ToString();
                FlowerUnitsInStock.Text = _updateFlower.UnitsInStock.ToString();
                var index = -1;
                foreach (var cate in _categories)
                {
                    index++;
                    if (cate.CategoryId == _updateFlower.CategoryId)
                    {
                        Category.SelectedIndex = index;
                        break;
                    }
                }

                index = -1;
                foreach (var supplier in _suppliers)
                {
                    index++;
                    if (_updateFlower.SupplierId == null)
                    {
                        break;
                    }

                    if (supplier.SupplierId == _updateFlower.SupplierId)
                    {
                        Supplier.SelectedIndex = index;
                        break;
                    }
                }
            }
        }

        private void OnClickSubmit(object sender, RoutedEventArgs e)
        {
            if (_isUpdate)
            {
                UpdateFlower();
            }
            else
            {
                CreateFlower();
            }
        }

        #endregion

        private void FlowerEditor_OnUnloaded(object sender, RoutedEventArgs e)
        {
            _onFinish?.Invoke();
        }
    }
}