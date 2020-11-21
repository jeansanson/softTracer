using MySql.Data.MySqlClient;
using SofTracerAPI.Commands.Projects.Requirements;
using SofTracerAPI.Models.Projects.Requirements;
using SofTracerAPI.Services;

namespace SoftTracerAPI.Repositories
{
    public class RequirementsRepository

    {
        private readonly MySqlConnection _connection;

        public RequirementsRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        #region CreateRequirements

        public void CreateRequirements(CreateRequirementsCommand command)
        {
            RequirementsService service = new RequirementsService();
            Requirement requirement = service.MapCommand(command);



        }

        #endregion

    }
}