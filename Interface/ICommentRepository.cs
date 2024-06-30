using AppUser.Models;

namespace AppUser.Interface
{
    public interface ICommentRepository
    {
        Task<ICollection<Comment>> GetAllAsync();

        Task<Comment> GetCommentAsync(int id);

        Task<bool> CommentExistAsync(int id);

        Task<Comment> CreateCommentAsync(int stockId,Comment comment);

        Task<Comment?> UpdateCommentAsync(int commentId, Comment comment);

        Task<Comment?> DeleteCommentAsync(int id);

        Task<bool> Save();

    }
}
