
using TrudoseAdminPortalAPI.Interface;
using Microsoft.AspNetCore.Mvc;

namespace TrudoseAdminPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyQuestionsListController : ControllerBase
    {
        private readonly ISurveyQuestionsList _symptomsRepository;

        public SurveyQuestionsListController(ISurveyQuestionsList symptomsRepository)
        {
            _symptomsRepository = symptomsRepository;
        }

        //[HttpGet("GetSurveyData")]
        //public async Task<IActionResult> GetSurveyData([FromQuery] List<int> symptomIds)
        //{
        //    if (symptomIds == null || !symptomIds.Any())
        //        return BadRequest("symptomIds field is required and cannot be empty.");

        //    var result = await _symptomsRepository.GetSurveyDataAsync(symptomIds);

        //    if (!result.Any())
        //        return NotFound("No data found for the given symptom IDs.");

        //    return Ok(result);
        //}



        /////////////////////////////////////////////////////////////////imp
        //[HttpPost("GetSurveyData")]
        //public async Task<IActionResult> GetSurveyData([FromBody] List<int> symptomIds)
        //{
        //    if (symptomIds == null || !symptomIds.Any())
        //        return BadRequest("symptomIds field is required and cannot be empty.");

        //    var result = await _symptomsRepository.GetSurveyDataAsync(symptomIds);

        //    if (!result.Any())
        //        return NotFound("No data found for the given symptom IDs.");

        //    return Ok(result);
        //}



        [HttpPost("GetSurveyQuestionsData")]
        public async Task<IActionResult> GetSurveyData([FromBody] List<int> symptomIds)
        {
            if (symptomIds == null || !symptomIds.Any())
                return BadRequest("Symptom IDs cannot be empty.");

            var result = await _symptomsRepository.GetSurveyDataAsync(symptomIds);
            return Ok(result);
        }


        //[HttpGet("GetSurveyData")]
        //public async Task<IActionResult> GetSurveyData([FromQuery] List<int> symptomIds)
        //{
        //    if (symptomIds == null || !symptomIds.Any())
        //        return BadRequest("Symptom IDs cannot be empty.");

        //    var result = await _symptomsRepository.GetSurveyDataAsync(symptomIds);
        //    return Ok(result);
        //}
    }
}
