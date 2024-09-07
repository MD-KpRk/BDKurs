using BDKurs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Gender
{
    [Key]
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