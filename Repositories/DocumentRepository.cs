using Microsoft.EntityFrameworkCore;
using PDFsignture.Data;
using PDFsignture.Interfaces;
using PDFsignture.Models;

namespace PDFsignture.Services
{
    public class DocumentRepository : IDocumentRepository
    {

        private readonly AppDbContext _context;

        public DocumentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Document> GetByIdAsync(int id)
            => await _context.Documents.FindAsync(id);

        public async Task<IEnumerable<Document>> GetUserDocumentsAsync(string userId)
            => await _context.Documents
                .Where(d => d.UserId == userId)
                .OrderByDescending(d => d.UploadedAt)
                .ToListAsync();

        public async Task AddAsync(Document document)
            => await _context.Documents.AddAsync(document);

        public async Task AddSignedDocumentAsync(SignedDocument signedDocument)
            => await _context.SignedDocuments.AddAsync(signedDocument);

        public async Task<IEnumerable<SignedDocument>> GetUserSignedDocumentsAsync(string userId)
            => await _context.SignedDocuments
                .Include(sd => sd.Document)
                .Include(sd => sd.Signature)
                .Where(sd => sd.UserId == userId)
                .OrderByDescending(sd => sd.SignedAt)
                .ToListAsync();

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
