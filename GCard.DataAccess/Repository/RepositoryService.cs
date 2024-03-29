﻿using GCard.Model;
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
        public IShoppingCartRepo ShoppingCartRepository { get; }
        public IOrderHeaderRepo OrderHeaderRepository { get; }
        public IRepository<OrderDetails> OrderDetailRepository { get; }
        public IRepository<ApplicationUser> ApplicationUserRepository { get; }

        public RepositoryService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            ItemTypeRepository = new Repository<ItemType>(_dbContext);
            OccasionRepository = new Repository<Occasion>(_dbContext);
            ProductItemRepository = new Repository<ProductItem>(_dbContext);
            ShoppingCartRepository = new ShoppingCartRepo(_dbContext); //Repository<ShoppingCart>(_dbContext);
            OrderHeaderRepository = new OrderHeaderRepo(_dbContext);
            OrderDetailRepository = new Repository<OrderDetails>(_dbContext);
            ApplicationUserRepository = new Repository<ApplicationUser>(_dbContext);
        }
        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
