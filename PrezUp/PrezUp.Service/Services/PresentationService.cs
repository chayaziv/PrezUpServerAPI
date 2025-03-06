using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrezUp.Core.Entity;
using PrezUp.Core.IRepositories;
using PrezUp.Core.IServices;

namespace PrezUp.Service.Services
{
    public class PresentationService: IPresentationService
    {
        readonly IRepositoryManager _repository;
        //readonly IMapper _mapper;

        public PresentationService(IRepositoryManager repository)
        {
            _repository = repository;
            //_mapper = mapper;
        }
        public async Task<List<Presentation>> getallAsync()
        {
            var list = _repository.Presentations.GetList();
            //var listDTOs = new List<Presentation>();
            //foreach (var item in list)
            //{
            //    listDTOs.Add(_mapper.Map<Presentation>(item));
            //}
            await _repository.SaveAsync();
            return list;
        }

        public async Task<Presentation> getByIdAsync(int id)
        {
            var item =  _repository.Presentations.GetById(id);

            //return _mapper.Map<Presentation>(item);
            await _repository.SaveAsync();
            return item;
        }

        public async Task<Presentation> addAsync(Presentation agreement)
        {
            //var model = _mapper.Map<Agreement>(agreement);
            var model = agreement;
            _repository.Presentations.Add(model);

            await _repository.SaveAsync();
            //return _mapper.Map<Presentation>(model);
            return model;
        }

        public async Task<Presentation> updateAsync(int id, Presentation agreement)
        {
            //var model = _mapper.Map<Agreement>(agreement);
            var model = agreement;
            var updated = _repository.Presentations.Update(model);
            await _repository.SaveAsync();
            //return _mapper.Map<Presentation>(updated);
            return updated;
        }

        public async Task<bool> deleteAsync(int id)
        {
            Presentation itemToDelete = _repository.Presentations.GetById(id);
            _repository.Presentations.Delete(itemToDelete);
            await _repository.SaveAsync();
            return true;
        }
    }
}
