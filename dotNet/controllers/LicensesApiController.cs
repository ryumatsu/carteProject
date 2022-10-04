using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Licenses;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;



namespace Sabio.Web.Api.Controllers
{
    [Route("api/licenses")]
    [ApiController]
    public class LicensesApiController : BaseApiController
    {
        private ILicensesService _licenseService = null;
        private IAuthenticationService<int> _authService = null;
        public LicensesApiController(ILicensesService service
            , ILogger<LicensesApiController> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _licenseService = service;
            _authService = authService;
        }

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> DeleteById(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _licenseService.DeleteById(id);
                response = new SuccessResponse();

                if (response == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application Resource not found.");
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        [HttpGet("{orgId:int}")] 
        public ActionResult<ItemsResponse<Organization>> GetByOrgId(int orgId)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Organization license = _licenseService.GetByOrgId(orgId);

                if (license == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Organization> { Item = license };
                }
            }

            catch (SqlException sqlEx)
            {
                iCode = 500;
                response = new ErrorResponse($"SqlException Error: {sqlEx.Message}");
                base.Logger.LogError(sqlEx.ToString());
            }

            catch (ArgumentException argEx)
            {
                iCode = 500;
                response = new ErrorResponse($"ArgumentException Error: {argEx.Message}");

            }

            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }

            return StatusCode(iCode, response);


        }

        [HttpPost("")]
        public ActionResult<ItemResponse<int>> Add(LicensesAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int userId = _authService.GetCurrentUserId();

                int id = _licenseService.Add(model, userId);

                ItemResponse<int> response = new ItemResponse<int>()

                { Item = id };

                result = Created201(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }

            return result;



        }

        [HttpGet("createdby")]
        public ActionResult<ItemResponse<Paged<Licenses>>> SelectByCreatedBy(int pageIndex, int pageSize, int createdById)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<Licenses> page = _licenseService.SelectByCreatedBy(pageIndex, pageSize, createdById);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Licenses>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(code, response);

        }

        [HttpGet("{id:int}")] 
        public ActionResult<ItemsResponse<Licenses>> GetById(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Licenses license = _licenseService.GetById(id);

                if (license == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Licenses> { Item = license };

                }
            }

            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }
            return StatusCode(iCode, response);
        }

     
        [HttpGet("{licensetype:int}")]
        public ActionResult<ItemsResponse<Licenses>> GetByLicenseType(int LicenseTypeId)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Licenses license = _licenseService.GetByLicenseType(LicenseTypeId);

                if (license == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Licenses> { Item = license };

                }
            }

            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }

            return StatusCode(iCode, response);


        }

        [HttpGet("")]
        public ActionResult<ItemResponse<Paged<Licenses>>> GetAll(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<Licenses> page = _licenseService.GetAll(pageIndex, pageSize);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Licenses>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpGet("query")]
        public ActionResult<ItemResponse<Paged<Licenses>>> GetQuery(int pageIndex, int pageSize, string query)
        {

            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<Licenses> page = _licenseService.GetQuery(pageIndex, pageSize, query);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Licenses>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpPut("{id:int}")] 
        public ActionResult<SuccessResponse> Update(LicensesUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                int userId = _authService.GetCurrentUserId();

                _licenseService.Update(model, userId);
                response = new SuccessResponse();

                if (response == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }
    }
}
