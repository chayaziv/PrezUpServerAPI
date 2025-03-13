using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PrezUp.Core.EntityDTO;
using PrezUp.Core.models;

namespace PrezUp.Core.IServices
{
   public interface IPresentationService
    {
        public Task<List<PresentationDTO>> getallAsync();

        public Task<PresentationDTO> getByIdAsync(int id);

        public Task<bool> deleteAsync(int id, int userId);

        Task<AudioResult> AnalyzeAudioAsync(IFormFile audio, bool isPublic, int userId);

        public Task<List<PresentationDTO>> GetPublicPresentationsAsync();
    }
}
