using BDKurs;
using BDKurs.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Gender : BDObject
{
    [Key]
    [NonEditable]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int GenderID { get; set; }

    [ColumnName("Название")]
    [Required]
    [MaxLength(10)]
    public string Name { get; set; } = "";


    override public string ToString()
    {
        return Name;
    }
}