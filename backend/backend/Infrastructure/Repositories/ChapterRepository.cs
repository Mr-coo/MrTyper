using backend.Domain.Models;
using backend.Domain.Repositories;
using backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Repositories
{
    public class ChapterRepository : IChapterRepository
    {
        private readonly AppDbContext _context;

        public ChapterRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Chapter>> all(Guid? userId)
        {
            return await _context.Chapters.
                Where(c => !c.IsPrivate || (userId != null && c.UserId == userId)).
                ToListAsync();
        }

        public async Task<Chapter?> get(Guid id)
        {
            return await _context.Chapters
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Chapter> store(Chapter chapter)
        {
            await _context.Chapters.AddAsync(chapter);
            await _context.SaveChangesAsync();
            return chapter;
        }

        public async Task<Chapter?> update(Guid id, Chapter chapter)
        {
            Chapter? existing = await _context.Chapters
                .FirstOrDefaultAsync(c => c.Id == id);

            if (existing == null)
                return null;

            existing.Name = chapter.Name;
            existing.IsPrivate = chapter.IsPrivate;

            await _context.SaveChangesAsync();

            return existing;
        }

        public async Task<Chapter?> delete(Guid id)
        {
            Chapter? chapter = await _context.Chapters
                .FirstOrDefaultAsync(c => c.Id == id);

            if (chapter == null)
                return null;

            _context.Chapters.Remove(chapter);
            await _context.SaveChangesAsync();

            return chapter;
        }
    }
}