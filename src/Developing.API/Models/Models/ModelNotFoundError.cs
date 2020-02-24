namespace Developing.API.Models.Models
{
    public sealed class ModelNotFoundError : NotFoundError
    {
        public ModelNotFoundError()
        {
            Error = "MODEL_NOT_FOUND";
        }
    }
}
