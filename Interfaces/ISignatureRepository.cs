using PDFsignture.Models;

namespace PDFsignture.Interfaces
{
    public interface ISignatureRepository
    {
        Task<Signature> GetByIdAsync(int id);
        Task<IEnumerable<Signature>> GetUserSignaturesAsync(string userId);
        Task AddAsync(Signature signature);
        Task SetDefaultSignatureAsync(string userId, int signatureId);
        Task SaveChangesAsync();
    }
}
