using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PrezUp.Core.EntityDTO;
using PrezUp.Core.IRepositories;
using PrezUp.Core.IServices;
using PrezUp.Core.Utils;

namespace PrezUp.Service.Services
{
    public class TagService : ITagService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
       

        public TagService(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            
        }
        public async Task<Result<List<TagDTO>>> GetTagsAsync()
        {
            var list = await _repository.Tags.GetListAsync();
            if (list == null || !list.Any())
                return Result<List<TagDTO>>.NotFound("No tags found");

            return Result<List<TagDTO>>.Success(_mapper.Map<List<TagDTO>>(list));
        }
    }
}
