using ClinicalWebApplication.DAL;
using ClinicalWebApplication.Models;
using Microsoft.Data.SqlClient;

namespace ClinicalWebApplication.BL
{
    public class StudyBL
    {

        public List<Study> GetAllStudiesList(SqlConnection de)
        {
            return new StudyDAL().GetAllStudiesList(de);
        }

        public List<Study> GetAllActiveStudies(SqlConnection de)
        {
            return new StudyDAL().GetAllActiveStudies(de);
        }


        public Study GetStudyById(int id, SqlConnection de)
        {
            return new StudyDAL().GetStudyById(id, de);
        }

        public Study GetActiveStudyById(int id, SqlConnection de)
        {
            return new StudyDAL().GetActiveStudyById(id, de);
        }


        public async Task<bool> AddStudy(Study _study, SqlConnection de)
        {
            if (String.IsNullOrEmpty(_study.OrganizationId.ToString()) || String.IsNullOrEmpty(_study.Allocation))
                return false;

            return await new StudyDAL().AddStudy(_study, de);
        }

        public async Task<bool> UpdateStudy(Study _study, SqlConnection de)
        {
            return await new StudyDAL().UpdateStudy(_study, de);
        }

        public async Task<bool> DeleteStudy(int id, SqlConnection de)
        {
            return await new StudyDAL().DeleteStudy(id, de);
        }

        public async Task<int> AddInvestigatorSite(InvestigatorSites site, SqlConnection de)
        {
            return await new StudyDAL().AddInvestigatorSite(site, de);
        }

        public async Task<int> AddMonitoringVisit(Monitoringvisit site, SqlConnection de)
        {
            return await new StudyDAL().AddMonitoringVisit(site, de);
        }

        public List<InvestigatorSites> GetAllActiveInvestigatorSites(SqlConnection de)
        {
            return new StudyDAL().GetAllActiveInvestigatorSites(de);
        }
        public List<GeneralFindings> GetAllActiveGeneralFindings(SqlConnection de)
        {
            return new StudyDAL().GetAllActiveGeneralFindings(de);
        }


        public InvestigatorSites GetActiveInvestigatorSiteById(int id, SqlConnection de)
        {
            return new StudyDAL().GetActiveInvestigatorSiteById(id, de);
        }

        public List<InvestigatorSites> GetActiveInvestigatorSitesByStudyId(int id, SqlConnection de)
        {
            return new StudyDAL().GetActiveInvestigatorSitesByStudyId(id, de);
        }
        public List<InvestigatorSites> GetAllActiveInvestigatorSitesById(int id, SqlConnection de)
        {
            return new StudyDAL().GetAllActiveInvestigatorSitesById(id, de);
        }
        public async Task<bool> UpdateInvestigatorSite(InvestigatorSites site, SqlConnection de)
        {
            return await new StudyDAL().UpdateInvestigatorSite(site, de);
        }

        public async Task<bool> AddProtocolDeviation(ProtocolDeviations protocolDeviations, SqlConnection de)
        {
            return await new StudyDAL().AddProtocolDeviation(protocolDeviations, de);
        }

        public ProtocolDeviations GetActiveProtocolDeviationById(int id, SqlConnection de)
        {
            return new StudyDAL().GetActiveProtocolDeviationById(id, de);
        }

        public Actions GetActiveActionById(int id, SqlConnection de)

        {
            return new StudyDAL().GetActiveActionById(id, de);
        }
        public StaffInvestigatorSite GetActiveStaffInvestigatorById(int id, SqlConnection de)

        {
            return new StudyDAL().GetActiveStaffInvestigatorById(id, de);
        }

        public async Task<bool> UpdateProtocolDeviation(ProtocolDeviations protocolDeviations, SqlConnection de)
        {
            return await new StudyDAL().UpdateProtocolDeviation(protocolDeviations, de);
        }
        public async Task<bool> UpdatesStaffInvestigator(StaffInvestigatorSite _STAFF, SqlConnection de)
        {
            return await new StudyDAL().UpdatesStaffInvestigatorn(_STAFF, de);
        }

        public async Task<bool> AddStaffInvestigatorSite(StaffInvestigatorSite staff, SqlConnection de)
        {
            return await new StudyDAL().AddStaffInvestigatorSite(staff, de);
        }

        public async Task<bool> AddAttendie(Attendies attendie, SqlConnection de)
        {
            return await new StudyDAL().AddAttendie(attendie, de);
        }
        public async Task<bool> AddGeneralFindings(GeneralFindings finding, SqlConnection de)
        {
            return await new StudyDAL().AddGeneralFindings(finding, de);
        }

        public async Task<bool> DeleteProtocolDeviation(int id, SqlConnection de)
        {
            return await new StudyDAL().DeleteProtocolDeviation(id, de);
        }


        //public async Task<int> DeleteGeneralFinding(int id, SqlConnection de)

        public async Task<bool> DeleteGeneralFinding(int id, SqlConnection de)
        {
            return await new StudyDAL().DeleteGeneralFinding(id, de);
        }
        public async Task<bool> DeleteMonitoringVisit(int id, SqlConnection de)
        {
            return await new StudyDAL().DeleteMonitoringVisit(id, de);
        }

