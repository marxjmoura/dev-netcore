namespace Developing.API.Models.Brands
{
    public sealed class DuplicateBrandNameError : UnprocessableEntityError
    {
        public DuplicateBrandNameError()
        {
            Error = "DUPLICATE_BRAND_NAME";
        }
    }
}
