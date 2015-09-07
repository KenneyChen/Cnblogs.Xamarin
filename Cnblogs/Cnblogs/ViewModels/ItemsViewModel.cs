using System;
using System.Diagnostics;
using System.Threading.Tasks;

using App96.Helpers;
using App96.Models;
using App96.Views;

using Xamarin.Forms;
using Cnblogs.Service;

namespace App96.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableRangeCollection<CnBlogsEntry> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "博客园";
            Items = new ObservableRangeCollection<CnBlogsEntry>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            //MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            //{
            //	var _item = item as Item;
            //	Items.Add(_item);
            //	await DataStore.AddItemAsync(_item);
            //});
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await FeedService.SendRequst<NewsFeed>(ServiceType.Home, new NewsData { PageIndex = 1, PageSize = 10 });
                if (items != null)
                {
                    Items.ReplaceRange(items.Items);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load items.",
                    Cancel = "OK"
                }, "message");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}