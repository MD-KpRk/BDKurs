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

    override public string ToString()
    {
        return Name;
    }
}
