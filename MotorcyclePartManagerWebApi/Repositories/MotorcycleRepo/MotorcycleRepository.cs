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
    public class MotorcycleRepository : IMotorcycleRepository
    {
        private readonly ProjectContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<MotorcycleRepository> _logger;
        private readonly IAuthorizationService _athorizationService;
        private readonly IUserContextService _userContextService;

        public MotorcycleRepository(ProjectContext context, IMapper mapper, ILogger<MotorcycleRepository> logger,
            IAuthorizationService authorizationService, IUserContextService userContextService)

        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _athorizationService = authorizationService;
            _userContextService = userContextService;
        }


        public async Task<List<Motorcycle>> GetMotorcyclesAsync()
        {
           var parts = await _context.Motorcycles.ToListAsync();

            if (parts is null)
            {
                throw new NotFoundException("Nie znaleziono motocykla");
            }

            return parts;
        }


        public async Task<Motorcycle> GetMotorcycleAsync(int id)
        {
            var motorcycle = await _context.Motorcycles.FindAsync(id);

            if (motorcycle is null)
            {
                throw new NotFoundException("Nie znaleziono motocykla");
            }

            return motorcycle;
        }


        public async Task AddMotorcycleAsync(Motorcycle motorcycle)
        {
            motorcycle.CreatedById = _userContextService.GetUserId;

            await _context.Motorcycles.AddAsync(motorcycle);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateMotorcycleAsync(Motorcycle motorcycle)
        {
            var selectedMotorcycle = await _context.Motorcycles.FindAsync(motorcycle.Id);
            if (selectedMotorcycle is null)
            {
                throw new NotFoundException("Nie znaleziono motocykla");
            }

            var authorizationResult = _athorizationService.AuthorizeAsync(_userContextService.User, motorcycle, new ResourceOperationRequirement(ResourceOperation.Update)).Result;
            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            _mapper.Map(motorcycle, selectedMotorcycle);

            await _context.SaveChangesAsync();
        }


        public async Task DeleteMotorcycleAsync(int id)
        {
            _logger.LogError($"Motorcycle with id: {id} Delete action invoked");

            var motorcycle = await _context.Motorcycles.FindAsync(id);

            if (motorcycle is null)
            {
                throw new NotFoundException("Nie znaleziono motocykla");
            }

            var authorizationResult = _athorizationService.AuthorizeAsync(_userContextService.User, motorcycle, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;
            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            _context.Motorcycles.Remove(motorcycle);
            await _context.SaveChangesAsync();
        }
    }
}
