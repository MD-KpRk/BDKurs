using Microsoft.EntityFrameworkCore;
using System.Windows.Forms;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
    {
    }

    public DbSet<Gender> Genders { get; set; }
    public DbSet<Publisher> Publishers { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Status> Statuses { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<ReaderCategory> ReaderCategories { get; set; }
    public DbSet<Reader> Readers { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<AccessCategory> AccessCategories { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<BookOrder> BookOrders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Здесь можно настроить дополнительные конфигурации, если необходимо
    }

    public static IQueryable GetDbSetByName(string entityName, LibraryDbContext _context)
    {
        // Получаем тип контекста
        Type contextType = _context.GetType();


        // Ищем свойство по имени entityName
        System.Reflection.PropertyInfo? property = contextType.GetProperty(entityName);
        MessageBox.Show("prop: " + property.Name);

        if (property == null)
        {
            throw new ArgumentException($"DbSet with name {entityName} not found.");
        }

        // Получаем значение DbSet из контекста
        var dbSet = property.GetValue(_context);

        // Возвращаем DbSet как IQueryable для дальнейшего использования
        return dbSet as IQueryable;
    }
}