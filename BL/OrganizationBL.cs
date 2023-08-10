using ClinicalWebApplication.DAL;
using ClinicalWebApplication.Models;
using Microsoft.Data.SqlClient;

namespace ClinicalWebApplication.BL
{
    public class OrganizationBL
    {
        public List<Organizations> GetAllOrganizationsList(SqlConnection de)
        {
            return new OrganizationDAL().GetAllOrganizationsList(de);
        }

        public List<Organizations> GetAllActiveOrganizations(SqlConnection de)
        {
            return new OrganizationDAL().GetAllActiveOrganizationsList(de);
        }


        public Organizations GetOrganizationById(int id, SqlConnection de)
        {
            return new OrganizationDAL().GetOrganizationById(id, de);
        }

        public Organizations GetActiveOrganizationById(int id, SqlConnection de)
        {
            return new OrganizationDAL().GetActiveOrganizationById(id, de);
        }
        public async Task<bool> AddOrganization(Organizations _Organization, SqlConnection de)
        {
            if (String.IsNullOrEmpty(_Organization.Name) || String.IsNullOrEmpty(_Organization.CreatedBy.ToString()))
                return false;

            return await new OrganizationDAL().AddOrganization(_Organization, de);
        }

        public async Task<bool> UpdateOrganization(Organizations _Organization, SqlConnection de)
        {
            return await new OrganizationDAL().UpdateOrganization(_Organization, de);
        }

        public async Task<bool> DeleteOrganization(int id, SqlConnection de)
        {
            return await new OrganizationDAL().DeleteOrganization(id, de);
        }
    }
}
