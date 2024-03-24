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
        private readonly CreditCardService _creditCardService;
        private readonly CardTransactionService _cardTransactionService;


        public ProductController(LoanService loanService, InsuranceService insuranceService, CreditCardService creditCardService, CardTransactionService cardTransactionService)
        {
            _loanService = loanService;
            _insuranceService = insuranceService;
            _creditCardService = creditCardService;
            _cardTransactionService = cardTransactionService;
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
        [HttpPost("creditcard")]
        public async Task<IActionResult> CreateCreditCard(CreditCardRequestDto creditCardDto)
        {
            try
            {
                // Converta o número do cartão para ulong
                if (!ulong.TryParse(creditCardDto.CardNumber, out ulong cardNumber))
                {
                    return BadRequest("Número do cartão de crédito inválido.");
                }

                // Crie um objeto CreditCard com os dados do DTO
                var creditCard = new CreditCard
                {
                    CardNumber = cardNumber,
                    HolderName = creditCardDto.HolderName,
                    ExpiryDate = creditCardDto.ExpiryDate,
                    CVV = creditCardDto.CVV,
                    TotalLimit = creditCardDto.TotalLimit,
                    UsedLimit = 0
                };

                // Chame o serviço para criar o cartão de crédito
                var createdCreditCard = await _creditCardService.CreateCreditCardAsync(creditCard);


                return Ok(createdCreditCard);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao criar o cartão de crédito: {ex.Message}");
            }
        }

        [HttpGet("creditcard")]
        public async Task<IActionResult> GetAllCreditCards()
        {
            var creditCards = await _creditCardService.GetAllCreditCardsAsync();
            return Ok(creditCards);
        }

        [HttpGet("creditcard/{id}")]
        public async Task<IActionResult> GetCreditCardById(int id)
        {
            var creditCard = await _creditCardService.GetCreditCardByIdAsync(id);
            if (creditCard == null)
            {
                return NotFound($"Credit card with ID {id} not found.");
            }
            return Ok(creditCard);
        }

        [HttpDelete("creditcard/{id}")]
        public async Task<IActionResult> DeleteCreditCard(int id)
        {
            try
            {
                await _creditCardService.DeleteCreditCardAsync(id);
                return Ok($"Credit card with ID {id} deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }
        [HttpPost("creditcard/transaction")]
        public async Task<IActionResult> MakeCreditCardTransaction(CardTransactionRequestDto transactionDto)
        {
            try
            {
                var success = await _cardTransactionService.MakeTransactionAsync(transactionDto.CreditCardId, transactionDto.Amount);
                if (success)
                {
                    return Ok("Credit card transaction successful.");
                }
                else
                {
                    return BadRequest("Credit card transaction failed. Insufficient credit limit.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing the credit card transaction: {ex.Message}");
            }
        }

        [HttpGet("creditcard/transaction")]
        public async Task<IActionResult> GetAllCreditCardTransactions()
        {
            var transactions = await _cardTransactionService.GetAllTransactionsAsync();
            return Ok(transactions);
        }

        [HttpGet("creditcard/transaction/{id}")]
        public async Task<IActionResult> GetCreditCardTransactionById(int id)
        {
            var transaction = await _cardTransactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                return NotFound($"Credit card transaction with ID {id} not found.");
            }
            return Ok(transaction);
        }

        [HttpDelete("creditcard/transaction/{id}")]
        public async Task<IActionResult> DeleteCreditCardTransaction(int id)
        {
            try
            {
                await _cardTransactionService.DeleteTransactionAsync(id);
                return Ok($"Credit card transaction with ID {id} deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
