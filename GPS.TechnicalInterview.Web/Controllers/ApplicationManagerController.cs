using GPS.ApplicationManager.Web.Controllers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace GPS.ApplicationManager.Web.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class ApplicationManagerController : ControllerBase
  {
    private readonly ILogger<ApplicationManagerController> _logger;
    private static readonly string _filePath = "loanApplication.json";

    public ApplicationManagerController(ILogger<ApplicationManagerController> logger)
    {
      _logger = logger;
    }

    private async static Task<List<LoanApplication>> GetApplicationsFromFileAsync()
    {
      if (System.IO.File.Exists(_filePath))
      {
        var existingJson = await System.IO.File.ReadAllTextAsync(_filePath);
        return JsonSerializer.Deserialize<List<LoanApplication>>(existingJson) ?? new List<LoanApplication>();
      }
      return new List<LoanApplication>();
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> CreateApplication([FromBody] LoanApplication loanApplication)
    {
      if (loanApplication == null ||
          string.IsNullOrEmpty(loanApplication.PersonalInformation.Name.First) ||
          string.IsNullOrEmpty(loanApplication.PersonalInformation.Name.Last) ||
          string.IsNullOrEmpty(loanApplication.PersonalInformation.PhoneNumber) ||
          string.IsNullOrEmpty(loanApplication.PersonalInformation.Email) ||
          string.IsNullOrEmpty(loanApplication.ApplicationNumber) ||
          loanApplication.LoanTerms.Amount <= 0 || 
          Regex.IsMatch(loanApplication.PersonalInformation.PhoneNumber, @"^[0-9]{10}$") == false || 
          Regex.IsMatch(loanApplication.PersonalInformation.Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$") == false)
      {
        return BadRequest("Invalid application data. All fields are required and must be valid.");
      }

      loanApplication.DateApplied = DateTime.UtcNow;
      var applications = await GetApplicationsFromFileAsync();
      applications.Add(loanApplication);
      var json = JsonSerializer.Serialize(applications);
      await System.IO.File.WriteAllTextAsync(_filePath, json);
      return Ok(new { message = "Created Successfully." });
    }

    // Please Note, this method was created with some line completion from CoPilot.
    public async Task<IActionResult> EditApplications([FromBody] LoanApplication loanApplication)
    {
      if (loanApplication == null ||
          string.IsNullOrEmpty(loanApplication.PersonalInformation.Name.First) ||
          string.IsNullOrEmpty(loanApplication.PersonalInformation.Name.Last) ||
          string.IsNullOrEmpty(loanApplication.PersonalInformation.PhoneNumber) ||
          string.IsNullOrEmpty(loanApplication.PersonalInformation.Email) ||
          string.IsNullOrEmpty(loanApplication.ApplicationNumber) ||
          loanApplication.LoanTerms.Amount <= 0 || 
          Regex.IsMatch(loanApplication.PersonalInformation.PhoneNumber, @"^[0-9]{10}$") == false || 
          Regex.IsMatch(loanApplication.PersonalInformation.Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$") == false)
      {
        return BadRequest("Invalid application data. All fields are required and must be valid.");
      }

      var applications = await GetApplicationsFromFileAsync();
      var existingApplication = applications.FirstOrDefault(a => a.ApplicationNumber == loanApplication.ApplicationNumber);
      if (existingApplication == null)
      {
        return NotFound("Application not found.");
      }

      existingApplication.PersonalInformation = loanApplication.PersonalInformation;
      existingApplication.LoanTerms = loanApplication.LoanTerms;
      existingApplication.Status = loanApplication.Status;
      var json = JsonSerializer.Serialize(applications);
      await System.IO.File.WriteAllTextAsync(_filePath, json);
      return Ok(new { message = "Updated Successfully." });
    }

    // Please Note, this method was created with some line completion from CoPilot. 
    public async Task<IActionResult> DeleteApplication(string applicationNumber)
    {
      if (string.IsNullOrEmpty(applicationNumber))
      {
        return BadRequest("Application number is required.");
      }

      var applications = await GetApplicationsFromFileAsync();
      var existingApplication = applications.FirstOrDefault(a => a.ApplicationNumber == applicationNumber);
      if (existingApplication == null)
      {
        return NotFound("Application not found.");
      }
      DialogResult result = MessageBox.Show("Are you sure you want to delete this application?", "Delete Application", MessageBoxButtons.YesNo);
      if (result == DialogResult.No)
      {
        return Ok(new { message = "Deletion Cancelled." });
      }
      else{
        applications.Remove(existingApplication);
        var json = JsonSerializer.Serialize(applications);
        await System.IO.File.WriteAllTextAsync(_filePath, json);
        return Ok(new { message = "Deleted Successfully." });
      }
    }
    // TODO: Add your CRUD (Read, Update, Delete) methods here:
  }
}
