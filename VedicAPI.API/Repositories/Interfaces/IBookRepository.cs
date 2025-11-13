using VedicAPI.API.Models;

namespace VedicAPI.API.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for Book entity operations
    /// </summary>
    public interface IBookRepository
    {
        Task<Book?> GetByIdAsync(int id);
        Task<IEnumerable<Book>> GetAllAsync();
        Task<IEnumerable<Book>> GetActiveAsync();
        Task<IEnumerable<Book>> GetByCategoryAsync(string category);
        Task<IEnumerable<Book>> SearchAsync(string searchTerm);
        Task<int> CreateAsync(Book book);
        Task<bool> UpdateAsync(Book book);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}

