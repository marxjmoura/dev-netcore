namespace Developing.API.Models.Brands
{
    public sealed class BrandNotFoundError : NotFoundError
    {
        public BrandNotFoundError()
        {
            Error = "BRAND_NOT_FOUND";
        }
    }
}
