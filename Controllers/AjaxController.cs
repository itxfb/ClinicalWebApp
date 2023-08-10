using ClinicalWebApplication.BL;
using ClinicalWebApplication.Filters;
using ClinicalWebApplication.HelpingClasses;
using ClinicalWebApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static ClinicalWebApplication.HelpingClasses.ProjectVariables;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace ClinicalWebApplication.Controllers
{
    public class AjaxController : Controller
    {
        private readonly SqlConnection de;
        private readonly GeneralPurpose gp;
        private readonly IConfiguration confg;

        public AjaxController(IConfiguration confg, IHttpContextAccessor haccess)
        {
            this.de = new SqlConnection(confg.GetConnectionString("Default"));
            this.gp = new GeneralPurpose(de, haccess);
            this.confg = confg;
            var request = haccess.HttpContext.Request;

            baseUrl = $"{request.Scheme}://{request.Host}";
        }

        #region User

        [HttpPost]
        public IActionResult GetUserDataTableList(int category = -1, string Name = "", string email = "")
        {

            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            List<User> ulist = new UserBL().GetAllUsersList(de).Where(x => x.Role != 1).ToList();
            if (CurrentUserRecord.Role != 1)
            {
                ulist = ulist.Where(a => a.OrganizationId == CurrentUserRecord.OrganizationId).ToList();
            }
            if (category == 5)
            {
                ulist = ulist.Where(x => x.Role == 5).ToList();
            }
            if (category == 4)
            {
                ulist = ulist.Where(x => x.Role == 4).ToList();
            }
            if (category == 3)
            {
                ulist = ulist.Where(x => x.Role == 3).ToList();
            }
            if (category == 2)
            {
                ulist = ulist.Where(x => x.Role == 2).ToList();
            }
            if (!String.IsNullOrEmpty(Name))
            {
                ulist = ulist.Where(x => x.FirstName.ToLower().Contains(Name.ToLower()) || x.LastName.ToLower().Contains(Name.ToLower())).ToList();
            }

            if (!String.IsNullOrEmpty(email))
            {
                ulist = ulist.Where(x => x.Email.ToLower().Contains(email.ToLower())).ToList();
            }

            int start = Convert.ToInt32(Request.Form["start"].FirstOrDefault());
            int length = Convert.ToInt32(Request.Form["length"].FirstOrDefault());
            string searchValue = Request.Form["search[value]"].FirstOrDefault();
            string sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"];
            string sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            if (sortColumnName != "" && sortColumnName != null)
            {
                if (sortColumnName != "0")
                {
                    if (sortDirection == "asc")
                    {
                        ulist = ulist.OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                    else
                    {
                        ulist = ulist.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                }
            }

            int totalrows = ulist.Count();

            //filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                ulist = ulist.Where(x => x.Email.ToLower().Contains(searchValue.ToLower())
                                    ).ToList();
            }

            int totalrowsafterfilterinig = ulist.Count();


            // pagination
            ulist = ulist.Skip(start).Take(length).ToList();

            List<User> udto = new List<User>();

            foreach (User u in ulist)
            {
                User obj = new User()
                {

                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Password = StringCipher.Decrypt(u.Password),
                    Email = u.Email,
                    StudyIds = u.StudyIds,
                    Role = u.Role,
                    IsActive = (int)u.IsActive,
                    Organization = u.Organization,


                };



                udto.Add(obj);
            }

            return Json(new { data = udto, draw = Request.Form["draw"].FirstOrDefault(), recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig });
        }


        [HttpPost]
        public bool StatusUpdate(int id, int val)
        {
            var user = new UserBL().GetActiveUserById(id, de);
            user.Status = val;
            var update = new UserBL().UpdateUser(user, de);
            if (update.Result == true)
                return true;
            else
                return false;
        }

        [HttpPost]
        public IActionResult GetUserById(int id)
        {
            User u = new UserBL().GetActiveUserById(id, de);
            if (u == null)
            {
                return Json(0);
            }

            User obj = new User()
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                StudyIds = u.StudyIds,
                OrganizationId = u.OrganizationId,
                Password = StringCipher.Decrypt(u.Password),
                Status = u.Status,

                Role = u.Role,


            };

            return Json(obj);
        }

        [HttpPost]
        public IActionResult fileUpload(IFormFile File)
        {
            try
            {

                var img = new UserBL().UploadImage(File);
                return Json(img.Result);
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region ValidateUser

        [HttpPost]
        public IActionResult ValidateEmail(string email, int id = -1)
        {
            return Json(gp.ValidateEmail(email, id));
        }

        [HttpPost]
        public IActionResult ValidateUsername(string username, int id = -1)
        {
            return Json(gp.ValidateUsername(username, id));
        }

        #endregion

        #region Organization

        public IActionResult GetAllOrganizations()
        {

            var organizationList = new OrganizationBL().GetAllActiveOrganizations(de);
            return Json(organizationList);
        }

        public IActionResult CheckOrganization(string org = "")
        {

            var organizationList = new List<Organizations>();
            if (!string.IsNullOrEmpty(org))
            {
                organizationList = new OrganizationBL().GetAllActiveOrganizations(de).Where(a => a.Name.ToLower().Trim() == org.ToLower().Trim()).ToList();

            }
            if (organizationList.Count() > 0)
            {
                return Json(false);


            }
            return Json(true);
        }

        public IActionResult CheckInvestigatorStaffemail(string email = "")
        {

            var StaffInvestigatorSite = new List<StaffInvestigatorSite>();
            if (!string.IsNullOrEmpty(email))
            {
                StaffInvestigatorSite = new StudyBL().GetAllActiveStaffInvestigator(de).Where(a => a.Email.ToLower().Trim() == email.ToLower().Trim()).ToList();

            }
            if (StaffInvestigatorSite.Count() > 0)
            {
                return Json(false);


            }
            return Json(true);
        }
        [HttpPost]
        public IActionResult GetOrganizationDataTableList(string Name = "")
        {
            List<Organizations> ulist = new OrganizationBL().GetAllActiveOrganizations(de);
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            if (CurrentUserRecord.Role != 1)
            {
                ulist = ulist.Where(a => a.Id == CurrentUserRecord.OrganizationId).ToList();
            }

            if (!String.IsNullOrEmpty(Name))
            {
                ulist = ulist.Where(x => x.Name.ToLower().Contains(Name.ToLower())).ToList();
            }


            int start = Convert.ToInt32(Request.Form["start"].FirstOrDefault());
            int length = Convert.ToInt32(Request.Form["length"].FirstOrDefault());
            string searchValue = Request.Form["search[value]"].FirstOrDefault();
            string sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"];
            string sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            if (sortColumnName != "" && sortColumnName != null)
            {
                if (sortColumnName != "0")
                {
                    if (sortDirection == "asc")
                    {
                        ulist = ulist.OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                    else
                    {
                        ulist = ulist.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                }
            }

            int totalrows = ulist.Count();

            //filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                ulist = ulist.Where(x => x.Name.ToLower().Contains(searchValue.ToLower())
                                    ).ToList();
            }

            int totalrowsafterfilterinig = ulist.Count();


            // pagination
            ulist = ulist.Skip(start).Take(length).ToList();

            List<Organizations> udto = new List<Organizations>();

            foreach (Organizations u in ulist)
            {
                Organizations obj = new Organizations()
                {
                    Id = u.Id,
                    Name = u.Name,
                    IsActive = (int)u.IsActive
                };

                udto.Add(obj);
            }

            return Json(new { data = udto, draw = Request.Form["draw"].FirstOrDefault(), recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig });
        }

        [HttpPost]
        public IActionResult GetOrganizationById(int id)
        {
            Organizations u = new OrganizationBL().GetOrganizationById(id, de);
            if (u == null)
            {
                return Json(0);
            }

            Organizations obj = new Organizations()
            {
                Id = u.Id,
                Name = u.Name,
                CreatedBy = u.CreatedBy,
                IsActive = u.IsActive,

            };

            return Json(obj);
        }


        #endregion

        #region Study

        public IActionResult GetAllStudies(int organizationid = -1)
        {
            var currentUserid = Convert.ToInt32(User.Claims.Where(a => a.Type == ClaimTypes.Sid).First().Value);
            var user = new UserBL().GetActiveUserById(currentUserid, de);
            var studieslist = new StudyBL().GetAllActiveStudies(de);
            if (organizationid != -1)
            {
                studieslist = new StudyBL().GetAllActiveStudies(de).Where(x => x.OrganizationId == organizationid).ToList();

            }
            if (user.Role != 1)
            {
                studieslist = studieslist.Where(a => a.OrganizationId == user.OrganizationId).ToList();
                if (user.Role == 3 || user.Role == 4 || user.Role == 5)
                {
                    //var userstudyids = user.StudyIds.Split(",");

                    //foreach (var item in userstudyids)
                    //{
                    //    studieslist = studieslist.Where(a => a.Id == Convert.ToInt32(item)).ToList();

                    //}

                    studieslist = studieslist.Where(a => user.StudyIds.Contains(a.Id.ToString())).ToList();


                }
            }
            return Json(studieslist);
        }

        [HttpPost]
        public IActionResult GetStudiesDataTableList(int id = -1, string Name = "")
        {
            List<Study> ulist = new StudyBL().GetAllActiveStudies(de);
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            if (CurrentUserRecord.Role != 1)
            {
                ulist = ulist.Where(a => a.OrganizationId == CurrentUserRecord.OrganizationId).ToList();
            }
            if (CurrentUserRecord.Role == 3)
            {
                if (id != -1)
                {
                    ulist = ulist.Where(a => a.Id == id).ToList();

                }
                else
                {
                    ulist = ulist.Where(a => CurrentUserRecord.StudyIds.Contains(a.Id.ToString())).ToList();

                }


            }

            if (CurrentUserRecord.Role == 4)
            {
                ulist = ulist.Where(a => CurrentUserRecord.StudyIds.Contains(a.Id.ToString())).ToList();
            }
            if (!String.IsNullOrEmpty(Name))
            {
                ulist = ulist.Where(x => x.Protocol_Title.ToLower().Contains(Name.ToLower())).ToList();
            }

            int start = Convert.ToInt32(Request.Form["start"].FirstOrDefault());
            int length = Convert.ToInt32(Request.Form["length"].FirstOrDefault());
            string searchValue = Request.Form["search[value]"].FirstOrDefault();
            string sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"];
            string sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            if (sortColumnName != "" && sortColumnName != null)
            {
                if (sortColumnName != "0")
                {
                    if (sortDirection == "asc")
                    {
                        ulist = ulist.OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                    else
                    {
                        ulist = ulist.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                }
            }

            int totalrows = ulist.Count();

            //filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                ulist = ulist.Where(x => x.Protocol_Title.ToLower().Contains(searchValue.ToLower())
                                    ).ToList();
            }

            int totalrowsafterfilterinig = ulist.Count();


            // pagination
            ulist = ulist.Skip(start).Take(length).ToList();


            return Json(new { data = ulist, draw = Request.Form["draw"].FirstOrDefault(), recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig });
        }


        [HttpPost]
        public IActionResult GetStudiesCardsPaginationList(int start, int length, string Name = "")
        {
            List<Study> ulist = new StudyBL().GetAllActiveStudies(de);

            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);

            if (CurrentUserRecord.Role != 1)
            {
                ulist = ulist.Where(a => a.OrganizationId == CurrentUserRecord.OrganizationId).ToList();
            }

            if (CurrentUserRecord.Role == 3)
            {
                ulist = ulist.Where(a => CurrentUserRecord.StudyIds.Contains(a.Id.ToString())).ToList();
            }

            if (!String.IsNullOrEmpty(Name))
            {
                ulist = ulist.Where(x => x.Protocol_Title.ToLower().Contains(Name.ToLower())).ToList();
            }

            int totalrows = ulist.Count();

            //filter
            if (!string.IsNullOrEmpty(Name))
            {
                ulist = ulist.Where(x => x.Protocol_Title.ToLower().Contains(Name.ToLower())
                                    ).ToList();
            }

            int totalrowsafterfilterinig = ulist.Count();


            // pagination
            ulist = ulist.Skip(start).Take(length).ToList();


            return Json(ulist);

        }


        [HttpPost]
        public IActionResult GetStudyById(int id)
        {
            Study u = new StudyBL().GetStudyById(id, de);

            if (u == null)
            {
                return Json(0);
            }



            return Json(u);
        }

        #endregion

        #region Investigator Sites

        [HttpPost]
        public IActionResult GetInvestigatorSiteDataTable(int id = -1, string Name = "", string siteNo = "", string Address = "")
        {
            List<InvestigatorSites> ulist = new StudyBL().GetAllActiveInvestigatorSites(de).OrderByDescending(a => a.Id).ToList();
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            if (CurrentUserRecord.Role == 3)
            {
                if (id != -1)
                {

                    ulist = ulist.Where(a => a.StudyId == id).ToList();

                }

            }

            else if (CurrentUserRecord.Role == 2)
            {
                ulist = ulist.Where(a => a.OrganizationId == CurrentUserRecord.OrganizationId).ToList();


            }
            else if (CurrentUserRecord.Role == 4)
            {

                ulist = ulist.Where(a => CurrentUserRecord.StudyIds.Contains(a.StudyId.ToString()) && CurrentUserRecord.StudyIds != null).ToList();

                //ulist = ulist.Where(x => x.StudyId.ToString().Contains(CurrentUserRecord.StudyIds)).ToList();

            }


            //}

            if (!String.IsNullOrEmpty(Name))
            {
                ulist = ulist.Where(x => x.Facility_Institution_Name.ToLower().Contains(Name.ToLower())).ToList();
            }

            if (!String.IsNullOrEmpty(siteNo))
            {
                ulist = ulist.Where(x => x.Site_No.ToString().ToLower().Contains(siteNo.ToString().ToLower())).ToList();
            }


            if (!String.IsNullOrEmpty(Address))
            {
                ulist = ulist.Where(x => x.Address.ToString().ToLower().Contains(Address.ToLower())).ToList();
            }

            int start = Convert.ToInt32(Request.Form["start"].FirstOrDefault());
            int length = Convert.ToInt32(Request.Form["length"].FirstOrDefault());
            string searchValue = Request.Form["search[value]"].FirstOrDefault();
            string sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"];
            string sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            if (sortColumnName != "" && sortColumnName != null)
            {
                if (sortColumnName != "0")
                {
                    if (sortDirection == "asc")
                    {
                        ulist = ulist.OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                    else
                    {
                        ulist = ulist.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                }
            }

            int totalrows = ulist.Count();

            //filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                ulist = ulist.Where(x => x.Facility_Institution_Name.ToLower().Contains(searchValue.ToLower())
                                    ).ToList();
            }

            int totalrowsafterfilterinig = ulist.Count();


            // pagination
            ulist = ulist.Skip(start).Take(length).ToList();


            return Json(new { data = ulist, draw = Request.Form["draw"].FirstOrDefault(), recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig });
        }


        [HttpPost]
        public IActionResult GetInvestigatorSiteById(int id)
        {
            InvestigatorSites u = new StudyBL().GetActiveInvestigatorSiteById(id, de);

            if (u == null)
            {
                return Json(0);
            }

            return Json(u);
        }

        [HttpPost]
        public IActionResult GetMonitoringVisitById(int id)
        {
           Monitoringvisit u = new StudyBL().GetActivesingleMonitoringVisitsbyId(id, de);

            if (u == null)
            {
                return Json(0);
            }

            return Json(u);
        }


        [HttpPost]
        public IActionResult GetProtocolDeviationById(int id)
        {
            ProtocolDeviations u = new StudyBL().GetActiveProtocolDeviationById(id, de);

            var Investigatorsite = new StudyBL().GetActiveInvestigatorSiteById(u.InvestigatorSiteId, de);

            if (Investigatorsite != null)
            {
                u.IsActive = Investigatorsite.StudyId;

            }
            else
            {
                u.IsActive = 0;

            }
            //using IsActive for the studyId which is not in use for anything// just for getting // wouldn't affect the output.

            if (u == null)
            {
                return Json(0);
            }

            return Json(u);
        }

        [HttpPost]
        public IActionResult GetActionByActionId(int id)
        {
            Actions Ac = new StudyBL().GetActiveActionById(id, de);

            if (Ac == null)
            {
                return Json(0);
            }

            return Json(Ac);
        }

        [HttpPost]
        public IActionResult GetInvestiGatorStaffById(int id)
        {
            StaffInvestigatorSite _staff = new StudyBL().GetActiveStaffInvestigatorById(id, de);

            if (_staff == null)
            {
                return Json(0);
            }

            return Json(_staff);
        }
        public IActionResult GetInformativeByInfoId(int id)
        {
            Informativs Ac = new StudyBL().GetActiveInformativebyId(id, de);

            if (Ac == null)
            {
                return Json(0);
            }

            return Json(Ac);
        }
        public IActionResult GetDecisionByDecisionId(int id)
        {
            Decisions Ac = new StudyBL().GetActiveDecisionById(id, de);

            if (Ac == null)
            {
                return Json(0);
            }

            return Json(Ac);
        }



        [HttpPost]
        public IActionResult GetProtocolDeviationDataTable(int id = -1, int investigatersiteid = -1, string primary_cRA = "", string Subject_status = "")
        {
            List<ProtocolDeviations> ulist = new StudyBL().GetAllActiveProtocolDeviations(de);
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);

            if (CurrentUserRecord.Role == 2)
            {
                ulist = ulist.Where(x => x.OrganizationId == CurrentUserRecord.OrganizationId).ToList();
            }
            if (CurrentUserRecord.Role == 3)
            {
                if (id != -1)
                {
                    ulist = ulist.Where(a => a.StudyId.Value == id).ToList();
                }

                else
                {
                    var plist = new List<ProtocolDeviations>();

                    ulist = ulist.Where(x => x.OrganizationId == CurrentUserRecord.OrganizationId).ToList();
                    foreach (var item in ulist)
                    {
                        int GetStdId = 0;
                        var instid = new StudyBL().GetActiveInvestigatorSiteById(item.InvestigatorSiteId, de);
                        if (instid != null)
                        {
                            GetStdId = instid.StudyId;
                        }
                        //if (CurrentUserRecord.StudyIds.Contains(instid.ToString()))
                        if (CurrentUserRecord.StudyIds.Contains(GetStdId.ToString()))
                        {
                            plist.Add(item);
                        }
                    }

                    ulist = plist;


                }



            }
            if (CurrentUserRecord.Role == 4 || CurrentUserRecord.Role == 5)
            {
                var plist = new List<ProtocolDeviations>();

                ulist = ulist.Where(x => x.OrganizationId == CurrentUserRecord.OrganizationId).ToList();
                ulist=ulist.Where(x=>CurrentUserRecord.StudyIds.Contains(x.StudyId.ToString())).ToList();
                //foreach (var item in ulist)
                //{
                //    var instid = new StudyBL().GetActiveInvestigatorSiteById(item.InvestigatorSiteId, de).StudyId;
                //    if (CurrentUserRecord.StudyIds.Contains(instid.ToString()))
                //    {
                //        plist.Add(item);
                //    }
                //}

                //ulist = plist;
            }

            if (!String.IsNullOrEmpty(primary_cRA))
            {
                ulist = ulist.Where(x => x.Primary_CRA.ToString().ToLower().Contains(primary_cRA.ToLower())).ToList();
            }
            else if (!String.IsNullOrEmpty(Subject_status))
            {
                ulist = ulist.Where(x => x.Subject_Status.ToString().ToLower().Contains(Subject_status.ToLower())).ToList();

            }

            //if (CurrentUserRecord.Role != 1)
            //{
            //    if (id != -1)
            //    {
            //        ulist = ulist.Where(a => a.InvestigatorSiteId == id).ToList();
            //    }
            //}


            int start = Convert.ToInt32(Request.Form["start"].FirstOrDefault());
            int length = Convert.ToInt32(Request.Form["length"].FirstOrDefault());
            string searchValue = Request.Form["search[value]"].FirstOrDefault();
            string sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"];
            string sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            if (sortColumnName != "" && sortColumnName != null)
            {
                if (sortColumnName != "0")
                {
                    if (sortDirection == "asc")
                    {
                        ulist = ulist.OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                    else
                    {
                        ulist = ulist.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                }
            }

            int totalrows = ulist.Count();

            //filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                ulist = ulist.Where(x => x.Primary_CRA.ToLower().Contains(searchValue.ToLower())).ToList();
            }

            int totalrowsafterfilterinig = ulist.Count();


            // pagination
            ulist = ulist.Skip(start).Take(length).ToList();


            return Json(new { data = ulist, draw = Request.Form["draw"].FirstOrDefault(), recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig });
        }

        public IActionResult GetMonitoringVisitDataTable(int id = -1, int investigatersiteid = -1, string Name = "", string VisitType = "")
        {
            List<Monitoringvisit> ulist = new StudyBL().GetAllActiveMonitoringVisits(de);
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);

            if (CurrentUserRecord.Role == 2)
            {
                ulist = ulist.Where(x => x.OrganizationId == CurrentUserRecord.OrganizationId).ToList();
            }
            if(CurrentUserRecord.Role==3)
            {
                ulist = ulist.Where(a => CurrentUserRecord.StudyIds.Contains(a.StudyId.ToString())).ToList();

            }

            if (CurrentUserRecord.Role == 5)
            {
                ulist = ulist.Where(a => CurrentUserRecord.StudyIds.Contains(a.StudyId.ToString())).ToList();

            }

           

            if (!String.IsNullOrEmpty(Name))
            {
                ulist = ulist.Where(x => x.MonitoringVisitTitle.ToString().ToLower().Contains(Name.ToLower()) && x.IsActive==1 &&x.MonitoringVisitTitle!=null).ToList();
            }
            else if (!String.IsNullOrEmpty(VisitType))
            {
                ulist = ulist.Where(x => x.VisitType.ToString().ToLower().Contains(VisitType.ToLower()) && x.IsActive==1).ToList();

            }

            //if (CurrentUserRecord.Role != 1)
            //{
            //    if (id != -1)
            //    {
            //        ulist = ulist.Where(a => a.InvestigatorSiteId == id).ToList();
            //    }
            //}


            int start = Convert.ToInt32(Request.Form["start"].FirstOrDefault());
            int length = Convert.ToInt32(Request.Form["length"].FirstOrDefault());
            string searchValue = Request.Form["search[value]"].FirstOrDefault();
            string sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"];
            string sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            if (sortColumnName != "" && sortColumnName != null)
            {
                if (sortColumnName != "0")
                {
                    if (sortDirection == "asc")
                    {
                        ulist = ulist.OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                    else
                    {
                        ulist = ulist.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                }
            }

            int totalrows = ulist.Count();

            //filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                ulist = ulist.Where(x => x.MonitoringVisitTitle.ToLower().Contains(searchValue.ToLower())).ToList();
            }

            int totalrowsafterfilterinig = ulist.Count();


            // pagination
            ulist = ulist.Skip(start).Take(length).ToList();

            //List<Monitoringvisit> udto = new List<Monitoringvisit>();
            List<MonitoringVisitDto> MonitoringVisitDto = new List<MonitoringVisitDto>();

            

            foreach (Monitoringvisit u in ulist)
            {
                var studybyId = new StudyBL().GetActiveStudyById(u.StudyId.Value, de);
                var study = "";
                if(studybyId != null)
                {
                    study = studybyId.Protocol_Title;

                }
                var organizationById = new OrganizationBL().GetOrganizationById(u.OrganizationId.Value, de);
                var organization = "";
                if (organizationById != null)
                {
                    organization = organizationById.Name;
                }
                var created_by = new UserBL().GetActiveUserById(u.Createdby.Value,de);
                var createdby = "";
                if (created_by!=null)
                {
                   createdby = created_by.FirstName + " " + created_by.LastName;

                }


                var investigatorsite = new StudyBL().GetActiveInvestigatorSiteById(u.InvestiGatorSitefId.Value, de);
                var investigatorsitenumber = "";
                if(investigatorsite!=null)
                {
                    investigatorsitenumber = investigatorsite.Facility_Institution_Name + "( " + investigatorsite.Site_No + " )";

                }


                MonitoringVisitDto obj = new MonitoringVisitDto()
                {

                    Id = u.Id,
                    MonitoringVisitTitle = u.MonitoringVisitTitle,
                    VisitDate = u.VisitDate,
                    VisitType = u.VisitType,
                    Study = study,
                    Status = u.Status,
                    InvestiGatorSitefId = investigatorsitenumber,
                    Organization = organization,
                    Createdby = createdby


                };



                MonitoringVisitDto.Add(obj);
            }



            return Json(new { data = MonitoringVisitDto, draw = Request.Form["draw"].FirstOrDefault(), recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig });
        }

        public IActionResult GetgeneralFindingsDataTable(int id = -1,int monitoringVisitId=-1, string Name = "", DateTime? searchdate = null)
        {
            List<GeneralFindings> ulist = new StudyBL().GetAllActiveGeneralFindings(de).Where(x=>x.MonitoringVisitId==monitoringVisitId && x.IsActive==1).OrderByDescending(a => a.MonitoringVisitId).ToList();
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);

            List<FindingsDataDto> Findinglist = new List<FindingsDataDto>();

            foreach(var item in ulist)
            {
                Findingsjsondto findingsjsondto = new Findingsjsondto();
                var questionstatus = "N/A";

                if(!string.IsNullOrEmpty(item.Findings))
                {
                    findingsjsondto = JsonSerializer.Deserialize<Findingsjsondto>(item.Findings);

                }
                else
                {
                    item.QuestionStatus = "Yes";
                    questionstatus = item.QuestionStatus;
                    
                }
                var monitoring = "N/A";
                var monitoringVisit=new StudyBL().GetActiveMonitoringVisitsbyId(item.MonitoringVisitId.Value,de).FirstOrDefault();
                if(monitoringVisit!=null)
                {
                    monitoring = monitoringVisit.MonitoringVisitTitle;
                }
                var CreatedBy=new UserBL().GetUserById(item.Createdby.Value,de).FirstName;
               
                FindingsDataDto obj = new FindingsDataDto()
                {
                    Id = item.Id,
                    MonitoringVisit = monitoring,
                    MonitoringVisitId=item.MonitoringVisitId.Value,
                    Createdby = CreatedBy,
                    Question=item.Question,
                    QuestionStatus= questionstatus,
                    Description=findingsjsondto.Description,
                    ActionResolution= findingsjsondto.Action_resolution,
                    Category=findingsjsondto.Category,
                    Status=findingsjsondto.Status,
                    Subject=findingsjsondto.Subject,
                    Significance= findingsjsondto.Significance,
                    //DateOccure= Convert.ToDateTime(findingsjsondto.Dateoccur),
                    DateOccure= findingsjsondto.Dateoccur,
                    StudyId=item.StudyId,
                    OrganizationId=item.OrganizationId,
                    InvestiGatorSitefId=item.InvestiGatorSitefId,
                    ReviewDate=item.ReviewDate,
                    ReviewComments=item.ReviewComments,
                    FindingStatus=item.FindingStatus
                };

                Findinglist.Add(obj);
            }

            if (CurrentUserRecord.Role == 3)
            {
                if (id != -1)
                {

                    Findinglist = Findinglist.Where(a => a.StudyId == id).ToList();

                }

            }

            else if (CurrentUserRecord.Role == 2)
            {
                Findinglist = Findinglist.Where(a => a.OrganizationId == CurrentUserRecord.OrganizationId).ToList();


            }
            else if (CurrentUserRecord.Role == 4)
            {

                Findinglist = Findinglist.Where(a => CurrentUserRecord.StudyIds.Contains(a.StudyId.ToString()) && CurrentUserRecord.StudyIds != null).ToList();

                //ulist = ulist.Where(x => x.StudyId.ToString().Contains(CurrentUserRecord.StudyIds)).ToList();

            }
            else if (CurrentUserRecord.Role == 5)
            {

                Findinglist = Findinglist.Where(a => CurrentUserRecord.StudyIds.Contains(a.StudyId.ToString()) && CurrentUserRecord.StudyIds != null).ToList();

                //ulist = ulist.Where(x => x.StudyId.ToString().Contains(CurrentUserRecord.StudyIds)).ToList();

            }

            //}

            if(!string.IsNullOrEmpty(Name))
            {
                Findinglist = Findinglist.Where(x => x.Question.ToLower().Contains(Name.ToLower())).ToList();

            }

            //if (searchdate!=null)
            //{
            //    Findinglist = Findinglist.Where(x => x.CreatedAt.ToShortDateString().Contains(searchdate.Value.ToShortDateString())).ToList();

            //}


            int start = Convert.ToInt32(Request.Form["start"].FirstOrDefault());
            int length = Convert.ToInt32(Request.Form["length"].FirstOrDefault());
            string searchValue = Request.Form["search[value]"].FirstOrDefault();
            string sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"];
            string sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            if (sortColumnName != "" && sortColumnName != null)
            {
                if (sortColumnName != "0")
                {
                    if (sortDirection == "asc")
                    {
                        Findinglist = Findinglist.OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                    else
                    {
                        Findinglist = Findinglist.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                }
            }

            int totalrows = Findinglist.Count();

            //filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                Findinglist = Findinglist.Where(x => x.Question.ToLower().Contains(searchValue.ToLower())
                                    ).ToList();
            }

            int totalrowsafterfilterinig = Findinglist.Count();


            // pagination
            Findinglist = Findinglist.Skip(start).Take(length).ToList();


            return Json(new { data = Findinglist, draw = Request.Form["draw"].FirstOrDefault(), recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig });
        }

        public IActionResult GetHighRiskInvestigatorSite(int id = -1, int monitoringVisitId = -1, string Name = "", string siteNo = "", string Address = "")
        {
            List<GeneralFindings> ulist = new StudyBL().GetAllActiveGeneralFindings(de).OrderByDescending(a => a.Id).ToList();
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);

            List<FindingsDataDto> Findinglist = new List<FindingsDataDto>();
            

            foreach (var item in ulist)
            {
                Findingsjsondto findingsjsondto = new Findingsjsondto();
                var questionstatus = "N/A";

                if (!string.IsNullOrEmpty(item.Findings))
                {
                    findingsjsondto = JsonSerializer.Deserialize<Findingsjsondto>(item.Findings);

                }
                else
                {
                    item.QuestionStatus = "Yes";
                    questionstatus = item.QuestionStatus;
                }
                var monitoring = "N/A";
                var monitoringVisit = new StudyBL().GetActiveMonitoringVisitsbyId(item.MonitoringVisitId.Value, de).FirstOrDefault();
                if (monitoringVisit != null)
                {
                    monitoring = monitoringVisit.MonitoringVisitTitle;
                }
                var CreatedBy = new UserBL().GetUserById(item.Createdby.Value, de).FirstName;

                FindingsDataDto obj = new FindingsDataDto()
                {
                    Id = item.Id,
                    MonitoringVisit = monitoring,
                    Createdby = CreatedBy,
                    Question = item.Question,
                    QuestionStatus = questionstatus,
                    Description = findingsjsondto.Description,
                    ActionResolution = findingsjsondto.Action_resolution,
                    Category = findingsjsondto.Category,
                    Status = findingsjsondto.Status,
                    Subject = findingsjsondto.Subject,
                    Significance = findingsjsondto.Significance,
                    DateOccure = findingsjsondto.Dateoccur,
                    StudyId = item.StudyId,
                    OrganizationId = item.OrganizationId,
                    InvestiGatorSitefId = item.InvestiGatorSitefId,
                    ReviewDate = item.ReviewDate,
                    ReviewComments = item.ReviewComments,
                    FindingStatus = item.FindingStatus
                };

                Findinglist.Add(obj);
            }


            Findinglist=Findinglist.Where(x=>x.Status=="Open").GroupBy(a=>a.InvestiGatorSitefId).Select(a=>a.First()).Take(10).ToList();



            if (CurrentUserRecord.Role == 3)
            {
                if (id != -1)
                {

                    Findinglist = Findinglist.Where(a => a.StudyId == id).ToList();

                }

            }

            else if (CurrentUserRecord.Role == 2)
            {
                Findinglist = Findinglist.Where(a => a.OrganizationId == CurrentUserRecord.OrganizationId).ToList();


            }
            else if (CurrentUserRecord.Role == 4)
            {

                Findinglist = Findinglist.Where(a => CurrentUserRecord.StudyIds.Contains(a.StudyId.ToString()) && CurrentUserRecord.StudyIds != null).ToList();

                //ulist = ulist.Where(x => x.StudyId.ToString().Contains(CurrentUserRecord.StudyIds)).ToList();

            }
            else if (CurrentUserRecord.Role == 5)
            {

                Findinglist = Findinglist.Where(a => CurrentUserRecord.StudyIds.Contains(a.StudyId.ToString()) && CurrentUserRecord.StudyIds != null).ToList();

                //ulist = ulist.Where(x => x.StudyId.ToString().Contains(CurrentUserRecord.StudyIds)).ToList();

            }

            //}

            List<InvestigatorSites> HighRiskInvestigatorSites = new List<InvestigatorSites>();
            foreach(var item in Findinglist)
            {
                var InvestigaotrSite = new StudyBL().GetActiveInvestigatorSiteById(item.InvestiGatorSitefId.Value,de);
                if(InvestigaotrSite != null)
                {
                    HighRiskInvestigatorSites.Add(InvestigaotrSite);
                }
                
            }


            if (!String.IsNullOrEmpty(Name))
            {
                HighRiskInvestigatorSites = HighRiskInvestigatorSites.Where(x => x.Facility_Institution_Name.ToLower().Contains(Name.ToLower())).ToList();
            }

            if (!String.IsNullOrEmpty(siteNo))
            {
                HighRiskInvestigatorSites = HighRiskInvestigatorSites.Where(x => x.Site_No.ToString().ToLower().Contains(siteNo.ToString().ToLower())).ToList();
            }


            if (!String.IsNullOrEmpty(Address))
            {
                HighRiskInvestigatorSites = HighRiskInvestigatorSites.Where(x => x.Address.ToString().ToLower().Contains(Address.ToLower())).ToList();
            }

            int start = Convert.ToInt32(Request.Form["start"].FirstOrDefault());
            int length = Convert.ToInt32(Request.Form["length"].FirstOrDefault());
            string searchValue = Request.Form["search[value]"].FirstOrDefault();
            string sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"];
            string sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            if (sortColumnName != "" && sortColumnName != null)
            {
                if (sortColumnName != "0")
                {
                    if (sortDirection == "asc")
                    {
                        HighRiskInvestigatorSites = HighRiskInvestigatorSites.OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                    else
                    {
                        HighRiskInvestigatorSites = HighRiskInvestigatorSites.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                }
            }

            int totalrows = HighRiskInvestigatorSites.Count();

            //filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                HighRiskInvestigatorSites = HighRiskInvestigatorSites.Where(x => x.Facility_Institution_Name.ToLower().Contains(searchValue.ToLower())).ToList();
            }

            int totalrowsafterfilterinig = ulist.Count();


            // pagination
            HighRiskInvestigatorSites = HighRiskInvestigatorSites.Skip(start).Take(length).ToList();


            return Json(new { data = HighRiskInvestigatorSites, draw = Request.Form["draw"].FirstOrDefault(), recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig });
        }

        public IActionResult GetAllInvestigatorSitesByStudyId(int id = -1)
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);

            List<InvestigatorSites> sites = new List<InvestigatorSites>();
            //if (CurrentUserRecord.Role == 1 || CurrentUserRecord.Role == 2)
            //{
            sites = new StudyBL().GetAllActiveInvestigatorSites(de);
            //}

            if (id != -1)
            {


                sites = new StudyBL().GetAllActiveInvestigatorSites(de).Where(a => a.StudyId == id).ToList();

            }
            //if(CurrentUserRecord.Role==5)
            //{
            //    sites = new StudyBL().GetAllActiveInvestigatorSites(de).Where(a => CurrentUserRecord.StudyIds.Contains(a.StudyId.ToString())).ToList();


            //}
            //if (CurrentUserRecord.Role == 2)
            //{
            //    sites = new StudyBL().GetAllActiveInvestigatorSites(de).Where(a => CurrentUserRecord.StudyIds.Contains(a.StudyId.ToString())).ToList();


            //}


            if (sites.Count() == 0)
            {
                return Json(0);
            }

            return Json(sites);
        }

        public IActionResult GetAllCRAsByStudyId(int id = -1)
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            List<User> u = new List<User>();


            u = new UserBL().GetActiveUserList(de).Where(a => a.Role == 5).ToList();

            if (CurrentUserRecord.Role == 1)
            {
                u = new UserBL().GetActiveUserList(de).Where(a => a.Role == 5).ToList();

            }

            else if (id != -1)
            {
                u = new UserBL().GetActiveUserList(de).Where(a => a.Role == 5 && a.StudyIds.Contains(id.ToString())).ToList();

            }
            else if (u.Count() == 0)
            {
                u = new UserBL().GetActiveUserList(de).Where(a => a.Role == 5).ToList();

            }


            if (u.Count() == 0)
            {
                return Json(0);
            }

            return Json(u);
        }

        public IActionResult GetInvestigatorSiteStaffByInvestgatorId(int Id = -1)
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            List<StaffInvestigatorSite> st = new List<StaffInvestigatorSite>();

            st = new StudyBL().GetActiveInvestigatorSiteStaffById(Id, de).Where(a=>a.Role==6).ToList();

            return Json(st);
        }


        public IActionResult GetMonitoringByInvestigatorSiteId(int Id = -1)
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            List<Monitoringvisit> Mv = new List<Monitoringvisit>();

            Mv = new StudyBL().GetMonitoringByInvestigatorSiteId(Id, de).ToList();

            return Json(Mv);
        }
        #endregion


        #region Manage ADI

        [HttpPost]
        public IActionResult GetActionsDataTable(int id = -1, string dt = "", string status = "")
        {
            List<Actions> actList = new StudyBL().GetAllActiveActions(de);
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;



            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            if (CurrentUserRecord.Role == 2)
            {
                actList = actList.Where(a => a.OrganizationId == CurrentUserRecord.OrganizationId).ToList();
            }
            if (CurrentUserRecord.Role == 3)
            {
                if (id != -1)
                {
                    actList = actList.Where(a => a.OrganizationId == CurrentUserRecord.OrganizationId && a.StudyId == id).ToList();

                }
                else
                {
                    actList = actList.Where(a => a.OrganizationId == CurrentUserRecord.OrganizationId).ToList();

                }
            }


            if (!string.IsNullOrEmpty(dt))
            {
                actList = actList.Where(x => x.Meeting_Date.ToString().Contains(dt.ToString())).ToList();
            }
            else if (!String.IsNullOrEmpty(status))
            {
                actList = actList.Where(x => x.Status.ToString().ToLower().Contains(status.ToLower())).ToList();

            }

            //if (CurrentUserRecord.Role != 1)
            //{
            //    if (id != -1)
            //    {
            //        ulist = ulist.Where(a => a.InvestigatorSiteId == id).ToList();
            //    }
            //}


            int start = Convert.ToInt32(Request.Form["start"].FirstOrDefault());
            int length = Convert.ToInt32(Request.Form["length"].FirstOrDefault());
            string searchValue = Request.Form["search[value]"].FirstOrDefault();
            string sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"];
            string sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            if (sortColumnName != "" && sortColumnName != null)
            {
                if (sortColumnName != "0")
                {
                    if (sortDirection == "asc")
                    {
                        actList = actList.OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                    else
                    {
                        actList = actList.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                }
            }

            int totalrows = actList.Count();

            //filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                actList = actList.Where(x => x.Status.ToLower().Contains(searchValue.ToLower())).ToList();
            }

            int totalrowsafterfilterinig = actList.Count();


            // pagination
            actList = actList.Skip(start).Take(length).ToList();


            return Json(new { data = actList, draw = Request.Form["draw"].FirstOrDefault(), recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig });
        }


        [HttpPost]
        public IActionResult GetStaffInvestigatorSiteDataTable(int InvestgatorSiteId = -1)
        {
            List<StaffInvestigatorSite> StaffInvestigatorList = new StudyBL().GetAllActiveStaffInvestigator(de).Where(a=>a.InvestigatorSiteId== InvestgatorSiteId).ToList();
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;



            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            //if (CurrentUserRecord.Role == 2)
            //{
            //    StaffInvestigatorList = StaffInvestigatorList.Where(a => a.OrganizationId == CurrentUserRecord.OrganizationId).ToList();
            //}
            //if (CurrentUserRecord.Role == 3)
            //{
            //    if (id != -1)
            //    {
            //        StaffInvestigatorList = StaffInvestigatorList.Where(a => a.OrganizationId == CurrentUserRecord.OrganizationId && a.StudyId == id).ToList();

            //    }
            //    else
            //    {
            //        StaffInvestigatorList = StaffInvestigatorList.Where(a => a.OrganizationId == CurrentUserRecord.OrganizationId).ToList();

            //    }
            //}


            //if (!string.IsNullOrEmpty(dt))
            //{
            //    actList = actList.Where(x => x.Meeting_Date.ToString().Contains(dt.ToString())).ToList();
            //}
            //else if (!String.IsNullOrEmpty(status))
            //{
            //    actList = actList.Where(x => x.Status.ToString().ToLower().Contains(status.ToLower())).ToList();

            //}

            //if (CurrentUserRecord.Role != 1)
            //{
            //    if (id != -1)
            //    {
            //        ulist = ulist.Where(a => a.InvestigatorSiteId == id).ToList();
            //    }
            //}


            int start = Convert.ToInt32(Request.Form["start"].FirstOrDefault());
            int length = Convert.ToInt32(Request.Form["length"].FirstOrDefault());
            string searchValue = Request.Form["search[value]"].FirstOrDefault();
            string sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"];
            string sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            if (sortColumnName != "" && sortColumnName != null)
            {
                if (sortColumnName != "0")
                {
                    if (sortDirection == "asc")
                    {
                        StaffInvestigatorList = StaffInvestigatorList.OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                    else
                    {
                        StaffInvestigatorList = StaffInvestigatorList.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                }
            }

            int totalrows = StaffInvestigatorList.Count();

            //filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                StaffInvestigatorList = StaffInvestigatorList.Where(x => x.Full_Name.ToLower().Contains(searchValue.ToLower())).ToList();
            }

            int totalrowsafterfilterinig = StaffInvestigatorList.Count();


            // pagination
            StaffInvestigatorList = StaffInvestigatorList.Skip(start).Take(length).ToList();


            return Json(new { data = StaffInvestigatorList, draw = Request.Form["draw"].FirstOrDefault(), recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig });
        }

        [HttpPost]
        public IActionResult GetInformativeDataTable(int id = -1, string dt = "", string status = "")
        {
            List<Informativs> actList = new StudyBL().GetAllActiveInformative(de);
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);

            if (CurrentUserRecord.Role == 2)
            {
                actList = actList.Where(a => a.OrganizationId == CurrentUserRecord.OrganizationId).ToList();
            }
            if (CurrentUserRecord.Role == 3)
            {
                if (id != -1)
                {
                    actList = actList.Where(a => a.OrganizationId == CurrentUserRecord.OrganizationId && a.StudyId == id).ToList();

                }
                else
                {
                    actList = actList.Where(a => a.OrganizationId == CurrentUserRecord.OrganizationId).ToList();

                }
            }

            if (!string.IsNullOrEmpty(dt))
            {
                actList = actList.Where(x => x.Informative_Date.ToString().Contains(dt.ToString())).ToList();
            }
            else if (!String.IsNullOrEmpty(status))
            {
                actList = actList.Where(x => x.Status.ToString().ToLower().Contains(status.ToLower())).ToList();

            }

            //if (CurrentUserRecord.Role != 1)
            //{
            //    if (id != -1)
            //    {
            //        ulist = ulist.Where(a => a.InvestigatorSiteId == id).ToList();
            //    }
            //}


            int start = Convert.ToInt32(Request.Form["start"].FirstOrDefault());
            int length = Convert.ToInt32(Request.Form["length"].FirstOrDefault());
            string searchValue = Request.Form["search[value]"].FirstOrDefault();
            string sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"];
            string sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            if (sortColumnName != "" && sortColumnName != null)
            {
                if (sortColumnName != "0")
                {
                    if (sortDirection == "asc")
                    {
                        actList = actList.OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                    else
                    {
                        actList = actList.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                }
            }

            int totalrows = actList.Count();

            //filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                actList = actList.Where(x => x.Status.ToLower().Contains(searchValue.ToLower())).ToList();
            }

            int totalrowsafterfilterinig = actList.Count();


            // pagination
            actList = actList.Skip(start).Take(length).ToList();


            return Json(new { data = actList, draw = Request.Form["draw"].FirstOrDefault(), recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig });
        }

        [HttpPost]
        public IActionResult GetDecisionsDataTable(int id = -1, string dt = "", string status = "")
        {
            List<Decisions> actList = new StudyBL().GetAllActiveDecisions(de);
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);

            if (CurrentUserRecord.Role == 2)
            {
                actList = actList.Where(a => a.OrganizationId == CurrentUserRecord.OrganizationId).ToList();
            }
            if (CurrentUserRecord.Role == 3)
            {
                if (id != -1)
                {
                    actList = actList.Where(a => a.OrganizationId == CurrentUserRecord.OrganizationId && a.StudyId == id).ToList();

                }
                else
                {
                    actList = actList.Where(a => a.OrganizationId == CurrentUserRecord.OrganizationId).ToList();

                }
            }
            if (!string.IsNullOrEmpty(dt))
            {

                actList = actList.Where(x => x.Decisions_Date.ToString().Contains(dt.ToString())).ToList();
            }
            else if (!String.IsNullOrEmpty(status))
            {
                actList = actList.Where(x => x.Risk_Impact.ToLower().ToString().Contains(status.ToLower())).ToList();

            }

            //if (CurrentUserRecord.Role != 1)
            //{
            //    if (id != -1)
            //    {
            //        ulist = ulist.Where(a => a.InvestigatorSiteId == id).ToList();
            //    }
            //}


            int start = Convert.ToInt32(Request.Form["start"].FirstOrDefault());
            int length = Convert.ToInt32(Request.Form["length"].FirstOrDefault());
            string searchValue = Request.Form["search[value]"].FirstOrDefault();
            string sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"];
            string sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            if (sortColumnName != "" && sortColumnName != null)
            {
                if (sortColumnName != "0")
                {
                    if (sortDirection == "asc")
                    {
                        actList = actList.OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                    else
                    {
                        actList = actList.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                }
            }

            int totalrows = actList.Count();

            //filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                actList = actList.Where(x => x.Risk_Impact.ToLower().Contains(searchValue.ToLower())).ToList();
            }

            int totalrowsafterfilterinig = actList.Count();


            // pagination
            actList = actList.Skip(start).Take(length).ToList();


            return Json(new { data = actList, draw = Request.Form["draw"].FirstOrDefault(), recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig });
        }



        #endregion




    }


}

