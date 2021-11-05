using LinqSpecs;

namespace Lithium.Api.Gallery;

class GalleryFilters
{
    public static Specification<Gallery> FilterDefault() =>
        new AdHocSpecification<Gallery>(_ => true);

    public static Specification<Gallery> FilterById(Guid galleryId) =>
        new AdHocSpecification<Gallery>(_ => _.Id == galleryId);
}
