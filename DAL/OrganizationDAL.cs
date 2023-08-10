using ClinicalWebApplication.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ClinicalWebApplication.DAL
{
    public class OrganizationDAL
    {
        public List<Organizations> GetAllOrganizationsList(SqlConnection de)
        {
            return de.Query<Organizations>("EXECUTE GetAllRecords Organizations").ToList();
        }

        public List<Organizations> GetAllActiveOrganizationsList(SqlConnection de)
        {
            return de.Query<Organizations>("EXECUTE GetAllRecords Organizations").OrderByDescending(x=>x.Id).ToList();
        }

        public Organizations GetOrganizationById(int id, SqlConnection de)
        {
            return de.Query<Organizations>("EXECUTE GetAllRecords Organizations,Id," + id + "").FirstOrDefault();
        }

        public Organizations GetActiveOrganizationById(int id, SqlConnection de)
        {
            return de.Query<Organizations>("EXECUTE GetAllRecords Organizations,Id," + id + "").FirstOrDefault();
        }

        public async Task<bool> AddOrganization(Organizations _organization, SqlConnection de)
        {
            try
            {
                var getPropandVal = new UserDAL().GetPropandVal(_organization);
                var add = await de.QueryAsync("EXECUTE InsertOrUpdate 0,Organizations," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return true;

            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateOrganization(Organizations _organization, SqlConnection de)
        {
            try
            {
                var getPropandVal = new UserDAL().GetUpdatePropandVal(_organization);
                //var query = "EXECUTE InsertOrUpdate " + _user.Id + ",Users,'" + getPropandVal + "'";
                var update = await de.QueryAsync("EXECUTE InsertOrUpdate " + _organization.Id + ",Organizations,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteOrganization(int id, SqlConnection de)
        {
            try
            {
                Organizations u = GetActiveOrganizationById(id, de);

                u.IsActive = 0;
                u.DeletedAt = DateTime.UtcNow;
                var getPropandVal = new UserDAL().GetUpdatePropandVal(u);
                var delete = await de.QueryAsync("EXECUTE InsertOrUpdate " + u.Id + ",Organizations,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
