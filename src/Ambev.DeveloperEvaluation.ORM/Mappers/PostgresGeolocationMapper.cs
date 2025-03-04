using Ambev.DeveloperEvaluation.Domain.Entities;
using NetTopologySuite.Geometries;

namespace Ambev.DeveloperEvaluation.ORM.Mappers;

public class PostgresGeolocationMapper
{
    public static Point ToPostgres(GeolocationInfo geo)
    {
        return new Point(geo.Longitude, geo.Latitude) { SRID = 4326 };
    }

    public static GeolocationInfo FromPostgres(Point point)
    {
        return new GeolocationInfo(point.Y, point.X); // Y = Latitude, X = Longitude
    }
}
