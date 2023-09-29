using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PatientManagement.DataBase;
using PatientManagement.Filters;
using PatientManagement.Repository;

namespace PatientManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository _patientRepository;

        public PatientController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPatients(string term, string sort, int page, int limit)
        {
            var patients = await _patientRepository.GetAllPatientsAsync();

            if (patients != null && patients.Count > 0)
            {
                var patientResult = await _patientRepository.GetPatients(term, sort, page, limit);

                Response.Headers.Add("X-Total-Count", patientResult.TotalCount.ToString()); //24
                Response.Headers.Add("X-Total-Pages", patientResult.TotalPages.ToString()); //3

                return Ok(patientResult.Patients);
            }
            else
            {
                return NotFound("No patients found");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Enter a valid id");
            }

            var patient = await _patientRepository.GetPatientByIdAsync(id);

            if (patient == null)
            {
                return NotFound("Patient not found");
            }
            return Ok(patient);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewPatient([FromBody] PatientModel patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Patient not found");
            }

            var id = await _patientRepository.AddNewPatientAsync(patient);
            return CreatedAtAction(nameof(GetPatientById), new { id = id, Controller = "Patient"}, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatientDetails([FromRoute] int id, [FromBody] PatientModel patient)
        {
            if (id <= 0)
            {
                return BadRequest("Enter valid id");
            }

            var patientt = await _patientRepository.GetPatientByIdAsync(id);

            if (patientt == null)
            {
                return NotFound("Patient not fonund");
            }

            await _patientRepository.UpdatePatientAsync(id, patient);
            return Ok("Details are updated");
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePatientPatch([FromBody] JsonPatchDocument patchDocument, [FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest("Enter valid id");
            }

            var patient = await _patientRepository.GetPatientByIdAsync(id);

            if (patient == null)
            {
                return NotFound("Patient not found");
            }
            await _patientRepository.UpdatePatientPatchAsync(id, patchDocument);
            return Ok("Details are updated through patch");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest("Enter valid id");
            }

            var patient = await _patientRepository.GetPatientByIdAsync(id);

            if (patient == null)
            {
                return NotFound("Patient not found");
            }
            await _patientRepository.DeletePatientAsync(id);
            return Ok("Patient details are deleted");
        }
    }
}
