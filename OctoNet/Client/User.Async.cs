﻿using OctoNet.Models;
using RestSharp;
using OctoNet.Authenticators;
using System;

namespace OctoNet
{
    public partial class OctoNetClient
    {

        public void LoginAsync(string email, string password, Action<RestResponse<UserLogin>> callback)
        {
            _restClient.BaseUrl = OctoNet.Resource.SecureLoginBaseUrl;
            _restClient.Authenticator = new OAuthAuthenticator(_restClient.BaseUrl, _apiKey, _appsecret, null, null);

            var request = _requestHelper.CreateLoginRequest(_apiKey, email, password);

            _restClient.ExecuteAsync<UserLogin>(request, (restResponse) =>
            {
                _userLogin = restResponse.Data;
                callback(restResponse);
			});

        }

        public void Account_InfoAsync(Action<RestResponse<AccountInfo>> callback)
        {
            //This has to be here as Octopart change their base URL between calls
            _restClient.BaseUrl = Resource.ApiBaseUrl;
            _restClient.Authenticator = new OAuthAuthenticator(_restClient.BaseUrl, _apiKey, _appsecret, _userLogin.Token, _userLogin.Secret);

            var request = _requestHelper.CreateAccountInfoRequest();

            _restClient.ExecuteAsync<AccountInfo>(request, callback);
        }

        public void CreateAccountAsync(string email, string firstName, string lastName, string password, Action<RestResponse> callback)
        {
            //This has to be here as Octopart change their base URL between calls
            _restClient.BaseUrl = Resource.ApiBaseUrl;
            _restClient.Authenticator = new OAuthAuthenticator(_restClient.BaseUrl, _apiKey, _appsecret, _userLogin.Token, _userLogin.Secret);

            var request = _requestHelper.CreateNewAccountRequest(_apiKey, email, firstName, lastName, password);

            _restClient.ExecuteAsync(request, callback);
        }
    }
}