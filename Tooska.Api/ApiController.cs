using System.Security.Claims;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Tooska.Api;

public interface IApiPageModel<TUser, TKey> 
    where TUser : IdentityUser<TKey>
    where TKey : IEquatable<TKey>
{
    public Task<TUser> UserAsync { get; }

    //public ApplicationUser User { get; }

    //public string UserId { get; }
    public string Username { get; }
    public string UserId { get; }
}

public class ApiController<DbContext, TUser, TRole, TKey> : ControllerBase, IApiPageModel<TUser, TKey>
    where DbContext : IdentityDbContext<TUser, TRole, TKey>
    where TUser : IdentityUser<TKey>
    where TRole : IdentityRole<TKey>
    where TKey : IEquatable<TKey>
{
    protected readonly UserManager<TUser> UserManager = null;
    protected DbContext Context { get; }

    protected async Task<TUser> GetCurrentUser()
    {
        var username = HttpContext?.User?.Identity?.Name;
        if (string.IsNullOrWhiteSpace(username))
            return null;
        return await UserManager.FindByNameAsync(username);
    }

    #region IPageModel interface

    Task<TUser> IApiPageModel<TUser, TKey>.UserAsync => string.IsNullOrWhiteSpace(Me.Username) ? null : UserManager.FindByNameAsync(Me.Username);
    //ApplicationUser IApiPageModel.User => Me.User;

    // ReSharper disable once MemberCanBePrivate.Global
    //string IApiPageModel.UserId => HttpContext?.User?.Identity?.Name;
    string IApiPageModel<TUser, TKey>.Username => HttpContext?.User?.Identity?.Name;
    string IApiPageModel<TUser, TKey>.UserId => HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    #endregion

    public ApiController(DbContext context, UserManager<TUser> userManager)
    {
        Context = context;
        UserManager = userManager;
    }

    // ReSharper disable once MemberCanBePrivate.Global
    protected IApiPageModel<TUser, TKey> Me => (IApiPageModel<TUser, TKey>) this;

    protected Result Success()
    {
        return new Result();
    }

    protected Result<T> Success<T>(T data)
    {
        return new Result<T>(data);
    }

    protected ResultList<T> Success<T>(List<T> data)
    {
        return new ResultList<T>(data);
    }

    protected Result Error()
    {
        return new Result(-1, "");
    }
    
    protected Result Error(string message)
    {
        return new Result(-1, message);
    }

    protected Result Error(Exception ex)
    {
        return new Result(-1, ex.Message);
    }

    protected Result Error(int code, string message)
    {
        return new Result(code, message);
    }

    protected Result<T> Error<T>(int code, string message)
    {
        return new Result<T>(code, message);
    }

    protected ResultList<T> ErrorList<T>(int code, string message)
    {
        return new ResultList<T>(code, message);
    }
}