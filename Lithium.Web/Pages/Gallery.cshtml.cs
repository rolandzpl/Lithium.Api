using Mapster;
using Lithium.Api;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    public class GalleryModel : PageModel
    {
        private readonly IGalleryApi galleryApi;
        private List<GalleryItem> galleries = new List<GalleryItem>();
        public IEnumerable<GalleryItem> Galleries => galleries;
        private List<ImageItem> images = new List<ImageItem>();
        public IEnumerable<ImageItem> Images => images;

        public Guid SelectedGallery { get; set; }

        public GalleryModel(IGalleryApi galleryApi) => this.galleryApi = galleryApi;

        public async Task OnGet(Guid? selectedGallery)
        {
            if (selectedGallery != null)
            {
                SelectedGallery = selectedGallery.Value;
            }
            var data = await galleryApi.GetGalleries();
            var data1 = await galleryApi.GetImages(SelectedGallery);
            galleries.AddRange(data.Select(_ => new GalleryItem
            {
                Id = _.Id,
                Title = _.Title ?? "No Title",
                Url = $"/Gallery/{_.Id}",
                CoverImageUrl = "https://localhost:7125/gallery/images/MjQ2MDRlNjktZjQ5Mi00OWI5LWE4NTQtODQ2MzdiMTEwMjMw"
            }));
            images.AddRange(data1.Select(_ => new ImageItem
            {
                ImageId = _.ImageId,
                ImageUrl = _.ImageUrl ?? "https://localhost:7125/gallery/images/MjQ2MDRlNjktZjQ5Mi00OWI5LWE4NTQtODQ2MzdiMTEwMjMw"
            }));
        }

        public class GalleryItem
        {
            public Guid Id { get; init; }
            public string Url { get; init; }
            public string Title { get; init; }
            public string CoverImageUrl { get; init; }
        }

        public class ImageItem
        {
            public Guid ImageId { get; init; }
            public string ImageUrl { get; init; }
        }
    }
}
