using LinqSpecs;

namespace Lithium.Api.Galleries;

public interface IGalleryRepository
{
    Task<DTO?> GetGalleryByIdAsync<DTO>(Guid galleryId);

    Task<IEnumerable<DTO>> GetGalleriesAsync<DTO>(Specification<Gallery> filter);
}
