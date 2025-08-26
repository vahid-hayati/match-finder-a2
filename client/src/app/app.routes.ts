import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { RegisterComponent } from './components/account/register/register.component';
import { FooterComponent } from './components/footer/footer.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { MemberListComponent } from './components/members/member-list/member-list.component';
import { MemberCardComponent } from './components/members/member-card/member-card.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { LoginComponent } from './components/account/login/login.component';

export const routes: Routes = [
    { path: '', component: HomeComponent },
    // { path: 'home', component: HomeComponent },
    { path: 'account/register', component: RegisterComponent },
    { path: 'account/login', component: LoginComponent },
    { path: 'footer', component: FooterComponent },
    { path: 'navbar', component: NavbarComponent },
    { path: 'members/member-list', component: MemberListComponent },
    { path: 'members/member-card', component: MemberCardComponent },
    { path: '**', component: NotFoundComponent }
];