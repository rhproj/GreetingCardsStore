using GCard.Model;
using GCard.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCard.DataAccess.Repository
{
    public interface IRepositoryService
    {
        IRepository<ItemType> ItemTypeRepository { get; }
        IRepository<Occasion> OccasionRepository { get; }
        IRepository<ProductItem> ProductItemRepository { get; }
        //IRepository<ProductItemVM> ProductItemVMRepository { get; }
        //IRepository<ShoppingCart> ShoppingCartRepository { get; }
        IShoppingCartRepo ShoppingCartRepository { get; }
        void Save();
    }
}
