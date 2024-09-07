using BDKurs;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class BookOrder
{
    [ColumnName("Номер")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BookOrderID { get; set; }

    [ColumnName("Дата ордера")]
    [Required]
    public DateTime BookOrderDate { get; set; }

    [ColumnName("Дата возврата")]
    [Required]
    public DateTime ReturnDate { get; set; }

    [ColumnName("Актуальная дата возврата")]
    public DateTime? ActualReturnDate { get; set; }

    [ColumnName("Номер книги")]
    [ForeignKey("ISBN")]
    public string BookID { get; set; }

    [ColumnName("Книга")]
    public Book Book { get; set; }

    [Hidden]
    [ForeignKey("ReaderID")]
    public int ReaderID { get; set; }

    [ColumnName("Читатель")]
    public Reader Reader { get; set; }

    [Hidden]
    [ForeignKey("EmployeeID")]
    public int EmployeeID { get; set; }

    [ColumnName("Сотрудник")]
    public Employee Employee { get; set; }

    override public string ToString()
    {
        return BookOrderID.ToString();
    }
}
