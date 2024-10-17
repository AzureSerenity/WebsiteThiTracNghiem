using Microsoft.EntityFrameworkCore;
using DoAnCoSo.Models;

public class EFCauHoiRepository : ICauHoiRepository
{
    private readonly ApplicationDbContext _context;
    public EFCauHoiRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    // Tương tự như EFProductRepository, nhưng cho DeThi
    public async Task<IEnumerable<CauHoi>> GetAllAsync()
    {
        // return await _context.Products.ToListAsync();
        return await _context.CauHois
        .Include(p => p.Cauhoi) // Include thông tin về product
        .ToListAsync();
    }
    public async Task<CauHoi> GetByIdAsync(int id)
    {
        // return await _context.Products.FindAsync(id);
        // lấy thông tin kèm theo DeThi
        return await _context.CauHois.Include(p => p.Cauhoi).FirstOrDefaultAsync(p => p.Mach == id);
    }
    public async Task AddAsync(CauHoi cauHoi)
    {
        _context.CauHois.Add(cauHoi);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(CauHoi cauHoi)
    {
        _context.CauHois.Update(cauHoi);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)
    {


        var cauHoi = await _context.CauHois.FindAsync(id);
        _context.CauHois.Remove(cauHoi);
        await _context.SaveChangesAsync();
    }

}