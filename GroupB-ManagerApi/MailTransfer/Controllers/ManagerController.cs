using MailTransfer.Infrastructure;
using MailTransfer.Model;
using MailTransfer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MailTransfer.Controllers
{
    [Route("api/Manager")]
    [ApiController]
    [EnableCors]
    
    public class ManagerController : ControllerBase
    {
        private readonly IMailService mailService;
        private readonly IMailRepository<mailData> mailRepository;
        public ManagerController(IMailService mailService,IMailRepository<mailData> _mailRepository)
        {
            this.mailService = mailService;
            mailRepository = _mailRepository;
        }
        [HttpGet("Approve")]
        [Authorize(Policy = "CustomerDataControl")]
        public async Task<IActionResult> SendMail(int Id)
        {
            try
            {
                await mailService.SendEmailAsync(Id);
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost("Reject")]
        [Authorize(Policy = "CustomerDataControl")]
        public async Task<IActionResult> Reject(FeedbackModel model)
        {
            try
            {
                await mailService.SendRejectedMaiAsync(model.id,model.feedback);
                return Ok();
            }catch(Exception ex)
            {
                throw;
            }
        }

        [HttpGet("PendingEnquiries")]
        [Authorize(Policy = "CustomerDataControl")]
        public ActionResult<RequestPendingRejectedApprovedModel> PendingList(int Id)
        {
            try
            {
                var PendingEnquiries =mailRepository.PendingEnquiries(Id);
                return Ok(PendingEnquiries);
            }catch(Exception ex)
            {
                throw;
            }
        }
        [HttpGet("RejectedEnquiries")]
        [Authorize(Policy = "CustomerDataControl")]
        public ActionResult<RequestPendingRejectedApprovedModel> RejectedList(int Id)
        {
            try
            {
                var PendingEnquiries = mailRepository.RejectedEnquiries(Id);
                return Ok(PendingEnquiries);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet("ApprovedEnquiries")]
        [Authorize(Policy = "CustomerDataControl")]
        public ActionResult<RequestPendingRejectedApprovedModel> ApprovedList(int Id)
        {
            try
            {
                var PendingEnquiries = mailRepository.ApprovedEnquiries(Id);
                return Ok(PendingEnquiries);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("EnquiryDetails")]
        [Authorize(Policy = "CustomerDataControl")]
        public ActionResult<EnquiryModel> EnquiryDetails(int Id)
        {
            try
            {
                var model = mailRepository.DisplayEnquiry(Id);
                return Ok(model);
            }catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
