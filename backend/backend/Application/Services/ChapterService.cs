using backend.Domain.Models;
using backend.Domain.Repositories;

namespace backend.Application.Services
{
    public class ChapterService
    {
        private readonly IChapterRepository _chapterRepository;

        public ChapterService(IChapterRepository chapterRepository)
        {
            _chapterRepository = chapterRepository;
        }

        public async Task<List<Chapter>> get(Guid? userId)
        {
            return await _chapterRepository.all(userId);
        }

        public async Task<Chapter?> getById(Guid id, Guid? userId)
        {
            var existing = await _chapterRepository.get(id);

            if (existing == null)
            {
                throw new Exception("Chapter not found");
            }
            if(existing.UserId != userId && existing.IsPrivate)
            {
                throw new Exception("This chapter is private");
            }

            return existing;
        }

        public async Task<Chapter?> create(string name, bool isPrivate, Guid userId)
        {
            Chapter chapter = new Chapter()
            {
                Name = name,
                IsPrivate = isPrivate, 
                UserId = userId
            };
            return await _chapterRepository.store(chapter);
        }

        public async Task<Chapter?> update(Guid id, string name, bool isPrivate, Guid userId)
        {
            Chapter? existing = await _chapterRepository.get(id);
            if (existing == null)
            {
                throw new Exception("Chapter not found");
            }

            if(existing.UserId != userId)
            {
                throw new UnauthorizedAccessException("You don't owned this chapter");
            }

            Chapter chapter = new Chapter()
            {
                Name = name,
                IsPrivate = isPrivate
            };
            return await _chapterRepository.update(id, chapter);
        }

        public async Task<Chapter?> delete(Guid id, Guid userId)
        {
            Chapter? existing = await _chapterRepository.get(id);
            if (existing == null)
            {
                throw new Exception("Chapter not found");
            }

            if (existing.UserId != userId)
            {
                throw new UnauthorizedAccessException("You don't owned this chapter");
            }

            return await _chapterRepository.delete(id);
        }
    }
}