using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.DAL
{
    public interface ILocationDAL
    {
        bool CreateLocation(Models.Location location);

        bool DeleteLocation(Models.Location location);

        IList<Models.Location> GetAllLocations();
    }
}
