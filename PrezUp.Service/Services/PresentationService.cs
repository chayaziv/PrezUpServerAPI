﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using PrezUp.Core.Entity;
using PrezUp.Core.EntityDTO;
using PrezUp.Core.IRepositories;
using PrezUp.Core.IServices;
using PrezUp.Core.models;
using PrezUp.Core.Utils;

namespace PrezUp.Service.Services
{
    public class PresentationService : IPresentationService
    {

        private readonly IRepositoryManager _repository;
        private readonly Is3Service _audioUploadService;
        private readonly IAnalysisService _audioAnalysisService;
        readonly IMapper _mapper;

        public PresentationService(
            IRepositoryManager repository,
            Is3Service audioUploadService,
            IAnalysisService audioAnalysisService,
            IMapper mapper)
        {
            _repository = repository;
            _audioUploadService = audioUploadService;
            _audioAnalysisService = audioAnalysisService;
            _mapper = mapper;
        }
        public async Task<Result<PresentationDTO>> AnalyzeAudioAsync(IFormFile audio, bool isPublic, string title, int userId)
        {

            string fileKey = $"recordings/{Guid.NewGuid()}.wav";
            var S3Result = await _audioUploadService.UploadFileToS3Async(audio.OpenReadStream(), fileKey);
            if (!S3Result.IsSuccess)
            {
                return Result<PresentationDTO>.Failure(S3Result.ErrorMessage);
            }
            var fileUrl = S3Result.Data.Url;
            var NLPResult = await _audioAnalysisService.AnalyzeAudioAsync(fileUrl);
            if (!NLPResult.IsSuccess)
            {
                return Result<PresentationDTO>.Failure(S3Result.ErrorMessage);
            }

            Presentation presentation = _mapper.Map<Presentation>(NLPResult.Data);

            presentation.IsPublic = isPublic;
            presentation.FileUrl = fileUrl ?? "";
            presentation.UserId = userId;
            presentation.Title = title ?? "";
            await _repository.Presentations.AddAsync(presentation);
            if (await _repository.SaveAsync() == 0)
            {
                return Result<PresentationDTO>.Failure("Error in saving to DB");
            }
            return Result<PresentationDTO>.Success(_mapper.Map<PresentationDTO>(presentation));

        }
        public async Task<Result<List<PresentationDTO>>> getallAsync()
        {
            var list = await _repository.Presentations.GetListAsync();
            var listDTOs = list.Select(item => _mapper.Map<PresentationDTO>(item)).ToList();
            return Result<List<PresentationDTO>>.Success(listDTOs);
        }

        public async Task<Result<PresentationDTO>> getByIdAsync(int id)
        {
            var item = await _repository.Presentations.GetByIdAsync(id);
            if (item == null)
                return Result<PresentationDTO>.NotFound("Presentation not found");
            return Result<PresentationDTO>.Success(_mapper.Map<PresentationDTO>(item));
        }

        public async Task<Result<List<PresentationDTO>>> GetPublicPresentationsAsync()
        {
            var list = await _repository.Presentations.GetPublicPresentationsAsync();
            var listDTOs = list.Select(item => _mapper.Map<PresentationDTO>(item)).ToList();
            return Result<List<PresentationDTO>>.Success(listDTOs);
        }

        public async Task<Result<bool>> deleteAsync(int id, int userId)
        {
            var itemToDelete = await _repository.Presentations.GetByIdAsync(id);
            if (itemToDelete == null)
            {
                return Result<bool>.NotFound("Presentation not found");
            }

            if (itemToDelete.UserId != userId)
            {
                return Result<bool>.Unauthorized("You are not authorized to delete this presentation");
            }

            if (!string.IsNullOrEmpty(itemToDelete.FileUrl))
            {
                await _audioUploadService.DeleteFileFromS3Async(itemToDelete.FileUrl);
            }

            _repository.Presentations.DeleteAsync(itemToDelete);
            await _repository.SaveAsync();
            return Result<bool>.Success(true);
        }

    }

}

