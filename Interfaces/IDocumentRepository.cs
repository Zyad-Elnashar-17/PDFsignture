using PDFsignture.Models;

namespace PDFsignture.Interfaces
{
    public interface IDocumentRepository
    {
        Task<Document> GetByIdAsync(int id);
        Task<IEnumerable<Document>> GetUserDocumentsAsync(string userId);
        Task AddAsync(Document document);
        Task AddSignedDocumentAsync(SignedDocument signedDocument);
        Task<IEnumerable<SignedDocument>> GetUserSignedDocumentsAsync(string userId);
        Task SaveChangesAsync();
    }
}
