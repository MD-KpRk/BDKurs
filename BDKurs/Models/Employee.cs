using BDKurs;
using BDKurs.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Employee : BDObject
{
    [ColumnName("Номер")]
    [Key]
    [NonEditable]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EmployeeID { get; set; }

    [ColumnName("Имя")]
    [Required]
    [MaxLength(30)]
    public string FirstName { get; set; } = "";

    [ColumnName("Фамилия")]
    [Required]
    [MaxLength(30)]
    public string LastName { get; set; }

    [ColumnName("Отчество")]
    [MaxLength(30)]
    public string? MiddleName { get; set; }

    [ColumnName("Пароль")]
    [Required]
    [MaxLength(20)]
    public string Passw { get; set; } = "";

    [ColumnName("Телефон")]
    [MaxLength(30)]
    public string? Phone { get; set; }

    [ColumnName("Эл. почта")]
    [MaxLength(30)]
    public string? Email { get; set; }

    [Hidden]
    [ForeignKey("GenderID")]
    public int GenderID { get; set; }

    [ColumnName("Пол")]
    [Required]
    public Gender Gender { get; set; }

    [Hidden]
    [ForeignKey("PositionID")]
    public int PositionID { get; set; }

    [ColumnName("Должность")]
    public Position Position { get; set; }

    [Hidden]
    [ForeignKey("AccessCategoryID")]
    public int AccessCategoryID { get; set; }

    [ColumnName("Доступ")]
    public AccessCategory AccessCategory { get; set; }

    override public string ToString()
    {
        return FirstName + " " + LastName + " " + AccessCategory;
    }
}
