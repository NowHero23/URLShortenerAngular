import { Component, inject, resource } from '@angular/core';
import { RouterLink } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { LocalStorageService } from '../../services/local-storage.service';

@Component({
  selector: 'app-login-button',
  imports: [RouterLink],
  templateUrl: './login-button.component.html',
  styleUrl: './login-button.component.css',
})
export class LoginButtonComponent {
  http = inject(HttpClient);
  localStorageService = inject(LocalStorageService);
  isLogin: boolean = false;

  isSignInResource = resource({
    loader: () => {
      const signIn = this.localStorageService.has('signIn');
      console.log(signIn);
      this.isLogin = signIn;
      return new Promise<string | null>((resolve, reject) => {
        return signIn;
      });

      // new Promise((resolve, reject) => {
      //   const signIn = this.localStorageService.get('signIn');
      //   console.log(signIn);
      //   return signIn;
      // });
    },
  });
  refreshResource() {
    this.isSignInResource.reload();
  }

  logOut = () => {
    this.http.post('api/auth/logout', {});
    this.localStorageService.remove('signIn');
    this.isSignInResource.reload();
  };
}
