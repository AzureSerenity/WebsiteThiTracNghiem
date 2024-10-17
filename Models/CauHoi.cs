using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnCoSo.Models
{
    public class CauHoi
    {
        [Key]
        public int Mach { get; set; }
        [ForeignKey("BaiThi"), Required]
        public int Mabt { get; set; }
        public string Cauhoi { get; set; }
        public string DapanA { get; set; }
        public string DapanB { get; set; }
        public string DapanC { get; set; }
        public string DapanD { get; set; }
        [Required]
        public string Dapan { get; set; }
        public string? SelectedAnswer { get; set; }
        public BaiThi? BaiThi { get; set; }
    }
}
