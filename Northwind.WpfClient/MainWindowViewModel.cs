// MainWindowViewModel.cs

namespace Northwind.WpfClient
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Net.Http;
    using System.Windows;
    using Common;
    using Northwind.Common;
    using WebApi2Services.Dto;

    internal class MainWindowViewModel : ViewModelBase
    {
        private ObservableCollectionEx<CategoryListItemDto> _categoryList;
        private RelayCommand _loadCategoryCommand;
        private bool _canExecuteLoadCategory = true;

        public RelayCommand LoadCategoryCommand
        {
            get
            {
                if (_loadCategoryCommand == null)
                {
                    _loadCategoryCommand = new RelayCommand("加载分类", OnLoadCategory, CanExecuteLoadCategory);
                }
                return _loadCategoryCommand;
            }
        }

        public ObservableCollectionEx<CategoryListItemDto> CategoryList
        {
            get
            {
                if (_categoryList == null)
                {
                    _categoryList = new ObservableCollectionEx<CategoryListItemDto>();
                }
                return _categoryList;
            }
            set
            {
                ContractUtil.RequiresNotNull(value, "value");
                SetAndRaisePropertyChanged(ref _categoryList, value);
            }
        }

        private async void OnLoadCategory(object obj)
        {
            _canExecuteLoadCategory = false;

            try
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(NorthwindApi.StringGetCategoryList);
                    response.EnsureSuccessStatusCode(); // Throw on error code.

                    var categories = await response.Content.ReadAsAsync<IEnumerable<CategoryListItemDto>>();
                    _categoryList.CopyFrom(categories);
                }
            }
            catch (Newtonsoft.Json.JsonException jEx)
            {
                // This exception indicates a problem deserializing the request body.
                MessageBox.Show(jEx.Message);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _canExecuteLoadCategory = true;
            }
        }

        private bool CanExecuteLoadCategory(object obj)
        {
            return _canExecuteLoadCategory;
        }
    }
}