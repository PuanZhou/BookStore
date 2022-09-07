using Bulkybook.DataAcess.Repository.IRepository;
using BulkyBookweb.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulkybook.DataAcess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            CoverType = new CorverTypeRepository(_db);
            Product=new ProductRepository(_db);

        }

        public ICategoryRepository Category { get; private set; }
        public ICorverTypeRepository CoverType { get; private set; }
        public IProductRepository Product { get; private set; } 

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
