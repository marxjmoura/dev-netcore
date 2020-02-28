namespace Developing.API.Models.Models
{
    public class DuplicateModelNameError : UnprocessableEntityError
    {
        public DuplicateModelNameError()
        {
            Error = "DUPLICATE_MODEL_NAME";
        }
    }
}
