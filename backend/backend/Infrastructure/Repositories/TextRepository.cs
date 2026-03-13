using backend.Domain.Models;
using backend.Domain.Repositories;
using backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Repositories
{
    public class TextRepository : ITextRepository
    {
        private readonly AppDbContext _context;

        public TextRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Text>> GetPublicTexts()
        {
            return await _context.Texts
                .Include(t => t.Chapter)
                .Where(t => !t.Chapter.IsPrivate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Text>> GetTextsByUser(Guid userId)
        {
            return await _context.Texts
                .Include(t => t.Chapter)
                .Where(t => !t.Chapter.IsPrivate || t.Chapter.UserId == userId)
                .ToListAsync();
        }

        public async Task<Text?> GetById(Guid id)
        {
            return await _context.Texts
                .Include(t => t.Chapter)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task Add(Text text)
        {
            await _context.Texts.AddAsync(text);
        }

        public Task Update(Text text)
        {
            _context.Texts.Update(text);
            return Task.CompletedTask;
        }

        public Task Delete(Text text)
        {
            _context.Texts.Remove(text);
            return Task.CompletedTask;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Text>> GetByChapterId(Guid chapterId, Guid? userId)
        {
            return await _context.Texts
                .Include(t => t.Chapter)
                .Where(t =>
                    t.ChapterId == chapterId &&
                    (
                        !t.Chapter.IsPrivate ||
                        t.Chapter.UserId == userId
                    )
                )
                .ToListAsync();
        }
    }
}