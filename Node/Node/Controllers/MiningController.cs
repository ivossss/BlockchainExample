﻿namespace Node.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Node.ApiModels;
    using Node.Interfaces;
    using Node.Models;

    [Produces("application/json")]
    [Route("api/Mining")]
    public class MiningController : Controller
    {
        private IBlockService blockService;

        public MiningController(IBlockService blockService)
        {
            this.blockService = blockService;
        }

        [HttpGet("get-mining-job/{minerAddress}")]
        public IActionResult GetNextMiningJob(string minerAddress)
        {
            try
            {
                //TODO: validate minerAddress
                BlockCandidate bc = this.blockService.ProcessNextBlockCandiate(minerAddress);
                var miningJob = new MiningJobResponseModel()
                {
                    BlockIndex = bc.Index,
                    TransactionsIncluded = bc.ConfirmedTransactions.Count,
                    ExpectedReward = 5000350, //TODO: calculate fees
                    RewardAddress = minerAddress,
                    BlockDataHash = bc.BlockDataHash,
                    Difficulty = bc.Difficulty
                };

                return Json(miningJob);
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not get mining job: {ex}");
            }
        }

        [HttpPost("submit-mined-job")]
        public IActionResult SubmitMinedJob([FromBody]MiningJobRequestModel miningJobRM)
        {
            try
            {
                this.blockService.ProcessNextBlock(miningJobRM);
                return Ok("New block sucessfully added to the chain.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex}");
            }
        }
    }
}