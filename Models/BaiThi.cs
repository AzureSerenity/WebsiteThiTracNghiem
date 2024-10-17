using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DoAnCoSo.Models
{
    public class BaiThi
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string TenBaiThi { get; set; }

        [Required, StringLength(50)]
        public string MaDe { get; set; }

        [Range(1, 420)]
        public int ThoiGianLamBai { get; set; }

        [Range(1, int.MaxValue)]
        public int SoLuongCauHoi { get; set; }

        public string? Mota { get; set; }

        public int solanlambai { get; set; }

        public List<CauHoi> CauHois { get; set; }
    }
}
