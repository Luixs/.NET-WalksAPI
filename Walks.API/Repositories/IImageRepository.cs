using Walks.API.Models.Domain;

namespace Walks.API.Repositories
{
    public interface IImageRepository
    {
        Task<Image> UploadImage(Image image);
    }
}
