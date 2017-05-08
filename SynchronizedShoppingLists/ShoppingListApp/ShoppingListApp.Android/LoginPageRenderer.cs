using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using ShoppingListApp;
using ShoppingListApp.Droid;
using Xamarin.Auth;
using Xamarin.Forms.Platform.Android;
using System.Json;
using ShoppingListApp.Models;

[assembly: ExportRenderer(typeof(LoginPage), typeof(LoginPageRenderer))]
namespace ShoppingListApp.Droid
{
    public class LoginPageRenderer : PageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(new ElementChangedEventArgs<Page>(e.OldElement, e.NewElement));

            // this is a ViewGroup - so should be able to load an AXML file and FindView<>
            var activity = this.Context as Activity;

            var auth = new OAuth2Authenticator(
                clientId: ApiKeys.FacebookApi,
                scope: "", // the scopes for the particular API you're accessing, delimited by "+" symbols
                authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri("https://www.facebook.com/connect/login_success.html"));

            auth.Completed += Auth_Completed;
            try
            {

            activity.StartActivity((Intent)auth.GetUI(activity));
            }catch(Exception err)
            {
            }
        }
        private async void Auth_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                //AccountStore.Create(Context).Save(e.Account, "Facebook");  Keystore/Keychain(ios) stuffs
                try
                {
                    var request = new OAuth2Request(
                            "GET",
                            new Uri("https://graph.facebook.com/me?fields=name,picture"),
                            //new Uri("https://graph.facebook.com/me"),
                            null,
                            e.Account);
                    var fbResponse = await request.GetResponseAsync();
                    var fbUser = JsonValue.Parse(fbResponse.GetResponseText());
                    UserInformation.UserName = fbUser["name"];
                    //UserInformation.Email = fbUser["email"];
                    var photo = fbUser["picture"]["data"]["url"];
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }


                //UserInformation.Photo = StreamImageSource
                //UserInformation.UserName = name;
                //ShoppingListApp.Models.ApiKeys.FaceBookEventArgs = eventArgs;
                App.SuccessfulLoginAction.Invoke();
                ApiKeys.SaveToken(e.Account.Properties["access_token"]);
                // Use eventArgs.Account to do wonderful things
                //App.SaveToken(eventArgs.Account.Username);

            }
            else
            {
                // The user cancelled
            }

        }
    }


}
