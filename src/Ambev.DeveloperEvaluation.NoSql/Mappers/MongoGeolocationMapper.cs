using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.NoSql.Mappers;

public class MongoGeolocationMapper
{
    public static double[] ToMongo(GeolocationInfo geo)
    {
        return [geo.Longitude, geo.Latitude]; // MongoDB usa [lon, lat]
    }

    public static GeolocationInfo FromMongo(double[] coordinates)
    {
        if (coordinates.Length != 2)
            throw new ArgumentException("MongoDB geolocation must have exactly 2 elements.");

        return new GeolocationInfo(coordinates[1], coordinates[0]); // Inverte para (lat, lon)
    }
}
