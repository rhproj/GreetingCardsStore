using GCard.Model;
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
        void Save();
    }
}
