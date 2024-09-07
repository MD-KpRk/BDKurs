using BDKurs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Publisher
{
    [ColumnName("Номер")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PublisherID { get; set; }


    [ColumnName("Название")]
    [Required]
    [MaxLength(120)]
    public string Name { get; set; } = "";

    [ColumnName("Адрес")]
    [MaxLength(120)]
    public string Address { get; set; }

    [ColumnName("Телефон")]
    [MaxLength(30)]
    public string Phone { get; set; }

    [ColumnName("Эл. почта")]
    [MaxLength(30)]
    public string Email { get; set; }


    override public string ToString()
    {
        return Name;
    }
}
