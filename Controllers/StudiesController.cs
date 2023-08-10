using ClinicalWebApplication.BL;
using ClinicalWebApplication.HelpingClasses;
using ClinicalWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Claims;
using System.Text.Json;

namespace ClinicalWebApplication.Controllers
{
    public class StudiesController : Controller
    {
        private readonly SqlConnection de;
        public StudiesController(IConfiguration confg, IHttpContextAccessor haccess)
        {
            this.de = new SqlConnection(confg.GetConnectionString("Default"));
            var request = haccess.HttpContext.Request;
        }

        public IActionResult ViewStudy(int id = -1, string msg = "", string color = "black")
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
                if (CurrentUserRecord.Role != 4)
                {
                    var study = new StudyBL().GetActiveStudyById(id, de);
                    ViewBag.Study = study;
                    ViewBag.StudyId = id;
                    ViewBag.Role = CurrentUserRecord.Role;
                    ViewBag.Message = msg;
                    ViewBag.Color = color;
                    return View();
                }
                return RedirectToAction("Index", "Admin");
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult AddInvestigatorSite(int id = -1, string msg = "", string color = "black")
        {
            try
            {
                var userid = Convert.ToInt32(User.Claims.Where(a => a.Type == ClaimTypes.Sid).First().Value);
                var currentUser = new UserBL().GetActiveUserById(Convert.ToInt32(userid), de);
                ViewBag.Role = currentUser.Role;
                ViewBag.StudyId = id;
                ViewBag.UserId = userid;
                ViewBag.organizationid = currentUser.OrganizationId;
                ViewBag.Message = msg;
                ViewBag.Color = color;
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }


        [HttpPost]
        //public async Task<IActionResult> PostAddInvestigatorSite(InvestigatorSites _site, string[] FullNames, string[] Emails, string[] Roles)
        public async Task<IActionResult> PostAddInvestigatorSite(InvestigatorSites _site, StaffInvestigatorSite _staff)
        {

            _site.IsActive = 1;
            _site.CreatedAt = GeneralPurpose.DateTimeNow();
            _site.Clinical_Trial_Agreements = await new UserBL().UploadImage(_site.Clinical_Trial);
            _site.Id = await new StudyBL().AddInvestigatorSite(_site, de);

            if (_site.Id == 0)
            {
                return RedirectToAction("AddInvestigatorSite", "Studies", new { id = _site.StudyId, msg = "Somethings' Wrong", color = "red" });
            }



            //if (FullNames.Count() != 0)
            //{
            //    for (int i = 0; i < FullNames.Length; i++)
            //    {
            //        if (FullNames[i]!="," && Emails[i] != "," && Roles[i]!=",")
            //        {
            var staff = new StaffInvestigatorSite()
            {
                Full_Name = _staff.Full_Name,
                InvestigatorSiteId = _site.Id,
                Role = _staff.Role,
                IsActive = 1,
                Email = _staff.Email
            };

            if (!await new StudyBL().AddStaffInvestigatorSite(staff, de))
            {
                return RedirectToAction("AddInvestigatorSite", "Studies", new { id = _site.StudyId, msg = "Something went wrong while adding staff!", color = "red" });
            }
            //        }



            //    }

            //}

            return RedirectToAction("AddInvestigatorSite", "Studies", new { id = _site.StudyId, msg = "Record Inserted Successfully", color = "green" });
        }

        [HttpPost]
        public async Task<IActionResult> PostUpdateInvestigatorSite(InvestigatorSites _site, string way = "")
        {
            InvestigatorSites site = new StudyBL().GetActiveInvestigatorSiteById(_site.Id, de);

            if (site == null)
            {
                return RedirectToAction("ViewInvestigatorSites", "Studies", new { msg = "Record not found", color = "red", way = way });
            }

            bool chkOrg = await new StudyBL().UpdateInvestigatorSite(_site, de);
            if (chkOrg)
            {
                return RedirectToAction("ViewInvestigatorSites", "Studies", new { id = site.StudyId, msg = "Record updated successfully", color = "green", way = way });
            }
            return RedirectToAction("ViewInvestigatorSites", "Studies", new { id = site.StudyId, msg = "Somethings' wrong", color = "red", way = way });
        }
        public IActionResult ViewInvestigatorSites(int id = -1, string msg = "", string color = "black")
        {
            try
            {
                var userid = Convert.ToInt32(User.Claims.Where(a => a.Type == ClaimTypes.Sid).First().Value);
                var currentUser = new UserBL().GetActiveUserById(Convert.ToInt32(userid), de);
                ViewBag.Role = currentUser.Role;
                ViewBag.StudyId = id;
                ViewBag.Message = msg;
                ViewBag.Color = color;
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult viewStaffInvestigatorSite(int id = -1,int investigatorSiteId= -1, string msg = "", string color = "black")
        {
            try
            {
                var userid = Convert.ToInt32(User.Claims.Where(a => a.Type == ClaimTypes.Sid).First().Value);
                var currentUser = new UserBL().GetActiveUserById(Convert.ToInt32(userid), de);
                ViewBag.Role = currentUser.Role;
                ViewBag.investigatorSiteId = investigatorSiteId;
                ViewBag.StudyId = id;
                ViewBag.UserId = userid;
                ViewBag.Message = msg;
                ViewBag.Color = color;
            }
            catch
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }
        public async Task<IActionResult> DeleteInvestigatorSite(int id)
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
                //var studyid = new StudyBL().GetActiveInvestigatorSiteById(id, de).StudyId;

                var stdyid = 0;
                if (CurrentUserRecord.Role == 3)
                {
                    var GetStdId = new StudyBL().GetActiveInvestigatorSiteById(id, de);
                    if (GetStdId != null)
                    {
                        stdyid = GetStdId.StudyId;
                    }

                    if (!await new StudyBL().DeleteInvestigatorSite(id, de))
                    {
                        return RedirectToAction("ViewInvestigatorSites", "Studies", new { id = stdyid, msg = "Somethings' wrong", color = "red" });

                    }
                    return RedirectToAction("ViewInvestigatorSites", "Studies", new { id = stdyid, msg = "Record deleted successfully!", color = "green" });

                }


                if (!await new StudyBL().DeleteInvestigatorSite(id, de))
                {
                    return RedirectToAction("ViewInvestigatorSites", "Studies", new { msg = "Somethings' wrong", color = "red" });

                }
                return RedirectToAction("ViewInvestigatorSites", "Studies", new { msg = "Record deleted successfully!", color = "green" });
            }
            catch
            {
                return RedirectToAction("NotFoundPage", "Error");
            }
        }
        public IActionResult AddProtocolDeviations(int id = -1, string msg = "", string color = "black")
        {
            try
            {
                var userid = Convert.ToInt32(User.Claims.Where(a => a.Type == ClaimTypes.Sid).First().Value);
                var currentUser = new UserBL().GetActiveUserById(Convert.ToInt32(userid), de);
                ViewBag.Role = currentUser.Role;
                ViewBag.StudyId = id;
                //ViewBag.StudyId = currentUser.StudyIds;
                ViewBag.UserId = userid;
                ViewBag.OrganizationId = currentUser.OrganizationId;
                ViewBag.Message = msg;
                ViewBag.Color = color;
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult AddStaffInvestigator(int id = -1, int investigatorsiteId = -1, string msg = "", string color = "black")
        {
            try
            {
                var userid = Convert.ToInt32(User.Claims.Where(a => a.Type == ClaimTypes.Sid).First().Value);
                var currentUser = new UserBL().GetActiveUserById(Convert.ToInt32(userid), de);
                ViewBag.Role = currentUser.Role;
                ViewBag.StudyId = id;
                //ViewBag.StudyId = currentUser.StudyIds;
                ViewBag.UserId = userid;
                ViewBag.OrganizationId = currentUser.OrganizationId;
                ViewBag.Message = msg;
                ViewBag.Color = color;
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public async Task<IActionResult> PostAddInvestigatorStaff(StaffInvestigatorSite _staffInvestigate, int StudyId = -1)
        {

            _staffInvestigate.IsActive = 1;
            if (!await new StudyBL().AddStaffInvestigatorSite(_staffInvestigate, de))
            {
                return RedirectToAction("AddStaffInvestigator", "Studies", new { investigatorsiteId = _staffInvestigate.InvestigatorSiteId, msg = "Somethings' Wrong", color = "red" });
            }

            return RedirectToAction("AddStaffInvestigator", "Studies", new { investigatorsiteId = _staffInvestigate.InvestigatorSiteId, msg = "Record Inserted Successfully", color = "green" });
        }

        public async Task<IActionResult> PostUpdateInvestigatorStaff(StaffInvestigatorSite _staffInvestigate, int StudyId = -1)
        {
            StaffInvestigatorSite site = new StudyBL().GetActiveStaffInvestigatorById(_staffInvestigate.Id, de);

            if (site == null)
            {
                return RedirectToAction("viewStaffInvestigatorSite", "Studies", new { investigatorSiteId= site.InvestigatorSiteId, msg = "Record not found", color = "red",});
            }


            _staffInvestigate.IsActive = 1;
            
            if (!await new StudyBL().UpdatesStaffInvestigator(_staffInvestigate, de))
            {
                return RedirectToAction("viewStaffInvestigatorSite", "Studies", new { investigatorsiteId = site.InvestigatorSiteId, msg = "Somethings' Wrong", color = "red" });
            }

            return RedirectToAction("viewStaffInvestigatorSite", "Studies", new { investigatorsiteId = site.InvestigatorSiteId, msg = "Record Updated Successfully", color = "green" });
        }

        public async Task<IActionResult> DeleteStaffInvestigator(int id)
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
                StaffInvestigatorSite Act = new StudyBL().GetSingleInvestigatorSiteStaffById(id, de);


                if (!await new StudyBL().DeleteInvestigatorStaff(id, de))
                {
                    return RedirectToAction("viewStaffInvestigatorSite", "Studies", new { investigatorSiteId = Act.InvestigatorSiteId, msg = "Record not found", color = "red", });

                }
                return RedirectToAction("viewStaffInvestigatorSite", "Studies", new { investigatorSiteId = Act.InvestigatorSiteId, msg = "Record Deleted Successfully", color = "Green", });

            }
            catch
            {
                return RedirectToAction("NotFoundPage", "Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAddProtocolDeviation(ProtocolDeviations _protocolDeviations, int StudyId = -1)
        {

            _protocolDeviations.IsActive = 1;

            _protocolDeviations.CreatedAt = GeneralPurpose.DateTimeNow();
            if (!await new StudyBL().AddProtocolDeviation(_protocolDeviations, de))
            {
                return RedirectToAction("AddProtocolDeviations", "Studies", new { id = StudyId, msg = "Somethings' Wrong", color = "red" });
            }

            return RedirectToAction("AddProtocolDeviations", "Studies", new { id = StudyId, msg = "Record Inserted Successfully", color = "green" });
        }
        public IActionResult ViewProtocolDeviations(int id = -1, int investigatersiteid = -1, string msg = "", string color = "black")
        {
            try
            {
                var userid = Convert.ToInt32(User.Claims.Where(a => a.Type == ClaimTypes.Sid).First().Value);
                var currentUser = new UserBL().GetActiveUserById(Convert.ToInt32(userid), de);
                var studyId = id;
                //if (currentUser.Role==3)
                //{
                //    studyId = id;

                //}
                //else if (currentUser.Role==1 /*|| currentUser.Role == 2*/)
                //{
                //    studyId = 0;

                //}
                //else
                //{
                //    studyId = new StudyBL().GetActiveInvestigatorSiteById(id, de).StudyId;

                //}
                ViewBag.Role = currentUser.Role;
                // ViewBag.InvestigatorSiteId = investigatorid;
                ViewBag.StudyId = id;
                ViewBag.investigatersiteid = investigatersiteid;
                ViewBag.OrganizationId = currentUser.OrganizationId;
                ViewBag.Message = msg;
                ViewBag.Color = color;
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostUpdateProtocolDeviations(ProtocolDeviations _protocolDeviations, int StudyId = -1, string way = "")
        {
            ProtocolDeviations site = new StudyBL().GetActiveProtocolDeviationById(_protocolDeviations.Id, de);

            if (site == null)
            {
                return RedirectToAction("ViewProtocolDeviations", "Studies", new { msg = "Record not found", color = "red", way = way });
            }

            bool chkOrg = await new StudyBL().UpdateProtocolDeviation(_protocolDeviations, de);
            if (chkOrg)
            {
                //return RedirectToAction("ViewProtocolDeviations", "Studies", new { id = site.InvestigatorSiteId, msg = "Record updated successfully", color = "green", way = way });
                return RedirectToAction("ViewProtocolDeviations", "Studies", new { investigatersiteid = site.InvestigatorSiteId, msg = "Record updated successfully", color = "green", way = way });
            }
            //return RedirectToAction("ViewProtocolDeviations", "Studies", new { id = site.InvestigatorSiteId, msg = "Somethings' wrong", color = "red", way = way });
            return RedirectToAction("ViewProtocolDeviations", "Studies", new { investigatersiteid = site.InvestigatorSiteId, msg = "Somethings' wrong", color = "red", way = way });
        }
        public async Task<IActionResult> DeleteProtocolDeviation(int id)
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {

                if (!await new StudyBL().DeleteProtocolDeviation(id, de))
                {
                    return RedirectToAction("ViewProtocolDeviations", "Studies", new { msg = "Somethings' wrong", color = "red" });

                }
                return RedirectToAction("ViewProtocolDeviations", "Studies", new { msg = "Record deleted successfully!", color = "green" });

            }
            catch
            {
                return RedirectToAction("NotFoundPage", "Error");
            }
        }





        #region Manage ADI 

        public IActionResult AddActions(int id = -1, string msg = "", string color = "black")
        {
            try
            {
                var userid = Convert.ToInt32(User.Claims.Where(a => a.Type == ClaimTypes.Sid).First().Value);
                var currentUser = new UserBL().GetActiveUserById(Convert.ToInt32(userid), de);
                ViewBag.Role = currentUser.Role;
                //ViewBag.StudyId = id;
                ViewBag.organizationid = currentUser.OrganizationId;
                ViewBag.StudyId = id;
                ViewBag.UserId = userid;
                ViewBag.Message = msg;
                ViewBag.Color = color;
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public async Task<IActionResult> PostAddActions(Actions _Actions, int StudyId = -1)
        {

            _Actions.IsActive = 1;
            _Actions.CreatedAt = GeneralPurpose.DateTimeNow();
            StudyId = _Actions.StudyId.Value;
            var getstudy = new StudyBL().GetActiveStudyById(StudyId, de).Protocol_Title;
            _Actions.Study = getstudy;
            if (!await new StudyBL().addAction(_Actions, de))
            {
                return RedirectToAction("AddActions", "Studies", new { id = StudyId, msg = "Somethings' Wrong", color = "red" });
            }

            return RedirectToAction("AddActions", "Studies", new { id = StudyId, msg = "Record Inserted Successfully", color = "green" });
        }

        public IActionResult AddInformativ(int id = -1, string msg = "", string color = "black")
        {
            try
            {
                var userid = Convert.ToInt32(User.Claims.Where(a => a.Type == ClaimTypes.Sid).First().Value);
                var currentUser = new UserBL().GetActiveUserById(Convert.ToInt32(userid), de);
                ViewBag.Role = currentUser.Role;
                //ViewBag.StudyId = id;
                ViewBag.organizationid = currentUser.OrganizationId;
                ViewBag.StudyId = id;
                ViewBag.UserId = userid;
                ViewBag.Message = msg;
                ViewBag.Color = color;
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public async Task<IActionResult> PostAddInformativ(Informativs _Info, int StudyId = -1)
        {

            _Info.IsActive = 1;
            _Info.CreatedAt = GeneralPurpose.DateTimeNow();
            StudyId = _Info.StudyId.Value;
            var getstudy = new StudyBL().GetActiveStudyById(StudyId, de).Protocol_Title;
            _Info.Study = getstudy;

            if (!await new StudyBL().addInformative(_Info, de))
            {
                return RedirectToAction("AddInformativ", "Studies", new { id = StudyId, msg = "Somethings' Wrong", color = "red" });
            }

            return RedirectToAction("AddInformativ", "Studies", new { id = StudyId, msg = "Record Inserted Successfully", color = "green" });
        }

        public IActionResult AddDecision(int id = -1, string msg = "", string color = "black")
        {
            try
            {
                var userid = Convert.ToInt32(User.Claims.Where(a => a.Type == ClaimTypes.Sid).First().Value);
                var currentUser = new UserBL().GetActiveUserById(Convert.ToInt32(userid), de);
                ViewBag.Role = currentUser.Role;
                //ViewBag.StudyId = id;
                ViewBag.organizationid = currentUser.OrganizationId;
                ViewBag.StudyId = id;
                ViewBag.UserId = userid;
                ViewBag.Message = msg;
                ViewBag.Color = color;
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public async Task<IActionResult> PostAddDecision(Decisions _decision, int StudyId = -1)
        {

            _decision.IsActive = 1;
            _decision.CreatedAt = GeneralPurpose.DateTimeNow();
            _decision.DecisionAttachment_path = await new UserBL().UploadImage(_decision.Decision_Attachment);

            StudyId = _decision.StudyId.Value;
            var getstudy = new StudyBL().GetActiveStudyById(StudyId, de).Protocol_Title;
            _decision.Study = getstudy;

            if (!await new StudyBL().addDecision(_decision, de))
            {
                return RedirectToAction("AddDecision", "Studies", new { id = StudyId, msg = "Somethings' Wrong", color = "red" });
            }

            return RedirectToAction("AddDecision", "Studies", new { id = StudyId, msg = "Record Inserted Successfully", color = "green" });
        }

        public IActionResult viewActions(int id = -1, string msg = "", string color = "black")
        {
            try
            {
                var userid = Convert.ToInt32(User.Claims.Where(a => a.Type == ClaimTypes.Sid).First().Value);
                var currentUser = new UserBL().GetActiveUserById(Convert.ToInt32(userid), de);
                ViewBag.Role = currentUser.Role;
                //ViewBag.StudyId = id;
                ViewBag.StudyId = id;
                ViewBag.UserId = userid;
                ViewBag.Message = msg;
                ViewBag.Color = color;
            }
            catch
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }
        public IActionResult viewInformative(int id = -1, string msg = "", string color = "black")
        {
            try
            {
                var userid = Convert.ToInt32(User.Claims.Where(a => a.Type == ClaimTypes.Sid).First().Value);
                var currentUser = new UserBL().GetActiveUserById(Convert.ToInt32(userid), de);
                ViewBag.Role = currentUser.Role;
                //ViewBag.StudyId = id;
                ViewBag.StudyId = id;
                ViewBag.UserId = userid;
                ViewBag.Message = msg;
                ViewBag.Color = color;
            }
            catch
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }

        public IActionResult viewDecisions(int id = -1, string msg = "", string color = "black")
        {
            try
            {
                var userid = Convert.ToInt32(User.Claims.Where(a => a.Type == ClaimTypes.Sid).First().Value);
                var currentUser = new UserBL().GetActiveUserById(Convert.ToInt32(userid), de);
                ViewBag.Role = currentUser.Role;
                //ViewBag.StudyId = id;
                ViewBag.StudyId = id;
                ViewBag.UserId = userid;
                ViewBag.Message = msg;
                ViewBag.Color = color;
            }
            catch
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }


        public async Task<IActionResult> PostUpdateActions(Actions _actions, int StudyId = -1)
        {
            Actions Act = new StudyBL().GetActiveActionById(_actions.Id, de);

            if (Act == null)
            {
                return RedirectToAction("viewActions", "Studies", new { msg = "Record not found", color = "red" });
            }
            StudyId = _actions.StudyId.Value;
            _actions.OrganizationId = Act.OrganizationId;
            var getstudy = new StudyBL().GetActiveStudyById(StudyId, de).Protocol_Title;
            _actions.Study = getstudy;

            bool chkOrg = await new StudyBL().UpdateActions(_actions, de);
            if (chkOrg)
            {
                return RedirectToAction("viewActions", "Studies", new { id = StudyId, msg = "Record updated successfully", color = "green" });
            }
            return RedirectToAction("viewActions", "Studies", new { id = StudyId, msg = "Somethings' wrong", color = "red" });
        }

        public async Task<IActionResult> DeleteAction(int id)
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
                Actions Act = new StudyBL().GetActiveActionById(id, de);


                if (!await new StudyBL().DeleteAction(id, de))
                {
                    return RedirectToAction("viewActions", "Studies", new {id=Act.StudyId,msg = "Somethings' wrong", color = "red" });

                }
                return RedirectToAction("viewActions", "Studies", new {id=Act.StudyId,msg = "Record deleted successfully!", color = "green" });

            }
            catch
            {
                return RedirectToAction("NotFoundPage", "Error");
            }
        }

        public async Task<IActionResult> PostUpdateInformative(Informativs _Info, int StudyId = -1)
        {
            Informativs Act = new StudyBL().GetActiveInformativebyId(_Info.Id, de);

            if (Act == null)
            {
                return RedirectToAction("viewInformative", "Studies", new { msg = "Record not found", color = "red" });
            }
            _Info.OrganizationId = Act.OrganizationId;
            StudyId = _Info.StudyId.Value;
            var getstudy = new StudyBL().GetActiveStudyById(StudyId, de).Protocol_Title;
            _Info.Study = getstudy;

            bool chkOrg = await new StudyBL().UpdateInformative(_Info, de);
            if (chkOrg)
            {
                return RedirectToAction("viewInformative", "Studies", new { id = StudyId, msg = "Record updated successfully", color = "green" });
            }
            return RedirectToAction("viewInformative", "Studies", new { id = StudyId, msg = "Somethings' wrong", color = "red" });
        }

        public async Task<IActionResult>DeleteInformative(int id)
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
                Informativs Act = new StudyBL().GetActiveInformativebyId(id, de);

                if (!await new StudyBL().DeleteInformative(id, de))
                {
                    return RedirectToAction("viewInformative", "Studies", new {id= Act.StudyId, msg = "Somethings' wrong", color = "red" });

                }
                return RedirectToAction("viewInformative", "Studies", new { id = Act.StudyId, msg = "Record deleted successfully!", color = "green" });

            }
            catch
            {
                return RedirectToAction("NotFoundPage", "Error");
            }
        }
        public async Task<IActionResult> PostUpdateDecision(Decisions _decisions, string selected_file = "", int StudyId = -1)
        {
            Decisions Act = new StudyBL().GetActiveDecisionById(_decisions.Id, de);

            if (_decisions.Decision_Attachment != null)
            {
                _decisions.DecisionAttachment_path = await new UserBL().UploadImage(_decisions.Decision_Attachment);

            }
            else
            {
                _decisions.DecisionAttachment_path = selected_file;

            }
            _decisions.OrganizationId = Act.OrganizationId;
            StudyId = _decisions.StudyId.Value;
            var getstudy = new StudyBL().GetActiveStudyById(StudyId, de).Protocol_Title;
            _decisions.Study = getstudy;

            if (Act == null)
            {
                return RedirectToAction("viewDecisions", "Studies", new { msg = "Record not found", color = "red" });
            }

            bool chkOrg = await new StudyBL().UpdateDecision(_decisions, de);
            if (chkOrg)
            {
                return RedirectToAction("viewDecisions", "Studies", new { id = StudyId, msg = "Record updated successfully", color = "green" });
            }
            return RedirectToAction("viewDecisions", "Studies", new { id = StudyId, msg = "Somethings' wrong", color = "red" });
        }

        public async Task<IActionResult> DeleteDecision(int id)
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
                Decisions Act = new StudyBL().GetActiveDecisionById(id, de);

                if (!await new StudyBL().DeleteDecision(id, de))
                {
                    return RedirectToAction("viewDecisions", "Studies", new {id= Act.StudyId, msg = "Somethings' wrong", color = "red" });

                }
                return RedirectToAction("viewDecisions", "Studies", new {id= Act.StudyId, msg = "Record deleted successfully!", color = "green" });

            }
            catch
            {
                return RedirectToAction("NotFoundPage", "Error");
            }
        }


        #endregion

        #region Manage Monitoring visit

        public IActionResult AddMonitoringVisit(int id = -1,int monitoringVisitId=-1,string msg = "", string color = "black")
        {
            try
            {
                var userid = Convert.ToInt32(User.Claims.Where(a => a.Type == ClaimTypes.Sid).First().Value);
                var currentUser = new UserBL().GetActiveUserById(Convert.ToInt32(userid), de);
                ViewBag.Role = currentUser.Role;
                //ViewBag.StudyId = id;
                ViewBag.organizationid = currentUser.OrganizationId;
                ViewBag.StudyId = id;
                ViewBag.UserId = userid;
                ViewBag.Message = msg;
                ViewBag.Color = color;
                ViewBag.monitoringId = monitoringVisitId;
                //if(monitoringVisitId!=-1)
                //{
                //    var mVisit = new StudyBL().GetActivesingleMonitoringVisitsbyId(monitoringVisitId,de);
                //    if(mVisit!=null)
                //    {

                //        ViewBag.MonitoringVisit= mVisit;
                //    }
                    

                //}

            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }



        public async Task<IActionResult> PostAddMonitoringVisit(Monitoringvisit _site, List<int> Attendies)
        {

            _site.IsActive = 1;
            _site.CreatedAt = GeneralPurpose.DateTimeNow();

            _site.Id = await new StudyBL().AddMonitoringVisit(_site, de);

            if (_site.Id == 0)
            {
                return RedirectToAction("AddMonitoringVisit", "Studies", new { id = _site.StudyId, msg = "Somethings' Wrong", color = "red" });
            }

            //var  result = string.Join<int>(", ", Attendies);



            for (int i = 0; i < Attendies.Count; i++)
            {

                var Attendie = new Attendies()
                {
                    MonitoringVisitId = _site.Id,
                    InvestiGatorSiteStaffId = Attendies[i],

                    IsActive = 1,
                    CreatedAt = DateTime.Now,
                    Createdby = _site.Createdby
                };

                if (!await new StudyBL().AddAttendie(Attendie, de))
                {
                    return RedirectToAction("AddMonitoringVisit", "Studies", new { id = _site.StudyId, msg = "Something went wrong while adding staff!", color = "red" });
                }


            }




            return RedirectToAction("AddMonitoringVisit", "Studies", new { id = _site.StudyId, msg = "Record Inserted Successfully", color = "green" });
        }

        public async Task<IActionResult> PostUpdateMonitoringVisit(Monitoringvisit _site, List<int> Attendies)
        {
            Monitoringvisit Mvt = new StudyBL().GetActivesingleMonitoringVisitsbyId(_site.Id, de);

            _site.IsActive = 1;
            _site.UpdatedAt = GeneralPurpose.DateTimeNow();

            

            if (Mvt == null)
            {
                return RedirectToAction("ViewMonitoringVisit", "Studies", new { monitoringVisitId = _site.Id, msg = "Something  Wrong please try again", color = "red" });
            }

            bool chkOrg = await new StudyBL().UpdateMonitoringVisit(_site, de);
            if (!chkOrg)
            {
                return RedirectToAction("ViewMonitoringVisit", "Studies", new { monitoringVisitId = _site.Id, msg = "Something  Wrong please try again", color = "green" });
            }

            var _Attendies = new StudyBL().GetAttendiesListByMonitoringvisitId(_site.Id, de);


            for (int i = 0; i < Attendies.Count; i++)
            {
                if (i > _Attendies.Count() - 1)
                {
                    var Attendie = new Attendies()
                    {
                        MonitoringVisitId = _site.Id,
                        InvestiGatorSiteStaffId = Attendies[i],

                        IsActive = 1,
                        CreatedAt = DateTime.Now,
                        Createdby = _site.Createdby
                    };

                    //var addattendie = new StudyBL().AddAttendie(Attendie, de);
                    if (!await new StudyBL().AddAttendie(Attendie, de))
                    {
                        return RedirectToAction("ViewMonitoringVisit", "Studies", new { monitoringVisitId = _site.Id, msg = "Something went wrong while adding Attendies please try again!", color = "red" });
                    }


                }

                

            }


            return RedirectToAction("ViewMonitoringVisit", "Studies", new { id = _site.StudyId, msg = "Record Updated Successfully", color = "green" });
        }

        public IActionResult CreateConfirmationLetter(int id = -1, int monitoringVisitId = -1, string msg = "", string color = "black")
        {
            try
            {
                var userid = Convert.ToInt32(User.Claims.Where(a => a.Type == ClaimTypes.Sid).First().Value);
                var currentUser = new UserBL().GetActiveUserById(Convert.ToInt32(userid), de);
                ViewBag.Role = currentUser.Role;
                //ViewBag.StudyId = id;
                ViewBag.organizationid = currentUser.OrganizationId;
                ViewBag.StudyId = id;
                ViewBag.UserId = userid;
                ViewBag.Message = msg;
                ViewBag.Color = color;
                ViewBag.monitoringId = monitoringVisitId;
                if (monitoringVisitId != -1)
                {
                    var mVisit = new StudyBL().GetActivesingleMonitoringVisitsbyId(monitoringVisitId, de);
                    if (mVisit != null)
                    {

                        ViewBag.MonitoringVisit = mVisit;
                    }


                }

            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public async Task<IActionResult> PostSendConfirmationLetter(Monitoringvisit _site,string confirmationletter="", List<int> Attendies=null)
        {

            //_site.IsActive = 1;
            //_site.CreatedAt = GeneralPurpose.DateTimeNow();

            //_site.Id = await new StudyBL().AddMonitoringVisit(_site, de);

            //if (_site.Id == 0)
            //{
            //    return RedirectToAction("AddMonitoringVisit", "Studies", new { id = _site.StudyId, msg = "Somethings' Wrong", color = "red" });
            //}

            //var  result = string.Join<int>(", ", Attendies);



            for (int i = 0; i < Attendies.Count; i++)
            {
                var investigator = new StudyBL().GetSingleInvestigatorSiteStaffById(Attendies[i],de);

                //if (checkEmail == false)
                //{
                //    int id = new UserBL().GetAllUsersList(de).Where(x => x.Email.ToLower() == Email.ToLower()).Select(x => x.Id).FirstOrDefault();

                    string BaseUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "/";

                    bool checkMail = MailSender.SendConfirmationLetterEmail(investigator.Email, confirmationletter, BaseUrl);

                if (!checkMail == true)
                {
                    return RedirectToAction("CreateConfirmationLetter", "Studies", new { msg = "Mail sending fail please try again!", color = "red" });

                }

            }




            return RedirectToAction("CreateConfirmationLetter", "Studies", new { id = _site.StudyId, msg = "Email Send Successfully", color = "green" });
        }
        public IActionResult CreateFollowUpLetter(int id = -1, int monitoringVisitId = -1, string msg = "", string color = "black")
        {
            try
            {
                var userid = Convert.ToInt32(User.Claims.Where(a => a.Type == ClaimTypes.Sid).First().Value);
                var currentUser = new UserBL().GetActiveUserById(Convert.ToInt32(userid), de);
                ViewBag.Role = currentUser.Role;
                //ViewBag.StudyId = id;
                ViewBag.organizationid = currentUser.OrganizationId;
                ViewBag.StudyId = id;
                ViewBag.UserId = userid;
                ViewBag.Message = msg;
                ViewBag.Color = color;
                ViewBag.monitoringId = monitoringVisitId;
                if (monitoringVisitId != -1)
                {
                    var mVisit = new StudyBL().GetActivesingleMonitoringVisitsbyId(monitoringVisitId, de);
                    if (mVisit != null)
                    {

                        ViewBag.MonitoringVisit = mVisit;
                    }


                }

            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public async Task<IActionResult> PostSendFollowUpLetter(Monitoringvisit _site,string followupletter="", List<int> Attendies=null)
        {

            //_site.IsActive = 1;
            //_site.CreatedAt = GeneralPurpose.DateTimeNow();

            //_site.Id = await new StudyBL().AddMonitoringVisit(_site, de);

            //if (_site.Id == 0)
            //{
            //    return RedirectToAction("AddMonitoringVisit", "Studies", new { id = _site.StudyId, msg = "Somethings' Wrong", color = "red" });
            //}

            //var  result = string.Join<int>(", ", Attendies);



            for (int i = 0; i < Attendies.Count; i++)
            {
                var investigator = new StudyBL().GetSingleInvestigatorSiteStaffById(Attendies[i],de);

                //if (checkEmail == false)
                //{
                //    int id = new UserBL().GetAllUsersList(de).Where(x => x.Email.ToLower() == Email.ToLower()).Select(x => x.Id).FirstOrDefault();

                    string BaseUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "/";

                    bool checkMail = MailSender.SendFollowupLetterEmail(investigator.Email, followupletter, BaseUrl);

                if (!checkMail == true)
                {
                    return RedirectToAction("CreateFollowUpLetter", "Studies", new { msg = "Mail sending fail please try again!", color = "red" });

                }

            }




            return RedirectToAction("CreateFollowUpLetter", "Studies", new { id = _site.StudyId, msg = "Email Send Successfully", color = "green" });
        }

        public IActionResult ViewMonitoringVisit(int id = -1, string msg = "", string color = "black")
        {
            try
            {
                var userid = Convert.ToInt32(User.Claims.Where(a => a.Type == ClaimTypes.Sid).First().Value);
                var currentUser = new UserBL().GetActiveUserById(Convert.ToInt32(userid), de);
                ViewBag.Role = currentUser.Role;
                //ViewBag.StudyId = id;
                ViewBag.organizationid = currentUser.OrganizationId;
                ViewBag.StudyId = id;
                ViewBag.UserId = userid;
                ViewBag.Message = msg;
                ViewBag.Color = color;
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }



        public IActionResult AddGeneralFinding(int id = -1, string msg = "", string color = "black")
        {
            try
            {
                var userid = Convert.ToInt32(User.Claims.Where(a => a.Type == ClaimTypes.Sid).First().Value);
                var currentUser = new UserBL().GetActiveUserById(Convert.ToInt32(userid), de);
                ViewBag.Role = currentUser.Role;
                //ViewBag.StudyId = id;
                ViewBag.organizationid = currentUser.OrganizationId;
                ViewBag.StudyId = id;
                ViewBag.UserId = userid;
                ViewBag.Message = msg;
                ViewBag.Color = color;
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public async Task<IActionResult> PostAddGeneralFinding(GeneralFindingsDto findingdto)
        {

            //IDictionary<int, string> d = new Dictionary<int, string>();

            //if (findingdto.Action_resolution.Count() > 0)
            //{
            //    var i = 0;
            //    foreach (var item in findingdto.Action_resolution)
            //    {
            //        i = i + 1;
            //        if (!string.IsNullOrEmpty(item))
            //        {
            //            d.Add(new KeyValuePair<int, string>(i, item));
            //        }
            //    }
            //}
            for (int i = 0; i < findingdto.Question.Count; i++)
            {
                string jsonStr = null;
                if(findingdto.Subject.Count > 0 || findingdto.Subject.Count <= findingdto.Question.Count &&  findingdto.Category!= null || findingdto.Category.Count <=findingdto.Question.Count && findingdto.Dateoccur.Count>0 && findingdto.Status.Count>0 && findingdto.Description.Count> 0 || findingdto.Description.Count <= findingdto.Question.Count && findingdto.Action_resolution.Count >0 || findingdto.Action_resolution.Count <= findingdto.Question.Count && findingdto.Significance.Count>0 &&  findingdto.Question.Count >0)
                {
                    if (findingdto.Subject[i]!=null && findingdto.Category[i]!=null && findingdto.Dateoccur[i] != null && findingdto.Status[i] !=null && findingdto.Description[i]!=null && findingdto.Action_resolution[i]!=null && findingdto.Significance[i]!=null && findingdto.Question[i]!=null)

                    //if (findingdto.Questionstatus[i] =="No")
                    {

                        var finding = new Findingsjsondto()
                        {
                            Subject = findingdto.Subject[i],
                            Category = findingdto.Category[i],
                            Status = findingdto.Status[i],
                            Significance = findingdto.Significance[i],
                            Description = findingdto.Description[i],
                            Action_resolution = findingdto.Action_resolution[i],
                            Question = findingdto.Question[i],
                            Dateoccur = Convert.ToDateTime(findingdto.Dateoccur[i]) 
                            //Dateoccur = null

                        };

                        jsonStr = JsonSerializer.Serialize(finding);


                    }


                }

                
               // var weatherForecast = JsonSerializer.Deserialize<Findingsjsondto>(jsonStr);

                var generals = new GeneralFindings()
                {
                    MonitoringVisitId = findingdto.MonitoringVisitId,
                    Createdby = findingdto.Createdby,
                    IsActive = 1,
                    CreatedAt=DateTime.UtcNow,
                    Question = findingdto.Question[i],
                    QuestionStatus= null,
                    Findings =jsonStr,
                    OrganizationId=findingdto.OrganizationId,
                    StudyId=findingdto.StudyId,
                    InvestiGatorSitefId=findingdto.InvestiGatorSitefId,
                    ReviewComments=findingdto.ReviewComments,
                    ReviewDate=findingdto.ReviewDate,
                    FindingStatus=findingdto.FindingStatus

                };

                if (!await new StudyBL().AddGeneralFindings(generals, de))
                {
                    return RedirectToAction("AddGeneralFinding", "Studies", new { id = "", msg = "Something went wrong please try again!", color = "red" });
                }

                //JavaScriptSerializer js = new JavaScriptSerializer();
                //string jsonData = js.Serialize(finding);

            }




            //for (int i = 0; i < findingdto.Category.Count; i++)
            //{

            //    var finding = new GeneralFindings()
            //    {
            //        MonitoringVisitId = findingdto.MonitoringVisitId,
            //        Createdby=findingdto.Createdby,
            //        IsActive=1,


            //    };

            //    if (!await new StudyBL().AddAttendie(finding, de))
            //    {
            //        return RedirectToAction("AddMonitoringVisit", "Studies", new { id = "", msg = "Something went wrong while adding staff!", color = "red" });
            //    }


            //}




            return RedirectToAction("AddGeneralFinding", "Studies", new { id ="", msg = "Record Inserted Successfully", color = "green" });
        }


        public IActionResult ViewGeneralFindings(int id = -1,int monitoringVisitId=-1,string msg = "", string color = "black")
        {
            try
            {
                var userid = Convert.ToInt32(User.Claims.Where(a => a.Type == ClaimTypes.Sid).First().Value);
                var currentUser = new UserBL().GetActiveUserById(Convert.ToInt32(userid), de);
                ViewBag.Role = currentUser.Role;
                //ViewBag.StudyId = id;
                ViewBag.organizationid = currentUser.OrganizationId;
                ViewBag.monitoringvisitid = monitoringVisitId;
                ViewBag.StudyId = id;
                ViewBag.UserId = userid;
                ViewBag.Message = msg;
                ViewBag.Color = color;
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult ViewHighRiskInvestigatorSite(int id = -1, int monitoringVisitId = -1, string msg = "", string color = "black")
        {
            try
            {
                var userid = Convert.ToInt32(User.Claims.Where(a => a.Type == ClaimTypes.Sid).First().Value);
                var currentUser = new UserBL().GetActiveUserById(Convert.ToInt32(userid), de);
                ViewBag.Role = currentUser.Role;
                //ViewBag.StudyId = id;
                ViewBag.organizationid = currentUser.OrganizationId;
                ViewBag.monitoringvisitid = monitoringVisitId;
                ViewBag.StudyId = id;
                ViewBag.UserId = userid;
                ViewBag.Message = msg;
                ViewBag.Color = color;
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult UpdateGeneralFinding(int id = -1, string msg = "", string color = "black")
        {
            try
            {
                var userid = Convert.ToInt32(User.Claims.Where(a => a.Type == ClaimTypes.Sid).First().Value);
                var currentUser = new UserBL().GetActiveUserById(Convert.ToInt32(userid), de);
                ViewBag.Role = currentUser.Role;
                //ViewBag.StudyId = id;
                ViewBag.organizationid = currentUser.OrganizationId;
                ViewBag.StudyId = id;
                ViewBag.UserId = userid;
                ViewBag.Message = msg;
                ViewBag.Color = color;
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }


         public async Task<IActionResult> DeleteGeneralFindings(int id)
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
           // var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
                var MonitoringVisitId = new StudyBL().GetActiveGeneralFindingById(id,de).MonitoringVisitId;   

                //var MonitoringVisitId = await new StudyBL().DeleteGeneralFinding(id, de);

                if (!await new StudyBL().DeleteGeneralFinding(id, de))

                {
                    return RedirectToAction("ViewGeneralFindings", "Studies", new { monitoringVisitId = MonitoringVisitId, msg = "Somethings' wrong", color = "red" });

                }
                return RedirectToAction("ViewGeneralFindings", "Studies", new { monitoringVisitId = MonitoringVisitId, msg = "Record deleted successfully!", color = "green" });

            }
            catch
            {
                return RedirectToAction("NotFoundPage", "Error");
            }
        }
         public async Task<IActionResult> DeleteMonitoringVisit(int id)
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
           // var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {

                if (!await new StudyBL().DeleteMonitoringVisit(id, de))
                {
                    return RedirectToAction("ViewMonitoringVisit", "Studies", new { msg = "Somethings' wrong", color = "red" });

                }
                return RedirectToAction("ViewMonitoringVisit", "Studies", new { msg = "Record deleted successfully!", color = "green" });

            }
            catch
            {
                return RedirectToAction("NotFoundPage", "Error");
            }
        }

        #endregion

    }
}
