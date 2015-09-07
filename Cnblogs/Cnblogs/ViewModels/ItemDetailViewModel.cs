using App96.Models;
using Cnblogs.Service;

namespace App96.ViewModels
{
	public class ItemDetailViewModel : BaseViewModel
	{
		public CnBlogsEntry Item { get; set; }
		public ItemDetailViewModel(CnBlogsEntry item = null)
		{
			Title = item.Title;
			Item = item;
		}

		int quantity = 1;
		public int Quantity
		{
			get { return quantity; }
			set { SetProperty(ref quantity, value); }
		}
	}
}