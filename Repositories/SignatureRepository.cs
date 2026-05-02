using PDFsignture.Data;
using PDFsignture.Interfaces;
using PDFsignture.Models;
using Microsoft.EntityFrameworkCore;

namespace PDFsignture.Services
{
    public class SignatureRepository : ISignatureRepository
    {

        private readonly AppDbContext _context;

        public SignatureRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<Signature> GetByIdAsync(int id)
            => await _context.Signatures.FindAsync(id);

        public async Task<IEnumerable<Signature>> GetUserSignaturesAsync(string userId)
            => await _context.Signatures
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

        public async Task AddAsync(Signature signature)
            => await _context.Signatures.AddAsync(signature);

        public async Task SetDefaultSignatureAsync(string userId, int signatureId)
        {
            var userSignatures = await _context.Signatures
                .Where(s => s.UserId == userId)
                .ToListAsync();

            foreach (var sig in userSignatures)
            {
                sig.IsDefault = (sig.Id == signatureId);
            }
        }

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
