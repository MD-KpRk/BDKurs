using BDKurs;
using BDKurs.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Status : BDObject
{
    [ColumnName("Номер")]
    [NonEditable]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StatusID { get; set; }

    [ColumnName("Название")]
    [Required]
    [MaxLength(20)]
    public string Name { get; set; } = "";

    override public string ToString()
    {
        return Name;
    }
}
