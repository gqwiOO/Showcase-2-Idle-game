// using System;
// using UnityEngine;
// using Zenject;
//
// namespace Core.Scripts.Services.Auth
// {
//     public interface IAuthService: IInitializable
//     {
//         string UserName { get; }
//         string UserEmail { get; }
//         
//         void Register(string login, string password);
//         
//         bool IsSignedIn { get; }
//     }
//
//     public class FirebaseAuthService: IAuthService
//     {
//         private FirebaseAuth _authInstance;
//         private FirebaseUser _currentUser;
//         
//         private Uri _userUrl;
//
//         public string UserName { get; private set; }
//
//         public string UserEmail { get;  private set;}
//
//         public bool IsSignedIn => _currentUser != null;
//
//         public void Initialize()
//         {
//             FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
//             {
//                 // _authInstance = FirebaseAuth.DefaultInstance;
//                 _authInstance = FirebaseAuth.GetAuth(FirebaseApp.DefaultInstance);
//                 AuthStateChanged(this, null);
//             });
//         }
//
//         private void AuthStateChanged(object sender, EventArgs eventArgs) {
//             if (_authInstance.CurrentUser != _currentUser) {
//                 bool signedIn = _currentUser != _authInstance.CurrentUser && _authInstance.CurrentUser != null
//                                                                   && _authInstance.CurrentUser.IsValid();
//                 if (!signedIn && _currentUser != null) 
//                     Debug.Log("Signed out " + _currentUser.UserId);
//                 
//                 _currentUser = _authInstance.CurrentUser;
//                 if (signedIn) {
//                     Debug.Log("Signed in " + _currentUser.UserId);
//                     UserName = _currentUser.DisplayName ?? "";
//                     UserEmail = _currentUser.Email ?? "";
//                     _userUrl = _currentUser.PhotoUrl == null ? new Uri("") : _currentUser.PhotoUrl;
//                 }
//             }
//         }
//
//
//         public void Register(string login, string password)
//         {
//             _authInstance.CreateUserWithEmailAndPasswordAsync(login, password).ContinueWith(task => {
//                 if (task.IsCanceled) {
//                     Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
//                     return;
//                 }
//                 if (task.IsFaulted) {   
//                     Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
//                     return;
//                 }
//
//                 AuthResult result = task.Result;
//                 Debug.LogFormat("Firebase user created successfully: {0} ({1})",
//                     result.User.DisplayName, result.User.UserId);
//             });
//         }
//
//     }
// }