using BDKurs;
using BDKurs.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class AccessCategory : BDObject
{
    [ColumnName("Номер")]
    [Key]
    [NonEditable]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AccessCategoryID { get; set; }

    [ColumnName("Название")]
    [Required]
    [MaxLength(30)]
    public string Name { get; set; } = "";

    [ColumnName("Добавление")]
    [Required]
    public bool AddAcess { get; set; }

    [ColumnName("Редактирование")]
    [Required]
    public bool EditAcess { get; set; }

    [ColumnName("Удаление")]
    [Required]
    public bool DeleteAcess { get; set; }

    override public string ToString()
    {
        return Name;
    }
}
