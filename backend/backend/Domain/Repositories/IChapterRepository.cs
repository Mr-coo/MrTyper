using backend.Domain.Models;

namespace backend.Domain.Repositories
{
    public interface IChapterRepository
    {
        Task<List<Chapter>> all(Guid? userId);
        Task<Chapter?> get(Guid id);
        Task<Chapter?> store(Chapter chapter);
        Task<Chapter?> delete(Guid id);
        Task<Chapter?> update(Guid id, Chapter chapter);

    }
}
