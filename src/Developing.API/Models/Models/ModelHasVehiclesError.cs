namespace Developing.API.Models.Models
{
    public sealed class ModelHasVehiclesError : UnprocessableEntityError
    {
        public ModelHasVehiclesError()
        {
            Error = "MODEL_HAS_VEHICLES";
        }
    }
}
