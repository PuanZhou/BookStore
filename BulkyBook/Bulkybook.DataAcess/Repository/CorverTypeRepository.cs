using Bulkybook.DataAcess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBookweb.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulkybook.DataAcess.Repository
{
    public class CorverTypeRepository : Repository<CoverType>, ICorverTypeRepository
    {
        private ApplicationDbContext _db;

        public CorverTypeRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        public void Update(CoverType obj)
        {
            _db.CoverTypes.Update(obj);
        }
    }
}
