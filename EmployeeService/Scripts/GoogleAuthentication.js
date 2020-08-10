/// <reference path="jquery-3.4.1.min.js" />

//example of return url from google with access token
//https://localhost:44326/Login.html#access_token=Y4JWgVijsaBfDeTKMQaTNXK5GB7OxfoG5WJ40qDaYvPJunVn_OFbuYJZXGfOXRMpdN60nV8dISvjh6QH8sM3G-r5FEw8bulXDJquHxY4XaL0-M_ZnFpfoNXDMq1lowxYyh0HdZrLreXdIRzL5J2y4Yh66CeQm0TRmW_bYLpPo204EMqo5UV2y7NOfHOTlJZTZFoQyfJTQjcb1pMbTGMQrCDHItFb1Afr4u418dYf8FasY7gTYLISb3-pIYNGc4BEIleonD6bRQtbZAv8lgvmS7IzbD4ywabYM0UikDoOPjN1R-ypDro8IRX8mCYhTrLAEQg8PmZ5I-aXortxtrgFreeTZL-_cYZbyKTBAV14DFM&token_type=bearer&expires_in=1209600&state=1QO3_M0xPuEcl2kvNMwkiwOpPv9flst3jXgGahE-lKI1


function getAccessToken() {
    //return the url's fragment(including the leading #)
    if (location.hash) {
        if (location.hash.split('access_token=')) {
            var accessToken = location.hash.split('access_token=')[1].split('&')[0];
            if (accessToken) {
                isUserRegistered(accessToken);
            }
        }
    }
}

//To check if the user is registered we issue a GET request 
//to / api / Account / UserInfo passing it the access token using Authorization header

function isUserRegistered(accessToken) {
    $.ajax({
        url: '/api/Account/UserInfo',
        method: 'GET',
        headers: {
            'content-type': 'application/JSON',
            'Authorization': 'Bearer ' + accessToken
        },
        success: function (response) {
            if (response.HasRegistered) {
                alert("registered");
                sessionStorage.setItem('accessToken', accessToken);
                sessionStorage.setItem('userName', response.Email);
                window.location.href = "../Data.html";
            }
            else {
                //use provider to differentiate Google or Facebook
                signupExternalUser(accessToken, response.provider);
            }
        }
    });
}

//If the Google authenticated user is not already registered with our application, we need to register him
function signupExternalUser(accessToken, provider) {
    $.ajax({
        url: '/api/Account/RegisterExternal',
        method: 'POST',
        headers: {
            'content-type': 'application/json',
            'Authorization': 'Bearer ' + accessToken
        },
        success: function () {
            //the same redirect url when user sign in with Google.
            window.location.href = "/api/Account/ExternalLogin?provider=" + provider + "&response_type=token&client_id=self&redirect_uri=https%3A%2F%2Flocalhost%3A44326%2FLogin.html&state=1QO3_M0xPuEcl2kvNMwkiwOpPv9flst3jXgGahE-lKI1";

        }
    });

}