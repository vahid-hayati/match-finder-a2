import { HttpClient } from '@angular/common/http';
import { inject, Injectable, PLATFORM_ID, signal } from '@angular/core';
import { map, Observable } from 'rxjs';
import { LoggedInUser } from '../models/logged-in.model';
import { Login } from '../models/login.model';
import { Member } from '../models/member.model';
import { Router } from '@angular/router';
import { isPlatformBrowser } from '@angular/common';
import { environment } from '../../environments/environment.development';
import { RegisterUser } from '../models/register-user.model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  http = inject(HttpClient);
  router = inject(Router);
  loggedInUserSig = signal<LoggedInUser | null>(null);
  platformId = inject(PLATFORM_ID); //server, browser

  private readonly _baseApiUrl: string = environment.baseApiUrl + 'api/';

  register(userInput: RegisterUser): Observable<LoggedInUser | null> {
    let response$: Observable<LoggedInUser | null> =
      this.http.post<LoggedInUser>(this._baseApiUrl + 'account/register', userInput)
        .pipe(map(response => {
          if (response) {
            this.setCurrentUser(response);

            this.router.navigateByUrl('members/member-list');
          }

          return null;
        }));

    // let response$: Observable<LoggedIn> = this.http.post<LoggedIn>(this._baseApiUrl + 'account/register', userInput);
    return response$;
  }

  login(userInput: Login): Observable<LoggedInUser | null> {
    let response$: Observable<LoggedInUser | null> =
      this.http.post<LoggedInUser>(this._baseApiUrl + 'account/login', userInput)
        .pipe(map(response => { // response: LoggedInUser
          if (response) {
            this.setCurrentUser(response);

            this.router.navigateByUrl('members/member-list');
          }

          return null;
        }));

    return response$;
  }

  authorizeLoggedInUser(): void {
    this.http.get<LoggedInUser>(this._baseApiUrl + 'account').subscribe({
      error: (err) => {
        console.log(err.error);
        this.logout();
      }
    });
  }

  setCurrentUser(loggedInUser: LoggedInUser): void {
    this.loggedInUserSig.set(loggedInUser);

    if (isPlatformBrowser(this.platformId))
      localStorage.setItem('loggedIn', JSON.stringify(loggedInUser));

    // this.router.navigateByUrl('/members/member-list');
  }

  logout(): void {
    this.loggedInUserSig.set(null);

    if (isPlatformBrowser(this.platformId))
      localStorage.clear();

    this.router.navigateByUrl('account/login');
  }

  /*
    setCurrentUser(loggedIn: LoggedIn): void {
    this.loggedInUserSig.set(loggedIn);
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem('loggedInUser', JSON.stringify(loggedIn));
    }
  }
  */
}
