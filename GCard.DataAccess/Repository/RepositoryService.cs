using GCard.Model;
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

        public RepositoryService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            ItemTypeRepository = new Repository<ItemType>(_dbContext);
            OccasionRepository = new Repository<Occasion>(_dbContext);
            ProductItemRepository = new Repository<ProductItem>(_dbContext);
        }
        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
