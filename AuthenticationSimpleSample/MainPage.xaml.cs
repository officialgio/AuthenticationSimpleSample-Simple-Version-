﻿using AuthenticationSimpleSample.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace AuthenticationSimpleSample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

    async void AzureADPageClicked(object sender, EventArgs args)
    {
        var authService = new AuthService(); // most likely you will inject it in constructor, but for simplicity let's initialize it here
        var result = await authService.LoginAsync(CancellationToken.None);
        var token = result?.IdToken; // you can also get AccessToken if you need it
        if (token is not null)
        {
            var handler = new JwtSecurityTokenHandler();
            var data = handler.ReadJwtToken(token);
            var claims = data.Claims.ToList();
            if (data is not null)
            {
                await Shell.Current.GoToAsync(nameof(SettingsPage));
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"Name: {data.Claims.FirstOrDefault(x => x.Type.Equals("name"))?.Value}");
                stringBuilder.AppendLine($"Email: {data.Claims.FirstOrDefault(x => x.Type.Equals("preferred_username"))?.Value}");
                LoginResultLabel.Text = stringBuilder.ToString();
            }
        }
    }
}
