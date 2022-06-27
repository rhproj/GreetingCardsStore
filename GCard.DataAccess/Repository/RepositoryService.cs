using GCard.Model;
using GCard.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCard.DataAccess.Repository
{
    public class RepositoryService : IRepositoryService
    {
        private readonly ApplicationDbContext _dbContext;
        public IRepository<ItemType> ItemTypeRepository { get; }
        public IRepository<Occasion> OccasionRepository { get; }
        public IRepository<ProductItem> ProductItemRepository { get; }
        public IRepository<ProductItemVM> ProductItemVMRepository { get; }

        public RepositoryService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            ItemTypeRepository = new Repository<ItemType>(_dbContext);
            OccasionRepository = new Repository<Occasion>(_dbContext);
            ProductItemRepository = new Repository<ProductItem>(_dbContext);
            ProductItemVMRepository = new Repository<ProductItemVM>(_dbContext);
        }
        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