        public async Task<bool> DeleteInvestigatorSite(int id, SqlConnection de)
        {
            return await new StudyDAL().DeleteInvestigatorSite(id, de);
        }

        public List<ProtocolDeviations> GetAllActiveProtocolDeviations(SqlConnection de)
        {
            return new StudyDAL().GetAllActiveProtocolDeviations(de);
        }
        public List<Monitoringvisit> GetAllActiveMonitoringVisits(SqlConnection de)
        {
            return new StudyDAL().GetAllActiveMonitoringVisit(de);
        }

        public List<Monitoringvisit> GetActiveMonitoringVisitsbyId(int id,SqlConnection de)
        {
            return new StudyDAL().GetActiveMonitoringVisitsbyId(id,de);
        }

        public List<Attendies> GetAttendiesListByMonitoringvisitId(int id, SqlConnection de)
        {
            return new StudyDAL().GetAttendiesListByMonitoringvisitId(id, de);
        }
        public Monitoringvisit GetActivesingleMonitoringVisitsbyId(int Id, SqlConnection de)
        {
            return new StudyDAL().GetActivesingleMonitoringVisitsbyId(Id, de);
        }

        public GeneralFindings GetActiveGeneralFindingById(int Id, SqlConnection de)
        {
            return new StudyDAL().GetActiveGeneralFindingById(Id, de);
        }

        #region Manage ADI
        public async Task<bool> addAction(Actions _action, SqlConnection de)
        {
            return await new StudyDAL().addNewActions(_action, de);
        }
        public async Task<bool> addInformative(Informativs _Info, SqlConnection de)
        {
            return await new StudyDAL().addInformative(_Info, de);
        }

        public async Task<bool> addDecision(Decisions _decision, SqlConnection de)
        {
            return await new StudyDAL().addNewDecision(_decision, de);
        }


        public List<StaffInvestigatorSite> GetAllActiveStaffInvestigator(SqlConnection de)
        {
            return new StudyDAL().GetAllActiveStaffInvestigator(de);
        }
        
        public List<Actions> GetAllActiveActions(SqlConnection de)
        {
            return new StudyDAL().GetAllActiveActions(de);
        }
        public List<Informativs> GetAllActiveInformative(SqlConnection de)
        {
            return new StudyDAL().GetAllActiveinformative(de);
        }

        public List<Decisions> GetAllActiveDecisions(SqlConnection de)

        {
            return new StudyDAL().GetAllActiveDecisions(de);
        }
        

            public async Task<bool> DeleteAction(int id, SqlConnection de)
        {
            return await new StudyDAL().DeleteAction(id, de);
        }

        public async Task<bool> DeleteInvestigatorStaff(int id, SqlConnection de)
        {
            return await new StudyDAL().DeleteInvestigatorStaff(id, de);
        }
        public async Task<bool> DeleteInformative(int id, SqlConnection de)
        {
            return await new StudyDAL().DeleteInformative(id, de);
        }
        
        public async Task<bool> DeleteDecision(int id, SqlConnection de)
        {
            return await new StudyDAL().DeleteDecision(id, de);
        }

        public async Task<bool> UpdateActions(Actions _Act, SqlConnection de)
        {
            return await new StudyDAL().UpdateAction(_Act, de);
        }
        public async Task<bool> UpdateInformative(Informativs _Act, SqlConnection de)
        {
            return await new StudyDAL().UpdateInformative(_Act, de);
        }

        public async Task<bool> UpdateDecision(Decisions _Act, SqlConnection de)
        {
            return await new StudyDAL().UpdateDecision(_Act, de);
        }

        public async Task<bool> UpdateMonitoringVisit(Monitoringvisit _Act, SqlConnection de)
        {
            return await new StudyDAL().UpdateMonitoringVisit(_Act, de);
        }
        public Decisions GetActiveDecisionById(int id, SqlConnection de)

        {
            return new StudyDAL().GetActiveDecisionById(id, de);
        }


        public Informativs GetActiveInformativebyId(int id, SqlConnection de)

        {
            return new StudyDAL().GetActiveInformativebyId(id, de);
        }

        public List<StaffInvestigatorSite> GetActiveInvestigatorSiteStaffById(int id, SqlConnection de)
        {
            return new StudyDAL().GetActiveInvestigatorSiteStaffById(id, de);
        }

        public StaffInvestigatorSite GetSingleInvestigatorSiteStaffById(int id, SqlConnection de)
        {
            return new StudyDAL().GetSingleInvestigatorSiteStaffById(id, de);
        }
        public List<Monitoringvisit> GetMonitoringByInvestigatorSiteId(int id, SqlConnection de)

        {
            return new StudyDAL().GetMonitoringByInvestigatorSiteId(id, de);
        }
        
        public List<StaffInvestigatorSite> GetAllInvestigatorSiteStaff(SqlConnection de)
        {
            return new StudyDAL().GetAllInvestigatorSiteStaff(de);
        }

        #endregion


    }
}
