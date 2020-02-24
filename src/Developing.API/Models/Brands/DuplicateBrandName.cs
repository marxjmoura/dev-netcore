namespace Developing.API.Models.Brands
{
    public sealed class DuplicateBrandName : UnprocessableEntityError
    {
        public DuplicateBrandName()
        {
            Error = "DUPLICATE_BRAND_NAME";
        }
    }
}
