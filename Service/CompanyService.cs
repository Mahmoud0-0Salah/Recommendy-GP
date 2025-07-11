﻿using AutoMapper;
using Contracts;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Shared.DTO.Company;
using Stripe;
using Microsoft.Extensions.Logging;
using Shared.RequestFeatures;
using Shared.DTO.Notification;
using Shared.DTO.Student;

namespace Service
{
    internal sealed class CompanyService : ICompanyService
    {
        private readonly IRepositoryManager _repository;
        private readonly IServiceManager _service;
        private readonly IMapper _mapper;
        INotificationService _notificationService;

		public CompanyService(IRepositoryManager repository, IMapper mapper, INotificationService notificationService, IServiceManager service)
        {
            _repository = repository;
            _mapper = mapper;
            _service = service;
            _notificationService = notificationService;
		}
		public CompanyViewDto GetCompany(string id, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(id, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(id);
            var companyviewDto = _mapper.Map<CompanyViewDto>(company);
            return companyviewDto;
        }

        public async Task UpdateCompany(string companyId, CompanyDto companyDto, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges);
            if (company == null)
            {
                throw new CompanyNotFoundException(companyId);
            }
            if (!string.IsNullOrEmpty(companyDto.CompanyUrl))
            {
                company.CompanyUrl = companyDto.CompanyUrl;
            }
            if (!string.IsNullOrEmpty(companyDto.UserName))
            {
                company.User.UserName = companyDto.UserName;
            }


            if (!string.IsNullOrEmpty(companyDto.Bio))
            {
                company.User.Bio = companyDto.Bio;
            }
            if (!string.IsNullOrEmpty(companyDto.PhoneNumber))
            {
                company.User.PhoneNumber = companyDto.PhoneNumber;
            }
			if (!string.IsNullOrEmpty(companyDto.NewPassword) && !string.IsNullOrEmpty(companyDto.OldPassword))
                await _service.UserService.ChangePasswordAsync(companyId,
                    new ChangePasswordDto { CurrentPassword = companyDto.OldPassword,NewPassword = companyDto.NewPassword});

			_repository.Company.UpdateCompany(company);
            _repository.Save();
        }
        public async Task<PagedList<CompanyViewDto>> GetUnverifiedCompaniesAsync(
     CompanyParameters companyParameters, bool trackChanges)
        {


            var companies = await _repository.Company.GetUnverifiedCompaniesAsync(companyParameters, trackChanges);

            var companyDtos = _mapper.Map<List<CompanyViewDto>>(companies);

            return new PagedList<CompanyViewDto>(
                companyDtos,
                companies.MetaData.TotalCount,
                companies.MetaData.CurrentPage,
                companies.MetaData.PageSize);
        }

        public async Task VerifyCompany(string companyId, string adminId, CompanyVerificationDto verificationDto, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges);
            if (company == null)
                throw new CompanyNotFoundException(companyId);
			if (company.IsVerified && verificationDto.IsVerified)
				throw new OrganizationVerifiedBadRequestException(company.CompanyId);
			if (!company.IsVerified && !verificationDto.IsVerified)
				throw new OrganizationUnVerifiedBadRequestException(company.CompanyId);

			company.IsVerified = verificationDto.IsVerified;
            company.VerificationNotes = verificationDto.VerificationNotes;

            _repository.Company.UpdateCompany(company);
            await _repository.SaveAsync();

			if (verificationDto.IsVerified)
				await _notificationService.CreateNotificationAsync(new NotificationCreationDto
			    {
				    ActorID = adminId,
				    ReceiverID = companyId,
				    Content = NotificationType.OrganizationVerified
			    });


			var (subject, message) = EmailTemplates.Organization.GetVerificationTemplate(
                verificationDto.IsVerified,
                verificationDto.VerificationNotes);
            await _service.EmailsService.Sendemail(company.User.Email, message, subject);
        }

    }
}
