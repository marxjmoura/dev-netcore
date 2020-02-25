namespace Developing.API.Models.Vehicles
{
    public sealed class VehicleNotFoundError : NotFoundError
    {
        public VehicleNotFoundError()
        {
            Error = "VEHICLE_NOT_FOUND";
        }
    }
}
