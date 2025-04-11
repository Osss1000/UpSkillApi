using Microsoft.EntityFrameworkCore;
using UpSkillApi.Data;
using UpSkillApi.DTOs;

namespace UpSkillApi.Repositories
{
    public class ProfessionRepository
    {
        private readonly UpSkillDbContext _context;

        public ProfessionRepository(UpSkillDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProfessionDto>> GetDistinctProfessionsAsync()
        {
            var professions = await _context.Workers
                .Where(w => !string.IsNullOrEmpty(w.Profession))
                .Select(w => w.Profession.Trim())
                .Distinct()
                .ToListAsync();

            return professions
                .Select(p => new ProfessionDto { Name = p })
                .ToList();
        }
    }
}