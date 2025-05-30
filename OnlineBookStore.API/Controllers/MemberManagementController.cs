﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Business.IService;
using OnlineBookStore.Business.Service;
using OnlineBookStore.Business.ViewModels;
using OnlineBookStore.Business.ViewModels.Member;

namespace OnlineBookStore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class MemberManagementController : BaseController
    {
        private readonly IMemberService _memberService;

        public MemberManagementController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet("GetAllMembers")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllMembers([FromQuery] MemberFilterRequest memberFilterRequest)
        {
            var result = await _memberService.GetAllMembersAsync(memberFilterRequest);
            return Ok(result);
        }

        [HttpDelete("DeleteMember")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var result = await _memberService.DeleteMemberAsync(id);
            return Ok(result);
        }
    }

}
