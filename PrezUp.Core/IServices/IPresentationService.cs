﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PrezUp.Core.Entity;
using PrezUp.Core.EntityDTO;


namespace PrezUp.Core.IServices
{
   public interface IPresentationService
    {
        public Task<List<PresentationDTO>> getallAsync();

        public Task<PresentationDTO> getByIdAsync(int id);

        public Task<PresentationDTO> addAsync(PresentationDTO agreement);

        public Task<PresentationDTO> updateAsync(int id, PresentationDTO agreement);

        public Task<bool> deleteAsync(int id);

        Task<AnalysisResult> AnalyzeAudioAsync(IFormFile audio);
    }
}
