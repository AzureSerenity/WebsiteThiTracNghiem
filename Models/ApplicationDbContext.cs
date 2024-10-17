using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DoAnCoSo.Models;

namespace DoAnCoSo.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<CauHoi> CauHois { get; set; }
        public DbSet<BaiThi> BaiThis { get; set; }
        public DbSet<KetQua> KetQuas { get; set; }
        public DbSet<LichSuLamBai> LichSuLamBais { get; set; }
        public DbSet<ChangePassword> ChangePassword { get; set; }
    }
}
