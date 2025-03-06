using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrezUp.Core.Entity;

namespace PrezUp.Core.IRepositories
{
    interface IRepositoryManager
    {
        public IRepository <Presentation> Presentations { get; }

        Task<int> SaveAsync();

    }
}
