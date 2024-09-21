using BDKurs;
using BDKurs.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Book : BDObject
{

    [ColumnName("ISBN")]
    [Key]
    [Required]
    [MaxLength(15)]
    public string ISBN { get; set; } = "";

    [ColumnName("Название")]
    [Required]
    [MaxLength(120)]
    public string Title { get; set; } = "";

    [ColumnName("Год публикации")]
    public int? PublicationYear { get; set; } // NULLABLE

    [Hidden]
    [ForeignKey("PublisherID")]
    public int? PublisherID { get; set; }
    [ColumnName("Издательство")]
    public Publisher? Publisher { get; set; }

    [Hidden]
    [ForeignKey("AuthorID")]
    public int? AuthorID { get; set; }

    [ColumnName("Автор")]
    public Author? Author { get; set; }

    [Hidden]
    [ForeignKey("StatusID")]
    public int? StatusID { get; set; }

    [ColumnName("Статус")]
    public Status? Status { get; set; }

    [Hidden]
    [ForeignKey("GenreID")]
    public int? GenreID { get; set; }
    [ColumnName("Жанр")]
    public Genre? Genre { get; set; }

    override public string ToString()
    {
        return Title;
    }
}
