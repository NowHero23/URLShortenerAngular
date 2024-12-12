import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { LocalStorageService } from '../../services/local-storage.service';

@Component({
  selector: 'app-login',
  imports: [RouterLink, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  http = inject(HttpClient);
  router = inject(Router);
  localStorageService = inject(LocalStorageService);

  public formGroup = new FormGroup({
    login: new FormControl(''),
    password: new FormControl(''),
  });

  onFormSubmit() {
    console.log(this.formGroup.value);

    this.http
      .post<any>(
        '/api/auth/login',
        {
          Login: this.formGroup.value.login,
          Password: this.formGroup.value.password,
        },
        {
          observe: 'response',
          responseType: 'json',
          withCredentials: true,
        }
      )
      .subscribe((resp) => {
        if (resp.status == 200) {
          this.localStorageService.set('signIn', 'true');
          this.router.navigateByUrl('/about');
        } else {
          this.formGroup.reset();
        }
      });
  }

  onInputLoginChange(e: Event): void {
    const targetDivElement = e.target as HTMLInputElement;
    const value = targetDivElement.value;
    this.formGroup.value.login = value;
  }
  onInputPasswordChange(e: Event): void {
    const targetDivElement = e.target as HTMLInputElement;
    const value = targetDivElement.value;
    this.formGroup.value.password = value;
  }
}
