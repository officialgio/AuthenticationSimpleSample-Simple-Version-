﻿using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationSimpleSample.Auth;
public class AuthService
{
    private readonly IPublicClientApplication authenticationClient;
    public AuthService()
    {
        // The AuthenticationClient is what we will call in other pages to authenticate users.
        // instantiate authentication client -> picks up the information (get tokens) 
        authenticationClient = PublicClientApplicationBuilder.Create(Constants.ClientId)
            .WithB2CAuthority(Constants.AuthoritySignIn) // uncomment to support B2C ->  provides the default Authority, or policy, that will be used to authenticate users.
            .WithRedirectUri($"msal{Constants.ClientId}://auth")
            .Build();
    }

    public async Task<AuthenticationResult> LoginAsync(CancellationToken cancellationToken)
    {
        AuthenticationResult result;
        try
        {
            result = await authenticationClient
                .AcquireTokenInteractive(Constants.Scopes) // call the AcquireTokenXXX method corresponding to the flow you want to use
                .WithPrompt(Prompt.ForceLogin) //  command builder => prompt user for credemtials
                .WithUseEmbeddedWebView(true)
#if ANDROID
                .WithParentActivityOrWindow(Microsoft.Maui.ApplicationModel.Platform.CurrentActivity)
#endif
                .ExecuteAsync(cancellationToken); //  call ExecuteAsync() to get your authentication result
            return result;
        }
        catch (MsalClientException)
        {
            return null;
        }
    }
}

