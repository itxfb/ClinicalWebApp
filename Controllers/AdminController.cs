using ClinicalWebApplication.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using static ClinicalWebApplication.HelpingClasses.ProjectVariables;
using ClinicalWebApplication.BL;
using System.Linq;
using System;
using System.Security.Claims;
using ClinicalWebApplication.Models;
using System.Threading.Tasks;
using ClinicalWebApplication.HelpingClasses;
using Newtonsoft.Json;
using System.Text.Json;

namespace ClinicalWebApplication.Controllers
{
    [Authorize]
    [ExceptionFilter]
    public class AdminController : Controller
    {
        private readonly SqlConnection de;

        public AdminController(IConfiguration confg, IHttpContextAccessor haccess)
        {
            this.de = new SqlConnection(confg.GetConnectionString("Default"));

            var request = haccess.HttpContext.Request;
            baseUrl = $"{request.Scheme}://{request.Host}";
        }

        #region Landing Page Admin
        public IActionResult Index()
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
                if (CurrentUserRecord.Role == 1)
                {
                    ViewBag.Auditors = new UserBL().GetActiveUserList(de).Where(x => x.Role == 4).Count();
                    ViewBag.StudyMembers = new UserBL().GetActiveUserList(de).Where(x => x.Role == 3).Count();
                    ViewBag.Organizations = new OrganizationBL().GetAllActiveOrganizations(de).Count();
                    ViewBag.Admins = new UserBL().GetActiveUserList(de).Where(x => x.Role == 2).Count();
                    ViewBag.Studies = new StudyBL().GetAllActiveStudies(de).Count();

                    

                }
                else
                {
                    ViewBag.Auditors = new UserBL().GetActiveUserList(de).Where(x => x.Role == 4 && x.OrganizationId == CurrentUserRecord.OrganizationId).Count();
                    ViewBag.StudyMembers = new UserBL().GetActiveUserList(de).Where(x => x.Role == 3 && x.OrganizationId == CurrentUserRecord.OrganizationId).Count();
                    ViewBag.Admins = new UserBL().GetActiveUserList(de).Where(x => x.Role == 2 && x.OrganizationId == CurrentUserRecord.OrganizationId).Count();
                    ViewBag.Studies = new StudyBL().GetAllActiveStudies(de).Where(x => x.OrganizationId == CurrentUserRecord.OrganizationId).Count();

                    ViewBag.protocoldeviations = new StudyBL().GetAllActiveProtocolDeviations(de).Where(a=>a.OrganizationId==CurrentUserRecord.OrganizationId).ToList().Count();//bbr


                }
                if (CurrentUserRecord.Role == 1 || CurrentUserRecord.Role == 2 || CurrentUserRecord.Role==5)
                {
                    //No of open findings

                    List<GeneralFindings> GeneralFindings = new StudyBL().GetAllActiveGeneralFindings(de).Where(x => !string.IsNullOrWhiteSpace(x.Findings)).OrderByDescending(a => a.Id).ToList();

                    if(CurrentUserRecord.Role==2)
                    {
                        GeneralFindings = GeneralFindings.Where(x => x.OrganizationId.Value == CurrentUserRecord.OrganizationId).ToList();
                    }
                    if (CurrentUserRecord.Role == 5)
                    {
                        GeneralFindings = GeneralFindings.Where(a => CurrentUserRecord.StudyIds.Contains(a.StudyId.ToString())).ToList();

                    }
                    List<FindingsDataDto> Findinglist = new List<FindingsDataDto>(); //list of serialize findings 

                    foreach (var item in GeneralFindings)
                    {
                        Findingsjsondto findingsjsondto = new Findingsjsondto();
                        var questionstatus = "N/A";

                        if (!string.IsNullOrEmpty(item.Findings))
                        {
                            findingsjsondto = System.Text.Json.JsonSerializer.Deserialize<Findingsjsondto>(item.Findings);

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

                    Findinglist = Findinglist.Where(x => x.Status == "Open").ToList();

                    ViewBag.NoofOpenFindings = Findinglist.Count();
                    

                    //No of open findings

                    if(CurrentUserRecord.Role!=2)
                    {
                        ViewBag.Actions = new StudyBL().GetAllActiveActions(de).ToList().Count();
                        ViewBag.Decisions = new StudyBL().GetAllActiveDecisions(de).ToList().Count();
                        ViewBag.Informativs = new StudyBL().GetAllActiveInformative(de).ToList().Count();

                        
                        

                    }
                    else
                    {
                        ViewBag.Actions = new StudyBL().GetAllActiveActions(de).Where(a=>a.OrganizationId==CurrentUserRecord.OrganizationId).ToList().Count();
                        ViewBag.Decisions = new StudyBL().GetAllActiveDecisions(de).ToList().Where(a=>a.OrganizationId==CurrentUserRecord.OrganizationId).Count();
                        ViewBag.Informativs = new StudyBL().GetAllActiveInformative(de).Where(a=>a.OrganizationId==CurrentUserRecord.OrganizationId).ToList().Count();

                    }
                    
                }

                //var monthly = new StudyBL().GetAllActiveGeneralFindings(de).Where(x=> !string.IsNullOrWhiteSpace(x.Findings)).ToList().Count();


                if (CurrentUserRecord.Role==5)
                {
                    ViewBag.monitoringvisits = new StudyBL().GetAllActiveMonitoringVisits(de).Where(a => CurrentUserRecord.StudyIds.Contains(a.StudyId.ToString())).ToList().Count();

                    ViewBag.monitoringvisitsthismonth = new StudyBL().GetAllActiveMonitoringVisits(de).Where(x=>CurrentUserRecord.StudyIds.Contains(x.StudyId.ToString()) && x.CreatedAt.Value.Date >= DateTime.Now.Date.AddDays(-30)).ToList().Count();

              
                    //ViewBag.numberoffindings = new StudyBL().GetAllActiveGeneralFindings(de).Where(x => !string.IsNullOrWhiteSpace(x.Findings) && x.StudyId.ToString().Contains(CurrentUserRecord.StudyIds) ).ToList().Count();

                }


                ViewBag.Role = CurrentUserRecord.Role;

                if(CurrentUserRecord.Role==3)
                {
                    return RedirectToAction("SMDashboard", "Admin");
                }
                
                return View();
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion

        #region Study Member
        public IActionResult SMDashboard()
        {

            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {

                if (CurrentUserRecord.Role == 3)
                {
                    return View();
                }
                return RedirectToAction("Index", "Admin");

            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult StudyDashboard(int id=-1)
        {

            
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
                var study = new StudyBL().GetActiveStudyById(id, de);
                ViewBag.StudyId = id;
                ViewBag.InvestigatorSites = new StudyBL().GetActiveInvestigatorSitesByStudyId(id, de).Count();

                List<ProtocolDeviations> ulist = new StudyBL().GetAllActiveProtocolDeviations(de);

                if(CurrentUserRecord.Role!=1)
                {
                    ulist = ulist.Where(x => x.OrganizationId == CurrentUserRecord.OrganizationId).ToList();

                }

                if (id!=-1)
                {
                  ulist=ulist.Where(x=>x.StudyId==id).ToList();
                    ViewBag.ProtocolDeviation = ulist.Count();

                }

                else
                {

                    var plist = new List<ProtocolDeviations>();

                    foreach (var item in ulist)
                    {
                        var instid = new StudyBL().GetActiveInvestigatorSiteById(item.InvestigatorSiteId, de);
                        int stdid = 0;
                        if (instid != null)
                        {
                            stdid = instid.StudyId;
                        }
                        //if (instid.ToString() == id.ToString())
                        if (CurrentUserRecord.StudyIds.Contains(stdid.ToString()))
                        {
                            plist.Add(item);
                        }
                    }

                    ViewBag.ProtocolDeviation = plist.Count();

                }

                
                if (CurrentUserRecord.Role == 3)
                {
                    return View(study);
                }
                return RedirectToAction("Index", "Admin");

            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }


        #endregion

        #region Manage User
        public IActionResult AddUser(string msg = "", string color = "black", int cat = 0)
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
                ViewBag.Message = msg;
                ViewBag.Color = color;
                ViewBag.cat = cat;
                ViewBag.Role = CurrentUserRecord.Role;
                if (CurrentUserRecord.Role == 2)
                {
                    ViewBag.Organization = CurrentUserRecord.OrganizationId;
                    ViewBag.AdminId = CurrentUserRecord.Id;
                }
                return View();
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public async Task<IActionResult> PostAddUser(User _user, string[] studies= null )
        {
            _user.Password = StringCipher.Encrypt(_user.Password);
            _user.IsActive = 1;
            _user.CreatedAt = GeneralPurpose.DateTimeNow();

            //var addorg = _user.OrganizationId;
            var getorg = new OrganizationBL().GetOrganizationById(_user.OrganizationId, de).Name;
            _user.Organization = getorg;

            if (studies.Count() != 0)
            {
                _user.StudyIds = String.Join(",",studies);
            }

            if (!await new UserBL().AddUser(_user, de))
            {
                return RedirectToAction("AddUser", "Admin", new { msg = "Somethings' Wrong", color = "red" });
            }

            return RedirectToAction("AddUser", "Admin", new { msg = "Record Inserted Successfully", color = "green", cat = _user.Role });
        }

        // [ValidationFilter(Roles = new int[] { 1 })]
        public IActionResult ViewUser(int category = -1, string msg = "", string color = "black")
        {

            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
                if (CurrentUserRecord.Role == 1 || CurrentUserRecord.Role == 2)
                {

                    ViewBag.category = category;
                    ViewBag.Message = msg;
                    ViewBag.Color = color;
                    if (CurrentUserRecord.Role == 2)
                    {
                        ViewBag.Organization = CurrentUserRecord.OrganizationId;
                        ViewBag.AdminId = CurrentUserRecord.Id;
                    }
                    return View();
                   
                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }
                
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public async Task<IActionResult> PostUpdateUser(User _user, string way = "")

        {
            User user = new UserBL().GetActiveUserById(_user.Id, de);

            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;


            if (user == null)
            {
                return RedirectToAction("ViewUser", "Admin", new { category = user.Role, msg = "Record not found", color = "red", way = way });
            }

            if (_user.Email != null)
            {
                user.Email = _user.Email;
            }
       

            if (_user.Password != null)
            {
                user.Password = StringCipher.Encrypt(_user.Password);
            }


            if (_user.FirstName != null)
            {
                user.FirstName = _user.FirstName;
            }

            if (_user.LastName != null)
            {
                user.LastName = _user.LastName;
            }
            if(_user.OrganizationId != 0 && user.Role!=1)
            {
                var getorg = new OrganizationBL().GetOrganizationById(_user.OrganizationId, de).Name;
                user.Organization = getorg;
            }
           

            
            //_user.Password = StringCipher.Encrypt(_user.Password);


            //bool chkUser = await new UserBL().UpdateUser(_user, de);
            bool chkUser = await new UserBL().UpdateUser(user, de);
            if (chkUser)
            {
                if (user.Role != 1 && user.Id!=Convert.ToInt32(Userid) )
                {
                    return RedirectToAction("ViewUser", "Admin", new { category = user.Role, msg = "User updated successfully", color = "green", way = way });
                }
                else
                {
                    return RedirectToAction("UpdateProfile", "Auth", new { msg = "Profile updated successfully", color = "green" });
                }
            }
            return RedirectToAction("UpdateProfile","Auth", new { category = user.Role, msg = "Somethings' wrong", color = "red" });
        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
               
                    var r = new UserBL().GetActiveUserById(id, de).Role;
                    if (!await new UserBL().DeleteUser(id, de))
                    {
                        return RedirectToAction("ViewUser", "Admin", new { category = r, msg = "Somethings' wrong", color = "red" });

                    }

                    return RedirectToAction("ViewUser", "Admin", new { msg = "Record deleted successfully!", color = "green", category = r });
            }
            catch
            {
                return RedirectToAction("NotFoundPage", "Error");
            }
        }     

        #endregion

        #region Manage Organizations
        public IActionResult AddOrganization(string msg = "", string color = "black", int cat = 0)
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
                if (CurrentUserRecord.Role == 1)
                {
                    ViewBag.Message = msg;
                    ViewBag.Color = color;
                    ViewBag.cat = cat;
                    ViewBag.AdminId = CurrentUserRecord.Id;
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Admin", msg = " Unauthorized Access !");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Admin");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAddOrganization(Organizations _organization)
        {

            _organization.IsActive = 1;
            _organization.CreatedAt = GeneralPurpose.DateTimeNow();

            if (!await new OrganizationBL().AddOrganization(_organization, de))
            {
                return RedirectToAction("AddOrganization", "Admin", new { msg = "Somethings' Wrong", color = "red" });
            }

            return RedirectToAction("AddOrganization", "Admin", new { msg = "Record Inserted Successfully", color = "green" });
        }
        public IActionResult ViewOrganizations(int category = -1, string msg = "", string color = "black")
        {

            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
                if (CurrentUserRecord.Role == 1)
                {
                    ViewBag.category = category;
                    ViewBag.Message = msg;
                    ViewBag.Color = color;
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostUpdateOrganization(Organizations _organization, string way = "")
        {
            Organizations organizations = new OrganizationBL().GetActiveOrganizationById(_organization.Id, de);

            if (organizations == null)
            {
                return RedirectToAction("ViewOrganizations", "Admin", new { msg = "Record not found", color = "red", way = way });
            }

            _organization.CreatedBy = organizations.CreatedBy;
            _organization.IsActive = 1;

            bool chkOrg = await new OrganizationBL().UpdateOrganization(_organization, de);
            if (chkOrg)
            {
                return RedirectToAction("ViewOrganizations", "Admin", new { msg = "User updated successfully", color = "green", way = way });
            }
            return RedirectToAction("ViewOrganizations", "Admin", new { msg = "Somethings' wrong", color = "red", way = way });
        }
        public async Task<IActionResult> DeleteOrganization(int id)
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
                if (CurrentUserRecord.Role == 1)
                {
                    if (!await new OrganizationBL().DeleteOrganization(id, de))
                    {
                        return RedirectToAction("ViewOrganizations", "Admin", new { msg = "Somethings' wrong", color = "red" });

                    }
                    return RedirectToAction("ViewOrganizations", "Admin", new { msg = "Record deleted successfully!", color = "green" });
                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            catch
            {
                return RedirectToAction("NotFoundPage", "Error");
            }
        }

        #endregion

        #region Manage Studies
        public IActionResult AddStudy(string msg = "", string color = "black", int cat = 0)
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {

                   
              
                    ViewBag.Message = msg;
                    ViewBag.Color = color;
                    ViewBag.Role = CurrentUserRecord.Role;       
                    ViewBag.OrganizationId = CurrentUserRecord.OrganizationId;
                    ViewBag.AdminId = CurrentUserRecord.Id;
                    return View();
            
            }
            catch
            {
                return RedirectToAction("Index", "Admin");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAddStudy(Study _study)
        {

            _study.IsActive = 1;
            _study.CreatedAt = GeneralPurpose.DateTimeNow();

            if (!await new StudyBL().AddStudy(_study, de))
            {
                return RedirectToAction("AddStudy", "Admin", new { msg = "Somethings' Wrong", color = "red" });
            }

            return RedirectToAction("AddStudy", "Admin", new { msg = "Record Inserted Successfully", color = "green" });
        }
        public IActionResult ViewStudies(int id=-1,int category = -1, string msg = "", string color = "black")
        {

            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
                    ViewBag.category = category;
                    ViewBag.StudyId = id;
                    ViewBag.Message = msg;
                    ViewBag.Color = color;
                    return View();
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostUpdateStudy(Study _study, string way = "")
        {
            Study study = new StudyBL().GetActiveStudyById(_study.Id, de);

            if (study == null)
            {
                return RedirectToAction("ViewStudies", "Admin", new { msg = "Record not found", color = "red", way = way });
            }

            bool chkOrg = await new StudyBL().UpdateStudy(_study, de);
            if (chkOrg)
            {
                return RedirectToAction("ViewStudies", "Admin", new {id= _study.Id, msg = "Record updated successfully", color = "green", way = way });
            }
            return RedirectToAction("ViewStudies", "Admin", new { id = _study.Id, msg = "Somethings' wrong", color = "red", way = way });
        }
        public async Task<IActionResult> DeleteStudy(int id)
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
                if (CurrentUserRecord.Role == 1 || CurrentUserRecord.Role==2)
                {
                    if (!await new StudyBL().DeleteStudy(id, de))
                    {
                        return RedirectToAction("ViewStudies", "Admin", new { msg = "Somethings' wrong", color = "red" });

                    }
                    return RedirectToAction("ViewStudies", "Admin", new { msg = "Record deleted successfully!", color = "green" });
                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            catch
            {
                return RedirectToAction("NotFoundPage", "Error");
            }
        }

        #endregion

    }
}
