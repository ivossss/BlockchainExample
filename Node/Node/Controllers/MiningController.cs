﻿namespace Node.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Node.Interfaces;
    using Node.Models;

    [Produces("application/json")]
    [Route("api/Mining")]
    public class MiningController : Controller
    {
        private INodeService nodeService;

        public MiningController(INodeService nodeService)
        {
            this.nodeService = nodeService;
        }

        [HttpGet("get-mining-job/{minerAddress}")]
        public IActionResult GetNextMiningJob(string minerAddress)
        {
            try
            {
                BlockCandidate bc = this.nodeService.ProcessNextBlockCandiate(minerAddress);
                var miningJob = new MiningJobResponseModel()
                {
                    BlockIndex = bc.Index,
                    TransactionsIncluded = bc.Transactions.Count,
                    ExpectedReward = 5000350, //TODO: where is this comming from
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
    }
}