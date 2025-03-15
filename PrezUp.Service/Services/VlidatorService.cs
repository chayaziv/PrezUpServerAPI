using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PrezUp.Core.EntityDTO;
using PrezUp.Core.IRepositories;
using PrezUp.Core.IServices;
using PrezUp.Core.Utils;
namespace PrezUp.Core.Service;


public class ValidatorService:IValidatorService
{
    private readonly IRepositoryManager _repository;

    // אתחול השירות עם UserService
    public ValidatorService(IRepositoryManager repository)
    {
        _repository = repository;
    }

    // פונקציה לבדוק אם השם מלא (לא ריק)
    public bool ValidateName(string name)
    {
        return !string.IsNullOrWhiteSpace(name);
    }

    // פונקציה לבדוק אם האימייל בתבנית תקינה
    public bool ValidateEmail(string email)
    {
        var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, emailPattern);
    }

    // פונקציה לבדוק אם הסיסמה מכילה לפחות 8 תווים עם אותיות, ספרות ואות גדולה
    public bool ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;

        // דרישות בסיסיות לסיסמה
        var passwordPattern = @"^(?=.*[A-Za-z])(?=.*\d).{2,}$";
        return Regex.IsMatch(password, passwordPattern);
    }

    // פונקציה לבדוק אם השדות לא ריקים
    public bool ValidateRequiredFields(UserDTO user)
    {
        return !string.IsNullOrWhiteSpace(user.Name) &&
               !string.IsNullOrWhiteSpace(user.Email) &&
               !string.IsNullOrWhiteSpace(user.Password);
    }

    // פונקציה לבדוק אם האימייל כבר קיים במערכת
    public async Task<bool> EmailExistsAsync(string email)
    {
        // נניח ש- UserService כולל פונקציה לחפש משתמש לפי אימייל
        return await _repository.Users.ExistsByEmailAsync(email);
    }

    // פונקציה לבדוק אם ה-UserDTO תקין
    public async Task<ValidResult> ValidateUserAsync(UserDTO user)
    {
        // בדיקת שדות חובה
        if (!ValidateRequiredFields(user))
        {
            return ValidResult.Failure("Name, email, and password fields are required.");
        }

        // בדיקת תקינות שם
        if (!ValidateName(user.Name))
        {
            return ValidResult.Failure("Invalid Name.");
        }

        // בדיקת תקינות אימייל
        if (!ValidateEmail(user.Email))
        {
            return ValidResult.Failure("Invalid Email.");
        }

        // בדיקת אם האימייל כבר קיים במערכת
        if (await EmailExistsAsync(user.Email))
        {
            return ValidResult.Failure("Email already exists.");
        }

        // בדיקת תקינות סיסמה
        if (!ValidatePassword(user.Password))
        {
            return ValidResult.Failure("Password must be at least 2 characters long, contain at least one uppercase letter and one number.");
        }

        return ValidResult.Success();
    }

   
}
