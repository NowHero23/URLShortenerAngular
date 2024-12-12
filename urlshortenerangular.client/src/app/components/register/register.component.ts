import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-register',
  imports: [RouterLink, ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  http = inject(HttpClient);
  router = inject(Router);

  public formGroup = new FormGroup({
    login: new FormControl(''),
    password: new FormControl(''),
  });

  onFormSubmit() {
    console.log(this.formGroup.value);

    this.http
      .post<any>(
        '/api/auth/register',
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
          this.router.navigateByUrl('/auth/login');
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
