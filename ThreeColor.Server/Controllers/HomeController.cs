using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using ThreeColor.Server.Abstract;
using ThreeColor.Server.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;
using ThreeColor.Data.Models;

namespace ThreeColor.Server.Controllers
{
    [Authorize]
    [RoutePrefix("api")]
    public class HomeController : ApiController
    {
        private readonly IDataRepository _dataRepository;

        public HomeController(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        [HttpGet]
        [Route("Test/{id}")]
        public IHttpActionResult GetTestById([FromUri]int? id)
        {
            if (id == null)
                return BadRequest("Id cannot be empty!");

            var result = _dataRepository.GetTest(id.Value);
            if (!result.IsSuccess)
                return Content(HttpStatusCode.BadRequest, result.Exception);

            return Ok(result.Data);
        }

        [HttpGet]
        [Route("Test/All")]
        public IHttpActionResult GetTests(bool includeDeleted = false)
        {
            var result = _dataRepository.GetTests(includeDeleted);
            if (!result.IsSuccess)
                return Content(HttpStatusCode.BadRequest, result.Exception);

            return Ok(result.Data);
        }

        [HttpGet]
        [Route("Point/{testId}/{includeDeleted}")]
        public IHttpActionResult GetPoints(int? testId, bool includeDeleted = false)
        {
            if (testId == null)
                return BadRequest("Id cannot be empty!");

            var result = _dataRepository.GetPoints(testId.Value, includeDeleted);
            if (!result.IsSuccess)
                return Content(HttpStatusCode.BadRequest, result.Exception);

            return Ok(result.Data);
        }

        [HttpPost]
        [Route("Test/Add")]
        public IHttpActionResult AddTest([FromBody]Tests test)
        {
            if (test == null)
                return BadRequest("Test cannot be empty!");
            var result = _dataRepository.AddTest(test);
            if (!result.IsSuccess)
                return Content(HttpStatusCode.BadRequest, result.Exception);

            return Ok(result.Data);
        }

        [HttpPost]
        [Route("Point/Add")]
        public IHttpActionResult AddPoint([FromBody]List<Points> points)
        {
            if (points == null)
                return BadRequest("Test cannot be empty!");
            var result = _dataRepository.AddPoints(points);
            if (!result.IsSuccess)
                return Content(HttpStatusCode.BadRequest, result.Exception);

            return Ok();
        }

        [HttpPost]
        [Route("Result/Add")]
        public IHttpActionResult AddResult([FromBody]Results result)
        {
            if (result == null)
                return BadRequest("Reslt cannot be empty!");
            var returnModel = _dataRepository.AddResult(result);
            if (!returnModel.IsSuccess)
                return Content(HttpStatusCode.BadRequest, returnModel.Exception);

            return Ok();
        }

        [HttpPut]
        [Route("Test/Update")]
        public IHttpActionResult UpdateTest([FromBody]Tests test)
        {
            if (test == null)
                return BadRequest("Reslt cannot be empty!");
            var returnModel = _dataRepository.UpdateTest(test);
            if (!returnModel.IsSuccess)
                return Content(HttpStatusCode.BadRequest, returnModel.Exception);

            return Ok();
        }

        [HttpPut]
        [Route("Point/Update")]
        public IHttpActionResult UpdatePoint([FromBody]List<Points> points)
        {
            if (points == null)
                return BadRequest("Reslt cannot be empty!");
            var returnModel = _dataRepository.UpdatePoints(points);
            if (!returnModel.IsSuccess)
                return Content(HttpStatusCode.BadRequest, returnModel.Exception);

            return Ok();
        }

        [HttpGet]
        [Route("Result/All")]
        public IHttpActionResult GetLastResults()
        {
            var returnModel = _dataRepository.GetLastResults();
            if (!returnModel.IsSuccess)
                return Content(HttpStatusCode.BadRequest, returnModel.Exception);

            return Ok(returnModel.Data);
        }

        [HttpGet]
        [Route("Result/{testId}")]
        public IHttpActionResult GetLastResults([FromUri] int testId)
        {
            var returnModel = _dataRepository.GetResultsByTest(testId);
            if (!returnModel.IsSuccess)
                return Content(HttpStatusCode.BadRequest, returnModel.Exception);

            return Ok(returnModel.Data);
        }
    }
}