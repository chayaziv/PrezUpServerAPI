using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrezUp.Core.IRepositories
{
    interface IRepository<T>
    {
        public List<T> GetList();

        public T GetById(int id);

        public T Add(T val);

        public T Update(T val);

        public void Delete(T id);
    }
}
