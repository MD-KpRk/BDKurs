using BDKurs;
using BDKurs.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Author : BDObject
{
    [ColumnName("Номер")]
    [NonEditable]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AuthorID { get; set; }

    [ColumnName("Имя")]
    [Required]
    [MaxLength(30)]
    public string FirstName { get; set; } = "";

    [ColumnName("Фамилия")]
    [Required]
    [MaxLength(30)]
    public string LastName { get; set; } = "";

    [ColumnName("Отчество")]
    [MaxLength(30)]
    public string? MiddleName { get; set; } // Nullable

    [ColumnName("Дата рождения")]
    public DateTime BirthDate { get; set; } // Nullable

    [Hidden]
    [ForeignKey("GenderID")]
    public int GenderID { get; set; }

    [Required]
    [ColumnName("Пол")]
    public Gender Gender { get; set; }

    [ColumnName("Биография")]
    [MaxLength(200)]
    public string? Biography { get; set; } // Nullable

    override public string ToString()
    {
        return FirstName +" "+ MiddleName;
    }
}
