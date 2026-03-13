using backend.Domain.Models;
using backend.Domain.Repositories;

namespace backend.Application.Services
{
    public class TextService
    {
        private readonly ITextRepository _repository;

        public TextService(ITextRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Text>> Get(Guid? userId)
        {
            if (userId == null)
                return await _repository.GetPublicTexts();

            return await _repository.GetTextsByUser(userId.Value);
        }

        public async Task<Text?> GetById(Guid id, Guid? userId)
        {
            var text = await _repository.GetById(id);

            if (text == null)
                return null;

            if (text.Chapter.IsPrivate && text.Chapter.UserId != userId)
                return null;

            return text;
        }

        public async Task<IEnumerable<Text>> GetByChapterId(Guid id, Guid? userId)
        {
            var texts = await _repository.GetByChapterId(id, userId);


            return texts;
        }

        public async Task<Text> Create(string content, Guid chapterId)
        {
            var text = new Text(content, chapterId);

            await _repository.Add(text);
            await _repository.Save();

            return text;
        }

        public async Task<bool> Update(Guid id, string content, Guid userId)
        {
            var text = await _repository.GetById(id);

            if (text == null)
                return false;

            if (text.Chapter.UserId != userId)
                return false;

            text.Content = content;

            await _repository.Update(text);
            await _repository.Save();

            return true;
        }

        public async Task<bool> Delete(Guid id, Guid userId)
        {
            var text = await _repository.GetById(id);

            if (text == null)
                return false;

            if (text.Chapter.UserId != userId)
                return false;

            await _repository.Delete(text);
            await _repository.Save();

            return true;
        }
    }
}