using Microsoft.AspNetCore.Components;
using MudBlazor;
using SecureOps.Models;
using SecureOps.Models.Dto;
using SecureOps.Pages;
using SecureOps.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using static Microsoft.AspNetCore.Components.NavigationManager ;
using static SecureOps.Pages.Login;
using static System.Net.WebRequestMethods;

public static class JwtHelper
{
    public static UserDto DecodeTokenToUser(string token)
    {
        if (string.IsNullOrEmpty(token)) return null;

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var user = new UserDto
        {
            Email = jwtToken.Claims.FirstOrDefault(c => c.Type == "Email")?.Value
                    ?? jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value,
            FullName = jwtToken.Claims.FirstOrDefault(c => c.Type == "FullName")?.Value,
            Id = Convert.ToInt32(jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value),
            Username = jwtToken.Claims.FirstOrDefault(c => c.Type == "Username")?.Value,

            Roles = jwtToken.Claims
                            .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
                            .Select(c => new Role { RoleName = c.Value })
                            .ToList()
        };

        return user;
    }
    public static Employee DecodeTokenToEmployee(string token)
    {
        if (string.IsNullOrEmpty(token)) return null;

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var employee = new Employee
        {
                



            FullName = jwtToken.Claims.FirstOrDefault(c => c.Type == "employeeFullName")?.Value,
            JobTitle = jwtToken.Claims.FirstOrDefault(c => c.Type == "employeeJobTitle")?.Value,
            Id = Convert.ToInt32( jwtToken.Claims.FirstOrDefault(c => c.Type == "employeeId")?.Value),
            UserId = Convert.ToInt32(jwtToken.Claims.FirstOrDefault(c => c.Type == "employeeUserId")?.Value)

        };

        return employee;
    }
    public static async Task<EmployeeDto> DecodeFromDatabaseTokenToEmployeeAsync(string token,
    HttpClient http,
    string baseUri)
    {
        if (string.IsNullOrEmpty(token)) return null;

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        int EmpID = Convert.ToInt32(jwtToken.Claims.FirstOrDefault(c => c.Type == "employeeId")?.Value);

        var employee = await http.GetFromJsonAsync<EmployeeDto>(
    $"{baseUri}api/employee/GetEmployee?empId={EmpID}");
   

        return employee;
    }
}
