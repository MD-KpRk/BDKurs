using BDKurs.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows.Forms;


public class Repository
{
    private readonly LibraryDbContext _context;

    public Repository(LibraryDbContext context)
    {
        _context = context;
    }

    // Метод для получения DbSet по строковому названию
    public List<BDObject> GetDbSetByName(string entityName)
    {
        // Получаем тип контекста
        var contextType = _context.GetType();

        // Ищем свойство по имени entityName
        var property = contextType.GetProperty(entityName);

        if (property == null)
        {
            throw new ArgumentException($"DbSet with name {entityName} not found.");
        }

        // Получаем значение DbSet как IQueryable
        var dbSet = property.GetValue(_context) as IQueryable;

        if (dbSet == null)
        {
            throw new InvalidOperationException($"Unable to retrieve DbSet for {entityName}.");
        }

        // Преобразуем результат в List<object>
        return dbSet.Cast<BDObject>().ToList();
    }
}

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
    {
    }

    public DbSet<Gender> Genders { get; set; }
    public DbSet<Publisher> Publishers { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Status> Statuss { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<ReaderCategory> ReaderCategorys { get; set; }
    public DbSet<Reader> Readers { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<AccessCategory> AccessCategorys { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<BookOrder> BookOrders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Здесь можно настроить дополнительные конфигурации, если необходимо
    }




}