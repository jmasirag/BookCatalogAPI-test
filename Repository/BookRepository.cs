using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using Repository.Models;


namespace Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetBooksPagedAsync(int pageNumber, int pageSize)
        {
            var queryable = _context.Books.AsQueryable();

            var items = await queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return items;
        }

        public async Task<Book> AddBookAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return book;
        }

        public async Task DeleteBookAsync(int id)
        {
            var bookToDelete = await _context.Books.FindAsync(id);

            if (bookToDelete != null)
            {
                _context.Books.Remove(bookToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }


        public async Task<Book> UpdateBookAsync(Book book)
        {
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return book;
        }
    }
}
