﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackKFHShortcuts.Models;
using BackKFHShortcuts.Models.Entities;
using BackKFHShortcuts.Models.Responses;
using BackKFHShortcuts.Models.Request;
using BackKFHShortcuts.Models.Authentication;

namespace BackKFHShortcuts.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ShortcutsContext _context;
        private readonly TokenService _tokenService;

        public AuthenticationController(ShortcutsContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // POST: Authentication/Login
        [HttpPost("[action]")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            var result = _tokenService.GenerateToken(request.Email, request.Password);
            if (result.IsValid)
            {
                return Ok(result.Response);
            }
            return BadRequest("Email and/or Password is incorrect");
        }
    }
}