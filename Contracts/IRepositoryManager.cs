﻿using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Contracts
{
    public interface IRepositoryManager
    {
        IAdminRepository Admin { get; }
        ICountryRepository Country { get; }
        IStudentRepository Student { get; }
        IUserRepository User { get; }
        ICompanyRepository Company { get; }
        IUniversityRepository university { get; }
        IScholarshipRepository Scholarship { get; } 

        IFileRepository File { get; }

        IInternshipRepository Intership { get; }

        IInternshipPositionRepository InternshipPosition { get; }

        IPositionRepository PositionRepository { get; }

        IUserCodeRepository UserCodeRepository { get; }

        IOpportunityRepository OpportunityRepository { get; }
		IFeedbackRepository FeedbackRepository { get; }
		INotificationRepository NotificationRepository { get; }
        IReportRepository ReportRepository { get; }
        IChatUsersRepository ChatUsersRepository { get; }

        IChatMessagesRepository ChatMessagesRepository { get; }

		ISkillsRepository SkillsRepository { get; }

        ICourseRepository CourseRepository { get; }

		void Save();
        Task SaveAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();

	}
}
