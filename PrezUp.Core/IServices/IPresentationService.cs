using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PrezUp.Core.Entity;
using PrezUp.Core.EntityDTO;
using PrezUp.Core.models;
using PrezUp.Core.Utils;

namespace PrezUp.Core.IServices
{
   public interface IPresentationService
    {
        public Task<Result<List<PresentationDTO>>> getallAsync();

        public Task<Result<PresentationDTO>> getByIdAsync(int id);

        public Task<Result<bool>> deleteAsync(int id, int userId);

        Task<Result<PresentationDTO>> AnalyzeAudioAsync(IFormFile audio, bool isPublic,string title, int userId, string tagsJson);

        public Task<Result<List<PresentationDTO>>> GetPublicPresentationsAsync();
        
    }
}
