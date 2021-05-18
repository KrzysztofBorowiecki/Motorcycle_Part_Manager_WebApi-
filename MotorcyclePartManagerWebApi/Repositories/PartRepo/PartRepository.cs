using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MotorcyclePartManagerWebApi.Authorization;
using MotorcyclePartManagerWebApi.CustomExceptions;
using MotorcyclePartManagerWebApi.Data;
using MotorcyclePartManagerWebApi.Entities;
using MotorcyclePartManagerWebApi.Models;
using MotorcyclePartManagerWebApi.Services.UserServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MotorcyclePartManagerWebApi.Repositories
{
    public class PartRepository : IPartRepository
    {
        private readonly ProjectContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<PartRepository> _logger;
        private readonly IAuthorizationService _athorizationService;
        private readonly IUserContextService _userContextService;
        public PartRepository(ProjectContext context, IMapper mapper, ILogger<PartRepository> logger,
            IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _athorizationService = authorizationService;
            _userContextService = userContextService;
        }

        public async Task<PagedResult<Part>> GetPartsAsync(PartQuery query)
        {
            var baseQuery = _context.Parts.Where(r => query.SearchPhrase == null
                || (r.Name.ToLower().Contains(query.SearchPhrase.ToLower())
                || r.Producer.ToLower().Contains(query.SearchPhrase.ToLower())
                || r.Date.ToString().Contains(query.SearchPhrase.ToLower())
                || r.Price.ToString().ToLower().Contains(query.SearchPhrase.ToLower())
                || r.BikeHours.ToString().ToLower().Contains(query.SearchPhrase.ToLower())));

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelectors = new Dictionary<string, Expression<Func<Part, object>>>
                {
                    { nameof(Part.Producer), r => r.Producer },
                    { nameof(Part.Date), r => r.Date },
                    { nameof(Part.BikeHours), r => r.BikeHours },
                };

                var selectedColumn = columnsSelectors[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.ASC
                    ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }

            var parts = await baseQuery
                .Skip(query.PageSize * (query.PageNumber) - 1)
                .Take(query.PageSize)
                .ToListAsync();

            var totalItemsCount = baseQuery.Count();

            var result = new PagedResult<Part>(parts, totalItemsCount, query.PageSize, query.PageNumber);
            return result;
        }

        public async Task<Part> GetPartAsync(int id)
        {
            var part = await _context.Parts.FindAsync();
            if (part is null)
            {
                throw new NotFoundException("Nie znaleziono części");
            }

            return part;
        }

        public async Task AddPartAsync(Part part)
        {
            //part.BelongsToMotorcycleId = _userContextService.GetUserId;
            await _context.Parts.AddAsync(part);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePartAsync(Part part)
        {
            var selectedPart = await _context.Parts.FindAsync(part.Id);
            if (selectedPart is null)
            {
                throw new NotFoundException("Nie znaleziono części");
            }

            var authorizationResult = _athorizationService.AuthorizeAsync(_userContextService.User, part, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;
            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            _mapper.Map(part, selectedPart);

            await _context.SaveChangesAsync();
        }

        public async Task DeletePartAsync(int id)
        {
            _logger.LogError($"Part with id: {id} Delete action invoked");

            var part = await _context.Parts.FindAsync(id);

            if (part is null)
            {
                throw new NotFoundException("Nie znaleziono części");
            }

            var authorizationResult = _athorizationService.AuthorizeAsync(_userContextService.User, part, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;
            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            _context.Parts.Remove(part);
            await _context.SaveChangesAsync();
        }
    }
}
