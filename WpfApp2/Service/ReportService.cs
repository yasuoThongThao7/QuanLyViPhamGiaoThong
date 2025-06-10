using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using WpfApp2.Model;

namespace WpfApp2.Service
{
    public class ReportService
    {
        private readonly AppDbContext _context;
        public ReportService() => _context = new AppDbContext();

        // Thêm biên bản
        public async Task AddReportAsync(Report report)
        {
            _context.Report.Add(report);
            await _context.SaveChangesAsync();
        }

        // Lấy toàn bộ danh sách
        public async Task<List<Report>> GetAllReportsAsync()
            => await _context.Report
                .AsNoTracking()
                .Include(r => r.Person)
                .Include(r => r.Vehicle)
                .Include(r => r.Police)
                    .ThenInclude(p => p.Person)
                .Include(r => r.Violations)
                    .ThenInclude(ri => ri.Violation)
                .ToListAsync();

        // Lấy theo Id biên bản
        public async Task<Report?> GetReportByIdAsync(int reportId)
         => await _context.Report
             .AsNoTracking()
             .Include(r => r.Person)
             .Include(r => r.Vehicle)
                 .ThenInclude(v => v.Person) // nếu muốn lấy chủ xe
             .Include(r => r.Vehicle)
                 .ThenInclude(v => v.Brand)  // nếu muốn lấy hãng xe
             .Include(r => r.Vehicle)
                 .ThenInclude(v => v.Type)   // nếu muốn lấy loại xe
             .Include(r => r.Police)
                 .ThenInclude(p => p.Person)
             .Include(r => r.Violations)
                 .ThenInclude(ri => ri.Violation)
             .FirstOrDefaultAsync(r => r.Id == reportId);


        // Lấy theo CCCD
        public async Task<List<Report>> GetReportsByPersonIdAsync(string personId)
            => await _context.Report
                .AsNoTracking()
                .Where(r => r.PersonId == personId)
                .Include(r => r.Person)
                .Include(r => r.Vehicle)
                .Include(r => r.Police)
                    .ThenInclude(p => p.Person)
                .Include(r => r.Violations)
                    .ThenInclude(ri => ri.Violation)
                .ToListAsync();

        // Lấy theo Id và tên người vi phạm
        public async Task<List<Report>> GetReportsByPersonIdAndNameAsync(string text)
            => await _context.Report
                .AsNoTracking()
                .Where(r => r.PersonId == text || r.Person!.Name!.Contains(text))
                .Include(r => r.Person)
                .Include(r => r.Vehicle)
                .Include(r => r.Police)
                    .ThenInclude(p => p.Person)
                .Include(r => r.Violations)
                    .ThenInclude(ri => ri.Violation)
                .ToListAsync();

        // Lấy số tiền phạt theo id
        public async Task<int> GetFineByIdAsync(int reportId)
            => await _context.Report
                .AsNoTracking()
                .Where(r => r.Id == reportId)
                .Select(r => r.TotalFine)
                .SumAsync();

        // Lấy số lượng biên bản theo id
        public async Task<int> GetReportCountByIdAsync(int reportId)
            => await _context.Report
                .AsNoTracking()
                .Where(r => r.Id == reportId)
                .CountAsync();

        // Lấy chưa thanh toán
        public async Task<List<Report>> GetUnpaidReportsAsync()
            => await _context.Report
                .AsNoTracking()
                .Where(r => r.IsPaid == false)
                .Include(r => r.Person)
                .Include(r => r.Vehicle)
                .Include(r => r.Police)
                    .ThenInclude(p => p.Person)
                .Include(r => r.Violations)
                    .ThenInclude(ri => ri.Violation)
                .ToListAsync();

        // Lấy số lượng
        public async Task<int> GetReportCountAsync()
            => await _context.Report
                .AsNoTracking()
                .CountAsync();

        // Lấy theo lỗi vi phạm
        public async Task<List<Report>> GetReportsByVehicleIdAsync(string vehicleId)
            => await _context.Report
                .AsNoTracking()
                .Where(r => r.VehicleId == vehicleId)
                .Include(r => r.Person)
                .Include(r => r.Vehicle)
                .Include(r => r.Police)
                    .ThenInclude(p => p.Person)
                .Include(r => r.Violations)
                    .ThenInclude(ri => ri.Violation)
                .ToListAsync();

        // Lấy chi tiết các lỗi vi phạm theo ID biên bản
        public async Task<List<ReportInfo>> GetViolationsByReportIdAsync(int reportId)
            => await _context.ReportInfo
                .AsNoTracking()
                .Where(ri => ri.ReportId == reportId)
                .Include(ri => ri.Violation)
                .ToListAsync();

        // Lấy báo cáo theo id cảnh sát
        public async Task<List<Report>> GetReportsByPoliceIdAsync(string policeId)
            => await _context.Report
                .AsNoTracking()
                .Where(r => r.PoliceId == policeId)
                .Include(r => r.Person)
                .Include(r => r.Vehicle)
                .Include(r => r.Police)
                    .ThenInclude(p => p.Person)
                .Include(r => r.Violations)
                    .ThenInclude(ri => ri.Violation)
                .ToListAsync();

        // Lấy theo trang
        public async Task<List<Report>> GetPageAsync(int pageNumber, int pageSize)
            => await _context.Report
                .AsNoTracking()
                .Include(r => r.Person)
                .Include(r => r.Vehicle)
                .Include(r => r.Police)
                    .ThenInclude(p => p.Person)
                .Include(r => r.Violations)
                    .ThenInclude(ri => ri.Violation)
                .Skip(pageSize * pageNumber)
                .Take(pageSize)
                .ToListAsync();

        // Lấy danh sách lỗi vi phạm trong biên bản
        public async Task<List<ReportInfo>> GetReportInfosByReportIdAsync(int reportId)
            => await _context.ReportInfo
                .AsNoTracking()
                .Where(ri => ri.ReportId == reportId)
                .Include(ri => ri.Violation)
                .ToListAsync();

        // Add toàn bộ danh sách ReportInfo
        public async Task AddReportInfosAsync(List<ReportInfo> reportInfos)
        {
            foreach (var reportInfo in reportInfos)
            {
                _context.ReportInfo.Add(reportInfo);
            }
            await _context.SaveChangesAsync();
        }
        // Hàm save dữ liệu 
        public async Task SaveChangesAsync(Report rp)
        {
            try
            {
                var report = await _context.Report.FindAsync(rp.Id);
                report.IsPaid = rp.IsPaid;
                report.PaidDate = rp.PaidDate;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lưu: " + ex.Message);
            }
        }

    }
}
