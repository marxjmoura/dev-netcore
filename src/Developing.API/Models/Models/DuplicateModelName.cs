namespace Developing.API.Models.Models
{
    public class DuplicateModelName : UnprocessableEntityError
    {
        public DuplicateModelName()
        {
            Error = "DUPLICATE_MODEL_NAME";
        }
    }
}
