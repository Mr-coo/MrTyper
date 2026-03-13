using backend.Domain.Models;

namespace backend.Domain.Repositories
{
    public interface ITextRepository
    {
        Task<IEnumerable<Text>> GetPublicTexts();

        Task<IEnumerable<Text>> GetTextsByUser(Guid userId);
        Task<IEnumerable<Text>> GetByChapterId(Guid chapterId, Guid? userId);

        Task<Text?> GetById(Guid id);

        Task Add(Text text);

        Task Update(Text text);

        Task Delete(Text text);

        Task Save();
    }
}