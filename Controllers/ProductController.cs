using APIBanco.Domain.Dtos;
using APIBanco.Domain.Models;
using APIBanco.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIBanco.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly LoanService _loanService;
        private readonly InsuranceService _insuranceService;

        public ProductController(LoanService loanService, InsuranceService insuranceService)
        {
            _loanService = loanService;
            _insuranceService = insuranceService;
        }

        [HttpPost("loan")]
        public async Task<IActionResult> CreateLoan(LoanRequestDto loanDto)
        {
            var loan = new Loan
            {
                LoanAmount = loanDto.LoanAmount,
                InterestRate = loanDto.InterestRate,
                LoanTermMonths = loanDto.LoanTermMonths,
                ContractDate = loanDto.ContractDate
            };

            var createdLoan = await _loanService.CreateLoanAsync(loan);

            return Ok(createdLoan);
        }

        [HttpPut("loan/{id}")]
        public async Task<IActionResult> UpdateLoan(int id, LoanRequestDto loanDto)
        {
            var existingLoan = await _loanService.GetLoanByIdAsync(id);
            if (existingLoan == null)
            {
                return NotFound($"Loan with ID {id} not found.");
            }

            existingLoan.LoanAmount = loanDto.LoanAmount;
            existingLoan.InterestRate = loanDto.InterestRate;
            existingLoan.LoanTermMonths = loanDto.LoanTermMonths;
            existingLoan.ContractDate = loanDto.ContractDate;

            await _loanService.UpdateLoanAsync(existingLoan);

            return Ok(existingLoan);
        }

        [HttpGet("loan")]
        public async Task<IActionResult> GetAllLoans()
        {
            var loans = await _loanService.GetAllLoansAsync();
            return Ok(loans);
        }

        [HttpGet("loan/{id}")]
        public async Task<IActionResult> GetLoanById(int id)
        {
            var loan = await _loanService.GetLoanByIdAsync(id);
            if (loan == null)
            {
                return NotFound($"Loan with ID {id} not found.");
            }
            return Ok(loan);
        }

        [HttpDelete("loan/{id}")]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            try
            {
                await _loanService.DeleteLoanAsync(id);
                return Ok($"Loan with ID {id} deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("insurance")]
        public async Task<IActionResult> CreateInsurance(InsuranceRequestDto insuranceDto)
        {
            var insurance = new Insurance
            {
                InsuranceType = insuranceDto.InsuranceType,
                Coverage = insuranceDto.Coverage,
                Company = insuranceDto.Company,
                PolicyNumber = insuranceDto.PolicyNumber,
                StartDate = insuranceDto.StartDate,
                EndDate = insuranceDto.EndDate,
                Premium = insuranceDto.Premium
            };

            var createdInsurance = await _insuranceService.CreateInsuranceAsync(insurance);

            return Ok(createdInsurance);
        }

        [HttpPut("insurance/{id}")]
        public async Task<IActionResult> UpdateInsurance(int id, InsuranceRequestDto insuranceDto)
        {
            var existingInsurance = await _insuranceService.GetInsuranceByIdAsync(id);
            if (existingInsurance == null)
            {
                return NotFound($"Insurance with ID {id} not found.");
            }

            existingInsurance.InsuranceType = insuranceDto.InsuranceType;
            existingInsurance.Coverage = insuranceDto.Coverage;
            existingInsurance.Company = insuranceDto.Company;
            existingInsurance.PolicyNumber = insuranceDto.PolicyNumber;
            existingInsurance.StartDate = insuranceDto.StartDate;
            existingInsurance.EndDate = insuranceDto.EndDate;
            existingInsurance.Premium = insuranceDto.Premium;

            await _insuranceService.UpdateInsuranceAsync(existingInsurance);

            return Ok(existingInsurance);
        }

        [HttpGet("insurance")]
        public async Task<IActionResult> GetAllInsurances()
        {
            var insurances = await _insuranceService.GetAllInsurancesAsync();
            return Ok(insurances);
        }

        [HttpGet("insurance/{id}")]
        public async Task<IActionResult> GetInsuranceById(int id)
        {
            var insurance = await _insuranceService.GetInsuranceByIdAsync(id);
            if (insurance == null)
            {
                return NotFound($"Insurance with ID {id} not found.");
            }
            return Ok(insurance);
        }

        [HttpDelete("insurance/{id}")]
        public async Task<IActionResult> DeleteInsurance(int id)
        {
            try
            {
                await _insuranceService.DeleteInsuranceAsync(id);
                return Ok($"Insurance with ID {id} deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
