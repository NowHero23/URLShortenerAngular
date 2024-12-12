import { AuthComponent } from './components/auth/auth.component';
import { RegisterComponent } from './components/register/register.component';
import { Routes } from '@angular/router';
import { AboutComponent } from './components/about/about.component';
import { LoginComponent } from './components/login/login.component';
import { UrlsTableComponent } from './components/urls-table/urls-table.component';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'table' },
  {
    path: 'table',
    component: UrlsTableComponent,
  },
  {
    path: 'auth',
    component: AuthComponent,
    pathMatch: 'prefix',
    children: [
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'logout', redirectTo: 'api/auth/logout', pathMatch: 'full' },
    ],
  },

  { path: 'about', component: AboutComponent, pathMatch: 'full' },
];
