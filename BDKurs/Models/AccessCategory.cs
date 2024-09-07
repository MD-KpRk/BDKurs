using BDKurs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class AccessCategory
{
    [ColumnName("Номер")]
    [Key]
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
