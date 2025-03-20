using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrezUp.Core.EntityDTO;
using PrezUp.Core.Utils;

namespace PrezUp.Core.IServices
{
    public interface ITagService
    {
        Task<Result<List<TagDTO>>> GetTagsAsync();
    }
}
