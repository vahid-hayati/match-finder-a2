import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { RegisterComponent } from './components/account/register/register.component';
import { FooterComponent } from './components/footer/footer.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { MemberListComponent } from './components/members/member-list/member-list.component';
import { MemberCardComponent } from './components/members/member-card/member-card.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { LoginComponent } from './components/account/login/login.component';
import { authGuard } from './guards/auth.guard';
import { authLoggedInGuard } from './guards/auth-logged-in.guard';
import { UserEditComponent } from './components/user/user-edit/user-edit.component';

export const routes: Routes = [
    { path: '', component: HomeComponent },
    // { path: 'home', component: HomeComponent },
    { path: 'account/register', component: RegisterComponent, canActivate:[authLoggedInGuard] },
    { path: 'account/login', component: LoginComponent, canActivate:[authLoggedInGuard] },
    { path: 'footer', component: FooterComponent },
    { path: 'navbar', component: NavbarComponent },
    { path: 'members/member-list', component: MemberListComponent, canActivate:[authGuard]},
    { path: 'members/member-card', component: MemberCardComponent },
    { path: 'user-edit', component: UserEditComponent, canActivate:[authGuard]},
    { path: '**', component: NotFoundComponent }
];

/*
{ path: '', component: HomeComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [authGuard],
        children: [
            { path: 'members', component: MemberListComponent },
            { path: 'user/user-edit', component: UserEditComponent },
            { path: 'no-access', component: NoAccessComponent },
            { path: 'friends', component: FriendsComponent },
            { path: 'messages', component: MessagesComponent },
            { path: 'admin', component: AdminPanelComponent }
        ]
    },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [authLoggedInGuard],
        children: [
            { path: 'account/login', component: LoginComponent },
            { path: 'account/register', component: RegisterComponent },
        ]
    },
    { path: 'navbar', component: NavbarComponent },
    { path: 'footer', component: FooterComponent },
    { path: 'server-error', component: ServerErrorComponent },
    { path: '**', component: NotFoundComponent, pathMatch: 'full' },
    { path: '**', component: NotFoundComponent }
*/