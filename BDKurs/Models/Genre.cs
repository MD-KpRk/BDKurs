using BDKurs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Genre
{
    [ColumnName("Номер")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int GenreID { get; set; }

    [ColumnName("Название")]
    [Required]
    [MaxLength(20)]
    public string Name { get; set; } = "";


    override public string ToString()
    {
        return Name;
    }
}
