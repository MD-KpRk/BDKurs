using BDKurs;
using BDKurs.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class BookOrder : BDObject
{
    [ColumnName("Номер")]
    [NonEditable]
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

    [Hidden]
    [ColumnName("Номер книги")]
    [ForeignKey("ISBN")]
    public string BookID { get; set; }

    [Required]
    [ColumnName("Книга")]
    public Book Book { get; set; }

    [Required]
    [Hidden]
    [ForeignKey("ReaderID")]
    public int ReaderID { get; set; }

    [Required]
    [ColumnName("Читатель")]
    public Reader Reader { get; set; }

    [Required]
    [Hidden]
    [ForeignKey("EmployeeID")]
    public int EmployeeID { get; set; }

    [Required]
    [ColumnName("Сотрудник")]
    public Employee Employee { get; set; }

    override public string ToString()
    {
        return BookOrderID.ToString();
    }
}
