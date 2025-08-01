using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using uweb4Media.Application.Interfaces.AppUserInterfaces;
using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Helper;

public class FirebaseAuthHelper
{
    public FirebaseAuthHelper()
    {
        // Servis ilk başlatıldığında bir kere initialize edilmeli!
        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("firebase-credentials.json")
            });
        }
    }

    public async Task<AppUser> GetUserFromFirebaseTokenAsync(string idToken, IAppUserRepository userRepository)
    {
        var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
        var email = decodedToken.Claims["email"].ToString();
        var user = await userRepository.GetByEmailAsync(email);

        if (user == null || !user.IsEmailVerified)
            throw new UnauthorizedAccessException("Mail doğrulanmadan ödeme yapılamaz!");

        return user;
    }
}