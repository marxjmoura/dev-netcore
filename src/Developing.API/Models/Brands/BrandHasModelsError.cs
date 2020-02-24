namespace Developing.API.Models.Brands
{
    public sealed class BrandHasModelsError : UnprocessableEntityError
    {
        public BrandHasModelsError()
        {
            Error = "BRAND_HAS_MODELS";
        }
    }
}
