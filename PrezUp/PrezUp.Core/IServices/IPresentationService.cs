using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrezUp.Core.Entity;

namespace PrezUp.Core.IServices
{
   public interface IPresentationService
    {
        public Task<List<Presentation>> getallAsync();

        public Task<Presentation> getByIdAsync(int id);

        public Task<Presentation> addAsync(Presentation agreement);

        public Task<Presentation> updateAsync(int id, Presentation agreement);

        public Task<bool> deleteAsync(int id);
    }
}
