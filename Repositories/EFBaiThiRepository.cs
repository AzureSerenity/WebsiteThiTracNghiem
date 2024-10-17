using Microsoft.EntityFrameworkCore;
using DoAnCoSo.Models;
using DoAnCoSo.Repositories;

public class EFBaiThiRepository : IBaiThiRepository
{
    private readonly ApplicationDbContext _context;
    public EFBaiThiRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Tương tự như EFProductRepository, nhưng cho Category
    public async Task<IEnumerable<BaiThi>> GetAllAsync()
    {
        // return await _context.Products.ToListAsync();
        return await _context.BaiThis
        .Include(p => p.CauHois) // Include thông tin về product
        .ToListAsync();
    }

    public async Task<BaiThi> GetByIdAsync(int id)
    {
        // return await _context.Products.FindAsync(id);
        // lấy thông tin kèm theo DeThi
        return await _context.BaiThis.Include(p => p.CauHois).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(BaiThi baiThi)
    {
        _context.BaiThis.Add(baiThi);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(BaiThi baiThi)
    {
        _context.BaiThis.Update(baiThi);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {


        var DeThi = await _context.BaiThis.FindAsync(id);
        _context.BaiThis.Remove(DeThi);
        await _context.SaveChangesAsync();
    }
}