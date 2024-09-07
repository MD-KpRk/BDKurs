using BDKurs;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Reader
{
    [ColumnName("Номер")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ReaderID { get; set; }

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
    public string? MiddleName { get; set; } // NULL

    [ColumnName("Дата рождения")]
    public DateTime? BirthDate { get; set; }

    [ColumnName("Адрес")]
    [MaxLength(120)]
    public string? Address { get; set; } // NULL

    [ColumnName("Телефон")]
    [MaxLength(30)]
    public string? Phone { get; set; } // NULL

    [ColumnName("Эл. почта")]
    [MaxLength(30)]
    public string? Email { get; set; } // NULL

    [Hidden]
    [ForeignKey("GenderID")]
    public int GenderID { get; set; }

    [Required]
    [ColumnName("Пол")]
    public Gender Gender { get; set; }

    [Required]
    [Hidden]
    [ForeignKey("ReaderCategoryID")]
    public int ReaderCategoryID { get; set; }
    [ColumnName("Категория")]
    public ReaderCategory ReaderCategory { get; set; }


    override public string ToString()
    {
        return FirstName.ToString();
    }
}
