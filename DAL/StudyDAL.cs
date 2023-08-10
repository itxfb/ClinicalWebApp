using ClinicalWebApplication.BL;
using ClinicalWebApplication.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ClinicalWebApplication.DAL
{
    public class StudyDAL
    {
        public List<Study> GetAllStudiesList(SqlConnection de)
        {
            return de.Query<Study>("EXECUTE GetAllRecords Study").ToList();
        }

        public List<Study> GetAllActiveStudies(SqlConnection de)
        {
            return de.Query<Study>("EXECUTE GetAllRecords Study").OrderByDescending(x => x.Id).ToList();
        }


        public List<ProtocolDeviations> GetAllActiveProtocolDeviations(SqlConnection de)
        {
            return de.Query<ProtocolDeviations>("EXECUTE GetAllRecords ProtocolDeviations").OrderByDescending(x => x.Id).ToList();
        }

        public Study GetStudyById(int id, SqlConnection de)
        {
            return de.Query<Study>("EXECUTE GetAllRecords Study,Id," + id + "").FirstOrDefault();
        }

        public Study GetActiveStudyById(int id, SqlConnection de)
        {
            return de.Query<Study>("EXECUTE GetAllRecords Study,Id," + id + "").FirstOrDefault();
        }

        public async Task<bool> AddStudy(Study study, SqlConnection de)
        {
            try
            {
                var getPropandVal = new UserDAL().GetPropandVal(study);
                var add = await de.QueryAsync("EXECUTE InsertOrUpdate 0,Study," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return true;

            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateStudy(Study study, SqlConnection de)
        {
            try
            {
                study.IsActive = 1;
                var getPropandVal = new UserDAL().GetUpdatePropandVal(study);
                //var query = "EXECUTE InsertOrUpdate " + _user.Id + ",Users,'" + getPropandVal + "'";
                var update = await de.QueryAsync("EXECUTE InsertOrUpdate " + study.Id + ",Study,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<InvestigatorSites> GetAllActiveInvestigatorSites(SqlConnection de)
        {
            return de.Query<InvestigatorSites>("EXECUTE GetAllRecords InvestigatorSites").ToList();
        }

        public List<GeneralFindings> GetAllActiveGeneralFindings(SqlConnection de)
        {
            return de.Query<GeneralFindings>("EXECUTE GetAllRecords GeneralFindings").ToList();
        }

        public InvestigatorSites GetActiveInvestigatorSiteById(int id, SqlConnection de)
        {
            //var test = de.Query<InvestigatorSites>("EXECUTE GetAllRecords InvestigatorSites,Id," + id + "").FirstOrDefault();


            return de.Query<InvestigatorSites>("EXECUTE GetAllRecords InvestigatorSites,Id," + id + "").FirstOrDefault();

        }

        public List<InvestigatorSites> GetActiveInvestigatorSitesByStudyId(int id, SqlConnection de)
        {
            return de.Query<InvestigatorSites>("EXECUTE GetAllRecords InvestigatorSites,StudyId," + id + "").ToList();
        }
        public List<InvestigatorSites> GetAllActiveInvestigatorSitesById(int id, SqlConnection de)
        {
            return de.Query<InvestigatorSites>("EXECUTE GetAllRecords InvestigatorSites,Id," + id + "").ToList();
        }


        public ProtocolDeviations GetActiveProtocolDeviationById(int id, SqlConnection de)
        {
            return de.Query<ProtocolDeviations>("EXECUTE GetAllRecords ProtocolDeviations,Id," + id + "").FirstOrDefault();
        }
        //public async Task<bool> UpdateStaffInvestigatorn(StaffInvestigatorSite _staff, SqlConnection de)
        //{
        //    return await new StudyDAL().UpdateProtocolDeviation(_staff, de);
        //}
        public GeneralFindings GetActiveGeneralFindingById(int id, SqlConnection de)
        {
            return de.Query<GeneralFindings>("EXECUTE GetAllRecords GeneralFindings,Id," + id + "").FirstOrDefault();
        }

        public Decisions GetActiveDecisionById(int id, SqlConnection de)
        {
            return de.Query<Decisions>("EXECUTE GetAllRecords Decisions,Id," + id + "").FirstOrDefault();
        }


        public async Task<bool> UpdateInvestigatorSite(InvestigatorSites site, SqlConnection de)
        {
            try
            {
                site.IsActive = 1;
                var getPropandVal = new UserDAL().GetUpdatePropandVal(site);
                var update = await de.QueryAsync("EXECUTE InsertOrUpdate " + site.Id + ",InvestigatorSites,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddStaffInvestigatorSite(StaffInvestigatorSite staff, SqlConnection de)
        {
            try
            {
                var getPropandVal = new UserDAL().GetPropandVal(staff);
                var add = await de.QueryAsync("EXECUTE InsertOrUpdate 0,StaffInvestigatorSites," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return true;
            }
            catch
            {
                return false;
            }
        }



        public async Task<bool> AddAttendie(Attendies attendie, SqlConnection de)
        {
            try
            {
                var getPropandVal = new UserDAL().GetPropandVal(attendie);
                var add = await de.QueryAsync("EXECUTE InsertOrUpdate 0,Attendies," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> AddGeneralFindings(GeneralFindings attendie, SqlConnection de)
        {
            try
            {
                var getPropandVal = new UserDAL().GetPropandVal(attendie);
                var add = await de.QueryAsync("EXECUTE InsertOrUpdate 0,GeneralFindings," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteInvestigatorSite(int id, SqlConnection de)
        {
            try
            {
                InvestigatorSites u = GetActiveInvestigatorSiteById(id, de);

                u.IsActive = 0;
                u.DeletedAt = DateTime.UtcNow;
                var getPropandVal = new UserDAL().GetUpdatePropandVal(u);
                var delete = await de.QueryAsync("EXECUTE InsertOrUpdate " + u.Id + ",InvestigatorSites,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<bool> DeleteStudy(int id, SqlConnection de)
        {
            try
            {
                Study u = GetActiveStudyById(id, de);

                u.IsActive = 0;
                u.DeletedAt = DateTime.UtcNow;
                var getPropandVal = new UserDAL().GetUpdatePropandVal(u);
                var delete = await de.QueryAsync("EXECUTE InsertOrUpdate " + u.Id + ",Study,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<int> AddInvestigatorSite(InvestigatorSites site, SqlConnection de)
        {
            try
            {
                var getPropandVal = new UserDAL().GetPropandVal(site);
                var add = await de.QueryAsync("EXECUTE InsertOrUpdate 0,InvestigatorSites," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return Convert.ToInt32(add.FirstOrDefault().Id);
            }
            catch
            {
                return 0;
            }
        }


        public async Task<int> AddMonitoringVisit(Monitoringvisit site, SqlConnection de)
        {
            try
            {
                var getPropandVal = new UserDAL().GetPropandVal(site);
                var add = await de.QueryAsync("EXECUTE InsertOrUpdate 0,Monitoringvisits," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return Convert.ToInt32(add.FirstOrDefault().Id);
            }
            catch
            {
                return 0;
            }
        }
        public async Task<bool> AddProtocolDeviation(ProtocolDeviations deviations, SqlConnection de)
        {
            try
            {
                var getPropandVal = new UserDAL().GetPropandVal(deviations);
                var add = await de.QueryAsync("EXECUTE InsertOrUpdate 0,ProtocolDeviations," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return true;

            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateProtocolDeviation(ProtocolDeviations _protocolDeviations, SqlConnection de)
        {
            try
            {
                _protocolDeviations.IsActive = 1;
                var getPropandVal = new UserDAL().GetUpdatePropandVal(_protocolDeviations);
                var update = await de.QueryAsync("EXECUTE InsertOrUpdate " + _protocolDeviations.Id + ",ProtocolDeviations,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        } 
        public async Task<bool> UpdatesStaffInvestigatorn(StaffInvestigatorSite _staff, SqlConnection de)
        {
            try
            {
                _staff.IsActive = 1;
                var getPropandVal = new UserDAL().GetUpdatePropandVal(_staff);
                var update = await de.QueryAsync("EXECUTE InsertOrUpdate " + _staff.Id + ",StaffInvestigatorSites,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteProtocolDeviation(int id, SqlConnection de)
        {
            try
            {
                ProtocolDeviations u = GetActiveProtocolDeviationById(id, de);

                u.IsActive = 0;
                u.DeletedAt = DateTime.UtcNow;
                var getPropandVal = new UserDAL().GetUpdatePropandVal(u);
                var delete = await de.QueryAsync("EXECUTE InsertOrUpdate " + u.Id + ",ProtocolDeviations,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }

        //public async Task<int> DeleteGeneralFinding(int id, SqlConnection de)

        public async Task<bool> DeleteGeneralFinding(int id, SqlConnection de)
        {
            try
            {
                GeneralFindings u = GetActiveGeneralFindingById(id, de);

                u.IsActive = 0;
                u.DeletedAt = DateTime.UtcNow;
                var getPropandVal = new UserDAL().GetUpdatePropandVal(u);
                var delete = await de.QueryAsync("EXECUTE InsertOrUpdate " + u.Id + ",GeneralFindings,'" + getPropandVal + "'");
                return true;
                //return u.MonitoringVisitId.Value;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteMonitoringVisit(int id, SqlConnection de)
        {
            try
            {
                Monitoringvisit u = GetActivesingleMonitoringVisitsbyId(id, de);

                u.IsActive = 0;
                u.DeletedAt = DateTime.UtcNow;
                var getPropandVal = new UserDAL().GetUpdatePropandVal(u);
                var delete = await de.QueryAsync("EXECUTE InsertOrUpdate " + u.Id + ",Monitoringvisits,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }



        #region Manage ADI

        public async Task<bool> addNewActions(Actions _Actions, SqlConnection de)
        {
            try
            {
                var getPropandVal = new UserDAL().GetPropandVal(_Actions);
                var add = await de.QueryAsync("EXECUTE InsertOrUpdate 0,Actions," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return true;

            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> addInformative(Informativs _Actions, SqlConnection de)
        {
            try
            {
                var getPropandVal = new UserDAL().GetPropandVal(_Actions);
                var add = await de.QueryAsync("EXECUTE InsertOrUpdate 0,Informativs," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return true;

            }
            catch
            {
                return false;
            }
        }


        public async Task<bool> addNewDecision(Decisions _dec, SqlConnection de)
        {
            try
            {
                var getPropandVal = new UserDAL().GetPropandVal(_dec);
                var add = await de.QueryAsync("EXECUTE InsertOrUpdate 0,Decisions," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return true;

            }
            catch
            {
                return false;
            }
        }

        public List<StaffInvestigatorSite> GetAllActiveStaffInvestigator(SqlConnection de)
        {
            return de.Query<StaffInvestigatorSite>("EXECUTE GetAllRecords StaffInvestigatorSites").OrderByDescending(a => a.Id).ToList();
        }

        public List<Actions> GetAllActiveActions(SqlConnection de)
        {
            return de.Query<Actions>("EXECUTE GetAllRecords Actions").OrderByDescending(a => a.Id).ToList();
        }
        public List<Informativs> GetAllActiveinformative(SqlConnection de)
        {
            return de.Query<Informativs>("EXECUTE GetAllRecords Informativs").OrderByDescending(a => a.Id).ToList();
        }

        public List<Decisions> GetAllActiveDecisions(SqlConnection de)
        {
            return de.Query<Decisions>("EXECUTE GetAllRecords Decisions").OrderByDescending(a => a.Id).ToList();
        }

        public Actions GetActiveActionById(int id, SqlConnection de)
        {
            return de.Query<Actions>("EXECUTE GetAllRecords Actions,Id," + id + "").FirstOrDefault();
        }
        public StaffInvestigatorSite GetActiveStaffInvestigatorById(int id, SqlConnection de)
        {
            return de.Query<StaffInvestigatorSite>("EXECUTE GetAllRecords StaffInvestigatorSites,Id," + id + "").FirstOrDefault();

        }
        public Informativs GetActiveInformativebyId(int id, SqlConnection de)
        {
            return de.Query<Informativs>("EXECUTE GetAllRecords Informativs,Id," + id + "").FirstOrDefault();
        }

        public async Task<bool> DeleteAction(int id, SqlConnection de)
        {
            try
            {
                Actions Ac = GetActiveActionById(id, de);
                
                Ac.IsActive = 0;
                Ac.DeletedAt = DateTime.UtcNow;
                var getPropandVal = new UserDAL().GetUpdatePropandVal(Ac);
                var delete = await de.QueryAsync("EXECUTE InsertOrUpdate " + Ac.Id + ",Actions,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteInvestigatorStaff(int id, SqlConnection de)
        {
            try
            {
                StaffInvestigatorSite Ac = GetSingleInvestigatorSiteStaffById(id, de);

                Ac.IsActive = 0;
                
                var getPropandVal = new UserDAL().GetUpdatePropandVal(Ac);
                var delete = await de.QueryAsync("EXECUTE InsertOrUpdate " + Ac.Id + ",StaffInvestigatorSites,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteInformative(int id, SqlConnection de)
        {
            try
            {
                Informativs Ac = GetActiveInformativebyId(id, de);

                Ac.IsActive = 0;
                Ac.DeletedAt = DateTime.UtcNow;
                var getPropandVal = new UserDAL().GetUpdatePropandVal(Ac);
                var delete = await de.QueryAsync("EXECUTE InsertOrUpdate " + Ac.Id + ",Informativs,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteDecision(int id, SqlConnection de)
        {
            try
            {
                Decisions Ac = GetActiveDecisionById(id, de);

                Ac.IsActive = 0;
                Ac.DeletedAt = DateTime.UtcNow;
                var getPropandVal = new UserDAL().GetUpdatePropandVal(Ac);
                var delete = await de.QueryAsync("EXECUTE InsertOrUpdate " + Ac.Id + ",Decisions,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<bool> UpdateAction(Actions _actions, SqlConnection de)
        {
            try
            {
                _actions.IsActive = 1;
                var getPropandVal = new UserDAL().GetUpdatePropandVal(_actions);
                var update = await de.QueryAsync("EXECUTE InsertOrUpdate " + _actions.Id + ",Actions,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UpdateInformative(Informativs _actions, SqlConnection de)
        {
            try
            {
                _actions.IsActive = 1;
                var getPropandVal = new UserDAL().GetUpdatePropandVal(_actions);
                var update = await de.QueryAsync("EXECUTE InsertOrUpdate " + _actions.Id + ",Informativs,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UpdateDecision(Decisions _actions, SqlConnection de)
        {
            try
            {
                _actions.IsActive = 1;
                var getPropandVal = new UserDAL().GetUpdatePropandVal(_actions);
                var update = await de.QueryAsync("EXECUTE InsertOrUpdate " + _actions.Id + ",Decisions,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        public async Task<bool> UpdateMonitoringVisit(Monitoringvisit _actions, SqlConnection de)
        {
            try
            {
                _actions.IsActive = 1;
                var getPropandVal = new UserDAL().GetUpdatePropandVal(_actions);
                var update = await de.QueryAsync("EXECUTE InsertOrUpdate " + _actions.Id + ",Monitoringvisits,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<StaffInvestigatorSite> GetActiveInvestigatorSiteStaffById(int Id, SqlConnection de)
        {
            //var test= de.Query<StaffInvestigatorSite>("EXECUTE GetAllRecords StaffInvestigatorSites,InvestigatorSiteId," + Id + "").ToList();

            return de.Query<StaffInvestigatorSite>("EXECUTE GetAllRecords StaffInvestigatorSites,InvestigatorSiteId," + Id + "").ToList();
        }
        public StaffInvestigatorSite GetSingleInvestigatorSiteStaffById(int Id, SqlConnection de)
        {
            //var test= de.Query<StaffInvestigatorSite>("EXECUTE GetAllRecords StaffInvestigatorSites,InvestigatorSiteId," + Id + "").ToList();

            return de.Query<StaffInvestigatorSite>("EXECUTE GetAllRecords StaffInvestigatorSites,Id," + Id + "").FirstOrDefault();
        }
        public List<Monitoringvisit> GetMonitoringByInvestigatorSiteId(int Id, SqlConnection de)
        {
            //var test= de.Query<StaffInvestigatorSite>("EXECUTE GetAllRecords Monitoringvisits,InvestigatorSiteId," + Id + "").ToList();

            return de.Query<Monitoringvisit>("EXECUTE GetAllRecords Monitoringvisits,InvestiGatorSitefId," + Id + "").ToList();
        }
        public List<Monitoringvisit> GetActiveMonitoringVisitsbyId(int Id, SqlConnection de)

        {
            //var test= de.Query<StaffInvestigatorSite>("EXECUTE GetAllRecords Monitoringvisits,InvestigatorSiteId," + Id + "").ToList();

            return de.Query<Monitoringvisit>("EXECUTE GetAllRecords Monitoringvisits,Id," + Id + "").ToList();
        }
         public List<Attendies> GetAttendiesListByMonitoringvisitId(int Id, SqlConnection de)

        {
            //var test= de.Query<StaffInvestigatorSite>("EXECUTE GetAllRecords Monitoringvisits,InvestigatorSiteId," + Id + "").ToList();

            return de.Query<Attendies>("EXECUTE GetAllRecords Attendies,MonitoringVisitId," + Id + "").ToList();
        }

        public Monitoringvisit GetActivesingleMonitoringVisitsbyId(int Id, SqlConnection de)

        {
            //var test= de.Query<StaffInvestigatorSite>("EXECUTE GetAllRecords Monitoringvisits,InvestigatorSiteId," + Id + "").ToList();

            return de.Query<Monitoringvisit>("EXECUTE GetAllRecords Monitoringvisits,Id," + Id + "").FirstOrDefault();
        }

        public List<Monitoringvisit> GetAllActiveMonitoringVisit(SqlConnection de)
        {
            return de.Query<Monitoringvisit>("EXECUTE GetAllRecords Monitoringvisits").OrderByDescending(x => x.Id).ToList();
        }

        public List<StaffInvestigatorSite> GetAllInvestigatorSiteStaff(SqlConnection de)
        {
            //var test= de.Query<StaffInvestigatorSite>("EXECUTE GetAllRecords StaffInvestigatorSites,InvestigatorSiteId," + Id + "").ToList();

            return de.Query<StaffInvestigatorSite>("EXECUTE GetAllRecords StaffInvestigatorSites").ToList();
        }

    }
}
