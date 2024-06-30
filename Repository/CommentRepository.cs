using AppUser.Data;
using AppUser.Interface;
using AppUser.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace AppUser.Repository
{
    public class CommentRepository:ICommentRepository
    {
        private readonly ApplicationDBContext _context;


        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }


        public async Task<bool> CommentExistAsync(int id)
        {
            return await _context.Comments.AnyAsync(c => c.Id == id);
        }



        public async Task<Comment> CreateCommentAsync(int stockId ,Comment comment)
        {
            comment.StockId = stockId;
            await _context.Comments.AddAsync(comment);
            await Save();
            return comment;
        }



        public async Task<Comment?> DeleteCommentAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null) return null;

             _context.Comments.Remove(comment);
             
            await Save();

            return comment;
        }



        public async Task<ICollection<Comment>> GetAllAsync()
        { 
            return await _context.Comments.Include(a => a.AppUser).ToListAsync();
        }



        public async Task<Comment> GetCommentAsync(int id)
        {
            return await _context.Comments.Include(a => a.AppUser).Where(s => s.Id == id).FirstOrDefaultAsync();
        }



        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();

            return saved > 0 ? true : false;

        }



        public async Task<Comment?> UpdateCommentAsync(int commentId, Comment comment)
        {
            var commentExist = await _context.Comments.FindAsync(commentId);

            if (commentExist == null)
                return null;

            commentExist.Content = comment.Content;
            commentExist.Title = comment.Title;

            await Save();

            return commentExist;
        }
    }
}
