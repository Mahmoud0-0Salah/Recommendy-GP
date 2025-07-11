﻿using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.Extensions.Logging;
using Service.Contracts;
using Shared.DTO.Internship;
using Shared.DTO.Notification;
using Shared.DTO.opportunity;
using Shared.DTO.Scholaship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
	public class OpportunityService : IOpportunityService
	{
		private readonly IRepositoryManager _repositoryManager;
		private readonly IMapper _mapper;
		public OpportunityService(IRepositoryManager repositoryManager, IMapper mapper)
		{
			_repositoryManager = repositoryManager;
			_mapper = mapper;
		}

		public async Task SavedOpportunity(string StudentId, SavedOpportunityDto savedOpportunityDto)
		{
			string ReceiverID;
			var student = _repositoryManager.Student.GetStudent(StudentId, false);
			if (student is null)
				throw new StudentNotFoundException(StudentId);
            if (savedOpportunityDto.Type == "I")
            {
                var internship =  await _repositoryManager.Intership.InternshipById(savedOpportunityDto.PostId, false);
                if (internship == null)
                    throw new InternshipNotFoundException(savedOpportunityDto.PostId);
				ReceiverID = internship.CompanyId;
            }
            else if (savedOpportunityDto.Type == "S")
            {
                var scholarship = _repositoryManager.Scholarship.GetScholarshipById(savedOpportunityDto.PostId, false);
                if (scholarship == null)
                    throw new ScholarshipNotFoundException(savedOpportunityDto.PostId);
				ReceiverID = scholarship.UniversityId;
			}
			else
            {
                throw new SavedPostNotFoundException();

            }
        



            var result = await _repositoryManager.OpportunityRepository.GetSavedOpportunity(StudentId, savedOpportunityDto.PostId, savedOpportunityDto.Type[0]);

			
			if (result == null)
			{
				

				var savedPost = _mapper.Map<SavedPost>(savedOpportunityDto);
				savedPost.StudentId = StudentId;
				await _repositoryManager.OpportunityRepository.SavedOpportunity(savedPost);
				await _repositoryManager.SaveAsync();
			}

		}
		public async Task DeleteOpportunity(string StudentId, SavedOpportunityDto savedOpportunityDto)
		{
			var student = _repositoryManager.Student.GetStudent(StudentId, false);
			if (student is null)
				throw new StudentNotFoundException(StudentId);

            if (savedOpportunityDto.Type == "I")
            {
                var internship = await _repositoryManager.Intership.InternshipById(savedOpportunityDto.PostId, false);
                if (internship == null)
                    throw new InternshipNotFoundException(savedOpportunityDto.PostId);

            }
            else if(savedOpportunityDto.Type=="S")
            {
                var scholarship = _repositoryManager.Scholarship.GetScholarshipById(savedOpportunityDto.PostId, false);
                if (scholarship == null)
                    throw new ScholarshipNotFoundException(savedOpportunityDto.PostId);
            }
			else
			{
                throw new SavedPostNotFoundException();

            }


            var result = await _repositoryManager.OpportunityRepository.GetSavedOpportunity(StudentId, savedOpportunityDto.PostId, savedOpportunityDto.Type[0]);
			if (result != null)
			{
				


				var savedPost = _mapper.Map<SavedPost>(savedOpportunityDto);
                savedPost.StudentId = StudentId;
                await _repositoryManager.OpportunityRepository.DeleteOpportunity(savedPost);
				await _repositoryManager.SaveAsync();

			}


			else
			{
				throw new SavedPostNotFoundException();

			}

		}
		public async Task<IEnumerable<GetScholarshipDto>> GetSavedScholarshipsAsync(string studentId)
		{

			var student = _repositoryManager.Student.GetStudent(studentId, false);
			if (student is null)
				throw new StudentNotFoundException(studentId);

			var savedPosts = await _repositoryManager.OpportunityRepository.GetSavedScholarshipsAsync(studentId, trackChanges: false);

			var scholarships = new List<GetScholarshipDto>();

			foreach (var savedPost in savedPosts)
			{
				var scholarship = _repositoryManager.Scholarship.GetScholarshipById(savedPost.PostId, trackChanges: false);
				if (scholarship != null)
				{
					var scholarshipDto = _mapper.Map<GetScholarshipDto>(scholarship);
					scholarships.Add(scholarshipDto);
				}
			}

			return scholarships;

		}
		public async Task<IEnumerable<InternshipDto>> GetSavedInternshipsAsync(string studentId)
		{

			var student = _repositoryManager.Student.GetStudent(studentId, false);
			if (student is null)
				throw new StudentNotFoundException(studentId);

			var savedPosts = await _repositoryManager.OpportunityRepository.GetSavedInternshipsAsync(studentId, trackChanges: false);

			var internships = new List<InternshipDto>();

			foreach (var savedPost in savedPosts)
			{
				var internship = await _repositoryManager.Intership.InternshipById(savedPost.PostId, trackChanges: false);
				if (internship != null)
				{
					var internshipDto = _mapper.Map<InternshipDto>(internship);
					internships.Add(internshipDto);
				}
			}

			return internships;
		}
	}
}
