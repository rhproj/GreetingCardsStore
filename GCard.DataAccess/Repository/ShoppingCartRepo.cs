using GCard.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCard.DataAccess.Repository
{
    public class ShoppingCartRepo : Repository<ShoppingCart>, IShoppingCartRepo
    {
        private readonly ApplicationDbContext _dbContext;
        public ShoppingCartRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public int DecrementCount(ShoppingCart cart, int count)
        {
            cart.Count -= count;
            _dbContext.SaveChanges();
            return cart.Count;
        }

        public int IncrementCount(ShoppingCart cart, int count)
        {
            cart.Count += count;
            _dbContext.SaveChanges();
            return cart.Count;
        }
    }
}
