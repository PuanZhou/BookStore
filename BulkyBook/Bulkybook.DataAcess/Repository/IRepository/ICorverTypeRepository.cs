using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulkybook.DataAcess.Repository.IRepository
{
    public interface ICorverTypeRepository:IRepository<CoverType>
    {
        void Update(CoverType obj);
    }
}
