using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using Org.BouncyCastle.Crypto.Macs;
using Root.API.Models;
using Root.API.Models.Authentication.Login;
using Root.API.Models.Authentication.SignUp;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using User.Management.Service.Models;
using User.Mangement.Service.Services;

namespace Root.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticationsController> _logger;

        public AuthenticationsController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IEmailService emailService,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AuthenticationsController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _emailService = emailService;
            _signInManager = signInManager;
            _logger = logger;
        }
        #region Test-Email
        //[HttpGet("test-email")]
        //public IActionResult TestEmail()
        //{
        //    var message = new Message(new string[] { "amroyasser55555@gmail.com" }, "Test", "<h1>Welcome<h1>");
        //    // Send a test email
        //    _emailService.SendEmail(message);
        //    return StatusCode(StatusCodes.Status200OK,
        //        new Response { Status = "Sucess", Message="Email Send SuccessFully" });
        //}
        #endregion
        #region Login With 2FA
        //[HttpPost("LogIn")]
        //public async Task<IActionResult> LogIn([FromBody] LoginModel loginModel)
        //{
        //     var user = await _userManager.FindByNameAsync(loginModel.Username);

        //    if (user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password))
        //    {

        //      var authClaims = new List<Claim>
        //      {
        //                new Claim(ClaimTypes.Name, user.UserName),
        //                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

        //      };
        //      var userRoles = await _userManager.GetRolesAsync(user);
        //        foreach(var role in userRoles)
        //        {
        //            authClaims.Add(new Claim(ClaimTypes.Role, role));
        //        }
        //        if (user.TwoFactorEnabled)
        //        {

        //            // Generate email confirmation token
        //            var token = await _userManager.GenerateTwoFactorTokenAsync(user,"Email");

        //            // Construct the confirmation link

        //            // Create the email message
        //            var message = new Message(new string[] { user.Email! }, "OTP Confirmation", token!);

        //            // Send the email
        //            _emailService.SendEmail(message);
        //            return StatusCode(StatusCodes.Status200OK
        //                , new Response { Status = "Succuess", Message = $"We have sent an OTP to your Email{user.Email}" });
        //        }
        //        var jwtToken = GetToken(authClaims);
        //        return Ok(new
        //        {
        //            token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
        //            expiration =jwtToken.ValidTo
        //        }) ;

        //    }
        //    return Unauthorized(new { Message = "Invalid username or password" });

        //}
        //[HttpPost("LogIn-2FA")]
        //public async Task<IActionResult> LogInWithOTp([FromBody] TwoFactorAuthModel model)
        //{
        //    try
        //    {
        //        var result = await _signInManager.TwoFactorSignInAsync(model.Username, model.Code, false, false);

        //        if (result.Succeeded)
        //        {
        //            // Successful two-factor authentication
        //            var user = await _userManager.FindByNameAsync(model.Username);

        //            var authClaims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Name, user.UserName),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //    };

        //            var userRoles = await _userManager.GetRolesAsync(user);
        //            foreach (var role in userRoles)
        //            {
        //                authClaims.Add(new Claim(ClaimTypes.Role, role));
        //            }

        //            var jwtToken = GetToken(authClaims);
        //            return Ok(new
        //            {
        //                token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
        //                expiration = jwtToken.ValidTo
        //            });
        //        }

        //        // Handle unsuccessful two-factor authentication
        //        return Unauthorized(new { Message = "Invalid two-factor authentication code" });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error in LogInWithOTp: {ex}");
        //        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Internal server error." });
        //    }
        //}
        #endregion

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser, string role="User")
        {
            var response = new Response();

            if (!ModelState.IsValid)
            {
                response.Status = "Error";
                response.Message = "Invalid input data";
                return BadRequest(response);
            }

            // Check if the user with the same email exists
            var existingUser = await _userManager.FindByEmailAsync(registerUser.Email);
            if (existingUser != null)
            {
                response.Status = "Error";
                response.Message = "Email is already registered";
                return BadRequest(response);
            }

            // Check if the user with the same username exists
            existingUser = await _userManager.FindByNameAsync(registerUser.UserName);
            if (existingUser != null)
            {
                response.Status = "Error";
                response.Message = "Username is already taken";
                return BadRequest(response);
            }

            // Create a new ApplicationUser
            var newUser = new ApplicationUser
            {
                Email = registerUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerUser.UserName,
                TwoFactorEnabled = true
            };

            // Create the user with the provided password
            var result = await _userManager.CreateAsync(newUser, registerUser.Password);

            if (!result.Succeeded)
            {
                response.Status = "Error";
                response.Message = "Registration failed";
                foreach (var error in result.Errors)
                {
                    response.Message += $"\n{error.Description}";
                }
                return BadRequest(response);
            }

            // Check if the specified role exists
            if (!await _roleManager.RoleExistsAsync(role))
            {
                response.Status = "Error";
                response.Message = "Specified role does not exist";
                return BadRequest(response);
            }

            // Assign the user to the specified role
            var roleResult = await _userManager.AddToRoleAsync(newUser, role);

            // Generate email confirmation token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

            // Construct the confirmation link
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Authentications", new { token, email = newUser.Email }, Request.Scheme);

            // Create the email message
            var message = new Message(new string[] { newUser.Email! }, "Configuration Email Link", confirmationLink!);

            // Send the email
            _emailService.SendEmail(message);

            if (!roleResult.Succeeded)
            {
                response.Status = "Error";
                response.Message = "Role assignment failed";
                foreach (var error in roleResult.Errors)
                {
                    response.Message += $"\n{error.Description}";
                }
                return BadRequest(response);
            }

            // Authenticate the user after registration and log in
            var authClaims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, newUser.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    };

            var userRoles = await _userManager.GetRolesAsync(newUser);
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var jwtToken = GetToken(authClaims);

            // Set the JWT token in a cookie
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict, // Adjust according to your requirements
                Expires = DateTime.UtcNow.AddHours(1) // Set the expiration time for the cookie
            };

            Response.Cookies.Append("JWTToken", new JwtSecurityTokenHandler().WriteToken(jwtToken), cookieOptions);

            response.Status = "Success";
            response.Message = $"User Created && Email Sent To {newUser.Email} Successfully";
            return Ok(response);
        }
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var response = new Response();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                response.Status = "Error";
                response.Message = "Email and token are required";
                return BadRequest(response);
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                response.Status = "Error";
                response.Message = "User not found";
                return BadRequest(response);
            }

            // Confirm the email
            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                response.Status = "Error";
                response.Message = "Email confirmation failed";
                foreach (var error in result.Errors)
                {
                    response.Message += $"\n{error.Description}";
                }
                return BadRequest(response);
            }

            response.Status = "Success";
            response.Message = "Email confirmation successful";
            return Ok(response);
        }

        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] LoginModel loginModel)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(loginModel.Username);

                if (user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password))
                {
                    var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

                    var userRoles = await _userManager.GetRolesAsync(user);
                    foreach (var role in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var jwtToken = GetToken(authClaims);

                    // Set the JWT token in a cookie
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict, // Adjust according to your requirements
                        Expires = DateTime.UtcNow.AddHours(1) // Set the expiration time for the cookie
                    };

                    Response.Cookies.Append("JWTToken", new JwtSecurityTokenHandler().WriteToken(jwtToken), cookieOptions);

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        expiration = jwtToken.ValidTo
                    });
                }

                return Unauthorized(new { Message = "Invalid username or password" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in LogIn: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Internal server error." });
            }
        }

        [HttpPost("Forget-Password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordViewModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    // Include the token in the email message
                    var resetPasswordLink = Url.Action(nameof(ResetPassword), "Authentication", new { token, email = user.Email }, Request.Scheme);
                    var messageContent = $"Click <a href='{resetPasswordLink}'>here</a> to reset your password. Your token: {token}";

                    // Create the email message
                    var message = new Message(new string[] { user.Email! }, "Forget Password Link", messageContent);

                    // Send the email
                    _emailService.SendEmail(message);

                    return StatusCode(StatusCodes.Status200OK,
                        new Response
                        {
                            Status = "Success",
                            Message = $"Password reset link sent to {user.Email}. Please check your email for further instructions.",
                        });
                }

                return StatusCode(StatusCodes.Status400BadRequest,
                       new Response { Status = "Error", Message = "Couldn't send the link to the email. Please try again." });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ForgetPassword: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Internal server error." });
            }
        }

        [HttpGet("Reset-Password")]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPassword { Token = token, Email = email };
            return Ok(model);            
        }

        [HttpPost("Reset-Password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetThePassword(ResetPassword resetPassword)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(resetPassword.Email);

                if (user != null)
                {
                    var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);

                    if (!resetPassResult.Succeeded)
                    {
                        foreach (var error in resetPassResult.Errors)
                        {
                            ModelState.AddModelError(error.Code, error.Description);
                        }

                        return BadRequest(ModelState);
                    }

                    return Ok(new Response { Status = "Success", Message = "Password has been changed successfully." });
                }

                return BadRequest(new Response { Status = "Error", Message = "User not found." });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ResetThePassword: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Internal server error." });
            }
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: authClaims,
                expires: DateTime.Now.AddHours(1), // Token expiration time
                signingCredentials: credentials
                );
            return token;
        }
    }
}
